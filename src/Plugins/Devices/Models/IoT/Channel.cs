using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PropertyChanged;

namespace BasicPlugin.Models.IoT
{
    [ImplementPropertyChanged]
    public class Channel : MarshalByRefObject, ICloneable
    {
        public event Action<Channel, Channel> DataChangedEvent;

        internal string Type { get; set; }

        public int Ch { get; set; } = 0;

        public int? Md { get; set; } = 0;

        [AlsoNotifyFor("Status")]
        public int Val { get; set; } = 0;

        public int? Stat { get; set; } = 0;

        public int? Cnting { get; set; } = 1;

        public int? OvLch { get; set; } = 0;

        [XmlIgnore]
        public bool Status
        {
            get
            {
                return this.Val == 1;
            }
            set
            {
                this.Val = value ? 1 : 0;
            }
        }

        public Channel()
        {

        }

        public Channel(string type) : this()
        {
            this.Type = type;
        }

        public Channel(int id, string type) : this()
        {
            this.Ch = id;
            this.Type = type;
        }

        internal void Update(Channel newData)
        {
            var oldInfo = this.Clone() as Channel;
            this.Val = newData.Val;

            if (newData.Md != null)
            {
                this.Md = newData.Md;
            }
            if (newData.Cnting != null)
            {
                this.Cnting = newData.Cnting;
            }
            if (newData.Stat != null)
            {
                this.Stat = newData.Stat;
            }
            if (newData.OvLch != null)
            {
                this.OvLch = newData.OvLch;
            }

            this.DataChangedEvent?.Invoke(oldInfo, this);
        }

        public object Clone()
        {
            return (Channel) this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"[{this.Type}] [\"Ch\" : {this.Ch}, \"Val\" : {this.Val}, \"Md\" : {this.Md}, \"Stat\" : {this.Stat}, \"Cnting\" : {this.Cnting}, \"OvLch\" : {this.OvLch}]";
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
