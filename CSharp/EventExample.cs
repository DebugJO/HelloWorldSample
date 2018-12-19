/// <summary>
/// Reference : Advanced C#: 08 Events
/// URL : https://www.youtube.com/watch?v=KVp_E-hTG0k
/// </summary>
using System;

namespace EventExam {
    class Program {
        static void Main (string[] args) {
            var tower = new ClockTower ();
            var john = new Person ("Jone", tower);
            //var sally = new Person("Sally", tower);

            tower.ChimeFivePM ();
            tower.ChimeSixAM ();
        }
    }

    public class Person {
        private string name;
        private ClockTower tower;
        public Person (string name, ClockTower tower) {
            this.name = name;
            this.tower = tower;
            // object sender, ClockTowerEventArgs args => (s, e)
            this.tower.Chime += (s, e) => {
                System.Console.WriteLine ("{0} heard the clock chime.", this.name);
                switch (e.Time) {
                    case 6:
                        System.Console.WriteLine ("{0} is waking up.", this.name);
                        break;
                    case 17:
                        System.Console.WriteLine ("{0} is going home.", this.name);
                        break;
                }
            };
        }
    }

    public class ClockTowerEventArgs : EventArgs {
        public int Time { get; set; }
    }

    public delegate void ChimeEventHandler (object sender, ClockTowerEventArgs args);
    public class ClockTower {
        public event ChimeEventHandler Chime;
        public void ChimeFivePM () => Chime (this, new ClockTowerEventArgs { Time = 17 });
        public void ChimeSixAM () => Chime (this, new ClockTowerEventArgs { Time = 6 });
    }
}
