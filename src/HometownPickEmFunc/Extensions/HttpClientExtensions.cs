using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HometownPickEmFunc.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<TResult> GetFromJsonAsync<TResult>(this HttpClient client, string requestUri,
            JsonSerializerOptions options)
        {
            var result = await client.GetAsync(requestUri);
            var json = await result.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResult>(json, options);
        }
    }
}