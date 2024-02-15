using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramService.Classes.Collections;
using InstagramService.Classes.Helpers;

namespace InstagramService.Classes
{
    public class InstaMediaProcessor
    {
        private readonly IInstaApi _api;

        public InstaMediaProcessor(IInstaApi api)
        {
            _api = api;
        }

        public async Task<IResult<InstaMediaInfos>> GetInfosAsync(string uri)
        {
            IResult<InstaMedia> mediaResult = await GetMediaAsync(uri);

            if (!mediaResult.Succeeded)
                return Result.Fail<InstaMediaInfos>(mediaResult.Info.Exception);

            InstaMedia media = mediaResult.Value;

            IReadOnlyList<string> mediaUris = InstaParseHelper.ParseUris(media);
            IReadOnlyList<InstaMediaType> mediaTypes = InstaParseHelper.ParseMediaTypes(media);
            IReadOnlyList<string> initialUris = media.Carousel?.Count > 0 ?
                CarouselHelper.GetCarouselInitialUris(uri, media.Carousel) : new[] { uri };

            InstaMediaInfos instaMediaInfos = new(mediaUris.Count, media);

            for (int i = 0; i < mediaUris.Count; i++)
            {
                instaMediaInfos[i] = new(mediaTypes[i], mediaUris[i], initialUris[i], i + 1);
            }

            return Result.Success(instaMediaInfos);
        }

        public async Task<IResult<InstaMedia>> GetMediaAsync(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? resultUrl))
                return Result.Fail<InstaMedia>("Invalid url");

            IResult<string> mediaIdResult = await _api.MediaProcessor.GetMediaIdFromUrlAsync(resultUrl);
            if (!mediaIdResult.Succeeded)
                return Result.Fail<InstaMedia>(mediaIdResult.Info.Exception);

            IResult<InstaMedia> mediaResult = await _api.MediaProcessor.GetMediaByIdAsync(mediaIdResult.Value);
            if (!mediaResult.Succeeded)
                return Result.Fail<InstaMedia>(mediaIdResult.Info.Exception);

            return mediaResult;
        }

        public async Task<IResult<FileInfo[]>> DownloadMediasAsync(InstaMediaStreams mediaStreams,
            string destinationFolderPath, string imageFormat = "jpeg", string videoFormat = "mp4")
        {
            int mediaStreamsCount = mediaStreams.Count();
            FileInfo[] fileInfos = new FileInfo[mediaStreamsCount];

            for (int i = 0; i < mediaStreamsCount; i++)
            {
                string filePath = $"{destinationFolderPath}\\{mediaStreams.Media.Code}";
                string extension = mediaStreams[i].MediaType == InstaMediaType.Image ? imageFormat : videoFormat;

                if (mediaStreamsCount != 1)
                    filePath += $"-{i + 1}.{extension}";
                else filePath += $".{extension}";

                try
                {
                    fileInfos[i] = new(filePath);
                    using Stream stream = fileInfos[i].OpenWrite();

                    await mediaStreams[i].Stream.CopyToAsync(stream);
                }
                catch (Exception ex)
                {
                    return Result.Fail<FileInfo[]>(ex);
                }
            }

            return Result.Success(fileInfos);
        }
    }
}