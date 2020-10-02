using PaperAPI.Entities;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaperAPI
{
    public class PaperAPI
    {
        static HttpClient client { get; set; } = new HttpClient();

        public async Task<AvailableMCVersions> GetMCVersions()
        {
            var json = await client.GetStringAsync("https://papermc.io/api/v1/paper/");

            AvailableMCVersions availableMCVersions = JsonSerializer.Deserialize<AvailableMCVersions>(json);

            return availableMCVersions;
        }

        public async Task<AvailableBuildVersions> GetBuildVersions(string version)
        {
            var json = await client.GetStringAsync($"https://papermc.io/api/v1/paper/{version}/");

            AvailableBuildVersions availableBuildVersions = JsonSerializer.Deserialize<AvailableBuildVersions>(json);

            return availableBuildVersions;
        }

        public async Task<Stream> DownloadFile(string version, string build)
        {
            var data = await client.GetStreamAsync($"https://papermc.io/api/v1/paper/{version}/{build}/download");
            return data;
        }
    }
}
