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
        /// ����� ������� ��������������� ������� Hard Play ��� � 15 �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            if (button1.Text == "������ ���� HardPlay")
            {
                label1.Text = "������� ���� HardPlay";
                button1.Text = "���������� ���� HardPlay";

                //�������� HardPlay
                HardPlayAuto.SendHardPlay();

                //�������� ������ �� ������ ������ 15 �����
                timer.Interval = DateTimer.milisec_15min;//����� ����� ������� � ������������ �� 15 �����
                timer.Elapsed += Timer_Elapsed;//������������ ������� �� ��������� �������
                timer.Enabled = true;//��������� �������
                timer.AutoReset = true;//������ �������
                timer.Start();//������ �������
                StartBotTelegram.SendMessageGrup($"������� Auto Hard Play - {DateTime.UtcNow.AddHours(3).ToString("dd.MM.yyyy HH:mm")}");

                //�������� ������ �� ������ ���������� ������ 1 ��� � ���� � 20:00 �� ���
                timerUpdate.Interval = DateTimer.milisecUpdate;//����� ����� ������� � ������������ �� 1 ��� � ���� � 20:00 �� ���
                timerUpdate.Elapsed += Timer_ElapsedUpdate;//������������ ������� �� ��������� �������
                timerUpdate.Enabled = true;//��������� �������
                timerUpdate.AutoReset = true;//������ �������
                timerUpdate.Start();//������ �������
            }
            else 
            {
                timer.Stop();//���� �������
                timerUpdate.Stop();//���� �������
                label1.Text = "";
                button1.Text = "������ ���� HardPlay";
                StartBotTelegram.SendMessageGrup($"��������� Auto Hard Play - {DateTime.UtcNow.AddHours(3).ToString("dd.MM.yyyy HH:mm")}");
            }

        }

        private void Timer_ElapsedUpdate(object? sender, ElapsedEventArgs e)
        {
            //������ ID ��������
            label3.Text = GettingAccountData.GetID();
            //GettingAccountData.GetID();
            StartBotTelegram.SendMessageGrup($"������� ������ ��������� - {DateTime.UtcNow.AddHours(3).ToString("dd.MM.yyyy HH:mm")}");
            //������ ���� � ������������ �� ������ ��� � ���� � 20:00
            DateTimer.Timer�alculationUpdate();
            //������������� ����� ���������� ����������
            timerUpdate.Interval = DateTimer.milisecUpdate;//����� ����� ������� � ������������ �� ��� � ���� � 20:00
            //timerUpdate.Elapsed += Timer_ElapsedUpdate;//������������ ������� �� ��������� �������
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            //�������� HardPlay
            var messageTg = HardPlayAuto.SendHardPlay();
            //�������� ��������� � ���
            StartBotTelegram.SendMessageGrup($"{messageTg} - {DateTime.UtcNow.AddHours(3).ToString("dd.MM.yyyy HH:mm")}");
        }

        /// <summary>
        /// ��� �������� ����� ������ ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //������ ���� � ������������ 15 �����
            DateTimer.Timer�alculation15min();

            //������ ���� � ������������ �� ������ ��� � ���� � 20:00
            DateTimer.Timer�alculationUpdate();

            //�������� �����
            GetTokenServer.GetToken();

            //������ ID ��������
            GettingAccountData.GetID();
        }
    }
}