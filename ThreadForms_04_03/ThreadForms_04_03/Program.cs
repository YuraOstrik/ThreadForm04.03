using System.Diagnostics.Tracing;
using System.Threading;
using System.Xml.Linq;
namespace ThreadForms_04_03
{
    internal class Program
    {

        public static void ThreadParam(object obj)
        {
            int delay = (int)obj;
            Thread t = Thread.CurrentThread;

            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Работает " + t.Name + " поток!");
                Thread.Sleep(delay);
            }
            Console.WriteLine("Завершает работу " + t.Name + " поток!");
        }

        static void Main(string[] args)
        {
            //List<int> list = new List<int> { 10, 100, 300, 500, 700 };

            //for (int i = 0; i < list.Count; i++)
            //{
            //    Thread th1 = new Thread(new ParameterizedThreadStart(ThreadParam));
            //    th1.IsBackground = true;
            //    th1.Name = (list[i]).ToString();
            //    th1.Start(list[i]);
            //}
            //Console.ReadKey();

            Bank bank = new Bank(10000, "National Bank", 5);

            bank.Money = 121212;
            bank.Name = "Monobank";
            bank.Percent = 10;

            Thread.Sleep(100);
        }

        public class Bank
        {
            private int money;
            private string name;
            private int percent;
            private string FilePath = "bank.txt";
            private static readonly object locker = new object();

            public int Money
            {
                get => money;
                set
                {
                    money = value;
                    Changed(nameof(Money), money.ToString());
                }
            }
            public string Name
            {
                get => name;
                set
                {
                    name = value;
                    Changed(nameof(Name), name);
                }
            }
            public int Percent
            {
                get => percent;
                set
                {
                    percent = value;
                    Changed(nameof(Percent), percent.ToString());
                }
            }
            public Bank(int m, string n, int p)
            {
                money = m;
                name = n;
                percent = p;
            }
            private void Changed(string propertyName, string newValue)
            {

                Thread th = new Thread(() =>
                    {
                        string Entry = $"{DateTime.Now}: {propertyName} changed in {newValue}";
                        lock (locker)
                        {
                            File.AppendAllText(FilePath, Entry);
                        }
                        Console.WriteLine($"-{Entry.Trim()}-");
                    });

                th.IsBackground = true;
                th.Start();
            }
        }
    }
}
