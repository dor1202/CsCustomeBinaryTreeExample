using System;

namespace DataModels
{
    public class RecentSales : IComparable<RecentSales>
    {
        public int Size { get; set; }
        public int Hight { get; set; }
        public DateTime Date { get; set; }
        public RecentSales(int size, int hight)
        {
            Size = size;
            Hight = hight;
            Date = DateTime.Now;
        }

        public RecentSales(int size, int details, DateTime date) // For testing the timer.
        {
            Size = size;
            Hight = details;
            Date = date;
        }

        public int CompareTo(RecentSales other)
        {
            if (other != null)
            {
                var result = this.Size.CompareTo(other.Size);
                if (result == 0)
                {
                    var nextResult = this.Hight.CompareTo(other.Hight);
                    if (nextResult == 0)
                        return 0;
                    return nextResult;
                }
                return result;
            }    
            else
                throw new ArgumentException("Object isn't RecentSales");
        }
    }
}
