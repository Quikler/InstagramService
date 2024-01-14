using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Classes;
using InstagramService.Classes.Helpers;
using InstagramApiSharp.API;
using InstagramService.Classes.Collections;

namespace InstagramService.Classes
{
    public class InstaStreamTaker
    {
        private readonly IInstaApi _api;
        private readonly InstaMediaProcessor _instaMediaProcessor;

        public InstaStreamTaker(IInstaApi api)
        {
            _api = api;
            _instaMediaProcessor = new(_api);
        }

        public async Task<IResult<InstaMediaStreams>> GetMediaStreamsAsync(string url)
        {
            IResult<InstaMedia> mediaResult = await _instaMediaProcessor.GetMediaAsync(url);

            if (!mediaResult.Succeeded)
                return Result.Fail<InstaMediaStreams>(mediaResult.Info.Exception);

            return await GetStreamsAsync(mediaResult.Value, url);
        }

        private static async Task<IResult<InstaMediaStreams>> GetStreamsAsync(InstaMedia media, string uri)
        {
            IReadOnlyList<string> mediaUris = InstaParseHelper.ParseUris(media);
            IReadOnlyList<InstaMediaType> mediaTypes = InstaParseHelper.ParseMediaTypes(media);
            IReadOnlyList<string> initialUris = media.Carousel?.Count > 0 ?
                CarouselHelper.GetCarouselInitialUris(uri, media.Carousel) : new[] { uri };

            InstaMediaStreams instaMediaStreams = new(mediaUris.Count, media);
            using HttpClient hc = new();

            try
            {
                for (int i = 0; i < mediaUris.Count; i++)
                {
                    using HttpResponseMessage response = await hc.GetAsync(mediaUris[i]);
                    if (!response.IsSuccessStatusCode)
                        return Result.Fail(response.StatusCode.ToString(), instaMediaStreams);

                    Stream source = await response.Content.ReadAsStreamAsync();
                    Stream dest = new MemoryStream();

                    source.CopyTo(dest);
                    dest.Seek(0, SeekOrigin.Begin);

                    instaMediaStreams[i] = new(dest, mediaTypes[i], mediaUris[i], initialUris[i], i + 1);
                }
            }
            catch (Exception ex)
            {
                return Result.Fail(ex, instaMediaStreams);
            }

            return Result.Success(instaMediaStreams);
        }
    }
}