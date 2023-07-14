using System.Text;

namespace ParserGameActiv
{
    internal class DataOutputSave
    {
        public static void SaveFileRivalsToday()
        {
            //Создаём папку если нет
            Directory.CreateDirectory($"./DataOnline");

            //Сохроняем данные в txt документ с именем дата создания 
            using (FileStream fs = new FileStream($@"./DataOnline/{DateTime.Now:dd.MM.yyyy}-RV.txt", FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var item in GettingAccountData.OnlineGames)
                {
                    stringBuilder.AppendLine($"{item.Key}:{item.Value}");
                }

                sw.Write(stringBuilder.ToString());
            }
        }

        public static void SaveFileGamesToday()
        {
            //Создаём папку если нет
            Directory.CreateDirectory($"./DataOnline");

            //Сохроняем данные в txt документ с именем дата создания 
            using (FileStream fs = new FileStream($@"./DataOnline/{DateTime.Now:dd.MM.yyyy}-SB.txt", FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var item in GettingAccountData.GamesToday)
                {
                    stringBuilder.AppendLine($"{item.Key}:{item.Value}");
                }

                sw.Write(stringBuilder.ToString());
            }
        }
    }
}
