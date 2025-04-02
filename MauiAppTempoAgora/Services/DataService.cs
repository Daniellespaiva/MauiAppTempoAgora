using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "7f62a3d87351c324f48320ea7f3792ff";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                $"q={cidade}&units=metric&appid={chave}";






            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    if (rascunho["sys"]?["sunrise"] != null && rascunho["sys"]?["sunset"] != null)
                    {

                        DateTime time = new();
                        DateTime sunrise = time.AddSeconds((double)(rascunho["sys"]?["sunrise"] ?? 0)).ToLocalTime();
                        DateTime sunset = time.AddSeconds((double)(rascunho["sys"]?["sunset"] ?? 0)).ToLocalTime();

                        t = new()
                        {
                            lat = (double)(rascunho["coord"]?["lat"] ?? 0),
                            lon = (double)(rascunho["coord"]?["lon"] ?? 0),
                            description = (string?)(rascunho["weather"]?[0]?["description"] ?? string.Empty),
                            main = (string?)(rascunho["weather"]?[0]?["main"] ?? string.Empty),
                            temp_min = (double)(rascunho["main"]?["temp_min"] ?? 0),
                            temp_max = (double)(rascunho["main"]?["temp_max"] ?? 0),
                            speed = (double)(rascunho["wind"]?["speed"] ?? 0),
                            visibility = (int)(rascunho["visibility"] ?? 0),
                            sunrise = sunrise.ToString(),
                            sunset = sunset.ToString(),
                        };//fecha objeto do tempo

                    }//fecha if se o status do servidor foi de sucesso

                }//fecha laço using

                return t;
            }
        }
    }
}






