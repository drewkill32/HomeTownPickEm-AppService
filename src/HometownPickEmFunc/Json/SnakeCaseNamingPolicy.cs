using System;
using System.Text;
using System.Text.Json;

namespace HometownPickEmFunc.Json
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "name cannot be null or empty");
            }

            var sBuilder = new StringBuilder();
            for (var i = 0; i < name.Length; i++)
            {
                if (i == 0)
                {
                    sBuilder.Append(char.ToLower(name[i]));
                }
                else if (char.IsUpper(name[i]))
                {
                    sBuilder.Append('_');
                    sBuilder.Append(char.ToLower(name[i]));
                }
                else
                {
                    sBuilder.Append(name[i]);
                }
            }

            return sBuilder.ToString();
        }
    }
}