using InstagramApiSharp.Classes.Models;
using System.Collections;

namespace InstagramService.Classes.Helpers
{
    internal static class InstaParseHelper
    {
        public static ArrayList ParseInstaMedias(InstaMedia media)
        {
            ArrayList arrayList = new();

            switch (media.MediaType)
            {
                case InstaMediaType.Image:
                    arrayList.Add(media.Images.MaxBy(im => im.Width * im.Height));
                    break;
                case InstaMediaType.Video:
                    arrayList.Add(media.Videos.MaxBy(im => im.Width * im.Height));
                    break;
                case InstaMediaType.Carousel:
                    arrayList.AddRange(CarouselHelper.GetCarouselMedias(media.Carousel));
                    break;
            }

            return arrayList;
        }

        public static IReadOnlyList<InstaMediaType> ParseMediaTypes(InstaMedia media)
        {
            List<InstaMediaType> mediaTypes = new();

            if (media.MediaType != InstaMediaType.Carousel)
            {
                mediaTypes.Add(media.MediaType);
                return mediaTypes;
            }

            ArrayList carouselMedias = CarouselHelper.GetCarouselMedias(media.Carousel);
            foreach (object carouselMedia in carouselMedias)
            {
                if (carouselMedia is InstaVideo)
                {
                    mediaTypes.Add(InstaMediaType.Video);
                }
                else
                {
                    mediaTypes.Add(InstaMediaType.Image);
                }
            }

            return mediaTypes;
        }

        public static IReadOnlyList<string> ParseUris(InstaMedia media)
        {
            List<string> mediaUris = new();

            switch (media.MediaType)
            {
                case InstaMediaType.Image:
                    mediaUris.Add(media.Images.MaxBy(im => im.Width * im.Height)!.Uri);
                    break;
                case InstaMediaType.Video:
                    mediaUris.Add(media.Videos.MaxBy(im => im.Width * im.Height)!.Uri);
                    break;
                case InstaMediaType.Carousel:
                    ArrayList carouselMedias = CarouselHelper.GetCarouselMedias(media.Carousel);
                    foreach (object carouselMedia in carouselMedias)
                    {
                        if (carouselMedia is InstaVideo video)
                        {
                            mediaUris.Add(video.Uri);
                        }
                        else
                        {
                            mediaUris.Add(((InstaImage)carouselMedia).Uri);
                        }
                    }
                    break;
            }

            return mediaUris;
        }
    }
}