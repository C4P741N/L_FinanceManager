using System.Globalization;

namespace MSota.Models
{
    public class Calendar_II
    {
        public DateTime xto_dateTime;
        public string xfrom_dateTime;

        public string fromDate { get; set; }
        public string toDate { get; set; }

        //public DateTime toDate
        //{
        //    get { return xto_dateTime; }
        //    set { xto_dateTime = Convert.ToDateTime(value); }
        //}

        public DateTime dt_ToDate
        {
            get { return xto_dateTime; }
            set
            {
                DateTime parsedDateTime;
                if (DateTime.TryParseExact(toDate, "ddd MMM dd HH:mm:ss 'GMT'zzz yyyy",
                                           CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime))
                {
                    xto_dateTime = parsedDateTime;
                }
                else
                {
                    // Handle the case where the conversion fails
                    throw new ArgumentException("Invalid date format");
                }
            }
        }


        //public DateTime toDate
        //{
        //    get { return xto_dateTime; }
        //    set => xto_dateTime = DateTime.TryParse(value, out var parsedDate) ? parsedDate : DateTime.MinValue;
        //}


        //public DateTime fromDate
        //{
        //    get { return xfrom_dateTime; }
        //    set { xfrom_dateTime = Convert.ToDateTime(value); }
        //}


    }
}
