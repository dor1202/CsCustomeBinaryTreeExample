using System;

namespace DataModels
{
    public class BoxHightDetails : IComparable<BoxHightDetails>
    {
        public int Hight { get; set; }
        public int Amount { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public BoxHightDetails(int hight, DateTime lastPurchaseDate,int amount = 1)
        {
            Hight = hight;
            Amount = amount;
            LastPurchaseDate = lastPurchaseDate;
        }

        public int CompareTo(BoxHightDetails other)
        {
            if (other != null)
                return this.Hight.CompareTo(other.Hight);
            else
                throw new ArgumentException("Object isn't DataY");
        }

        public override string ToString()
        {
            return $"  * Hight size: {Hight}, Amount: {Amount}, Last purchase date: {LastPurchaseDate:dd/MM/yyyy}.";
        }
    }
}
