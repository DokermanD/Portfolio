using Microsoft.Win32;

namespace ParserGameActiv
{
    internal class ConfigProgram
    {
        /// <summary>
        /// Проверка существует ли ветка в реестре (если нет создаём с параметрами по умолчанию)
        /// </summary>
        /// <param name="auto"></param>
        /// <param name="save"></param>
        /// <param name="dataToSend"></param>
        public static void CheckConfig(string auto, bool save, bool dataToSend)
        {
            //Проверка ветки в реестре
            if (Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Parser Online Games") == null)
            {
                using RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Parser Online Games");
                key.SetValue("AutoPars", auto);
                key.SetValue("SaveПК", save);
                key.SetValue("SendServer", dataToSend);
                key.SetValue("LastDate", "Данные не парсились");
                key.SetValue("LastStatus", "---");
            }
        }

        /// <summary>
        /// Берём настройки из реестра для настройки формы
        /// </summary>
        /// <returns>возвращает кортеж string auto, string save, string dataToSend, string date</returns>
        public static (string auto, string save, string dataToSend, string date, string status) GetToSettingsFile()
        {

            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Parser Online Games");
            var auto = key.GetValue("AutoPars").ToString();
            var save = key.GetValue("SaveПК").ToString();
            var dataToSend = key.GetValue("SendServer").ToString();
            var date = key.GetValue("LastDate").ToString();
            var status = key.GetValue("LastStatus").ToString();


            var result = (auto, save, dataToSend, date, status);

            return result;
        }

        /// <summary>
        /// Обновления данные в реестре при смене настроек
        /// </summary>
        /// <param name="_auto">Автопарсинг</param>
        /// <param name="_save">Сохранение на ПК</param>
        /// <param name="_dataToSend">Отправка на сервер</param>
        /// <param name="_date">Дата последнего парсинга</param>
        public static void UpdateConfigFile(string _auto, bool _save, bool _dataToSend, string _date, string _status)
        {
            using RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Parser Online Games");
            key.SetValue("AutoPars", _auto);
            key.SetValue("SaveПК", _save);
            key.SetValue("SendServer", _dataToSend);
            key.SetValue("LastDate", _date);
            key.SetValue("LastStatus", _status);
        }
    }
}
