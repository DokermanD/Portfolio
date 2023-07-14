﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RpBot_Request
{
    public class DeleteRequest
    {
        HttpWebRequest _request;
        string _adress;


        //Словaрь для заголовков в запросы
        public Dictionary<string, string> Headers { get; set; }
        public string Response { get; set; }

        //Стандартные заголовки
        public string Accept { get; set; }
        public string Host { get; set; }
        public string Data { get; set; }
        public string Referer { get; set; }
        public string Useragent { get; set; }
        public string ContentType { get; set; }
        public string Authorization { get; set; }
        //public string Connection { get; set; }
        //Прокси Fiddler (для просмотра в фидлере)
        //public WebProxy Proxy { get; set; }

        //Конструктор класса
        public DeleteRequest(string adress)
        {

            _adress = adress;
            Headers = new Dictionary<string, string>();
        }

        public async void Run(CookieContainer cookieContainer)
        {
            _request = (HttpWebRequest)WebRequest.Create(_adress);
            _request.Method = "DELETE";
            _request.Timeout = 600000;
            _request.CookieContainer = cookieContainer;
            //_request.Proxy = Proxy;
            _request.Accept = Accept;
            _request.Host = Host;
            _request.Referer = Referer;
            _request.UserAgent = Useragent;
            _request.ContentType = ContentType;


            foreach (var pair in Headers)
            {
                _request.Headers.Add(pair.Key, pair.Value);
            }

            //Кодируем передаваемые данные и передаём длинну строки
            byte[] sentData = Encoding.UTF8.GetBytes(Data);
            _request.ContentLength = sentData.Length;

            //Добавляем данные в запрос
            Stream sendStream = _request.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();

            try
            {
                //Получаем ответ от сервера
                HttpWebResponse response = (HttpWebResponse)_request.GetResponse();
                var stream = response.GetResponseStream();

                //Проверка что ответ от сервера получен и пишим его в Response
                if (stream != null) Response = new StreamReader(stream).ReadToEnd();
                response.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                await Task.Delay(200);
            }


        }
    }
}
