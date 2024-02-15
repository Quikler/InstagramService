using InstagramApiSharp.Classes.Models;
using InstagramService.Classes.Models;
using System.Collections;

namespace InstagramService.Classes.Collections
{
    public class InstaMediaStreams : IEnumerable<InstaMediaStream>, IDisposable
    {
        private readonly List<InstaMediaStream> _mediaStreams;

        public InstaMedia Media { get; }

        public InstaMediaStreams(int size, InstaMedia media)
        {
            _mediaStreams = new(Enumerable.Repeat<InstaMediaStream>(default!, size));
            Media = media;
        }

        public InstaMediaStream this[int index]
        {
            get => _mediaStreams[index];
            set => _mediaStreams[index] = value;
        }

        public IEnumerator<InstaMediaStream> GetEnumerator() => _mediaStreams.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        ~InstaMediaStreams() => Dispose();
        public void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }

        private void Cleanup()
        {
            foreach (var ims in _mediaStreams)
            {
                ims.Dispose();
            }
        }
    }
}