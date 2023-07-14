using Newtonsoft.Json.Linq;
using RpBot_Request;
using System.Net;

namespace HardPlay
{
    internal class GetTokenServer
    {
        //Здесь храним куки
        public static CookieContainer cookieContainer = new();
        //Здесь храним токен
        public static string Token { get; set; }
        public static string TokenAPI { get; set; }

        /// <summary>
        /// Метод запрашивает токен у сервера и хранит в переменной (Token) во время работы
        /// </summary>
        /// <returns></returns>
        public static void GetToken()
        {
            TokenAPI = "866e9a08-2073-4005-9a78-d74ef367b566";

            //Логин данные
            string login = "coins.for.you22@gmail.com";
            string pass = "YzEm588Um5";


            //Получаем токен
            //Создаём пост запрос к веб порталу и заполняем заголовки
            var postRequest = new PostRequest("https://api.rpbot.pro/auth/login")
            {
                Data = $"{{\"email\":\"{login}\",\"password\":\"{pass}\"}}",
                Accept = "application/json",
                Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.160 YaBrowser/22.5.3.684 Yowser/2.5 Safari/537.36",
                ContentType = "application/json",
                Referer = "https://app.rpbot.pro/",
                Host = "api.rpbot.pro"
            };


            //Добавляем нестандартные загаловки
            postRequest.Headers.Add("Origin", "https://app.rpbot.pro");
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

                Token = json["jwt"].ToString();
            }
            catch (Exception e)
            {
                Form1.timer.Stop();//Стопаем таймер
                //DataOutputToSend.OutputToSend("Ошибка при получении токена!");//Отправка сообщения об ошибке на север
                MessageBox.Show(e.Message, "Ошибка при получении токена!");
            }
        }
    }
}
