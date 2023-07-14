using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Parser_PS_Games_2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Экземпляры классов
        Stopwatch stopWatch = new Stopwatch();
        ChekingProxy chekingProxy = new ChekingProxy();
        LinksPages linksPages = new LinksPages();
        ParsingGames parsingGames = new ParsingGames();
        ErrorLog errorLog = new ErrorLog();

        //Пееременные
        bool STOP = true;
        bool banAll = true;
        //Запус парсинга
        private async void button1_Click(object sender, EventArgs e)
        {
            // 0) Запуск отсчёта и отображения на форме общего времени работы
                button1.Enabled = false;
                TotalWorkingTimeAsync();
                errorLog.ErrorLogSeve("Блок 0","Запуск отсчёта времени");

            // 1) Чекер прокси на валидность.----------------------------------------------------------------------------------
                errorLog.ErrorLogSeve("Блок 1", "Чекер прокси на валидность.");
                textBox1.Text += "Идёт проверка прокси на валид..." + Environment.NewLine;
                //Запускаем проверку на наличие прокси
                await chekingProxy.ChekProxyAsync();//Пишем в текстбокс результат проверки прокси

                //Запускаем проверку на валидность прокси
                await chekingProxy.ChekProxyValidAsync(chekingProxy.ProxyList);
                                  
                //Вывод в форму количества валидных и невалидных проксей
                label5.Text = chekingProxy.ValidProxy.ToString();
                label6.Text = chekingProxy.ErrorProxy.ToString();


            // 2) Сбор ссылок на страницы с играми.------------------------------------------------------------------------------
                errorLog.ErrorLogSeve("Блок 2", "Сбор ссылок на страницы с играми.");
                textBox1.Text += Environment.NewLine + "Начал сбор страниц с играми..." + Environment.NewLine;
                //Запуск метода по сбору ссылок на страницы
                linksPages.ParsingLinkPage(chekingProxy.ProxyListValid);
                //Ждём завершения парсинга потоками
                await Task.Run(() =>
                {
                    while (true)
                    {
                        if (linksPages.ListPageLinks.Count > 0)
                        {
                            label10.Text = linksPages.ListPageLinks.Count.ToString();
                            Thread.Sleep(3000);
                        }
                        else
                        {
                            if (linksPages.threadStopMonitor == linksPages.colPotok)
                            {
                                Thread.Sleep(5000);
                                break;
                            }

                        }
                        label5.Text = (linksPages.colPotok - linksPages.threadStopMonitor).ToString();
                    }
                });

                label10.Text = "0";
                //Передаём свойству количество игр которые надо спарисить (для формы)
                label11.Text = linksPages.HomePageLinksGames.Count.ToString();
                textBox1.Text += Environment.NewLine + $"Собрал ссылки на парсинг игр {label11.Text}" + Environment.NewLine;

            // 3) Сбор данных со страницы игры.-----------------------------------------------------------------------------------
                errorLog.ErrorLogSeve("Блок 3", "Сбор данных со страницы игры.");
                textBox1.Text += Environment.NewLine + "Начинаю сбор даных по играм..." + Environment.NewLine;
                parsingGames.ParssAllGames(linksPages, chekingProxy, label5.Text);

                //Ждём завершения парсинга потоками
                await Task.Run(() =>
                {
                    while (true)
                        {
                            if (parsingGames.LinksGames.Count > 0)
                            {
                                if (parsingGames.сloseThread == linksPages.colPotok)
                                {
                                    textBox1.Text += "Все потоки закончили работу!" + Environment.NewLine;
                                    break;
                                }
                                else
                                {
                                    label11.Text = parsingGames.LinksGames.Count.ToString();
                                    Thread.Sleep(3000);
                                }
                            }
                             //Закрывшиеся потоки из за бана прокси
                            else if (parsingGames.сloseThread == linksPages.colPotok)
                            {
                                textBox1.Text += "Похоже что в бан ушли все прокси, парсинг остановлен!" + Environment.NewLine;
                                banAll = false;
                                break;
                            }
                            else
                            {
                                Thread.Sleep(10000);
                                break;
                            }

                            label5.Text = (parsingGames.colPotok - parsingGames.сloseThread).ToString();


                        }
                });

            //Если все потоки закроются из за бана прокси этот блок не выполняется
                if (banAll)
                {
                    label11.Text = "0";
                    textBox1.Text += "Парсинг закончен успешно." + Environment.NewLine;

                    // 4) Формируем результат парсинга. ------------------------------------------------------------------------------
                    errorLog.ErrorLogSeve("Блок 4", "Формируем результат парсинга.");
                    //Сохраняем список в блакнот
                    string fileName = $@"Pars-{DateTime.Now.ToString("dd.MM.yyyy - HH;mm")}.txt";
                    int schet = 0;

                    using (FileStream fs = new FileStream($@"./{fileName}", FileMode.Create))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        foreach (var item in parsingGames.rezaltParsing)
                        {
                            sw.WriteLine(item);
                            schet++;
                        }
                    }

                    textBox1.Text += Environment.NewLine + $"Спарсили {schet} игр." + Environment.NewLine;
                    Process.Start(fileName); //Запускаем созданный фаил с парсингом

                    stopWatch.Stop();//Стопаем отсчёт времени
                    button1.Enabled = true;
                    STOP = false;
                    errorLog.ErrorLogSeve("Блок 4", "Парсинг закончен.");
                }
                else
                {
                    errorLog.ErrorLogSeve("Блок 4", "Формируем то что удалось спарсить.");
                    //Сохраняем список в блакнот
                    string fileName = $@"Pars-{DateTime.Now.ToString("dd.MM.yyyy - HH;mm")}.txt";
                    int schet = 0;

                    using (FileStream fs = new FileStream($@"./{fileName}", FileMode.Create))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        foreach (var item in parsingGames.rezaltParsing)
                        {
                            sw.WriteLine(item);
                            schet++;
                        }
                    }

                    stopWatch.Stop();//Стопаем отсчёт времени
                    button1.Enabled = true;
                    STOP = false;
                    errorLog.ErrorLogSeve("Бан прокси", "Парсинг остановлен из за бана всех прокси!");
                    textBox1.Text += "Список игр будет неполным, только то что успел собрать парсер!!" + Environment.NewLine;
                }
        }

        //Метод считает общее время работы программы
        private async void TotalWorkingTimeAsync()
        {
            await Task.Run(() =>
            {
                stopWatch.Start();//Запуск отсчёта времени

                while (STOP)
                {
                    TimeSpan ts = stopWatch.Elapsed;
                    label7.Text = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                }
            });
        }
    }
}
