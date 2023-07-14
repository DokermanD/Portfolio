using Newtonsoft.Json.Linq;
using RpBot_Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HardPlay
{
    internal class HardPlayAuto
    {
        /// <summary>
        /// Отправка ID для HardPlay
        /// </summary>
        /// <returns></returns>
        public static string SendHardPlay()
        {           
            
            //Создаём пост запрос к веб порталу и заполняем заголовки
            var postRequest = new PostRequest("https://*************************")
            {
                Data = $"{{\"accountIds\":[{GettingAccountData.idStr}],\"hard\":true}}",
                Accept = "application/json",
                Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.160 YaBrowser/22.5.3.684 Yowser/2.5 Safari/537.36",
                ContentType = "application/json",
                Referer = "https://******************/",
                Host = "**********************"
            };


            //Добавляем нестандартные загаловки
            postRequest.Headers.Add("Authorization", "Bearer " + GetTokenServer.Token);
            postRequest.Headers.Add("Origin", "https://***************");
            postRequest.Headers.Add("sec-ch-ua", "\" Not A; Brand\";v=\"99\", \"Chromium\";v=\"100\", \"Yandex\";v=\"22\"");
            postRequest.Headers.Add("sec-ch-ua-mobile", "?0");
            postRequest.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            postRequest.Headers.Add("Sec-Fetch-Dest", "empty");
            postRequest.Headers.Add("Sec-Fetch-Mode", "cors");
            postRequest.Headers.Add("Sec-Fetch-Site", "same-site");

            int counActiv;
            try
            {
                //Отправляем запрос
                postRequest.Run(GetTokenServer.cookieContainer);
                //Получаем ответ от сервера в json и парсим токен
                var response = postRequest.Response;
                counActiv = response.Count();
                var json = JObject.Parse(response);
            }
            catch (Exception e)
            {                
                return $"Ошибка при отправке данных на HardPlay!";
            }

            return $"Отправил {counActiv} на Hard Play ";
        }
    }
}
