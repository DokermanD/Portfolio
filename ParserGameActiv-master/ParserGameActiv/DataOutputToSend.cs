using RestSharp;
using System.Text;

namespace ParserGameActiv
{
    internal class DataOutputToSend
    {
        static StringBuilder data = new StringBuilder();
        /// <summary>
        /// Упаковка полученых данных для отправки RivalsToday
        /// </summary>
        /// <returns></returns>
        public static string DataPackagingRivals()
        {
            foreach (var item in GettingAccountData.OnlineGames)
            {
                data.Append($"\"{item.Key}\":{item.Value},");
            }

            return $"{{{data.ToString().Trim(',')}}}";
        }


        /// <summary>
        /// Упаковка полученых данных для отправки GamesToday
        /// </summary>
        /// <returns></returns>
        public static string DataPackagingGamesToday()
        {
            foreach (var item in GettingAccountData.GamesToday)
            {
                data.Append($"\"{item.Key}\":{item.Value},");
            }

            return $"{{{data.ToString().Trim(',')}}}";
        }

        /// <summary>
        /// Отправка на сервер спаршеных данных RivalsToday
        /// </summary>
        /// <returns></returns>
        public static async Task OutputToSendRivalsToday()
        {
            try
            {
                var client = new RestClient("https://***********");
                var request = new RestRequest("******************", Method.Post);

                var data = DataPackagingRivals();
                request.AddBody(data);

                var response = await client.ExecutePostAsync(request);

                if (response.Content.Contains("ok"))
                {
                    var count = response.Content.Split(',')[1].Split(':')[1].Replace("}", "");
                    Form1.statusServerResponse = $"Отправлено - {count}";
                }
                else
                {
                    Form1.statusServerResponse = $"Ошибка!";
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                data.Clear();
            }
        }

        /// <summary>
        /// Отправка на сервер спаршеных данных GamesToday
        /// </summary>
        /// <returns></returns>
        public static async Task OutputToSendGamesToday()
        {
            try
            {
                var client = new RestClient("https://***************");
                var request = new RestRequest("**********************", Method.Post);

                var data = DataPackagingRivals();
                request.AddBody(data);

                var response = await client.ExecutePostAsync(request);

            }
            catch (Exception)
            {

            }
            finally
            {
                data.Clear();
            }
        }
    }
}
