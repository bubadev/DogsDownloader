using System.IO;
using System.Net;
using System.Drawing;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using DogDownloader;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Diagnostics;

namespace ImageDownloader
{
    internal class Program
    {
        static HttpClient client = new HttpClient(); 
        static async Task Main(string[] args)
        {
            using(var dogClient  = new DogClient())
            {
                var url = await dogClient.GetRandomDogImageUrlAsync();

                
                var bytearray = await GetImageAsync(url);

                // Путь прописан руками для скорости, не успевал причесать код
                var filePath = "C:/Projects/DogsDownloader/ImageDownloader/ImageDownloader/Images/dog.jpg";
                File.WriteAllBytes(filePath, bytearray);
                Process.Start(new ProcessStartInfo()
                {
                    FileName = filePath,
                    UseShellExecute = true
                });

                var breed = DogClient.GetBreedFromUrl(url);

                var urls = await dogClient.GetDogImageUrlByBreedAsync(breed, 3);

                for (int i = 0; i < urls.Length; i++)
                {
                    var imageBytes = await GetImageAsync(url);
                    var imagePath = $"C:/Projects/DogsDownloader/ImageDownloader/ImageDownloader/Images/dog{i}.jpg";
                    File.WriteAllBytes(imagePath, imageBytes);
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = imagePath,
                        UseShellExecute = true
                    });
                }
            }
            Console.ReadKey();
        }
        static async Task<byte[]> GetImageAsync(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}