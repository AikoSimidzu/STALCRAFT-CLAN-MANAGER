namespace StalcraftClanManager
{
    using System;
    using System.Net.Http;
    using System.Text.Json.Nodes;
    using System.Text.Json;
    using System.IO;
    using System.Net.Http.Json;
    using System.Windows;
    using System.Text;

    internal class Discord
    {        
        public static DiscordData LoadData()
        {
            if(File.Exists(Path.Combine(Environment.CurrentDirectory, "Discord.cfg")))
            {
                return JsonSerializer.Deserialize<DiscordData>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Discord.cfg")));
            }
            else return new DiscordData();
        }

        public static async void Send(string token, string channelID, string Text)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                HttpClient hClient = new HttpClient(handler);

                JsonObject json = new JsonObject
                {
                    { "mobile_network_type", "unknown" },
                    { "content", Text },
                    { "tts", "false" },
                    { "flags", "0" }
                };

                hClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
                hClient.DefaultRequestHeaders.Add("Authorization", token);

                JsonContent js = JsonContent.Create(json);

                Clipboard.SetText(await hClient.PostAsync(new Uri($"https://discord.com/api/v9/channels/{channelID}/messages"), js).Result.Content.ReadAsStringAsync());
            }
        }

        public static async void SendFile(string token, string channelID)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                HttpClient hClient = new HttpClient(handler);                

                hClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
                hClient.DefaultRequestHeaders.Add("Authorization", token);

                using var file = File.Open(Path.Combine(Environment.CurrentDirectory, "Players.cfg"), FileMode.Open);
                MultipartFormDataContent mpC = new MultipartFormDataContent
                {
                    { new StreamContent(file), "img", Path.GetFileName(file.Name) },
                    { new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(string.Concat("Таблица на: `", DateTime.Now, "`")))), "content" }
                };

                Clipboard.SetText(await hClient.PostAsync(new Uri($"https://discord.com/api/v9/channels/{channelID}/messages"), mpC).Result.Content.ReadAsStringAsync());
            }
        }
    }
}
