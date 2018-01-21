using System;
using System.Linq;

namespace Strata.Utils {
    public static class DateUtil {

        private static DayOfWeek[] Weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

        public static DateTime GetDateInXWorkingDays(this DateTime date, int xDays) {
            var count = xDays;
            DateTime newDate = date;

            while (count > 0) {
                newDate = newDate.AddDays(1);
                if (newDate.IsWorkingDay()) {
                    count--;
                }
            }

            return newDate;
        }

        private static bool IsWorkingDay(this DateTime date) {
            if (Weekend.Contains(date.DayOfWeek)) return false;

            return true;
        }
    }
}
