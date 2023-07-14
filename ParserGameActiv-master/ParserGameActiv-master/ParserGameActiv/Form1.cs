using ParserOnlineGames;
using System.Globalization;
using System.Threading;
using System.Timers;

namespace ParserGameActiv
{
    public partial class Form1 : Form
    {
        public static string TokenAPI { get; set; }
        public static DateTime newDatePars;//��� �������� � ������ ���� ������ ��������
        public static string statusServerResponse;//��� �������� � ������ ������� ��������
        public static bool buttonEnable;
        public static double couuntMilliseconds;//��� ������ ������� � ������������ ��� ������ ��������;
        public static System.Timers.Timer timerRivals = new();
        public static System.Timers.Timer timerGames = new();

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ��������� ����� ��� ������� (������ ��������� �� ������� , ���� ��� ��� ��������� �� �������)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //����� ��� API
            TokenAPI = "Yr56JpxKgQapZzKXnz7TgcvxVuq5Jj";
            //�������� ���� �� ���������
            ConfigProgram.CheckConfig(button2.Text, checkBox2.Checked, checkBox1.Checked);

            //�������� ������� �� � ������������ � ������
            label7.Text = RegistryAutoRun.AutoRun();
            //button3.Enabled = buttonEnable;

            //����������� ����� 
            var (ayto, save, dataToSend, date, status) = ConfigProgram.GetToSettingsFile();
            //���� ��� ������� �����������
            if (ayto == "����������")
            {
                button2.Text = "����������";
                label4.Text = "������� ���� ���� �����.";

                //������ ������� �� ��������
                couuntMilliseconds = DateTimer.Timer�alculation();
                label5.Text = newDatePars.ToString("dd.MM.yyyy HH:mm");//��������� ����� ���� ��������
                //��������� ������� rivals
                timerRivals.Interval= couuntMilliseconds;//����� ����� �� �������� � ������������
                timerRivals.Elapsed += Timer_ElapsedRivalsToday;//������������ ������� �� ��������� �������
                timerRivals.Enabled = true;//��������� �������
                timerRivals.AutoReset = true;//������ �������
                timerRivals.Start();//������ �������

                //��������� ������� GamesToday
                timerGames.Interval = DateTimer.Timer�alculationGamesToday();//����� ����� �� �������� � ������������
                timerGames.Elapsed += Timer_ElapsedGamesToday;//������������ ������� �� ��������� �������
                timerGames.Enabled = true;//��������� �������
                timerGames.AutoReset = true;//������ �������
                timerGames.Start();//������ �������
            }

            label3.Text = date;//���� ���������� ��������
            label8.Text = status;//������ �������� ������ �� ������

            //��������� ���������
            checkBox1.Checked = Convert.ToBoolean(dataToSend);
            checkBox2.Checked = Convert.ToBoolean(save);
        }

        /// <summary>
        /// ������ �������� �� ������������ ������� 1 ��� � ���� � 20:30
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Timer_ElapsedGamesToday(object? sender, ElapsedEventArgs e)
        {
            //�������� ������ � ������� � ������ ������� � ������� (���������� ������� ���� ��������)
            label10.Text = GettingAccountData.GetData();

            //���� ������� ������� ������ ���������� �� ������
            if (checkBox1.Checked)
            {
                //�������� ��������� ������ �� ������
                await DataOutputToSend.OutputToSendGamesToday();
            }
            //���� ������� ������� ������ ���������� � ���� �� ��
            if (checkBox2.Checked)
            {
                //���������� ������ � ����
                DataOutputSave.SaveFileGamesToday();
            }

            //������ ������� �� ��������
            timerGames.Interval = DateTimer.Timer�alculationGamesToday();
            
        }

        /// <summary>
        /// ������ ���� ���������� � ������� ���������� � ��������� �� ������ ���� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button1_Click(object sender, EventArgs e)
        {
            //�������� �����
            //GetTokenServer.GetToken();

            //�������� ������ � ������� � ������ ������� � �������
            label3.Text = GettingAccountData.GetData();
            //���������� �������� � �������
            ConfigProgram.UpdateConfigFile(button2.Text, checkBox2.Checked, checkBox1.Checked, label3.Text, label8.Text);

            //���� ������� ������� ������ ���������� �� ������
            if (checkBox1.Checked)
            {
                //�������� ��������� ������ �� ������
                await DataOutputToSend.OutputToSendRivalsToday();
                await DataOutputToSend.OutputToSendGamesToday();
                label8.Text = statusServerResponse;
            }

            //���� ������� ������� ������ ���������� � ���� �� ��
            if (checkBox2.Checked)
            {
                //���������� ������ � ����
                DataOutputSave.SaveFileRivalsToday();
                DataOutputSave.SaveFileGamesToday();
            }

            //������� ������ �� ����� � ��� �����
            //FormDataOutput formDataOutput = new()
            //{
               // Owner = this//�������� ������ ����� � ��������
            //};
            //formDataOutput.ShowDialog();//����� ����� � ����������� ��������
        }


        /// <summary>
        /// ������ ��������������� ����� ����� 1 ��� � ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //������ � ��������� ���������
            if (button2.Text == "���������")
            {
                button2.Text = "����������";
                label4.Text = "������� ���� ���� �����.";

                //������ ������� �� ��������
                timerRivals.Interval = DateTimer.Timer�alculation();
                //����� ���� �� �����
                label5.Text = newDatePars.ToString("dd.MM.yyyy HH:mm");//��������� ����� ���� ��������

                //��������� �������                
                timerRivals.Elapsed += Timer_ElapsedRivalsToday;//������������ ������� �� ��������� �������
                timerRivals.Enabled = true;//��������� �������
                timerRivals.AutoReset = true;//������ �������
                timerRivals.Start();//������ �������

                //��������� ������� GamesToday
                timerGames.Interval = DateTimer.Timer�alculationGamesToday();//����� ����� �� �������� � ������������
                timerGames.Elapsed += Timer_ElapsedGamesToday;//������������ ������� �� ��������� �������
                timerGames.Enabled = true;//��������� �������
                timerGames.AutoReset = true;//������ �������
                timerGames.Start();//������ �������
            }
            else
            {
                button2.Text = "���������";//������ �� ������ ��������� �������
                label4.Text = "";//������ ������ ��� ������� ��������
                label5.Text = "";//������ ���� �������� �� �����
                timerRivals.Stop();
                timerGames.Stop();
            }

            //������ ��������� � ����� 
            ConfigProgram.UpdateConfigFile(button2.Text, checkBox2.Checked, checkBox1.Checked, label3.Text, label8.Text);
        }

        /// <summary>
        /// ������ �������� �� ������������ ������� 1 ��� � ���� � 19:00
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Timer_ElapsedRivalsToday(object? sender, ElapsedEventArgs e)
        {

            //�������� ������ � ������� � ������ ������� � ������� (���������� ������� ���� ��������)
            label3.Text = GettingAccountData.GetData();

            //���� ������� ������� ������ ���������� �� ������
            if (checkBox1.Checked)
            {
                //�������� ��������� ������ �� ������
                await DataOutputToSend.OutputToSendRivalsToday();
                label8.Text = statusServerResponse;
            }

            //���� ������� ������� ������ ���������� � ���� �� ��
            if (checkBox2.Checked)
            {
                //���������� ������ � ����
                DataOutputSave.SaveFileRivalsToday();
            }

            //-- ��������������� ������� � �������� ������� � ������� --//
            //������ ������� �� ��������
            timerRivals.Interval = DateTimer.Timer�alculation();
            label5.Text = newDatePars.ToString("dd.MM.yyyy HH:mm");

            //���������� ����� �������
            ConfigProgram.UpdateConfigFile(button2.Text, checkBox2.Checked, checkBox1.Checked, label3.Text, label8.Text);
        }

        //���������� �������� � ����� ��� ���������� �������� ��������� ������ � ����� �� ��. 
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ConfigProgram.UpdateConfigFile(button2.Text, checkBox2.Checked, checkBox1.Checked, label3.Text, label8.Text);
        }
        //���������� �������� � ����� ��� ���������� �������� ��������� ������ �� ������.
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ConfigProgram.UpdateConfigFile(button2.Text, checkBox2.Checked, checkBox1.Checked, label3.Text, label8.Text);
        }
    }
}