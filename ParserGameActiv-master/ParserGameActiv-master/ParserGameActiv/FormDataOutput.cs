using ParserGameActiv;
using System.Data;

namespace ParserOnlineGames
{
    public partial class FormDataOutput : Form
    {
        public FormDataOutput()
        {
            InitializeComponent();
        }

        private void FormDataOutput_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            
            //Выводим заголовки колонок в таблицу
            DataTable table = new DataTable();

            table.Columns.Add("Email PSN", typeof(string));
            table.Columns.Add("Count games", typeof(int));

            //Заполняем строки таблицы данными из словаря
            foreach (var item in GettingAccountData.OnlineGames)
            {
                table.Rows.Add(item.Key, item.Value);
            }

            dataGridView1.DataSource = table;

        }
    }
}
