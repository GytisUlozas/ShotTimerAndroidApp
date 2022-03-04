using System;
using System.Threading;
using System.Threading.Tasks;

namespace DryFireTimer.Models
{
    public class Timer
    {
        private bool run = false;
        private int count = 0; //Tenths of a second not a full one
        public bool IsRunning => run;
        public int StopCount { get; set; } = 0; //Tenths of a second not a full one

        public event Action<int> OnIncrement = (_) => { };
        public event Action OnCountEnd = () => { };
        public event Action OnCountStart = () => { };

        private async void RunAsync()
        {
            await Task.Run(
                async () =>
                {
                    if (count == 0)
                        OnCountStart();
                    while (run && count < StopCount)
                    {
                        await Task.Delay(100);
                        count++;
                        OnIncrement(count);
                    }
                    if (count == StopCount)
                        OnCountEnd();
                });
        }

        public void Start()
        {
            run = true;
            RunAsync();
        }
        public void Pause()
            => run = false;

        public void Reset()
        {
            run = false;
            count = 0;
            OnIncrement(count);
        }

        //Static methods used for formatting. I don't know if they ought to be here.
        public static string Min(int x)
         => (x / 600).ToString();

        public static string Sec(int x)
         => ((x % 600) / 10).ToString();

        public static string ToSeconds(int x)
            => $"{x / 10}.{x % 10}";

        public static bool ToTime(string x, out int time)
        {
            x = x.Trim();

            if (decimal.TryParse(x, out decimal result))
            {
                result = Math.Round(result, 1);
                result *= 10;
                time = (int)Math.Round(result);
                return true;
            }
            else 
            {
                time = 0;
                return false;
            }
        }

        public static bool ToReps(string x, out int reps)
        {
            x = x.Trim();
            if (int.TryParse(x, out reps))
                return true;
            else
                return false;
        }
    }
}
