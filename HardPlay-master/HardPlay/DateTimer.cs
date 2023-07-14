using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardPlay
{
    
    internal class DateTimer
    {
        //Хранит расчитаное время в милисек 15 минут
        public static double milisec_15min;
        public static double milisecUpdate;
        /// <summary>
        /// Расчитывает при старте 15 минут в милисекундах
        /// </summary>
        public static void TimerСalculation15min()
        {
            var nowDate = Convert.ToDateTime(DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
            var newDateSend = nowDate.AddMinutes(15);
            milisec_15min = (newDateSend - nowDate).TotalMilliseconds;
        }



        /// <summary>
        /// Метод расчитывает вреся для следующего сбора статы от последней даты сбора
        /// </summary>
        public static void TimerСalculationUpdate()
        {
            double milisec;
            DateTime oldDate;
            DateTime newDatePars;
            DateTime nowDate;
            DateTime nowDatePars;


            nowDatePars = DateTime.Now.Date.AddHours(20).AddMinutes(00);
            nowDate = Convert.ToDateTime(DateTime.UtcNow.AddHours(3).ToString("dd.MM.yyyy HH:mm"));
            milisecUpdate = (nowDatePars - nowDate).TotalMilliseconds;

            if (milisecUpdate <= 0)
            {
                oldDate = Convert.ToDateTime(DateTime.Now.ToString("dd.MM.yyyy"));
                // Дата и время следующего сбора (+ 1 день и 12 часов к oldDate)
                newDatePars = oldDate.AddDays(1).AddHours(20).AddMinutes(00);
                // Расчитываем время до следующего сбора в милисекундах
                milisecUpdate = (newDatePars - nowDate).TotalMilliseconds;
            }
        }
    }
}
