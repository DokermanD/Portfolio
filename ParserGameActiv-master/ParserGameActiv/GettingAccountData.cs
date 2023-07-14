using Newtonsoft.Json.Linq;
using RestSharp;
using System.Text;

namespace ParserGameActiv
{
    internal class GettingAccountData
    {
        //Словарь с результатами выборки
        public static Dictionary<string, int> OnlineGames = new();
        public static Dictionary<string, int> GamesToday = new();

        /// <summary>
        /// Парсит данные и кладёт в словарь
        /// </summary>
        /// <returns> Возвращает текущую дату парсинга </returns>
        public static string GetData()
        {
            //Чистим старые данные в словаре
            OnlineGames.Clear();
            GamesToday.Clear();

            //Три попытки спарсить данные с сервака
            for (int i = 0; i < 5; i++)
            {
                //Делаем запросы и кладём результат выборки в словарь
                try
                {
                    var client = new RestClient("https://ykserver.host/RpBotMiddleware");
                    var request = new RestRequest($"remote/management/accounts?accessToken={Form1.TokenAPI}", Method.Get);

                    var response = client.Get(request);

                    //Проверка ответа сервера на null
                    if (response.Content != null)
                    {
                        var job = JArray.Parse(response.Content);
                                 
                        //Обработка 
                        SampleData(job);
                        break;
                    }
                    
                }
                catch (Exception e)
                {
                    //Сохранение ошибки парсинга
                    saveErrors(e.Message.ToString());
                }

                //Пауза перед следующим запросам
                Task.Delay(1000);
            }
            
            return DateTime.Now.ToString("dd.MM.yyyy");
        }

        //Выборка данных из ответа сервера и сохранение в словарь
        public static void SampleData(JArray json)
        {
            foreach (var item in json)
            {
                var email = item["email"];
                var rivalsToday = item["rivalsToday"];
                var gamesToday = item["gamesToday"];

                if (!OnlineGames.ContainsKey(email.ToString()))
                {
                    OnlineGames.Add(Convert.ToString(email), Convert.ToInt32(rivalsToday));
                }
                if (!GamesToday.ContainsKey(email.ToString()))
                {
                    GamesToday.Add(Convert.ToString(email), Convert.ToInt32(gamesToday));
                }
            }
        }

        public static void saveErrors(string error)
        {
            //Сохроняем данные в txt документ Errors
            using (FileStream fs = new FileStream($@"./DataOnline/Errors.txt", FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
               sw.Write($"{DateTime.UtcNow.AddHours(3)} - {error}");
            }
        }
    }
}
