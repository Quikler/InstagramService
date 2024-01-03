using InstagramApiSharp.API;

namespace InstagramService.Classes
{
    public class InstaService
    {
        private readonly IInstaApi _api;

        public InstaAccountSession AccountSession { get; }
        public InstaMediaProcessor MediaHelper { get; }
        public InstaStreamTaker StreamTaker { get; }

        public InstaService(IInstaApi api)
        {
            _api = api;
            AccountSession = new(_api);
            MediaHelper = new(_api);
            StreamTaker = new(_api);
        }
    }
}