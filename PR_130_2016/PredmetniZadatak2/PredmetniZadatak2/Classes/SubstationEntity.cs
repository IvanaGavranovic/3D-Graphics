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
            return String.Format($"Substation {Name}: x={Math.Round(X, 2)},y={Math.Round(Y, 2)}");
        }

        //public override void SetDefaultColor()
        //{
        //    Shape.Fill = Brushes.LightGreen;
        //}
    }
}
