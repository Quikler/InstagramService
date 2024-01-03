using InstagramApiSharp.Classes.Models;

namespace InstagramService.Classes.Models
{
    public class InstaMediaStream : IDisposable
    {
        public Stream Stream { get; }
        public InstaMediaType MediaType { get; }
        public string Uri { get; }
        public string InitialUri { get; }

        internal InstaMediaStream(Stream stream, InstaMediaType MediaType, string uri, string initialUri)
        {
            Stream = stream;
            this.MediaType = MediaType;
            Uri = uri;
            InitialUri = initialUri;
        }

        ~InstaMediaStream() => Stream?.Dispose();
        public void Dispose()
        {
            Stream?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}