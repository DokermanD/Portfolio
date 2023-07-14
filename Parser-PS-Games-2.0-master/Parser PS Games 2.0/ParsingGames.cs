using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Parser_PS_Games_2._0
{
    internal class ParsingGames :ErrorLog
    {
        //Блокировка списков
        object LockListProxy = new object();//Блочим лист с прокси
        object LockListLink = new object();//Блочим лист с ссылками
        object LockListSave = new object();//Блочим лист для сохранения результата

        //Список с ссылками на игры
        public List<string> LinksGames = new List<string>();
        //Список на валидные прокси
        public List<string> ProxyListValid = new List<string>();
        //Список результатов парсинга
        public List<string> rezaltParsing = new List<string>();

        //Переменные
        public int colPotok = 0;    

        //Переменные
        public int сloseThread = 0;
        int threadName = 0;
        public async void ParssAllGames(LinksPages linksPages, ChekingProxy chekingProxy, string _colPotok) 
        {
            //Обнуляем количество закрывшихся потокав
            сloseThread = 0;
            //Получаем ссылки на игры
            LinksGames = linksPages.HomePageLinksGames;
            //Получаем список с валидными прокси
            ProxyListValid = linksPages.ProxyListValidNew;
            //Чистим от пустых строк
            ProxyListValid.RemoveAll(x => x == string.Empty || x == null);
            LinksGames.RemoveAll(x => x == string.Empty || x == null);
            colPotok = Convert.ToInt32(_colPotok);

            //Логирование
            ErrorLogSeve("ParsingGames", $"Количество игр для парсинга - {LinksGames.Count}");

            //Начала работы с потоками
            for (int i = 0; i < colPotok; i++)
            {
                //-------------------------------------------------------------------------------- Запуск потоков
                //Пауза перед запуском потока
                //await Task.Delay(5000);

                //Запуск основных потоков в работу
                new Thread(async () =>
                {
                    HtmlAgilityPack.HtmlDocument doc = null;
                    HtmlWeb web = new HtmlWeb();
                    Thread currentThread = Thread.CurrentThread;//Получаем текущий поток
                    currentThread.Name = $"Поток-№{threadName++}";
                    ErrorLogSeve($"Запущен {currentThread.Name}","");

                    //Переменные
                    string proxyStr = string.Empty; //Строка для проки
                    string linkStr = string.Empty; // Строка для ссылки на страницу игры
                    string idition = null;//Строка с эдишенами через разделитель
                    int schetEd = 0; //Счётчик количества эдишенов
                    bool stepNext = true;//Выполняем или нет следующий шаг
                    bool parssMain = false;//Нужно ли парсить с главной карточки
                    bool errorPars = true;//При ошибке парсинга пропускаем страницу

                    //Получаем прокси
                    lock (LockListProxy)
                    {
                        proxyStr = ProxyListValid[0];//Берём строку
                        ProxyListValid.RemoveAt(0);//Удаляем строку из списка
                        ProxyListValid.Add(proxyStr);
                    }

                    // Начало парсинга
                    while (true)
                    {
                        await Task.Delay(1000);//Устанавливаем время отправки запроса
                        //Организация доступа к блокам выполнения
                        stepNext = true;
                        errorPars = true;
                        parssMain = false;

                        //Парсим по 1 странице кадым потокам с 1 прокси 
                        if (LinksGames.Count > 0)
                        {
                            //------------------------------------------------------------------------------------------- БЛОК 1 - Загрузка страницы и проверка на бан
                            lock (LockListLink)
                            {
                                linkStr = LinksGames[0];//Берём строку
                                LinksGames.RemoveAt(0);//Удаляем строку из списка
                            }
                            //Запрос страницы с игрой и проверка на бан прокси
                            try
                            {
                                //Запрвшивае страницу с игрой
                                doc = web.Load(linkStr.Split('|')[1], proxyStr.Split(':')[0], Convert.ToInt32(proxyStr.Split(':')[1]), proxyStr.Split(':')[2], proxyStr.Split(':')[3]);

                                //Проверка на бан прокси
                                var banPrixy = doc.DocumentNode.SelectSingleNode("//h1").InnerText;
                                if (banPrixy == "Access Denied")
                                {
                                    ErrorLogSeve($"Прокси {proxyStr} получил бан!", $"{ currentThread.Name} завершил работу.");
                                    сloseThread++;
                                    break;
                                }

                            }
                            catch (Exception ex)
                            {
                                lock (LockListLink)
                                {
                                    LinksGames.Add(linkStr);//Кладём строку обратно в список
                                }

                                //Описываем ошибку
                                var errorMessage = $"Не загрузилась страница с прокси Блок -1 {proxyStr}";
                                stepNext = false;//Блокируем дальнейшее выполнение шагов

                                //Логирование ошибок 
                                ErrorLogSeve(errorMessage, ex.Message);
                                await Task.Delay(10000);//Ждём 10 секунд и пробуем ещё раз
                            }


                            //------------------------------------------------------------------------------------------- БЛОК 2 - Сбор Эдишен данных
                            if (stepNext)
                            {
                                //Парсим Editions если они есть
                                ParsEditions();

                                if (parssMain)
                                {
                                    //Парсим главную карточку если нет Editions
                                    ParsMain();
                                }
                            }
                        }
                        else break;
                    }
                    //Метод парсинга Editions
                    void ParsEditions()
                    {
                        //Парсим Эдишены
                        var editionColection = doc.DocumentNode.SelectNodes(@"//article");

                        //Если нет Эдишинов переходим к методу ParsMain() 
                        if (editionColection != null)
                        {
                            //Перебераем колекцию и вытаскиваем нужные данные (Название Тип консоли и цену Edition)
                            foreach (var edition in editionColection)
                            {
                                try
                                {
                                    var priceEdition = string.Empty;//Переманная для цены
                                    string consolуTypуEd = string.Empty;//Тип консоли с Эдишен карточки
                                    var nameEdition = string.Empty;//Имя Эдишена

                                    //Берём название ---------------------------------------------------------------------
                                    nameEdition = edition.SelectSingleNode(".//h3").InnerText.Replace("|","");

                                    //Берём тип консоли - Тащим коллекцию спанов -----------------------------------------
                                    var consolуTypуEdColection = edition.SelectNodes(".//div[@class='psw-l-space-x-2 psw-l-line-center psw-p-t-5']//span");//Вытаскиваем все спаны

                                    if (consolуTypуEdColection != null)
                                    {
                                        //Пакуем спаны в одну строку с разделителем
                                        foreach (var item in consolуTypуEdColection)
                                        {
                                            if (item.InnerText.Length == 3)
                                            {
                                                consolуTypуEd += item.InnerText.Trim() + "/";
                                            }
                                        }

                                        //Чистим от последнего слеша строку
                                        consolуTypуEd = consolуTypуEd.TrimEnd('/');
                                    }
                                    else
                                    {
                                        //Берём тип консоли с главной карточки
                                        consolуTypуEd = TypeConsoleMain();
                                    }

                                    //Берём цену -------------------------------------------------------------------------
                                    if (edition.SelectSingleNode(".//span[@class='psw-t-title-m']") == null)
                                    {
                                        priceEdition = edition.SelectSingleNode(".//span[@class='psw-t-title-m psw-m-r-4']").InnerText;
                                    }
                                    else priceEdition = edition.SelectSingleNode(".//span[@class='psw-t-title-m']").InnerText;

                                    //Берём картинку ---------------------------------------------------------------------
                                    var imageLinkEdition = edition.SelectSingleNode(".//img[@class='psw-center psw-l-fit-contain']").GetAttributeValue("src", "Нет ссылки на картинку");

                                    //Покуем строки ----------------------------------------------------------------------
                                    idition += $@"{linkStr.Split('|')[1]}|{linkStr.Split('|')[0]} {nameEdition.Replace("&quot;", " ").Replace("&#x27;", "`").Replace("&amp;", " ")}|{consolуTypуEd}|{priceEdition}|{imageLinkEdition}^";
                                    //                ссылка на игу           имя игры                     название Editions                                                           тип консоли       цена      ссылка на картинку
                                }
                                catch (Exception ex)
                                {
                                    var errorMassege = $"Ошибка парсинга (Edition) на странице Блок-2:{Environment.NewLine}{linkStr.Split(';')[1]}";

                                    //Логирование ошибок
                                    ErrorLogSeve(errorMassege, ex.Message);
                                    errorPars = false;
                                    stepNext = false;//Блокируем дальнейшее выполнение шагов
                                }
                            }
                            if (errorPars)//Если была ошибка пропускааем этот блок
                            {
                                //Чистка от последнего разделителя
                                idition = idition.TrimEnd('^');

                                //Подсчёт количества эдишенов
                                foreach (char item in idition)
                                {
                                    if (item == '^') schetEd++;
                                }

                                lock (LockListSave)
                                {
                                    //Пакуем в таблицу результаты парсинга
                                    for (int b = 0; b < schetEd + 1; b++)
                                    {
                                        rezaltParsing.Add($@"{idition.Split('^')[b]}");
                                    }

                                    idition = string.Empty;
                                    schetEd = 0;
                                }
                            }
                        }
                        else
                        {
                            parssMain = true;
                        }
                    }
                    //Метод парсинга главной карточки
                    void ParsMain()
                    {
                        //Переменные
                        var consolуTypуMain = string.Empty;
                        var priceMain = string.Empty;   

                        //Парсим главную часть
                        var main = doc.DocumentNode.SelectSingleNode(@"//div[@class='psw-c-bg-card-1 psw-p-y-7 psw-p-x-8 psw-m-sub-x-8 psw-m-sub-x-6@below-tablet-s psw-p-x-6@below-tablet-s']");

                        if (main != null)//Проверка на null
                        {
                            try
                            {
                                //Проверка есть ли значение в спан
                                if (main.SelectNodes(".//div[@class='psw-l-space-x-2 psw-l-line-left psw-m-t-4']//span") == null)
                                {
                                    consolуTypуMain = "-";
                                }
                                else
                                {
                                    consolуTypуMain = TypeConsoleMain();
                                   
                                }

                                //Берём цену
                                if (main.SelectSingleNode(".//div[@class='psw-m-t-7']//span[@class='psw-t-title-m']") == null)
                                {
                                    if (main.SelectSingleNode(".//div[@class='psw-m-t-7']//span[@class='psw-t-title-m psw-m-r-4']") == null)
                                    {
                                        priceMain = "-";
                                    }
                                    else priceMain = main.SelectSingleNode(".//div[@class='psw-m-t-7']//span[@class='psw-t-title-m psw-m-r-4']").InnerText;
                                }
                                else priceMain = main.SelectSingleNode(".//div[@class='psw-m-t-7']//span[@class='psw-t-title-m']").InnerText;

                                try
                                {
                                    //Покуем строки ----------------------------------------------------------------------
                                    idition += $@"{linkStr.Split('|')[1]}|{linkStr.Split('|')[0]}|{consolуTypуMain}|{priceMain}|{linksPages.imageAllGames[linkStr.Split('|')[0]]}";
                                    //             ссылка на игру             имя игры               тип консоли       цена
                                }
                                catch (Exception)
                                {
                                    //Покуем строки ----------------------------------------------------------------------
                                    idition += $@"{linkStr.Split('|')[1]}|{linkStr.Split('|')[0]}|{consolуTypуMain}|{priceMain}|Ошибка!";
                                    //             ссылка на игру             имя игры               тип консоли       цена

                                }


                                lock (LockListSave)
                                {
                                    //Пакуем в таблицу результаты парсинга
                                    rezaltParsing.Add($@"{idition}");

                                    idition = string.Empty;
                                }

                            }
                            catch (Exception ex)
                            {
                                var errorMassege = $"Ошибка парсинга на странице Блок-2:{Environment.NewLine}{linkStr.Split(';')[1]}";
                                //Логирование ошибок
                                ErrorLogSeve(errorMassege, ex.Message);
                            }
                        }
                    }
                    //Метод берёт тип консоли с главной карточки если нет в Эдишене
                    string TypeConsoleMain()
                    {
                        //Берём тип консоли - Тащим коллекцию спанов -----------------------------------------
                        var consolуTypуCol = doc.DocumentNode.SelectSingleNode(@"//div[@class='psw-c-bg-card-1 psw-p-y-7 psw-p-x-8 psw-m-sub-x-8 psw-m-sub-x-6@below-tablet-s psw-p-x-6@below-tablet-s']");
                        var consolуTypу = string.Empty;

                        if (consolуTypуCol != null)
                        {
                            var consolуTypуMain = consolуTypуCol.SelectNodes(@".//div[@class='psw-l-space-x-2 psw-l-line-left psw-m-t-4']//span");
                            //Пакуем спаны в одну строку с разделителем только PS4 и PS5 те что больше 3 символов не берём
                            foreach (var item in consolуTypуMain)
                            {
                                if (item.InnerText.Length == 3) 
                                {
                                    consolуTypу += item.InnerText.Trim() + "/";
                                }
                            }

                            //Чистим от последнего слеша строку
                            consolуTypу = consolуTypу.TrimEnd('/');
                        }

                        return consolуTypу;
                    }
                }).Start();
            }
        }
    }
}
