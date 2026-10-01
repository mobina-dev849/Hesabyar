using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Globalization;

public static class DateHelper
{
    public static string ToPersianDate(this DateTime TransactionDate)
    {
        PersianCalendar pc = new PersianCalendar();

        return $"{pc.GetYear(TransactionDate)}/" +
               $"{pc.GetMonth(TransactionDate):00}/" +
               $"{pc.GetDayOfMonth(TransactionDate):00}";
    }
}
