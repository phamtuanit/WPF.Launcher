using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using PropertyChanged;

namespace BasicPlugin.WeighingScale
{

    [ImplementPropertyChanged]
    public class Weight
    {
        public double Value { get; set; } = 0.00;

        public string Unit { get; set; } = "kg";
    }

    [ImplementPropertyChanged]
    public class AutoIncreaseWeight : Weight
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(typeof(AutoIncreaseWeight));

        public double Min { get; set; } = 0;

        public double Max { get; set; } = 200;

        public float Step { get; set; } = 10;

        public int Deplay { get; set; } = 1 * 1000;

        public bool IsEnableLoop { get; set; }

        public bool IsEnable { get; private set; }

        public void StartIncreasement()
        {
            Logger.Info("Start auto increase weight.");

            this.IsEnable = true;
            this.Value = this.Min;
            while (this.IsEnable)
            {
                Thread.Sleep(this.Deplay);
                if (this.Value + this.Step >= this.Max)
                {
                    this.Value = this.Max;
                    if (this.IsEnableLoop)
                    {
                        Thread.Sleep(this.Deplay);
                        this.Value = this.Min;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    this.Value += this.Step;
                }
                Logger.Info("Stop auto increase weight.");
            }
            this.IsEnable = false;
        }

        public void StopIncreasement()
        {
            Logger.Info("Stop auto increase weight.");
            this.IsEnable = false;
        }
    }

    [ImplementPropertyChanged]
    public class WeighingScaleDevice
    {
        private static ILog Logger = LogManager.GetLogger(typeof(WeighingScaleDevice));

        public string Id { get; set; }

        public bool IsConnected { get; set; }

        public AutoIncreaseWeight Weight { get; set; }

        public WeighingScaleDevice(string id)
        {
            this.Id = id;
            this.Weight = new AutoIncreaseWeight();
        }

        public void RefreshSetting()
        {
            if(this.Weight.IsEnable)
            {
                Task.Run(() =>
                {
                    try
                    {
                        this.Weight.StartIncreasement();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Cannot start auto increase weight.", ex);
                    }
                });
            }
            else
            {
                this.Weight.StopIncreasement();
            }
        }
    }
}
