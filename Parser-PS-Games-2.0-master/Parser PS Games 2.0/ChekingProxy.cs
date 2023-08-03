using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser_PS_Games_2._0
{
    internal class ChekingProxy : ErrorLog
    {
        HtmlAgilityPack.HtmlDocument doc = null;
        HtmlWeb web = new HtmlWeb();

        // Список для прокси из файла
        public List<string> ProxyList = new List<string>();//Бесплатные прокси
        public List<string> ProxyListFile = new List<string>();//Перебор прокси из файла
        //Список для проверенных прокси с ошибкой
        List<string> ProxyListError = new List<string>();
        //Список для проверенных прокси
        public List<string> ProxyListValid = new List<string>();
        //Список для проверенных прокси
        public List<string> ProxyListBan = new List<string>();
        //Список для проверенных прокси с ошибкой
        public List<string> ShowProxyListError = new List<string>();
        //Список для проверенных прокси с ошибкой
        public List<string> ShowProxyListBan = new List<string>();

        //Свойства для отправки в форму
        public int ValidProxy { get; set; }
        public int ErrorProxy { get; set; }
        public int BanProxy { get; set; }
        public string MessageCheck { get; set; }

        //Блокировка списков
        object LockListProxy = new object();//Блочим лист с прокси
        object Lock = new object();//Блочим лист с ссылками
        object LockListSave = new object();//Блочим лист для сохранения результата

        //Переменные
        private string text = string.Empty;//Ответ по наличию прокси
        private int validProxy = 0;
        private int errorProxy = 0;
        int potokCol = 50; //Переменная задаёт кол-во потоков для чекера прокси

        //Проверка наличия файла и прокси в нём.
        public async Task ChekProxyAsync()
        {
            await Task.Run(() =>
            {
                //Берём прокси из списка если есть с удалением
                if (File.Exists(@"./Proxy.txt"))
                {
                    ProxyListFile = File.ReadAllLines(@"./Proxy.txt").ToList();
                    int schet = 0;
                    foreach (var item in ProxyListFile)
                    {
                        foreach (char sim in item)
                        {
                            if (sim == ':') schet++;
                        }
                        if (schet == 1)
                        {
                            ProxyList.Add(item + "::");
                        }
                        else if (schet == 2)
                        {
                            ProxyList.Add(item + ":");
                        }
                        else if (schet == 3)
                        {
                            ProxyList.Add(item);
                        }
                        schet = 0;
                    }
                }
            });
        }

        //Чекаем прокси на валидность
        public async Task ChekProxyValidAsync(List<string> proxyList)
        {
            string linkValid = @"https://store.playstation.com/en-tr/pages/browse/1";

            //Переменные
            int threadStop = 0;
                           
            //Первая проверка прокси на валидность
            for (int i = 0; i < potokCol; i++)
            {
                new Thread(async () =>
                {
                    HtmlAgilityPack.HtmlDocument doc = null;
                    HtmlWeb web = new HtmlWeb();

                    var proxy = string.Empty;
                    //await Task.Delay(100);

                    while (true)
                    {
                        //Проверка наличия прокси в списке
                        lock (LockListProxy)
                        {
                            if (proxyList.Count == 0)
                            {
                                threadStop++;
                                break;
                            }
                                
                            //Берём прокси с удалением
                            proxy = proxyList[0];
                            proxyList.RemoveAt(0);
                        }
                        try
                        {
                            //Грузим первую страницу сайта для проверки прокси
                            doc = web.Load(linkValid, proxy.Split(':')[0], Convert.ToInt32(proxy.Split(':')[1]), proxy.Split(':')[2], proxy.Split(':')[3]);

                            if (doc != null)//Доп проверка на загрузку
                            {
                                if (doc.DocumentNode.SelectSingleNode("//h1").InnerText == "All Games")//Доп проверка на бан
                                {
                                    lock (LockListProxy)
                                    {
                                        ProxyListValid.Add(proxy);
                                    }
                                    lock (Lock)
                                    {
                                        validProxy++;
                                    }
                                }
                                else
                                {
                                    lock (LockListProxy)
                                    {
                                        ProxyListError.Add(proxy);
                                        ShowProxyListBan.Add(proxy);
                                    }
                                    lock (Lock)
                                    {
                                        errorProxy++;
                                    }

                                }
                            }
                        }
                        catch (Exception)
                        {
                            //Плохие прокси убераем на повторную проверку
                            lock (LockListProxy)
                            {
                                ProxyListError.Add(proxy);
                            }
                            lock (Lock)
                            {
                                errorProxy++;
                            }
                        }
                    }

                }).Start();
            }

            await Task.Run(async () =>
            {
                while (true)
                {
                    if (threadStop >= potokCol)
                    {
                        await Task.Delay(10000);
                        threadStop = 0;
                        break;
                    }
                    else await Task.Delay(3000);
                }
            });

            //Отдаём значения в свойства
            ValidProxy = validProxy;
            ErrorProxy = errorProxy;

            //Логирование
            ErrorLogSeve("ChekingProxy", $"Валидных прокси - {validProxy}\nНе вылидных прокси - {errorProxy}");
            
        }
    }
}
