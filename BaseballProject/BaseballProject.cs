using System;
using System.IO;

/* Author:  Andrew Davidson
 * Date:    03/04/2019
 * 
 * This application tracks baseball players batting average using parallel arrays, which includes a 2d array.
 * The 12 player names are stored in an external data file. The names are read into the program, 
 * and populated into an array. The user can switch between data entry, and summary display during runtime. Data
 * entry allows the user to enter the player number, at bats, and hits. Display summary will display the player names,
 * at bats, hits, and average.
 */


namespace BaseballProject
{
    class BaseballProject
    {
        static string[] arrPlayers = new string[12];
        static int[ , ] arrBatsHits = new int[12,2];

        static void Main(string[] args)
        {
            Init();
            Menu();
        }

        public static void Init()
        {
            LoadPlayersArray();
            InitializeAccumulatingArrays();
        }

        public static void LoadPlayersArray()
        {
            //Sets up FileStream / StreamReader
            FileStream fsPlayers = new FileStream("players.dat", FileMode.Open, FileAccess.Read);
            StreamReader srPlayers = new StreamReader(fsPlayers);

            //Loads player names into arrPlayers
            string record;
            record = srPlayers.ReadLine();
            
            while (record != null)
            {
                for (int x = 0; x < arrPlayers.Length; x++)
                {
                    arrPlayers[x] = record;
                    record = srPlayers.ReadLine();
                }      
            }
        }

        public static void InitializeAccumulatingArrays()
        {
            //Initializes parallel accumulating 2d array to 0.
            for (int x = 0; x < arrBatsHits.GetLength(0); x++)
            {
                for (int y = 0; y < arrBatsHits.GetLength(1); y++)
                {
                    arrBatsHits[x, y] = 0;
                }
            }
        }

        public static void Menu()
        {
            //Displays a menu, and prompts the user to enter an option. Loops until valid.
            bool errSw = true;
            int optionSelect = 0;

            Console.Clear();
            Console.WriteLine("Please select an option:");

            while (errSw)
            {
                Console.WriteLine("1 - Data Entry");
                Console.WriteLine("2 - Display Summary");
                Console.WriteLine("3 - Exit Program");

                try
                {
                    optionSelect = Convert.ToInt32(Console.ReadLine());
                    if (optionSelect < 1 || optionSelect > 3)
                    {
                        Console.WriteLine("\nInvalid option. Please enter 1-3.");
                    }
                    else
                    {
                        errSw = false;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInvalid option. Please enter 1-3.");
                }
            }

            //Calls appropriate method based on option selected.
            switch (optionSelect)
            {
                case 1:
                    Option1();
                    break;
                case 2:
                    Option2();
                    break;
                case 3:
                    Environment.Exit(1);
                    break;
            }
        }

        public static void Option1()
        {
            bool errSw;
            int playerNum = 0, bats = 0, hits = 0;
            string again = "Y";

            //Calls methods to prompt for data entry. Loops until valid and hits do not exceed at bats.
            while (again == "Y" || again == "y")
            {
                Console.Clear();
                errSw = true;

                while (errSw)
                {
                    playerNum = GetPlayerNum();
                    bats = GetAtBats();
                    hits = GetHits();

                    if (hits > bats)
                    {
                        Console.WriteLine("\nError! Hits can't be greater than At Bats. Please re-enter data.");
                    }
                    else
                    {
                        errSw = false;
                    }
                }

                //Places data entered into appropriate slots of arrBatsHits (one off).
                arrBatsHits[playerNum - 1, 0] += bats;
                arrBatsHits[playerNum - 1, 1] += hits;

                //Asks the user if they want to enter more data.
                Console.WriteLine("Data submitted! Would you like to enter more data? (Y/N)");
                again = Console.ReadLine();
            }

            Menu();
        }

        public static int GetPlayerNum()
        {
            int p = 0;
            bool errSw = true;

            //Prompts the user to enter a Player Number (1-12). Loops until valid.
            while (errSw)
            {
                Console.WriteLine("Enter Player Number (1-12).");
                try
                {
                    p = Convert.ToInt32(Console.ReadLine());
                    if (p < 1 || p > 12)
                    {
                        Console.WriteLine("\nInvalid Player Number. Please enter 1-12.");
                    }
                    else
                    {
                        errSw = false;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInvalid Player Number. Please enter 1-12.");
                }
            }

            return p;
        }

        public static int GetAtBats()
        {
            int b = 0;
            bool errSw = true;

            //Prompts the user to enter "at bats" for their entered player. Loops until valid.
            while (errSw)
            {
                Console.WriteLine("Enter number of At Bats.");
                try
                {
                    b = Convert.ToInt32(Console.ReadLine());
                    if (b < 0)
                    {
                        Console.WriteLine("\nInvalid At Bats. Please enter a whole number.");
                    }
                    else
                    {
                        errSw = false;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInvalid At Bats. Please enter a whole number.");
                }
            }

            return b;
        }

        public static int GetHits()
        {
            int h = 0;
            bool errSw = true;

            //Prompts the user to enter hits for their entered player. Loops until valid.
            while (errSw)
            {
                Console.WriteLine("Enter number of Hits.");
                try
                {
                    h = Convert.ToInt32(Console.ReadLine());
                    if (h < 0)
                    {
                        Console.WriteLine("\nInvalid Hits. Please enter a whole number.");
                    }
                    else
                    {
                        errSw = false;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInvalid Hits. Please enter a whole number.");
                }
            }

            return h;
        }

        public static void Option2()
        {
            //Calls method to print the summary screen. Stays on summary screen until user presses enter. 
            Console.Clear();

            PrintSummary();

            Console.WriteLine("\nPress Enter to return to main menu...");
            Console.ReadLine();
            Menu();
        }

        public static void PrintSummary()
        {
            double avg = 0;
            string avgString = "";

            //Header
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine(String.Format("{0,-16} | {1,-7} | {2,-4} | {3,-7}", "Player Name", "At Bats", "Hits", "Average"));
            Console.WriteLine("-------------------------------------------");

            //Calls method to calculate average (passing x), and then formats / prints summary report
            for (int x = 0; x < arrBatsHits.GetLength(0); x++)
            {
                avg = CalculateAverage(x);
                avgString = String.Format("{0:#.000}", avg);
                Console.WriteLine(String.Format("{0,-16} | {1,7} | {2,4} | {3,7}", arrPlayers[x], arrBatsHits[x, 0], arrBatsHits[x, 1], avgString));     
            }
        }

        public static double CalculateAverage(int x)
        {
            if (arrBatsHits[x, 0] == 0)
            {
                return 0;
            }
            else
            {
                return 1.0 * arrBatsHits[x, 1] / arrBatsHits[x, 0];
            }
        }
    }
}
