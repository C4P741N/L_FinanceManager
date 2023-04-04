using System.Globalization;
using System;

namespace MSota.Models
{
    public class Calendar
    {
        DateTime xto_dateTime;
        DateTime xfrom_dateTime;
        public string to 
        {
            get { return xto_dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"); }
            set { xto_dateTime = Convert.ToDateTime(value); }
        }
        public string from
        {
            get { return xfrom_dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"); }
            set { xfrom_dateTime = Convert.ToDateTime(value); }
        }

    }
}
