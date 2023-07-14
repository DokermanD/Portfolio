using ParserOnlineGames;
using System.Globalization;
using System.Threading;
using System.Timers;

namespace ParserGameActiv
{
    public partial class Form1 : Form
    {
        public static string TokenAPI { get; set; }
        public static DateTime newDatePars;//Для хранения и вывода даты нового парсинга
        public static string statusServerResponse;//Для хранения и вывода статуса парсинга
        public static bool buttonEnable;
        public static double couuntMilliseconds;//Для записи времени в милисекундах для нового парсинга;
        public static System.Timers.Timer timerRivals = new();
        public static System.Timers.Timer timerGames = new();

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Настройка формы при запуске (читаем настройки из реестра , если там нет оставляем по дефолту)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //Токен для API
            TokenAPI = "Yr56JpxKgQapZzKXnz7TgcvxVuq5Jj";
            //Проверка есть ли настройки
            ConfigProgram.CheckConfig(button2.Text, checkBox2.Checked, checkBox1.Checked);

            //Проверка записан ли в автозагрузки в реестр
            label7.Text = RegistryAutoRun.AutoRun();
            //button3.Enabled = buttonEnable;

            //Настраиваем форму 
            var (ayto, save, dataToSend, date, status) = ConfigProgram.GetToSettingsFile();
            //Если был включён автопарсинг
            if (ayto == "Остановить")
            {
                button2.Text = "Остановить";
                label4.Text = "Запущен авто сбор статы.";

                //Расчёт времени до парсинга
                couuntMilliseconds = DateTimer.TimerСalculation();
                label5.Text = newDatePars.ToString("dd.MM.yyyy HH:mm");//Установка новой даты парсинга
                //Установка таймера rivals
                timerRivals.Interval= couuntMilliseconds;//Задаём время до парсинга в милисекундах
                timerRivals.Elapsed += Timer_ElapsedRivalsToday;//Срабатывание события по оканчанию таймера
                timerRivals.Enabled = true;//Включение таймера
                timerRivals.AutoReset = true;//Повтор таймера
                timerRivals.Start();//Запуск таймера

                //Установка таймера GamesToday
                timerGames.Interval = DateTimer.TimerСalculationGamesToday();//Задаём время до парсинга в милисекундах
                timerGames.Elapsed += Timer_ElapsedGamesToday;//Срабатывание события по оканчанию таймера
                timerGames.Enabled = true;//Включение таймера
                timerGames.AutoReset = true;//Повтор таймера
                timerGames.Start();//Запуск таймера
            }

            label3.Text = date;//Дата последнего парсинга
            label8.Text = status;//Статус отправки данных на сервер

            //Установка чекбоксов
            checkBox1.Checked = Convert.ToBoolean(dataToSend);
            checkBox2.Checked = Convert.ToBoolean(save);
        }

        /// <summary>
        /// Запуск парсинга по срабатыванию таймера 1 раз в день в 20:30
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Timer_ElapsedGamesToday(object? sender, ElapsedEventArgs e)
        {
            //Получаем данные с сервера и делаем выборку в словарь (возвращает текущую дату парсинга)
            label10.Text = GettingAccountData.GetData();

            //Если чекбокс отмечен данные отправятся на сервер
            if (checkBox1.Checked)
            {
                //Отправка полученых данных на сервер
                await DataOutputToSend.OutputToSendGamesToday();
            }
            //Если чекбокс отмечен данные сохранятся в фаил на пк
            if (checkBox2.Checked)
            {
                //Сохранение данных в фаил
                DataOutputSave.SaveFileGamesToday();
            }

            //Расчёт времени до парсинга
            timerGames.Interval = DateTimer.TimerСalculationGamesToday();
            
        }

        /// <summary>
        /// Ручной сбор статистики с выводом результата и отправкой на сервер если выбрано
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button1_Click(object sender, EventArgs e)
        {
            //Получаем токен
            //GetTokenServer.GetToken();

            //Получаем данные с сервера и делаем выборку в словарь
            label3.Text = GettingAccountData.GetData();
            //Обновление настроек в реестре
            ConfigProgram.UpdateConfigFile(button2.Text, checkBox2.Checked, checkBox1.Checked, label3.Text, label8.Text);

            //Если чекбокс отмечен данные отправятся на сервер
            if (checkBox1.Checked)
            {
                //Отправка полученых данных на сервер
                await DataOutputToSend.OutputToSendRivalsToday();
                await DataOutputToSend.OutputToSendGamesToday();
                label8.Text = statusServerResponse;
            }

            //Если чекбокс отмечен данные сохранятся в фаил на пк
            if (checkBox2.Checked)
            {
                //Сохранение данных в фаил
                DataOutputSave.SaveFileRivalsToday();
                DataOutputSave.SaveFileGamesToday();
            }
        }


        /// <summary>
        /// Запуск автоматического сбора статы 1 раз в день
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //Запуск и остановка Автосбора
            if (button2.Text == "Запустить")
            {
                button2.Text = "Остановить";
                label4.Text = "Запущен авто сбор статы.";

                //Расчёт времени до парсинга
                timerRivals.Interval = DateTimer.TimerСalculation();
                //Вывод даты на форму
                label5.Text = newDatePars.ToString("dd.MM.yyyy HH:mm");//Установка новой даты парсинга

                //Установка таймера                
                timerRivals.Elapsed += Timer_ElapsedRivalsToday;//Срабатывание события по оканчанию таймера
                timerRivals.Enabled = true;//Включение таймера
                timerRivals.AutoReset = true;//Повтор таймера
                timerRivals.Start();//Запуск таймера

                //Установка таймера GamesToday
                timerGames.Interval = DateTimer.TimerСalculationGamesToday();//Задаём время до парсинга в милисекундах
                timerGames.Elapsed += Timer_ElapsedGamesToday;//Срабатывание события по оканчанию таймера
                timerGames.Enabled = true;//Включение таймера
                timerGames.AutoReset = true;//Повтор таймера
                timerGames.Start();//Запуск таймера
            }
            else
            {
                button2.Text = "Запустить";//Меняем на кнопке автосбора надпись
                label4.Text = "";//Чистим строку что запущен автосбор
                label5.Text = "";//Чистим дату парсинга на форме
                timerRivals.Stop();
                timerGames.Stop();
            }

            //Меняем настройки в файле 
            ConfigProgram.UpdateConfigFile(button2.Text, checkBox2.Checked, checkBox1.Checked, label3.Text, label8.Text);
        }

        /// <summary>
        /// Запуск парсинга по срабатыванию таймера 1 раз в день в 19:00
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Timer_ElapsedRivalsToday(object? sender, ElapsedEventArgs e)
        {

            //Получаем данные с сервера и делаем выборку в словарь (возвращает текущую дату парсинга)
            label3.Text = GettingAccountData.GetData();

            //Если чекбокс отмечен данные отправятся на сервер
            if (checkBox1.Checked)
            {
                //Отправка полученых данных на сервер
                await DataOutputToSend.OutputToSendRivalsToday();
                label8.Text = statusServerResponse;
            }

            //Если чекбокс отмечен данные сохранятся в фаил на пк
            if (checkBox2.Checked)
            {
                //Сохранение данных в фаил
                DataOutputSave.SaveFileRivalsToday();
            }

            //-- Переопределения таймера с расчётам времени в милисек --//
            //Расчёт времени до парсинга
            timerRivals.Interval = DateTimer.TimerСalculation();
            label5.Text = newDatePars.ToString("dd.MM.yyyy HH:mm");

            //Обновление файла конфига
            ConfigProgram.UpdateConfigFile(button2.Text, checkBox2.Checked, checkBox1.Checked, label3.Text, label8.Text);
        }

        //Обновления настроек в файле при измененнии чекбокса Сохранять данные в папку на пк. 
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ConfigProgram.UpdateConfigFile(button2.Text, checkBox2.Checked, checkBox1.Checked, label3.Text, label8.Text);
        }
        //Обновления настроек в файле при измененнии чекбокса Отправить данные на сервер.
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ConfigProgram.UpdateConfigFile(button2.Text, checkBox2.Checked, checkBox1.Checked, label3.Text, label8.Text);
        }
    }
}
