using System;

namespace Singleton {
    class Program {
        static void Main (string[] args) {
            Console.WriteLine ("Hello World!");
            MyClass.Instance.UserName = "홍길동";
            MyClass.Instance.Print ();
            MyClass.Instance.UserName = "마바리";
            Console.WriteLine (MyClass.Instance.UserName);
            Calculate.Instance.a = "a";
            Calculate.Instance.b = "b";
            Calculate.Instance.print ();
        }
    }

    class MyClass : Singleton<MyClass> {
        public string UserName { get; set; } = "Default Name";
        public void Print () {
            Console.WriteLine ("Hello Singleton : " + UserName);
        }
    }

    public class Singleton<T> where T : class, new () {
        protected static object _instanceLock = new object ();
        protected static volatile T _instance;
        public static T Instance {
            get {
                lock (_instanceLock) {
                    if (null == _instance)
                        _instance = new T ();
                }
                return _instance;
            }
        }
    }

    public sealed class Calculate {
        private Calculate () { }
        private static Calculate instance = null;
        public static Calculate Instance {
            get {
                if (instance == null) {
                    instance = new Calculate ();
                }
                return instance;
            }
        }

        public string a { get; set; } = "0";
        public string b { get; set; } = "0";

        public string Division (string a, string b) {
            double.TryParse (a, out double x);
            double.TryParse (b, out double y);
            if (double.IsInfinity (x / y) || double.IsNaN (x / y)) {
                return "0";
            }
            return (x / y).ToString ();
        }
        public void print () {
            Console.WriteLine ($"계산해 봅시다! : {Division(a, b)}");
        }
    }
}
