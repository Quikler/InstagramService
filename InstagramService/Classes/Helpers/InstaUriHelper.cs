namespace InstagramService.Classes.Helpers
{
    internal static class InstaUriHelper
    {
        public static string GetPartWithoutQuery(string url)
            => new Uri(url).GetLeftPart(UriPartial.Path);

        public static string GetPartWithoutQuery(Uri uri)
            => uri.GetLeftPart(UriPartial.Path);
    }
}