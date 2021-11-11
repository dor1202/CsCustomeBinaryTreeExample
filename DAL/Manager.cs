using DataModels;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class Manager
    {
        
        // Data members:
        static Manager _manager;
        Logic _logic;

        // Ctor:
        public Manager(Predicate<string> predicate, int expirationTimePeriod, int HowMuchTimeForStartTheClock, int maxCapacityPerItem = 20)
        {
            _logic = new Logic(predicate, expirationTimePeriod, HowMuchTimeForStartTheClock, maxCapacityPerItem);
        }

        public Manager() // For singeltone.
        {
            _logic = new Logic(null,10, 10,20);
        }

        public static Manager Entities
        {
            get
            {
                if (_manager == null)
                    _manager = new Manager();
                return _manager;
            }
        }

        // Methods:
        public void FillBox(int baseSize,int hightSize,int amount) => _logic.FillBox(baseSize, hightSize, amount);
        public LogWithStatus FindPotentialBoxes(int baseSize, int hightSize, int quantity) => _logic.GetPotentialBoxesPublic(baseSize, hightSize, quantity);
        public List<LogWithStatus> DrawBoxesFromInventory(int baseSize, int hightSize, int quantity) => _logic.DrawBoxesPublic(baseSize, hightSize, quantity);
        public void Print() => _logic.Print();

        /*
         * Methods inside Logic:
         *  FillBox - done!
         *  FindBox - done!
         *  Print - done!
         *  Timer - done!
         *  ContaineInSales -  done!
         *  MoveToStart -  done!
         *  RemoveNodeInSales - done!
         *  Print offer method - done!
         */
    }
}
