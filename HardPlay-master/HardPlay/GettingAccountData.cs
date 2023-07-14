using Newtonsoft.Json.Linq;
using RestSharp;
using RpBot_Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardPlay
{
    internal class GettingAccountData
    {
        //Список с результатами выборки
        public static List<string> AkkauntsId = new List<string>();
        //Строка для отправки с ID
        public static string idStr = string.Empty;

        /// <summary>
        /// Парсит ID и кладёт в список
        /// </summary>
        /// <returns> Возвращает текущую дату парсинга </returns>
        public static string GetID()
        {
            //Чистим старые данные в списке
            AkkauntsId.Clear();

            for (int i = 0; i < 5; i++)
            {
                //Делаем запросы и кладём результат выборки в словарь
                try
                {
                    var client = new RestClient("https://***************");
                    var request = new RestRequest($"remote/management/accounts?accessToken={GetTokenServer.TokenAPI}", Method.Get);

                    var response = client.ExecuteGet(request);
                    
                    if (response.StatusCode.ToString() == "OK")
                    {
                        var arrId = JArray.Parse(response.Content);

                        SampleData(arrId);
                        break;
                    }
                }
                catch (Exception e)
                {

                }

                //Пауза перед запросом данных
                Task.Delay(1000);
            }
            
         

            //Сборка строки с ID для отправки на сервер
            CollectStringId();

            return DateTime.Now.ToString("dd.MM.yyyy");
        }

        //Выборка данных из ответа сервера и сохранение в словарь
        public static void SampleData(JArray json)
        {
            foreach (var item in json)
            {
                var id = item["id"];

                AkkauntsId.Add(id.ToString());
            }
        }

        public static void CollectStringId()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var item in AkkauntsId)
            {
                stringBuilder.Append($"{item},");
            }

            //Чистим список
            AkkauntsId.Clear();
            //Копируем строку в переменную
            idStr = stringBuilder.ToString().Trim(',');
            //Чистим stringBilder
            stringBuilder.Clear();
        }
    }
}
