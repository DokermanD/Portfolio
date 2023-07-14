using Microsoft.Win32;

namespace ParserGameActiv
{
    internal class DateTimer
    {
        /// <summary>
        /// Метод расчитывает время для следующего сбора статы от последней даты сбора RivalsToday
        /// </summary>
        public static double TimerСalculation()
        {
            double milisec;
            DateTime nowDateNodTame;
            DateTime newDatePars;
            DateTime nowDate;


            newDatePars = DateTime.Now.Date.AddHours(19).AddMinutes(00);
            nowDate = Convert.ToDateTime(DateTime.UtcNow.AddHours(3).ToString("dd.MM.yyyy HH:mm"));
            milisec = (newDatePars - nowDate).TotalMilliseconds;

            //Проверка если уже больше 19:00 часов
            if (milisec <= 0)
            {
                //Текущая дата без времени
                nowDateNodTame = Convert.ToDateTime(DateTime.Now.ToString("dd.MM.yyyy"));

                // Дата и время следующего сбора (+ 1 день и 19:00 часов к nowDateNodTame)
                newDatePars = nowDateNodTame.AddDays(1).AddHours(19).AddMinutes(00);

                // Расчитываем время до следующего сбора в милисекундах
                milisec = (newDatePars - nowDate).TotalMilliseconds;
            }

            Form1.newDatePars = newDatePars;

            return milisec;
        }

        /// <summary>
        /// Метод расчитывает время для следующего сбора статы от последней даты сбора RivalsToday
        /// </summary>
        public static double TimerСalculationGamesToday()
        {
            double milisec;
            DateTime nowDateNodTame;
            DateTime newDatePars;
            DateTime nowDate;
            DateTime nowDatePars;


            nowDatePars = DateTime.Now.Date.AddHours(20).AddMinutes(30);
            nowDate = Convert.ToDateTime(DateTime.UtcNow.AddHours(3).ToString("dd.MM.yyyy HH:mm"));
            milisec = (nowDatePars - nowDate).TotalMilliseconds;

            //Проверка если уже больше 20:30 часов
            if (milisec <= 0)
            {
                //Текущая дата без времени
                nowDateNodTame = Convert.ToDateTime(DateTime.Now.ToString("dd.MM.yyyy"));

                // Дата и время следующего сбора (+ 1 день и 20:30 часов к nowDateNodTame)
                newDatePars = nowDateNodTame.AddDays(1).AddHours(20).AddMinutes(30);

                // Расчитываем время до следующего сбора в милисекундах
                milisec = (newDatePars - nowDate).TotalMilliseconds;
            }

            return milisec;
        }
    }
}
