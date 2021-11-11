using DAL;
using DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    class UI
    {
        // Data members:
        Manager _manage;

        // Ctor:
        public UI(int expirationTimePeriod, int HowMuchTimeForStartTheClock, int maxCapacityPerItem = 20)
        {
            _manage = new Manager(PrintSelfCheckLog, expirationTimePeriod,HowMuchTimeForStartTheClock,maxCapacityPerItem);
        }

        // The main program:
        public void StartProgram()
        {
            // Sets the culture to English - For showing the coppy right sign.
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");

            Console.OutputEncoding = Encoding.UTF8;

            do
            {
                Console.Clear();
                Console.WriteLine("\n-------------------------------------------\n");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\t-------------------");
                Console.WriteLine("\t|  BOXES PROJECT  |");
                Console.WriteLine("\t-------------------\n");
                Console.ResetColor();
                Console.WriteLine("\t© Made by Dor Schreiber\n");
                Console.WriteLine("Pick an option by entering 1-3");
                Console.WriteLine("1. Fill Box.");
                Console.WriteLine("2. Show exist boxes.");
                Console.WriteLine("3. Find best box for a requested size.\n");
                Console.WriteLine("<><><><><><><><><><><><><><><><><><><><><><>\n");
                Console.WriteLine(" => For explanation about the colors\n     meanning write '? / help'.\n");
                Console.WriteLine(" => For exit and saving the changes\n     write 'exit'.");
                Console.WriteLine("\n-------------------------------------------\n");
                string input = Console.ReadLine();
                if (input == "exit") break;
                if(input == "?" || input == "help") ExplainUserMethod();
                int realInput;
                bool isNum = int.TryParse(input, out realInput);
                // Check if the input is a number and the number between 1-3.
                if (!isNum || realInput < 1 || realInput > 3) continue;
                ReferToManagar(realInput);
            } while (true);
            // out of the program, only when exiting.

            // save the binary tree to the data base in here. (future plan...)
        }

        private void ExplainUserMethod()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("-- DarkYellow color is for suggestion before the order.");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("-- Blue color means we draw from the inventory.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-- Yellow color means we draw the last item in the exact hight.");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("-- Dark Magenta color means we draw the last item in this size.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("-- Red color is for the self check notification.");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("-- Cyan color is for filling and finding errors.");
            DoneMsg();
        }

        private void ReferToManagar(int number)
        {
            switch (number)
            {
               case 1: // Fill boxes.
                    {
                        FillBoxes();
                        DoneMsg();
                    }
                    break;
                case 2: // Print the binary tree.
                    {
                        _manage.Print();
                        DoneMsg();
                    }
                    break;
                case 3: // Find best box.
                    {
                        FindBestBox();
                        DoneMsg();
                    }
                    break;
            }
        }

        private void FillBoxes()
        {
            int baseSize = 0, hightSize = 0, amount = 0;
            EnterBaseAndHight(ref baseSize, ref hightSize);
            if (baseSize == 0 || hightSize == 0) return;
            WithOrWithoutQuantity(ref amount, 20);
            if (amount == -1) return;
            try
            {
                _manage.FillBox(baseSize, hightSize, amount);
            }
            catch (Exception ex)
            {
                MessageWithColor(ex.Message, UIPrompt.FILL_OR_FIND_ERROR);
                Console.WriteLine("Press enter to return.");
                Console.Read();
            }
        }

        private void FindBestBox()
        {
            int baseSize = 0, hightSize = 0, quantity = 0;
            EnterBaseAndHight(ref baseSize, ref hightSize);
            if (baseSize == 0 || hightSize == 0) return;
            WithOrWithoutQuantity(ref quantity, 1);
            if (quantity == -1) return;
            try
            {
                LogWithStatus output = _manage.FindPotentialBoxes(baseSize, hightSize, quantity);
                MessageWithColor(output.Output, output.Status);
                SuggestOrder();
            }
            catch (Exception ex)
            {
                MessageWithColor(ex.Message, UIPrompt.FILL_OR_FIND_ERROR);
                Console.WriteLine("Press enter to return.");
                Console.Read();
            }

            // Interanl method:
            void SuggestOrder()
            {
                do
                {
                    Console.WriteLine("Would you like to accept the order? y/n");
                    var key = Console.ReadKey();
                    Console.ReadLine();
                    char c = key.KeyChar;
                    if (c == 'y')
                    {
                        List<LogWithStatus> output = _manage.DrawBoxesFromInventory(baseSize, hightSize, quantity);
                        PrintTheRemove(output);
                        return;
                    }
                    else if (c == 'n') return;
                } while (true);

                // Internal method:
                void PrintTheRemove(List<LogWithStatus> output)
                {
                    foreach (var item in output)
                        MessageWithColor(item.Output, item.Status);
                }
            }
        }

        private void WithOrWithoutQuantity(ref int quantity, int deafualtVal)
        {
            do
            {
                Console.WriteLine($"Do you want to change the default amount from {deafualtVal}? ");
                Console.WriteLine("For changeing write 'y'");
                Console.WriteLine("Or write 'n'");
                var key = Console.ReadKey();
                Console.ReadLine();
                char c = key.KeyChar;
                if (c == 'y')
                {
                    bool isNum;
                    do
                    {
                        Console.WriteLine("Enter requested quantity: ");
                        Console.WriteLine("Back to the menu write 'return'. \n");
                        string tmp = Console.ReadLine();
                        if (tmp == "return")
                        {
                            quantity = -1;
                            return;
                        }
                        isNum = int.TryParse(tmp, out quantity);
                    } while (!isNum);
                    return;
                }
                else if (c == 'n')
                {
                    quantity = deafualtVal;
                    return;
                }
            } while (true);
        }

        private void EnterBaseAndHight(ref int baseSize, ref int hightSize)
        {
            bool isNum;
            do
            {
                Console.WriteLine("Enter base size: ");
                Console.WriteLine("Back to the menu write 'return'. ");
                string tmp = Console.ReadLine();
                if (tmp == "return") return;
                isNum = int.TryParse(tmp, out baseSize);
                if (baseSize == 0) isNum = false;
            } while (!isNum);
            do
            {
                Console.WriteLine("Enter hight size:");
                Console.WriteLine("Back to the menu write 'return'. ");
                string tmp = Console.ReadLine();
                if (tmp == "return") return;
                isNum = int.TryParse(tmp, out hightSize);
                if (hightSize == 0) isNum = false;
            } while (!isNum);
            return;
        }

        private void MessageWithColor(string input, UIPrompt ui_prompt)
        {
            switch(ui_prompt){
                case UIPrompt.DRAW_FROM_INVENTORY:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case UIPrompt.DRAW_LAST_ITEM_EXACT_HIGHT:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case UIPrompt.DRAW_LAST_ITEM_IN_EXACT_BASE_SIZE:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;

                case UIPrompt.SELF_CHECK_NOTIFICATION:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case UIPrompt.FILL_OR_FIND_ERROR:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case UIPrompt.DONE:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case UIPrompt.CASUAL:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case UIPrompt.SUGGEST_ORDER:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
            }

            Console.WriteLine(input);
            Console.ResetColor();
        }

        private bool PrintSelfCheckLog(string ques) // Predicate method for printing the self check actions.
        {
            var result = ques.Split(new[] { '\r', '\n' });
            for (int i = 1; i < result.Length; i += 2)
            {
                if(i == 1) // Firest line.
                    MessageWithColor(result[i], UIPrompt.SELF_CHECK_NOTIFICATION);
                else if(i == result.Length - 3) // Last line.
                    MessageWithColor(result[i], UIPrompt.DONE);
                else
                    MessageWithColor(result[i], UIPrompt.CASUAL);
            }
            return true;
        }

        private void DoneMsg()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Done!");
            Console.ResetColor();
            Console.WriteLine("Press enter to return.");
            Console.Read();
        }
    }
}
