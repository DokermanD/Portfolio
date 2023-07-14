using Newtonsoft.Json.Linq;
using RpBot_Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sing_in_by_link
{
    public class SetCodeApi
    {
        //Счётчик отправленых акаунтов на сайн
        static int schet = 0;
        public static void SetCode()
        {
            int Id = 0;
            string code = string.Empty;
            for (int i = schet; i < GetFileString.filesCode.Count; i++)
            {
                try
                {
                    //Console.WriteLine($"Vzial {GetFileString.filesCode.Count} links na otpravku.");
                    schet++;
                    //Получаем из строки id и code
                    Id = Convert.ToInt32(GetFileString.filesCode[i].Split(';')[1]);
                    code = new Regex(@"code=\w+").Match(GetFileString.filesCode[i]).ToString().Replace("code=", "");

                    //Если кон получен, отправляем запрос на сервер по одному
                    if (code != "")
                    {
                        //Создаём пост запрос к веб порталу и заполняем заголовки
                        var postRequest = new PostRequest("https://api.rpbot.pro/accounts/sign/in/link")
                        {
                            Data = $"{{\"accountId\":{Id},\"code\":\"{code}\"}}",
                            Accept = "application/json",
                            Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.160 YaBrowser/22.5.3.684 Yowser/2.5 Safari/537.36",
                            ContentType = "application/json",
                            Referer = "https://app.rpbot.pro/",
                            Host = "api.rpbot.pro"
                        };


                        //Добавляем нестандартные загаловки
                        postRequest.Headers.Add("Authorization", $"Bearer {Token.TokenProg}");
                        postRequest.Headers.Add("Origin", "https://app.rpbot.pro");
                        postRequest.Headers.Add("sec-ch-ua", "\" Not A; Brand\";v=\"99\", \"Chromium\";v=\"100\", \"Yandex\";v=\"22\"");
                        postRequest.Headers.Add("sec-ch-ua-mobile", "?0");
                        postRequest.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                        postRequest.Headers.Add("Sec-Fetch-Dest", "empty");
                        postRequest.Headers.Add("Sec-Fetch-Mode", "cors");
                        postRequest.Headers.Add("Sec-Fetch-Site", "same-site");

                        var error = string.Empty;
                        try
                        {
                            //Отправляем запрос
                            postRequest.Run(Token.cookieContainer);
                            //Получаем ответ от сервера в json и парсим токен
                            var response = postRequest.Response;
                            //var json = JObject.Parse(response);

                            //error = json["error"].ToString();

                            Console.WriteLine($"Otpravil code na sain - №{schet} id - {Id} code - {code}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Oshibka pri otpravke! - №{schet} ({Id}:{code})");
                        }
                    }
                }
                catch (Exception)
                {
                }
                
            }

            //Чистим таблицу с сылками
            GetFileString.filesCode.Clear();
        }
    }
}
