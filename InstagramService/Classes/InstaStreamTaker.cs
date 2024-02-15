using InstagramApiSharp.Classes;
using InstagramApiSharp.API;
using InstagramService.Classes.Models;
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
            IResult<InstaMediaInfos> instaMediaInfosResult = await _instaMediaProcessor.GetInfosAsync(url);

            if (!instaMediaInfosResult.Succeeded)
                return Result.Fail<InstaMediaStreams>(instaMediaInfosResult.Info.Exception);

            InstaMediaInfos instaMediaInfos = instaMediaInfosResult.Value;

            return await GetStreamsAsync(instaMediaInfos);
        }

        public Task<IResult<InstaMediaStreams>> GetMediaStreamsAsync
            (InstaMediaInfos instaMediaInfos) => GetStreamsAsync(instaMediaInfos);

        private static async Task<IResult<InstaMediaStreams>> GetStreamsAsync(InstaMediaInfos instaMediaInfos)
        {
            InstaMediaStreams instaMediaStreams = new(instaMediaInfos.Count(), instaMediaInfos.Media);
            using HttpClient hc = new();

            try
            {
                for (int i = 0; i < instaMediaInfos.Count(); i++)
                {
                    InstaMediaInfo mediaInfo = instaMediaInfos[i];

                    using HttpResponseMessage response = await hc.GetAsync(mediaInfo.Uri);
                    if (!response.IsSuccessStatusCode)
                        return Result.Fail(response.StatusCode.ToString(), instaMediaStreams);

                    Stream source = await response.Content.ReadAsStreamAsync();
                    Stream dest = new MemoryStream();

                    source.CopyTo(dest);
                    dest.Seek(0, SeekOrigin.Begin);

                    instaMediaStreams[i] = new(dest, mediaInfo.MediaType, mediaInfo.Uri, mediaInfo.InitialUri, i + 1);
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