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
    public class NodeEntity : Entity
    {
        public NodeEntity() : base() { }

        public override string ToString()
        {
            return String.Format($"Id: {Id}\nType: NodeEntity\nName: {Name}");
        }
    }
}
