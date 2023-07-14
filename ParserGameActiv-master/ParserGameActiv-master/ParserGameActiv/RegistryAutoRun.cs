using Microsoft.Win32;

namespace ParserGameActiv
{
    internal class RegistryAutoRun
    {
        //Заносим програму в реест в автозагрузки
        public static string AutoRun()
        {
            if (Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run").GetValueNames().Contains("ParserOnlineGames"))
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"))
                {
                    var oldPatch = key?.GetValue("ParserOnlineGames");
                    var newPatch = Environment.CurrentDirectory + "\\ParserOnlineGames.exe";

                    if (oldPatch == newPatch)
                    {
                        Form1.buttonEnable = false;
                        return "Парсер добавлен в реестр автозагрузки";
                    }
                    else
                    {
                        //Пишем в автозагрузки прогу
                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"))
                        {
                            var patch = Environment.CurrentDirectory;
                            key1.SetValue("ParserOnlineGames", Environment.CurrentDirectory + "\\ParserOnlineGames.exe");
                        }
                        Form1.buttonEnable = false;
                    }

                }
            }
            else
            {
                //Пишем в автозагрузки прогу
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"))
                {
                    var patch = Environment.CurrentDirectory;
                    key.SetValue("ParserOnlineGames", Environment.CurrentDirectory + "\\ParserOnlineGames.exe");
                }
                Form1.buttonEnable = false;
            }

            return "Парсер добавлен в реестр автозагрузки";
        }
    }
}
