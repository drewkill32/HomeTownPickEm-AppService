using System;
using System.Linq;

namespace HomeTownPickEm.Utils
{
    public static class LogoHelper
    {
        public static string GetSingleLogo(string logos)
        {
            return (logos.Split(';', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()
                    ?? "https://placehold.jp/50x50.png").Replace("http://", "https://");
        }
    }
}