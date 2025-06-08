using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Radionova.Logic
{
    public class CountdownTimer
    {
        public TimeSpan MaxTime { get; }
        public TimeSpan Elapsed { get; private set; }

        private Timer timer;
        public event Action<TimeSpan> TimeUpdated;
        public event Action TimeEnded;

        public CountdownTimer(TimeSpan maxTime)
        {
            MaxTime = maxTime;
            Elapsed = TimeSpan.Zero;

            timer = new Timer { Interval = 1000 };
            timer.Tick += (s, e) =>
            {
                Elapsed = Elapsed.Add(TimeSpan.FromSeconds(1));
                var remaining = MaxTime - Elapsed;

                TimeUpdated?.Invoke(remaining);

                if (remaining.TotalSeconds <= 0)
                {
                    Stop();
                    TimeEnded?.Invoke();
                }
            };
        }

        public void Start() => timer.Start();
        public void Stop() => timer.Stop();
    }
}