using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sing_in_by_link
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Token.GetToken();

            Console.WriteLine("Gotov k rabote, proverka spiska links");
            //Проверка файла с ссылками для отправки
            while (true)
            {
                GetFileString.GetDataString();
                Task.Delay(30000).Wait();
            }
            
        }
    }
}
