using System;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main()
        {
            Person p = new("Jone", new ClockTower());
            p.ChimeFivePM();
            p.ChimeSixAM();
        }
    }

    public class Person
    {
        private readonly string name;
        private readonly ClockTower tower;

        public Person(string name, ClockTower tower)
        {
            this.name = name;
            this.tower = tower;

            this.tower.Chime += (s, e) =>
            {
                System.Console.WriteLine($"{this.name} heard the clock chime. {e.Time} o'clock");
                switch (e.Time)
                {
                    case 6:
                        System.Console.WriteLine($"{this.name} is waking up.");
                        break;
                    case 17:
                        System.Console.WriteLine($"{this.name} is going home.");
                        break;
                    default:
                        System.Console.WriteLine($"No Schedule");
                        break;
                }
            };
        }

        public void ChimeFivePM()
        {
            tower.ChimeFivePM();
        }

        public void ChimeSixAM()
        {
            tower.ChimeSixAM();
        }
    }

    public class ClockTowerEventArgs : EventArgs
    {
        public int Time { get; set; }
    }

    public delegate void ChimeEventHandler(object sender, ClockTowerEventArgs args);

    public class ClockTower
    {
        public event ChimeEventHandler Chime;
        public void ChimeFivePM() => Chime?.Invoke(this, new ClockTowerEventArgs { Time = 17 });
        public void ChimeSixAM() => Chime?.Invoke(this, new ClockTowerEventArgs { Time = 6 });
    }
}
