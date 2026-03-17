using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPCaseManager
{
    public class DateCalcs
    {

        private static int[] mday = {
        0,              // Jan
        31,             // Feb
        59,             // March
        90,             // April
        120,            // May
        151,            // June
        181,            // July
        212,            // August
        243,            // September
        273,            // October
        304,            // November
        334,            // December
        365,            // Jan (reflect)
        396,            // Feb (reflect)
};

        /// <summary>
        /// Compute Mountain Standard time from the current time zone UTC
        /// </summary>
        /// <returns>DateTime.Now adjusted to Mountain Standard Time</returns>
        public static DateTime StandardMountainUTC()
        {
            int MST_OFFSET = -7;
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime localTime = localZone.ToLocalTime(DateTime.Now.ToUniversalTime());

            // Mountain Standard Time offset frpm UTC
            DateTime MtnUtc = localTime.Add(new TimeSpan(MST_OFFSET, 0, 0));
            // Check daylight savings time span
            if (localZone.IsDaylightSavingTime(MtnUtc))
            //if ((MtnUtc >= dstStartDate(MtnUtc.Year)) && (MtnUtc <= dstEndDate(MtnUtc.Year)))
            {
                MtnUtc = MtnUtc.AddHours(1);
            }

            return (MtnUtc.ToUniversalTime());
        }

        /// <summary>
        /// DateAdd recreation of the Visual Basic function. <br></br>
        /// Converted from the C++ code in VisionPlus
        /// /*Sample Code:
        ///    * EndDate = DateAdd('d', -1, StartDate); 
        ///    * */
        /// </summary>
        /// <author>Rick Justesen</author>
        /// <param name="interval"></param>
        /// <param name="span"></param>
        /// <param name="startDate"></param>
        /// <returns>DateTime</returns>

        public static DateTime DateAdd(char interval, int span, DateTime startDate)
        {

            int start_yr, start_mo, start_day;
            start_yr = startDate.Year;
            start_mo = startDate.Month;
            start_day = startDate.Day;

            switch (interval)
            {
                case 'y':
                    start_yr += span;
                    break;
                case 'q':
                    start_yr += (span / 2);
                    start_mo += (span % 2) * 6;
                    if (start_mo > 12)
                    {
                        start_yr++;
                        start_mo -= 12;
                    }
                    break;
                case 'm':
                    if ((start_mo + span) > 12)
                    {
                        start_yr += ((start_mo + span) / 12);
                        start_mo = ((start_mo + span) % 12);
                    }
                    else
                    {
                        start_mo += span;
                    }
                    break;
                case 'w':
                case 'd':
                default:
                    // throw new Exception("DateAdd does not support these intervals");
                    break;
            }

            return (new DateTime(start_yr, start_mo, start_day));
        }

        /// <summary>
        ///  Given a VB date object compute the actual .NET DateTime
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static DateTime ConvertVBDate(int days)
        {

            DateTime dt = new DateTime(1899, 12, 30);
            DateTime birthdate = dt.AddDays(days);
            return birthdate;
        }

        /// <summary>
        ///  GConvert a .NET dateTime to a VB date (int)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static int ConvertVBDate(DateTime date)
        {
            DateTime origDate = new DateTime(1899, 12, 30);
            int days = Convert.ToInt32(DateCalcs.DateDiff("d", origDate, date));
            return days;
        }

        /// <summary>
        /// recreation of the Visual Basic function
        ///     /*Sample Code:
        ///    * System.DateTime dt1 = new System.DateTime(1974,12,16);
        ///    * System.DateTime dt2 = new System.DateTime(1973,12,16);
        ///    * System.Console.Write(Convert.ToString(DateDiff("t", dt1, dt2)));
        ///    * */
        /// </summary>
        /// <param name="howtocompare"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>double</returns>
        /// 
        public static double DateDiff(string howtocompare, System.DateTime startDate, System.DateTime endDate)
        {
            double diff = 0;
            try
            {
                System.TimeSpan TS = new System.TimeSpan(endDate.Ticks - startDate.Ticks);
                switch (howtocompare.ToLower())
                {
                    case "n":
                        diff = Convert.ToDouble(TS.TotalMinutes);
                        break;
                    case "s":
                        diff = Convert.ToDouble(TS.TotalSeconds);
                        break;
                    case "t":
                        diff = Convert.ToDouble(TS.Ticks);
                        break;
                    case "mm":
                        diff = Convert.ToDouble(TS.TotalMilliseconds);
                        break;
                    case "ww":
                        diff = Convert.ToDouble(TS.TotalDays / 7);
                        break;
                    case "m":
                        diff = Convert.ToDouble((TS.TotalDays / 365) * 12);
                        break;
                    case "yyyy":
                        diff = Convert.ToDouble(TS.TotalDays / 365);
                        break;
                    case "q":
                        diff = Convert.ToDouble((TS.TotalDays / 365) / 4);
                        break;
                    case "d":
                        diff = Convert.ToDouble(TS.TotalDays);
                        break;
                    default:
                        //d
                        diff = Convert.ToDouble(TS.TotalDays);
                        break;
                }
            }
            catch
            {
                diff = -1;
            }
            return diff;
        }

        public static double Days360(DateTime StartDate, DateTime EndDate, bool method)
        {
            double TempMonths;
            int StartDay;
            int EndDay;

            StartDay = StartDate.Day;
            EndDay = EndDate.Day;
            if (!method)
            {
                if (StartDay > 30)
                {
                    StartDate = DateAdd('d', -1, StartDate);
                }
                if (EndDay == 31 && StartDay > 30)
                {
                    EndDate = DateAdd('d', 1, EndDate);
                }
                else
                {
                    if (EndDay == 31)
                    {
                        EndDate = DateAdd('d', -1, EndDate);
                    }
                }
            }
            else
            {
                if (StartDay > 30)
                {
                    StartDate = DateAdd('d', -1, StartDate);
                }
                if (EndDay > 30)
                {
                    EndDate = DateAdd('d', -1, EndDate);
                }
            }
            StartDay = StartDate.Day;
            EndDay = EndDate.Day;
            TempMonths = DateDiff("m", StartDate, EndDate);

            return (TempMonths * 30) + (EndDay - StartDay);
        }

        public static double AgeFromDate(DateTime startDate, DateTime refDate)
        {
            double varAge = 0;
            try
            {
                DateTime dt = new DateTime(refDate.Year, startDate.Month, startDate.Day);

                varAge = DateDiff("yyyy", startDate, refDate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Math.Round(varAge, 1);
        }

        public static DateTime DateFromAge(int age)
        {
            DateTime date;
            date = DateAdd('y', (int)(age * -1), DateTime.Now);
            return date;
        }

        /// <summary>
        /// AgeNearest Computes the age at the birthday nearest the reference date.           
        /// </summary>
        /// <Author>Rick Justesen</Author>
        /// <param name="refdate"> Reference Date</param>
        /// <param name="dob"> Date of Birth</param>
        /// <returns>Age Nearest Birthday</returns>
        public static int AgeNearest(DateTime refdate, DateTime dob)
        {
            int age = 0;
            try
            {
                // Compute birthday with same year as reference date
                DateTime bday = new DateTime(refdate.Year, dob.Month, dob.Day);
                int jdiff;


                // Compute age as of that birthday
                age = (int)DateDiff("yyyy", dob, bday);
                // Figure offset from reference date
                // This will be zero if the reference date is a birthday,
                // positive if the birthday falls after the reference date,
                // or negative if it precedes the reference.
                jdiff = (int)DateDiff("d", bday, refdate);

                if (System.Math.Abs(jdiff) >= 182)
                {
                    // If ref date is not a birthday, nearest birthday will be either
                    // the one computed, or will be offset one year.
                    // Determine which by checking difference against half a year.
                    age += (jdiff < 0L) ? -1 : 1;
                }
            }
            catch
            {
                return age = 0;
            }

            return age;
        }
        /* end AgeNearest(CDATE refdate, CDATE dob) */

        /// <summary>
        /// AgeNext Computes the age at the next birthday after the reference date.          
        /// </summary>
        /// <author>Rick Justesen</author>
        /// <param name="refdate">Reference Date</param>
        /// <param name="dob"></param>
        /// <returns>age at next birthday</returns>
        public static int AgeNext(DateTime refdate, DateTime dob)
        {
            DateTime bday = new DateTime(refdate.Year, dob.Month, dob.Day);
            int jdiff;
            int age;

            // Compute age as of that birthday
            age = (int)DateDiff("yyyy", dob, bday);

            // Figure offset from reference date
            // This will be zero if the reference date is a birthday,
            // positive if the birthday falls after the reference date,
            // or negative if it precedes the reference.
            jdiff = (int)DateDiff("d", refdate, bday);

            if (jdiff < 0L)
            {
                // If the computed birthday preceeds the reference date,
                // add one to the age (go to the next birthday).
                age += 1;
            }

            return age;
        }
        /// <summary>
        /// AgeLast Computes the age at the last birthday after the reference date.          
        /// </summary>
        /// <author>Rick Justesen</author>
        /// <param name="refdate"></param>
        /// <param name="dob"></param>
        /// <returns>age at last birthday</returns>
        public static int AgeLast(DateTime refdate, DateTime dob)
        {
            DateTime bday = new DateTime(refdate.Year, dob.Month, dob.Day);
            int jdiff;
            int age;

            // Compute age as of that birthday
            age = (int)DateDiff("yyyy", dob, bday);

            // Figure offset from reference date
            // This will be zero if the reference date is a birthday,
            // positive if the birthday falls after the reference date,
            // or negative if it precedes the reference.
            jdiff = (int)DateDiff("d", bday, refdate);

            if (jdiff > 0L)
            {
                // If the computed birthday follows the reference date,
                // subtract from the age (go to the last birthday).
                age -= 1;
            }

            return age;
        }


        public static int PackDate(DateTime date)
        {
            return (date.Year * 10000) + (date.Month * 100) + date.Day;
        }

        public static DateTime UnPackDate(int date)
        {
            int[] ymt = ldiv(date, 10000);
            int[] ym = ldiv(date, 10000);
            int[] md = ldiv(ym[1], 100);
            int y = (int)ym[0];
            int m = md[0];
            int d = md[1];

            return new DateTime(y, m, d);
        }
        /* end UnPackDate(CDATE date, int *y, int *m, int *d) */


        public static int Julian(int cdate)
        {

            int jdate;

            DateTime date = UnPackDate(cdate);
            int y = date.Year;
            int m = date.Month;
            int d = date.Day;


            if (m <= 2)
            {
                // Rotate so that feb is at end of year
                y -= 1;
                m += 12;
            }
            m -= 1;     // Shift to zero base

            y -= 1600;

            // Compute number of days @ 365 days/yr 
            // and add a leap day every 4 years
            jdate = (y * 365) + (y / 4);

            // Subtract out a leap day every century
            jdate -= y / 100;

            // And add one every 400 years, including 1600
            jdate += 1 + (y / 400);

            // Now figure days at beginning of month
            jdate += mday[m];

            // And add day of month
            return (jdate + d - 1);
        }

        private static int[] ldiv(int a, int b)
        {
            int remainder, quotient = Math.DivRem(a, b, out remainder);
            return new int[] { quotient, remainder };
        }


        public static DateTime Calendar(int jdate)
        {

            int YEAR_2000 = 146097;      // Julian date of 1/1/2000
            int m, d, y;
            int lenyr1 = 366;
            int century;
            int b = 1600;
            bool noleap = false;
            int leapyr;


            // Figure 400 year cycle to which the date belongs
            // Naturally support just 1600-1999 and 2000-2399.
            if (jdate >= YEAR_2000)
            {
                // If 2000 up, just compute from there
                b = 2000;
                jdate -= YEAR_2000;
            }

            //// Figure correct century. There are 36525 days in the
            //// 0-99 century and 36524 in the rest.
            if (jdate >= 36525)
            {
                jdate -= 36525;
                int[] ldiv_t = ldiv(jdate, 36524);
                century = ldiv_t[0] + 1;
                jdate = ldiv_t[1];
            }
            else
            {
                century = 0;
            }

            // Figure four-year to which the year belongs.
            // There are 1461 days in four years, with exceptions
            if (century > 0)
            {
                // No leap year in first year of century
                if (jdate < 1460)
                {
                    y = 0;
                    d = (int)jdate;
                    lenyr1 = 365;
                    noleap = true;
                }
                else
                {
                    int[] ldt = ldiv(jdate - 1460, 1461);

                    y = 4 + (4 * ldt[0]);
                    d = ldt[1];
                }
            }
            else
            {
                // every 400 years this is easy
                int[] ldt = ldiv(jdate, 1461);
                y = (int)ldt[0] * 4;
                d = (int)ldt[1];
            }

            // Year is now a first estimate with 4 years
            // Pin down to actual year
            y += (century * 100) + b;

            if (d >= lenyr1)
            {
                int[] dt;
                d -= lenyr1;

                dt = ldiv(d, 365);
                y = y + 1 + dt[0];
                d = dt[1];
            }

            // Year is now correct.
            // d is day of year.
            // Find month and day of month
            leapyr = !noleap && ((y & 3) == 0) ? 0 : 1;

            if (d < 31)
            {
                // Jan
                m = 1;
            }
            else if (d < (59 + leapyr))
            {
                // Feb
                m = 2;
                d -= 31;
            }
            else
            {
                // Mar - Dec - correct for leap 
                d -= leapyr;
                for (m = 11; m > 1; m--)
                {
                    if (d >= mday[m])
                    {
                        d -= mday[m];
                        break;
                    }
                }
                m += 1;
            }

            return new DateTime(y, m, d + 1);
        }


    }


}
