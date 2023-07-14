using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser_PS_Games_2._0
{
    internal class ErrorLog
    {
        object LockSeveErrors = new object();//Блочим лист с логам
        public async Task ErrorLogSeve(string mess, string ex)
        {
            await Task.Run(() =>
            {
                lock (LockSeveErrors) 
                {
                    using (FileStream fs = new FileStream("./ErrorLogs.txt", FileMode.Append))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine($"{DateTime.Now.ToString("dd.MM.yy/HH:mm")} {mess}{Environment.NewLine}{ex}{Environment.NewLine}");
                    }
                }
            });
        }
    }
}
