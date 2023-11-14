using System.Data;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace DogDownloader
{
    public class DogClient : IDisposable
    {
        HttpClient _client = new HttpClient();

        public async Task<string> GetRandomDogImageUrlAsync()
        {
            var path = "https://dog.ceo/api/breeds/image/random";

            HttpResponseMessage response = await _client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            var responseModel = await response.Content.ReadFromJsonAsync<ResponseSingleMessageModel>();
            return responseModel.Message;
        }
        public async Task<string[]> GetDogImageUrlByBreedAsync(string breed, int count = 1)
        {
            var path = $"https://dog.ceo/api/breed/{breed}/images/random/{count}";

            HttpResponseMessage response = await _client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            var responseModel = await response.Content.ReadFromJsonAsync<ResponseManyMessageModel>();
            return responseModel.Message;
        }

        public static string GetBreedFromUrl(string url)
        {
            Regex regex = new Regex(@"https://images.dog.ceo/breeds/(\S+)/\S*");
            MatchCollection matches = regex.Matches(url);
            return matches.FirstOrDefault().Groups[1].Value;
        }

        #region Dispose
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}