using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace PredmetniZadatak2.Classes
{
    public enum EntityType
    {
        Substation,
        Node,
        Switch
    };

    public class Entity
    {
        private UInt64 id;
        private string name;
        private double x;
        private double y;

        private double mapX;
        private double mapY;

        public UInt64 Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        public double MapX { get => mapX; set => mapX = value; }
        public double MapY { get => mapY; set => mapY = value; }
        public int counter { get; set; }
        public Entity() { }
    }
}
