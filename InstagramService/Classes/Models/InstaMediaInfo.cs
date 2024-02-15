using InstagramApiSharp.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramService.Classes.Models
{
    public class InstaMediaInfo
    {
        public InstaMediaType MediaType { get; }
        public string Uri { get; }
        public string InitialUri { get; }
        public int CarouselIndex { get; }

        public InstaMediaInfo(InstaMediaType mediaType, string uri, string initialUri, int carouselIndex)
        {
            MediaType = mediaType;
            Uri = uri;
            InitialUri = initialUri;
            CarouselIndex = carouselIndex;
        }
    }
}
