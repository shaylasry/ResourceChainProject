using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ResourceChainProject.Interfaces;

namespace ResourceChainProject.Storages
{
    public class WebServiceStorage<T> : IReadableStorage<T>
    {
        private readonly string _url;
        private static readonly HttpClient _http = new();

        public WebServiceStorage(string url)
        {
            _url = url;
        }

        public bool HasValue => true;
        public bool IsExpired => false;

        public async Task<T?> Read()
        {
            try
            {
                var response = await _http.GetAsync(_url);
                if (!response.IsSuccessStatusCode)
                    return default;

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default;
            }
        }
    }
} 