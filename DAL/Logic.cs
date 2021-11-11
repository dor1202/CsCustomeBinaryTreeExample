using CollectionsGeneric;
using System;
using System.Threading;
using DataModels;
using System.Text;
using System.Collections.Generic;

namespace DAL
{
    public class Logic
    {
        // Predicate:
        Predicate<string> UserPrompt;

        // Properties:
        public BinaryTree<Box> BinaryTreeBoxes { get; set; }

        // Data members:
        Timer _timer;
        DoubleLinkedList<RecentSales> _salesByDate;
        static int _numberOfTries;
        int _isRecursive;
        int _hightSizeRequired;
        int _maxCapacityPerItem;

        // ctor:
        public Logic(Predicate<string> predicate, int expirationTimePeriod, int HowMuchTimeForStartTheClock, int maxCapacityPerItem = 20)
        {
            UserPrompt = predicate;
            BinaryTreeBoxes = new BinaryTree<Box>();
            _salesByDate = new DoubleLinkedList<RecentSales>();
            Boot(BinaryTreeBoxes); // for filling the trees and the linked list.
            TimeSpan dueTime = new TimeSpan(0, 0, 0, HowMuchTimeForStartTheClock);
            TimeSpan period = new TimeSpan(0, 0, 0, expirationTimePeriod);
            _timer = new Timer(CheckExpaired, null, dueTime, period);
            _maxCapacityPerItem = maxCapacityPerItem;
            // Same as the mainPage,
            // The second int means how much time since starting the program invoking the timer,
            // the first int means hwow much time between invokes.
        }

        // Methods:
        private void Boot(BinaryTree<Box> tree)
        {
            // testing the invoke method for removing 1X1 , 1X2 and 1X3 boxes.
            Box d1 = new Box(1);
            d1.BinaryTreeHight.Add(new BoxHightDetails(1, new DateTime(2019, 8, 13)));
            _salesByDate.AddFirst(new RecentSales(1, 1, new DateTime(2019, 8, 13)));
            d1.BinaryTreeHight.Add(new BoxHightDetails(2, new DateTime(2019, 8, 11)));
            _salesByDate.AddFirst(new RecentSales(1, 2, new DateTime(2019, 8, 11)));
            d1.BinaryTreeHight.Add(new BoxHightDetails(3, new DateTime(2020, 8, 29)));
            _salesByDate.AddFirst(new RecentSales(1, 3, new DateTime(2020,8,29)));
            d1.BinaryTreeHight.Add(new BoxHightDetails(4, new DateTime(2020, 8, 13)));
            _salesByDate.AddFirst(new RecentSales(1, 4));
            tree.Add(d1);

            // Simple boxes.
            for (int i = 2; i <= 6; i++)
            {
                Box data = new Box(i);
                for (int j = 1; j <= 4; j++)
                {
                    data.BinaryTreeHight.Add(new BoxHightDetails(j, new DateTime(2020, 8, 13)));
                    _salesByDate.AddFirst(new RecentSales(i, j));
                }
                tree.Add(data);
            }
        }

        private void CheckExpaired(object obj)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\n*Checking for expired boxes, please don't touch the keyboard*");
            var endNode = _salesByDate.end;
            do
            {
                double gapInDays = (DateTime.Now - endNode.data.Date).TotalDays;
                if (gapInDays > 14)
                {
                    sb.AppendLine($"    => Removing: size {endNode.data.Size} hight {endNode.data.Hight} gap {gapInDays:F0} days.");

                    _salesByDate.RemoveLast();
                    
                    Box box = new Box(endNode.data.Size);
                    BinaryTreeBoxes.FindBestMatch(box, out box); // O(log(n))
                    box.BinaryTreeHight.Remove(new BoxHightDetails(endNode.data.Hight, new DateTime())); // O(log(n))

                    endNode = endNode.prev;

                    // Inside the loop we:
                    // removing the box from the tree (inventory), removing the box from the buyByDate collection, and going 1 node back.
                }
                else break;
            } while (true);
            sb.AppendLine("Done!");
            var meanningLess = UserPrompt(sb.ToString()); // for displaying the actions.
            // i wanted to use a discarded bool "_", but i don't know if the tester have above c# 7.0
            // _ = UserPrompt(sb.ToString());
        }

        public void FillBox(int baseSize, int hightSize, int fillAmount)
        {
            Box box = BinaryTreeBoxes.Contain(new Box(baseSize)); // O(log(n))
            if (box != default(Box) || box != null) // exist box size.
            {
                BoxHightDetails hight = box.BinaryTreeHight.Contain(new BoxHightDetails(hightSize, DateTime.Now)); // O(log(n))

                if (hight != default(BoxHightDetails) || hight != null) // exist hight.
                {
                    if (hight.Amount + fillAmount > _maxCapacityPerItem)
                    {
                        hight.Amount = _maxCapacityPerItem;
                        hight.LastPurchaseDate = DateTime.Now;
                        UpdateSales(box.Size, hight.Hight);
                        throw new Exception("Order With exist amount surpassing the max amount of an item. Filled to the max capacity.");
                    }
                    else
                    {
                        hight.Amount += fillAmount;
                        hight.LastPurchaseDate = DateTime.Now;
                        UpdateSales(box.Size, hight.Hight);
                    }
                }

                else // add new hight.
                {
                    BoxHightDetails newHight = new BoxHightDetails(hightSize, DateTime.Now);
                    box.BinaryTreeHight.Add(newHight);
                    UpdateSales(box.Size, newHight.Hight);
                }
            }
            else // add new box with a new hight.
            {
                Box newBox = new Box(baseSize);
                BoxHightDetails newHight = new BoxHightDetails(hightSize, DateTime.Now);
                newBox.BinaryTreeHight.Add(newHight); // O(log(n))
                BinaryTreeBoxes.Add(newBox); // O(log(n))
                UpdateSales(newBox.Size, newHight.Hight);
            }
        }

        private void UpdateSales(int size, int hight)
        {
            RecentSales sale = new RecentSales(size, hight);
            if (_salesByDate.Contain(ref sale)) // O(n)
            {
                sale.Date = DateTime.Now;
                _salesByDate.MoveToStart(sale); // O(n)
            }
            else
                _salesByDate.AddFirst(new RecentSales(size, hight)); // O(1)
        }

        public List<LogWithStatus> DrawBoxesPublic(int baseSize, int hightSize, int quantity)
        {
            List<LogWithStatus> outputList = new List<LogWithStatus>();
            _hightSizeRequired = hightSize;
            DrawBoxes(baseSize, hightSize, quantity, outputList);
            _hightSizeRequired = 0;
            return outputList;
            // Sending a log about the actions taken in the collection.
        }
        private void DrawBoxes(int baseSize, int hightSize, int quantity, List<LogWithStatus> outputList)
        {
            if (_numberOfTries < 3)
            {
                // Finding closest Box.
                Box box = FindBox();

                _numberOfTries = 0; // For trying inside the DataX 3 time.

                // Finding closest hight inside the box.
                BoxHightDetails hight = FindHight(box);

                if (hight.Amount > quantity) // more then one item in the inventory.
                    NotAllTheItemsTaken(box, hight);

                else // last item in the inventory.
                {
                    LastItemInInventory(box, hight);

                    if (box.BinaryTreeHight.GetDepth() == 0) // Empty tree.
                        EmptyTreeOption(box);

                    if (quantity != 0) // More boxes required.
                        MoreBoxesWanted(box, hight);

                    if (_isRecursive != 0) // fold the recursion when reach to 0 quantity, if not recursive continue.
                    {
                        UpdateSales(box.Size, hight.Hight);
                        _isRecursive--;
                        return;
                    }
                }
            }

            else // Out of tries.
            {
                _numberOfTries = 0;
                throw new Exception("Couldn't find boxes in the requested size.");
            }

            // Internal methods:
            Box FindBox()
            {
                Box box = new Box(baseSize);
                try { BinaryTreeBoxes.FindBestMatch(box, out box); } // O(log(n))
                catch { _numberOfTries++; DrawBoxes(baseSize + 1, hightSize, quantity, outputList); } // adding a try and trying with a bigger one.
                return box;
            }

            BoxHightDetails FindHight(Box box)
            {
                BoxHightDetails hight = new BoxHightDetails(hightSize, DateTime.Now);
                try { box.BinaryTreeHight.FindBestMatch(hight, out hight); } // O(log(n))
                catch { _numberOfTries++; DrawBoxes(box.Size, hightSize + 1, quantity, outputList); } // adding a try and trying with a bigger one.
                return hight;
            }

            void NotAllTheItemsTaken(Box box, BoxHightDetails hight)
            {
                hight.Amount -= quantity;
                hight.LastPurchaseDate = DateTime.Now;
                outputList.Add(new LogWithStatus($"Draw box for the customer: size {box.Size} hight {hight.Hight}", UIPrompt.DRAW_FROM_INVENTORY));
            }

            void LastItemInInventory(Box box, BoxHightDetails hight)
            {
                quantity -= hight.Amount;
                box.BinaryTreeHight.Remove(hight); // O(log(n))
                _salesByDate.RemoveNode(new RecentSales(box.Size, hight.Hight)); // O(n)

                outputList.Add(new LogWithStatus($"Draw box for the customer: size {box.Size} hight {hight.Hight}", UIPrompt.DRAW_FROM_INVENTORY));
                outputList.Add(new LogWithStatus($"Deleted from Box size {box.Size} the hight {hight.Hight}, Cause : last item in the picked hight.", UIPrompt.DRAW_LAST_ITEM_EXACT_HIGHT));
            }

            void EmptyTreeOption(Box box)
            {
                BinaryTreeBoxes.Remove(box); // O(log(n))
                outputList.Add(new LogWithStatus($"Deleted Box size {box.Size}, Cause : last item in the picked size", UIPrompt.DRAW_LAST_ITEM_IN_EXACT_BASE_SIZE));
            }

            void MoreBoxesWanted(Box box, BoxHightDetails hight)
            {
                _isRecursive++;
                if (box.BinaryTreeHight.Root == null) // Empty hight, move to the next base.
                    DrawBoxes(box.Size + 1, _hightSizeRequired, quantity, outputList);
                else if (hightSize.CompareTo(box.BinaryTreeHight.MaxVal().Hight) >= 0) // After deleting the hight we check if the exist max is smaller.
                    DrawBoxes(box.Size + 1, _hightSizeRequired, quantity, outputList);
                else
                    DrawBoxes(box.Size, hight.Hight + 1, quantity, outputList);
            }
        }

        public LogWithStatus GetPotentialBoxesPublic(int baseSize, int hightSize, int quantity)
        {
            StringBuilder sb = new StringBuilder();
            _hightSizeRequired = hightSize;
            GetPotentialBoxes(baseSize, hightSize, quantity, sb);
            _hightSizeRequired = 0;
            LogWithStatus logWithStatus = new LogWithStatus(sb.ToString(), UIPrompt.SUGGEST_ORDER);
            return logWithStatus;
        }
        private void GetPotentialBoxes(int baseSize, int hightSize, int quantity, StringBuilder sb)
        {
            if (_numberOfTries < 3)
            {
                // Finding closest Box.
                Box box = FindBox();

                _numberOfTries = 0; // For trying inside the DataX 3 time.

                // Finding closest hight inside the box.
                BoxHightDetails hight = FindHight(box);

                if (hight.Amount > quantity) // more then one item in the inventory.
                {
                    NotAllTheItemsTaken(box, hight);
                }
                else // last item in the inventory.
                    LastItemInInventoryOption(box, hight);
            }

            else // Out of tries.
            {
                _numberOfTries = 0;
                throw new Exception("Couldn't find the requested order.");
            }

            // Internal methods:
            Box FindBox()
            {
                Box box = new Box(baseSize);
                try { BinaryTreeBoxes.FindBestMatch(box, out box); } // O(log(n))
                catch { _numberOfTries++; GetPotentialBoxes(baseSize + 1, hightSize, quantity, sb); } // adding a try and trying with a bigger one.
                return box;
            }

            BoxHightDetails FindHight(Box box)
            {
                BoxHightDetails hight = new BoxHightDetails(hightSize, DateTime.Now);
                try 
                {
                    box.BinaryTreeHight.FindBestMatch(hight, out hight); // O(log(n))
                }
                catch { _numberOfTries++; GetPotentialBoxes(box.Size, hightSize + 1, quantity, sb); } // adding a try and trying with a bigger one.
                return hight;
            }

            void NotAllTheItemsTaken(Box box, BoxHightDetails hight)
            {
                sb.AppendLine($"Avilable box for the customer: size {box.Size} hight {hight.Hight} amount {hight.Amount}.");
                quantity -= hight.Amount;
            }

            void LastItemInInventoryOption(Box box, BoxHightDetails hight)
            {
                sb.AppendLine($"Avilable box for the customer: size {box.Size} hight {hight.Hight} amount {hight.Amount}.");
                quantity -= hight.Amount;

                if (quantity != 0) // More boxes required.
                    if (hightSize.CompareTo(box.BinaryTreeHight.MaxVal().Hight) == 0)
                        GetPotentialBoxes(box.Size + 1, _hightSizeRequired, quantity, sb);
                    else
                        GetPotentialBoxes(box.Size, hight.Hight + 1, quantity, sb);
            }
        }

        public void Print() => BinaryTreeBoxes.PrintInOrder(); // O(n)
    }
}
