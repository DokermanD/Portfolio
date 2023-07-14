using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Parser_PS_Games_2._0
{
    internal class LinksPages : ErrorLog
    {
        HtmlAgilityPack.HtmlDocument doc = null;
        HtmlWeb web = new HtmlWeb();
        
        object LockListProxy = new object();//Блочим лист с прокси
        object LockListLink = new object();//Блочим лист с прокси
        object LockListSave = new object();//Блочим лист для сохранения результата

        //Словарь для хранения Имени игры и ссылки на картинку
        public Dictionary<string, string> imageAllGames= new Dictionary<string, string>();

        //Список ссылок на страницы
        public List<string> ListPageLinks = new List<string>();
        //Список ссылок на страницы
        public List<string> ListProxyBan = new List<string>();
        //Список ссылок на страницы
        public List<string> ProxyListValidNew = new List<string>();
        //Список ссылок на страницу игры
        public List<string> HomePageLinksGames = new List<string>();

        //public string ErrorGetPages { get; set; } // Заносим сюда ошибки загрузок страниц
        public int TotalPages { get; set; }// Заносим сюда количество страниц на сайте

        //Переменные
        public int colPotok = 0;
        public int threadStopMonitor = 0;//Переменная для мониторинга количества потоков

        int colPage = 0;
        string proxyStr = string.Empty;
            


        //Парсим все страницы с играми
        public void ParsingLinkPage(List<string> ProxyListValid)
        {
            //Расчитываем количество потоков и формируем список ссылок на страницы
            ColThredParsing(ProxyListValid);

            //Парсим ссылки на игры со всех страниц сайта
            ParsingGamesLinksPage(ProxyListValidNew);

            //Логирование
            ErrorLogSeve("LinksPages", $"Количество страниц для парсинга  игр - {TotalPages}");

        }

        //**************************************************************************************************** Методы

        //Расчитываем количество потоков и формируем список страниц
        public void ColThredParsing(List<string> _proxyListValid)
        {
            for (int i = 0; i < _proxyListValid.Count(); i++)
            {
                //Получаем прокси
                proxyStr = _proxyListValid[0];//Берём строку
                _proxyListValid.RemoveAt(0);//Удаляем строку из списка
                _proxyListValid.Add(proxyStr);

                try
                {
                    //Получаем последний номер страницы
                    var doc = web.Load(@"https://store.playstation.com/en-tr/pages/browse/1", proxyStr.Split(':')[0], Convert.ToInt32(proxyStr.Split(':')[1]), proxyStr.Split(':')[2], proxyStr.Split(':')[3]);
                    //Проверка на бан прокси
                    var banPrixy = doc.DocumentNode.SelectSingleNode("//h1").InnerText;
                    if (banPrixy == "Access Denied")
                    {
                        ListProxyBan.Add(proxyStr);
                        continue;
                    }

                    //Получаем 5 элемент с номерам всех страниц сайта
                    var rezalt = doc.DocumentNode.SelectSingleNode("//ol/li[5]/button/span").InnerText;

                    //Берём максимальное число из коллекци переводим в int
                    colPage = Convert.ToInt32(rezalt);
                }
                catch (Exception ex)
                {

                    ErrorLogSeve($"Ошибка подключения по прокси {proxyStr}", ex.Message);
                    continue;
                }
                

                //Формируем список для парсинга
                for (int a = 1; a < colPage + 1; a++) ListPageLinks.Add($"https://store.playstation.com/en-tr/pages/browse/{a}");
                //Передаём свойству количество страниц для парсинга (для формы)
                TotalPages = ListPageLinks.Count;
                break;
            }

            //Проверка есть ли баны и если есть чистим список с валидными прокси
            if (ListProxyBan.Count > 0)
            {
                foreach (var item in _proxyListValid)
                {
                    var schet = true;
                    for (int i = 0; i < ListProxyBan.Count; i++)
                    {
                        if (item == _proxyListValid[i])
                        {
                            schet = false;
                            break;
                        }
                    }
                    if (schet)
                    {
                        ProxyListValidNew.Add(item);
                    }
                }
            }
            else
            {
                ProxyListValidNew = _proxyListValid;
            }

            //Получаем количество потоков
            colPotok = ProxyListValidNew.Count();
        }

        //Парсим количество страниц
        public void ParsingGamesLinksPage(List<string> _proxyListValid)
        {
            //Создания и запуск потоков
            for (int i = 0; i < colPotok; i++)
            {
                new Thread(async () =>
                {
                    await Task.Delay(100);
                    //Переменные
                    string linkPage = string.Empty;
                    string errorMessage = string.Empty;
                    int threadStop = 0;
                    

                    //Получаем прокси
                    lock (LockListProxy)
                    {
                        proxyStr = _proxyListValid[0];//Берём строку
                        _proxyListValid.RemoveAt(0);//Удаляем строку из списка
                        //_proxyListValid.Add(proxyStr);
                    }
                    //Основной цикл парсинга страниц
                    while (true)
                    {
                        //Переменные
                        bool error = true;//Блокировка дальнейшего выполнения при получении ошибки
                        //Устанавливаем время отправки запроса
                        await Task.Delay(300);

                        if (ListPageLinks.Count > 0)//Проверка списка на наличие ссылок
                        {
                            //********************************************************************** Блок 1
                            lock (LockListLink)
                            {
                                linkPage = ListPageLinks[0];//Берём строку
                                ListPageLinks.RemoveAt(0);//Удаляем строку из списка
                            }
                            
                            try
                            {
                                //Запрвшивае основную с играми HTML страницу переберая все
                                doc = web.Load(linkPage, proxyStr.Split(':')[0], Convert.ToInt32(proxyStr.Split(':')[1]), proxyStr.Split(':')[2], proxyStr.Split(':')[3]);

                                //Проверка на бан или не получение всей страницы
                                if (doc != null)
                                {
                                    if (doc.DocumentNode.SelectSingleNode("//h1").InnerText == "Access Denied")
                                    {
                                        ErrorLogSeve("Прокси был забанен", proxyStr);//Логируем ошибку в текстовый фаил
                                        break;//Выходим из цикла и завершается поток
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                threadStop++;

                                lock (LockListLink)
                                {
                                    ListPageLinks.Add(linkPage);//Кладём строку обратно в список если не получилось загрузить страницу
                                }
                                //Проверка на выход потока
                                if (threadStop >= 10)
                                {
                                    threadStopMonitor++;
                                    break;
                                }

                                errorMessage = $"Не загрузилась страница с прокси {proxyStr}" + Environment.NewLine;
                                await Task.Delay(6000);
                                error = false;
                                //Логирование
                                ErrorLogSeve(errorMessage, ex.Message);//Логируем ошибку в текстовый фаил
                            }

                            //********************************************************************** Блок 2

                            if (error)
                            {
                                var homePage = doc.DocumentNode.SelectNodes("//ul[@class='psw-grid-list psw-l-grid']/li/div/a");

                                //Вытаскиваем ссылку на игру и название
                                if (homePage != null)
                                {
                                    foreach (var item in homePage)
                                    {
                                        try
                                        {
                                            //Вытаскиваем ссылку на игру ссылку на картинку и название
                                            var link = item.GetAttributeValue("href", "--");
                                            var title = item.SelectSingleNode(@".//section[@class='psw-product-tile__details psw-m-t-2']/span[2]").InnerText;
                                            var imageLink = item.SelectSingleNode(@".//img").GetAttributeValue("src", "");
                                            //Чистим ссылку картинки от хвоста
                                            imageLink = Regex.Match(imageLink, "https://.*(\\.png|\\.jpg)").Value;

                                            //Собераем библиотеку Ключ:название игры Значение:ссылка на картинку
                                            //Проверка на дубли
                                            lock (LockListSave)
                                            {
                                                if (!imageAllGames.ContainsKey(title))
                                                {
                                                    imageAllGames.Add(title.Replace("&quot;", " ").Replace("&#x27;", "`").Replace("&amp;", " "), imageLink);
                                                }
                                            }
                                            
                                            //Добавляем в начало адрес сайта
                                            var fullLink = $@"{title.Replace("&quot;", " ").Replace("&#x27;", "`").Replace("&amp;", " ")}|https://store.playstation.com{link}";

                                            lock (LockListSave)
                                            {
                                                //Кладём в список homePageList
                                                HomePageLinksGames.Add(fullLink.Replace("&quot;", " ").Replace("&#x27;", "`").Replace("&amp;", " "));
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            errorMessage = $"Ошибка парсинга игр с общей страницы. (LinksPages)" + Environment.NewLine;
                                            //Логирование
                                            ErrorLogSeve(errorMessage, ex.Message);//Логируем ошибку в текстовый фаил

                                        }
                                    }
                                }
                            }
                        }
                        else 
                        {
                            lock (LockListProxy)
                            {
                                _proxyListValid.Add(proxyStr);
                                threadStopMonitor++;
                                break;//Если список пуст выходим из цикла
                            }
                        } 
                            
                    }

                }).Start();
            }
        }
    }
}
