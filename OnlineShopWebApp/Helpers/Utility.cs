namespace OnlineShopWebApp.Helpers
{
    public static class Utility
    {
        public static string GetLasChars(byte[] token)
        {
            return token[7].ToString();
        }
    }
}
