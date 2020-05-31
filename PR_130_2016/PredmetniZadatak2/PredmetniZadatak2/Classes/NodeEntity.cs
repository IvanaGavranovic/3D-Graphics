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
            return String.Format($"Node {Name}\nx={Math.Round(X, 2)},y={Math.Round(Y, 2)}");
        }

        //public override void SetDefaultColor()
        //{
        //    Shape.Fill = Brushes.Orange;
        //}
    }
}
