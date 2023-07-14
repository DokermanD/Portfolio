using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sing_in_by_link
{
    internal class GetFileString
    {
        static public List<string> filesCode = new List<string>();
        public static void GetDataString()
        {
            try
            {
                //Бекрём строки из файла
                var dataFile = File.ReadAllLines("./Редиректы.txt");
                //Проверка что файил не пустой
                if (dataFile.Length != 0)
                {
                    //Копируем в список програмы
                    for (int i = 0; i < dataFile.Length; i++)
                    {
                        filesCode.Add(dataFile[i]);
                    }
                    
                }

                SetCodeApi.SetCode();
            }
            catch (Exception)
            {
                Console.WriteLine("Oshibka, poprobuy cherez 30 sek");
            }
            
        }
    }
}
