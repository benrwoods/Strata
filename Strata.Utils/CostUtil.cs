using System;

namespace Strata.Utils {
    public static class CostUtil {

        public static double ApplyDiscount(double total, int percentDiscount) {

            var discountMultiplyer = (100d - percentDiscount) / 100d;
            return Math.Round(total * discountMultiplyer, 2);
        }
    }
}
