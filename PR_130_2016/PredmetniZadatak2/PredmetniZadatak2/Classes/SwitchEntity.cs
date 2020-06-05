using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace PredmetniZadatak2.Classes
{
    [Serializable]
    [XmlRoot("NetworkModel")]
    public class SwitchEntity: Entity
    {
        private string status;

        public string Status { get => status; set => status = value; }


        public SwitchEntity() : base() { }

        public override string ToString()
        {
            return String.Format($"Id: {Id}\nType: SwitchEntity\nName: {Name}\nStatus: {Status}");
        }
    }
}
