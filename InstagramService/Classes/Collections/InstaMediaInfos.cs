using InstagramApiSharp.Classes.Models;
using InstagramService.Classes.Models;
using System.Collections;

namespace InstagramService.Classes.Collections
{
    public class InstaMediaInfos : IEnumerable<InstaMediaInfo>
    {
        private readonly List<InstaMediaInfo> _mediaInfos;

        public InstaMedia Media { get; }

        public InstaMediaInfos(int size, InstaMedia media)
        {
            _mediaInfos = new(Enumerable.Repeat<InstaMediaInfo>(default!, size));
            Media = media;
        }

        public InstaMediaInfo this[int index]
        {
            get => _mediaInfos[index];
            set => _mediaInfos[index] = value;
        }

        public IEnumerator<InstaMediaInfo> GetEnumerator() => _mediaInfos.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
