using InstagramApiSharp.Classes.Models;
using System.Collections;

namespace InstagramService.Classes.Helpers
{
    internal static class CarouselHelper
    {
        private const string IMG_INDEX = "img_index=";

        public static ArrayList GetCarouselMedias(InstaCarousel carouselItems)
        {
            ArrayList arrayList = new();

            foreach (var item in carouselItems)
            {
                // if videos exist then it's 100% video
                if (item.Videos.Count > 0)
                {
                    arrayList.Add(item.Videos.MaxBy(im => im.Width * im.Height));
                }
                else
                {
                    arrayList.Add(item.Images.MaxBy(im => im.Width * im.Height));
                }
            }

            return arrayList;
        }

        public static IReadOnlyList<string> GetCarouselInitialUris(string url, InstaCarousel instaCarouselItems)
        {
            Uri uri = new(url);

            string[] uris = new string[instaCarouselItems.Count];
            string uriWithoutQuery = InstaUriHelper.GetPartWithoutQuery(uri);

            List<string> queries = uri.GetComponents(UriComponents.Query, UriFormat.Unescaped).Split('&').ToList();
            int queriesImgIndexIndex = queries.FindIndex(s => s.Contains(IMG_INDEX));

            if (queriesImgIndexIndex == -1)
            {
                queries.Add(string.Empty);
                queriesImgIndexIndex = queries.Count - 1;
            }

            for (int i = 0; i < instaCarouselItems.Count; i++)
            {
                queries[queriesImgIndexIndex] = IMG_INDEX + (i + 1);
                uris[i] = $"{uriWithoutQuery}?{string.Join('&', queries)}";
            }

            return uris;
        }
    }
}