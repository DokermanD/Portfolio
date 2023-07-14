using Newtonsoft.Json.Linq;
using RpBot_Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sing_in_by_link
{
    internal class Token
    {
        //Здесь храним куки
        public static CookieContainer cookieContainer = new CookieContainer();
        //Переменные и списки
        public static string TokenProg = string.Empty;//Хранит полученый токен для запросов
        
        /// <summary>
        /// Метод запрашивает токен у сервера и хранит в переменной (TokenProg) во время работы
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            //Логин данные
            string login = "**********************";
            string pass = "***********************";


            //Получаем токен
            //Создаём пост запрос к веб порталу и заполняем заголовки
            var postRequest = new PostRequest("https://**************************")
            {
                Data = $"{{\"email\":\"{login}\",\"password\":\"{pass}\"}}",
                Accept = "application/json",
                Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.160 YaBrowser/22.5.3.684 Yowser/2.5 Safari/537.36",
                ContentType = "*********************",
                Referer = "https://********************/",
                Host = "*******************"
            };


            //Добавляем нестандартные загаловки
            postRequest.Headers.Add("Origin", "https://**************");
            postRequest.Headers.Add("sec-ch-ua", "\" Not A; Brand\";v=\"99\", \"Chromium\";v=\"100\", \"Yandex\";v=\"22\"");
            postRequest.Headers.Add("sec-ch-ua-mobile", "?0");
            postRequest.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            postRequest.Headers.Add("Sec-Fetch-Dest", "empty");
            postRequest.Headers.Add("Sec-Fetch-Mode", "cors");
            postRequest.Headers.Add("Sec-Fetch-Site", "same-site");

            try
            {
                //Отправляем запрос
                postRequest.Run(cookieContainer);
                //Получаем ответ от сервера в json и парсим токен
                var response = postRequest.Response;
                var json = JObject.Parse(response);

                TokenProg = json["jwt"].ToString();
                Console.WriteLine("Poluchil token.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Oshibka pri poluchenii tokenf!");
            }

            return TokenProg;
        }
    }
}
