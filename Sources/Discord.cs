namespace StalcraftClanManager
{
    using System;
    using System.Net.Http;
    using System.Text.Json.Nodes;
    using System.Text.Json;
    using System.IO;
    using System.Net.Http.Json;
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

        public static async void Send(string webHook, string Text)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                HttpClient hClient = new HttpClient(handler);

                JsonObject json = new JsonObject
                {
                    { "content", Text }
                };

                JsonContent js = JsonContent.Create(json);

                await hClient.PostAsync(new Uri(webHook), js);
            }
        }

        public static async void SendFile(string webHook)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                HttpClient hClient = new HttpClient(handler);

                using var file = File.Open(Path.Combine(Environment.CurrentDirectory, "Players.cfg"), FileMode.Open);
                MultipartFormDataContent mpC = new MultipartFormDataContent
                {
                    { new StreamContent(file), "img", Path.GetFileName(file.Name) },
                    { new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(string.Concat("Таблица на: `", DateTime.Now, "`")))), "content" }
                };

                await hClient.PostAsync(new Uri(webHook), mpC);
            }
        }
    }
}
