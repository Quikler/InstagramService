using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramService.Classes.Collections;

namespace InstagramService.Classes
{
    public class InstaMediaProcessor
    {
        private readonly IInstaApi _api;

        public InstaMediaProcessor(IInstaApi api)
        {
            _api = api;
        }

        public async Task<IResult<InstaMedia>> GetMediaAsync(string url)
        {
            if (!IsUrlValid(url, out Uri? resultUrl))
                return Result.Fail<InstaMedia>("Invalid url");

            IResult<string> mediaIdResult = await _api.MediaProcessor.GetMediaIdFromUrlAsync(resultUrl);
            if (!mediaIdResult.Succeeded)
                return Result.Fail<InstaMedia>(mediaIdResult.Info.Exception);

            IResult<InstaMedia> mediaResult = await _api.MediaProcessor.GetMediaByIdAsync(mediaIdResult.Value);
            if (!mediaResult.Succeeded)
                return Result.Fail<InstaMedia>(mediaIdResult.Info.Exception);

            return mediaResult;

            static bool IsUrlValid(string url, out Uri? resultUrl)
            {
                if (Uri.TryCreate(url, UriKind.Absolute, out resultUrl))
                    return true;
                return false;
            }
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