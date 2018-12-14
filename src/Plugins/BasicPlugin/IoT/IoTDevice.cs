using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicPlugin.Models.IoT;
using PropertyChanged;

namespace BasicPlugin.IoT
{
    public class IoTDevice : MarshalByRefObject
    {
        public static IoTDevice Instance;

        public string Id { get; set; }

        public event Action<Channel, Channel> ChannelChangedEvent;

        public IList<Channel> InputChannels { get; private set; } = new List<Channel>();

        public IList<Channel> OutputChannels { get; private set; } = new List<Channel>();

        public Channel InCh0 => this.InputChannels[0];

        public Channel InCh1 => this.InputChannels[1];

        public Channel InCh2 => this.InputChannels[2];

        public Channel InCh3 => this.InputChannels[3];

        public Channel OutCh0 => this.OutputChannels[0];

        public Channel OutCh1 => this.OutputChannels[1];

        public Channel OutCh2 => this.OutputChannels[2];

        public Channel OutCh3 => this.OutputChannels[3];

        public IoTDevice(string id)
        {
            this.Id = id;
            this.InputChannels.Add(new Channel(0, "Input"));
            this.InputChannels.Add(new Channel(1, "Input"));
            this.InputChannels.Add(new Channel(2, "Input"));
            this.InputChannels.Add(new Channel(3, "Input"));

            this.OutputChannels.Add(new Channel(0, "Output"));
            this.OutputChannels.Add(new Channel(1, "Output"));
            this.OutputChannels.Add(new Channel(2, "Output"));
            this.OutputChannels.Add(new Channel(3, "Output"));

            this.RegisterEvent(this.InputChannels);
            this.RegisterEvent(this.OutputChannels);
        }

        private void RegisterEvent(IList<Channel> inputChannels)
        {
            foreach (var ch in inputChannels)
            {
                ch.DataChangedEvent += OnDataChanged;
            }
        }

        private void OnDataChanged(Channel oldCh, Channel newCh)
        {
            this.ChannelChangedEvent?.Invoke(oldCh, newCh);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
