﻿using InstagramApiSharp.Classes.Models;

namespace InstagramService.Classes.Models
{
    public class InstaMediaStream : IDisposable
    {
        public Stream Stream { get; }
        public InstaMediaType MediaType { get; }
        public string Uri { get; }
        public string InitialUri { get; }
        public int CarouselIndex { get; }

        public InstaMediaStream(Stream stream, InstaMediaType mediaType, 
            string uri, string initialUri, int carouselIndex)
        {
            Stream = stream;
            MediaType = mediaType;
            Uri = uri;
            InitialUri = initialUri;
            CarouselIndex = carouselIndex;
        }

        ~InstaMediaStream() => Stream?.Dispose();
        public void Dispose()
        {
            Stream?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}