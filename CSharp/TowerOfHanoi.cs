using System;

namespace TowerOfHanoi
{
    class TowerOfHanoi
    {
        int numDics;
        public TowerOfHanoi()
        {
            numDics = 0;
        }

        public TowerOfHanoi(int value)
        {
            numDics = value;
        }

        public int dicsNumbers
        {
            get { return numDics; }
            set
            {
                if (value > 0)
                    numDics = value;
            }
        }

        public int MoveTower(int i, int from, int to, int othter)
        {
            int count = 0;
            if (i > 0)
            {
                count = MoveTower(i - 1, from, othter, to);
                Console.WriteLine($"Move disk {i} from tower {from} to tower{to}");
                count++;
                count += MoveTower(i - 1, othter, to, from);
            }
            return count;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            TowerOfHanoi tower = new TowerOfHanoi();
            string sNumdisc;
            Console.WriteLine("Enter the number of discs: ");
            sNumdisc = Console.ReadLine();

            // tower.dicsNumbers = Convert.ToInt32(sNumdisc);
            if (int.TryParse(sNumdisc, out int n))
            {
                tower.dicsNumbers = n;
            }
            else
            {
                // sNumdisc = "10";    
                // tower.dicsNumbers = 10;
                Console.WriteLine("Please Numbers only");
                return;
            }
            Console.WriteLine($"Count : {tower.MoveTower(tower.dicsNumbers, 1, 3, 2)}");
            Console.WriteLine(Math.Pow(2, Convert.ToInt32(sNumdisc)) - 1);
            sw.Stop();
            Console.WriteLine($"End Time : {sw.Elapsed.ToString()}");
            Console.ReadLine();
        }
    }
}
