using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HardPlay
{
    internal class StartBotTelegram
    {
        //Создаём подключение с токеном
        static TelegramBotClient client = new TelegramBotClient("токен для телеграм бота");
        //Токен группы
        public static long catIdGrup = ****************;


        /// <summary>
        /// Запуск прослушки сервера Телеграм
        /// </summary>
        public static void Start()
        {
            //Запуск прослушки сервера
            client.StartReceiving(Update, Error);
        }

        public static void SendMessageGrup(string message)
        {
            client.SendTextMessageAsync(catIdGrup, message);
        }

        //Основной метод получения сообщений от пользователя
        public async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
        }

        //Метоб обработки ошибок
        static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}
