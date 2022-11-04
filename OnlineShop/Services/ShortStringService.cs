namespace OnlineShop.Services
{
    public class ShortStringService : IShortStringService
    {
        public string GetShortString(string text)
        {
            if (text.Length > 20)
            {
                return text.Substring(0, 20) + "...";
            }
            return text;
        }
    }
}
