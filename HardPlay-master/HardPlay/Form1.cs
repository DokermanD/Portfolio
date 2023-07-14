using System.Timers;

namespace HardPlay
{
    public partial class Form1 : Form
    {
        public static System.Timers.Timer timer = new();
        public static System.Timers.Timer timerUpdate = new();

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод запуска автоматического нажатия Hard Play раз в 15 минут
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            if (button1.Text == "Запуск авто HardPlay")
            {
                label1.Text = "Запущен авто HardPlay";
                button1.Text = "Остановить авто HardPlay";

                //Отправка HardPlay
                HardPlayAuto.SendHardPlay();

                //Стартуем таймер на повтор каждые 15 минут
                timer.Interval = DateTimer.milisec_15min;//Задаём время повтора в милисекундах на 15 минут
                timer.Elapsed += Timer_Elapsed;//Срабатывание события по оканчанию таймера
                timer.Enabled = true;//Включение таймера
                timer.AutoReset = true;//Повтор таймера
                timer.Start();//Запуск таймера
                StartBotTelegram.SendMessageGrup($"Включён Auto Hard Play - {DateTime.UtcNow.AddHours(3).ToString("dd.MM.yyyy HH:mm")}");

                //Стартуем таймер на повтор обнавления списка 1 раз в день в 20:00 по мск
                timerUpdate.Interval = DateTimer.milisecUpdate;//Задаём время повтора в милисекундах на 1 раз в день в 20:00 по мск
                timerUpdate.Elapsed += Timer_ElapsedUpdate;//Срабатывание события по оканчанию таймера
                timerUpdate.Enabled = true;//Включение таймера
                timerUpdate.AutoReset = true;//Повтор таймера
                timerUpdate.Start();//Запуск таймера
            }
            else 
            {
                timer.Stop();//Стоп таймера
                timerUpdate.Stop();//Стоп таймера
                label1.Text = "";
                button1.Text = "Запуск авто HardPlay";
                StartBotTelegram.SendMessageGrup($"Отключили Auto Hard Play - {DateTime.UtcNow.AddHours(3).ToString("dd.MM.yyyy HH:mm")}");
            }

        }

        private void Timer_ElapsedUpdate(object? sender, ElapsedEventArgs e)
        {
            //Парсим ID акаунтов
            label3.Text = GettingAccountData.GetID();
            //GettingAccountData.GetID();
            StartBotTelegram.SendMessageGrup($"Обновил список аккаунтов - {DateTime.UtcNow.AddHours(3).ToString("dd.MM.yyyy HH:mm")}");
            //Расчёт даты в милисекундах на обнову раз в день в 20:00
            DateTimer.TimerСalculationUpdate();
            //Переназначаем время следующего обновления
            timerUpdate.Interval = DateTimer.milisecUpdate;//Задаём время повтора в милисекундах на раз в день в 20:00
            //timerUpdate.Elapsed += Timer_ElapsedUpdate;//Срабатывание события по оканчанию таймера
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            //Отправка HardPlay
            var messageTg = HardPlayAuto.SendHardPlay();
            //Отправка сообщения в бот
            StartBotTelegram.SendMessageGrup($"{messageTg} - {DateTime.UtcNow.AddHours(3).ToString("dd.MM.yyyy HH:mm")}");
        }

        /// <summary>
        /// При загрузке формы парсим ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //Расчёт даты в милисекундах 15 минит
            DateTimer.TimerСalculation15min();

            //Расчёт даты в милисекундах на обнову раз в день в 20:00
            DateTimer.TimerСalculationUpdate();

            //Получаем токен
            GetTokenServer.GetToken();

            //Парсим ID акаунтов
            GettingAccountData.GetID();
        }
    }
}