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
    public class SubstationEntity: Entity
    {
        public SubstationEntity() : base() { }

        public override string ToString()
        {
            return String.Format($"Id: {Id}\nType: SubstationEntity\nName: {Name}");
        }

        //public override void SetDefaultColor()
        //{
        //    Shape.Fill = Brushes.LightGreen;
        //}
    }
}
