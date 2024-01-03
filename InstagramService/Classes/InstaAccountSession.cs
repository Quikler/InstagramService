using InstagramApiSharp.API;
using InstagramApiSharp.Classes;

namespace InstagramService.Classes
{
    public class InstaAccountSession
    {
        private readonly IInstaApi _api;
        public InstaAccountSession(IInstaApi api)
        {
            _api = api;
        }

        public async Task<IResult<InstaLoginResult>> LoginAsync(string accountSessionFilePath, bool saveAccountStateData = false)
        {
            using FileStream fs = new(accountSessionFilePath, FileMode.OpenOrCreate);
            try
            {
                await _api.LoadStateDataFromStreamAsync(fs); // loading account state data
            }
            catch // empty/wrong serialization, io, empty file exceptions etc
            {
                var resultLogining = await _api.LoginAsync();
                if (!resultLogining.Succeeded)
                    return resultLogining;

                if (saveAccountStateData)
                    await SaveAccountStateDataAsync(fs); // saving account state data cuz logining was succeeded
            }
            return Result.Success(InstaLoginResult.Success);
        }

        public async Task SaveAccountStateDataAsync(Stream destination)
        {
            using Stream sourceStream = await _api.GetStateDataAsStreamAsync();
            await sourceStream.CopyToAsync(destination);
        }

        public async Task SaveAccountStateDataAsync(string destination)
        {
            using Stream sourceStream = await _api.GetStateDataAsStreamAsync();
            using Stream destinationStream = new FileStream(destination, FileMode.OpenOrCreate);
            await sourceStream.CopyToAsync(destinationStream);
        }
    }
}