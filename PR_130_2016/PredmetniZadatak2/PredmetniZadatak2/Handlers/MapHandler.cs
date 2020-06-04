using PredmetniZadatak2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PredmetniZadatak2.Handlers
{
    public class MapHandler
    {
        public static Dictionary<int, Entity> Entities = new Dictionary<int, Entity>();
        public static List<PointStack> Points = new List<PointStack>();

        private static bool HasEntityInRadius(double x, double y, out int index)
        {
            index = 0;
            bool retVal = false;

            double radius = 0.05;

            foreach (PointStack obj in Points)
            {
                if (x >= obj.X - radius && x <= obj.X + radius && y >= obj.Y - radius && y <= obj.Y + radius)
                {
                    retVal = true;
                    break;
                }
                index++;
            }
            return retVal;
        }

        public static NetworkModel LoadModelToMap(NetworkModel networkModel, ModelVisual3D myModel)
        {
            networkModel = XmlHandler.Load<NetworkModel>(@"..\..\Geographic.xml");

            // SUBSTATIONS
            for (int i = 0; i < networkModel.Substations.Count; i++)
            {
                double latitude, longitude, mapX, mapY;

                CoordinatesHandler.ToLatLon(networkModel.Substations[i].X, networkModel.Substations[i].Y, 34, out latitude, out longitude);
                networkModel.Substations[i].X = latitude;
                networkModel.Substations[i].Y = longitude;

                CoordinatesHandler.FromCoordinatesToMapPosition(latitude, longitude, out mapX, out mapY);
                networkModel.Substations[i].MapX = mapX;
                networkModel.Substations[i].MapY = mapY;

                if (mapX != -1 && mapY != -1)
                {
                    double z = 0;

                    if (Points.Count != 0 || i != 0)
                    {
                        int index = 0;

                        if (HasEntityInRadius(mapX, mapY, out index))
                        {
                            Points[index].Count++;
                            z = 0.05 * Points[index].Count;
                        }
                        else
                        {
                            z = 0;
                            Points.Add(new PointStack(mapX, mapY, 0));
                        }
                    }
                    else
                    {
                        z = 0;
                        Points.Add(new PointStack(mapX, mapY, 0));
                    }
                    GeometryModel3D model3D = ScreenHandler.Make3DCube(mapX, mapY, z, 0.05, EntityType.Substation);
                  //  Transformation.models.Add(model3D);

                  //  Entities.Add(Transformation.models.Count - 1, networkModel.Substations[i]); /// ???

                    ScreenHandler.Draw3DCube(model3D, myModel);
                }
            }

            // NODES
            for (int i = 0; i < networkModel.Nodes.Count; i++)
            {
                double latitude, longitude, mapX, mapY;

                CoordinatesHandler.ToLatLon(networkModel.Nodes[i].X, networkModel.Nodes[i].Y, 34, out latitude, out longitude);
                networkModel.Nodes[i].X = latitude;
                networkModel.Nodes[i].Y = longitude;

                CoordinatesHandler.FromCoordinatesToMapPosition(latitude, longitude, out mapX, out mapY);
                networkModel.Nodes[i].MapX = mapX;
                networkModel.Nodes[i].MapY = mapY;

                if (mapX != -1 && mapY != -1)
                {
                    double z = 0;

                    if (i != 0)
                    {
                        int index = 0;

                        if (HasEntityInRadius(mapX, mapY, out index))
                        {
                            Points[index].Count++;
                            z = 0.05 * Points[index].Count;
                        }
                        else
                        {
                            z = 0;
                            Points.Add(new PointStack(mapX, mapY, 0));
                        }
                    }
                    else
                    {
                        z = 0;
                        Points.Add(new PointStack(mapX, mapY, 0));
                    }

                    GeometryModel3D model3D = ScreenHandler.Make3DCube(mapX, mapY, z, 0.05, EntityType.Node);
                  //  Transformation.models.Add(model3D);

                  //  Entities.Add(Transformation.models.Count - 1, networkModel.Nodes[i]);

                    ScreenHandler.Draw3DCube(model3D, myModel);
                }
            }

            // SWITCHES
            for (int i = 0; i < networkModel.Switches.Count; i++)
            {
                double latitude, longitude, mapX, mapY;

                CoordinatesHandler.ToLatLon(networkModel.Switches[i].X, networkModel.Switches[i].Y, 34, out latitude, out longitude);
                networkModel.Switches[i].X = latitude;
                networkModel.Switches[i].Y = longitude;

                CoordinatesHandler.FromCoordinatesToMapPosition(latitude, longitude, out mapX, out mapY);
                networkModel.Switches[i].MapX = mapX;
                networkModel.Switches[i].MapY = mapY;

                if (mapX != -1 && mapY != -1)
                {
                    double z = 0;

                    if (i != 0)
                    {
                        int index = 0;

                        if (HasEntityInRadius(mapX, mapY, out index))
                        {
                            Points[index].Count++;
                            z = 0.05 * Points[index].Count;
                        }
                        else
                        {
                            z = 0;
                            Points.Add(new PointStack(mapX, mapY, 0));
                        }
                    }
                    else
                    {
                        z = 0;
                        Points.Add(new PointStack(mapX, mapY, 0));
                    }

                    GeometryModel3D model3D = ScreenHandler.Make3DCube(mapX, mapY, z, 0.05, EntityType.Switch);
                   // Transformation.models.Add(model3D);

                   // Entities.Add(Transformation.models.Count - 1, networkModel.Switches[i]);

                    ScreenHandler.Draw3DCube(model3D, myModel);
                }
            }

            // LINES
            int cnt = 0;

            for (int i = 0; i < networkModel.Lines.Count; i++)
            {
                double latitude1, longitude1, mapX1, mapY1, latitude2, longitude2, mapX2, mapY2;

                for (int j = 0; j < networkModel.Lines[i].Vertices.Count - 1; j++)
                {
                    CoordinatesHandler.ToLatLon(networkModel.Lines[i].Vertices[j].X, networkModel.Lines[i].Vertices[j].Y, 34, out latitude1, out longitude1);
                    networkModel.Lines[i].Vertices[j].X = latitude1;
                    networkModel.Lines[i].Vertices[j].Y = longitude1;

                    CoordinatesHandler.FromCoordinatesToMapPosition(latitude1, longitude1, out mapX1, out mapY1);

                    CoordinatesHandler.ToLatLon(networkModel.Lines[i].Vertices[j + 1].X, networkModel.Lines[i].Vertices[j + 1].Y, 34, out latitude2, out longitude2);
                    networkModel.Lines[i].Vertices[j + 1].X = latitude2;
                    networkModel.Lines[i].Vertices[j + 1].Y = longitude2;

                    CoordinatesHandler.FromCoordinatesToMapPosition(latitude2, longitude2, out mapX2, out mapY2);

                    if (mapX1 != -1 && mapY1 != -1 && mapX2 != -1 && mapY2 != -1)
                    {
                        cnt++;
                        ScreenHandler.DrawLine(mapX1, mapY1, mapX2, mapY2, myModel);
                    }
                }
            }

            return networkModel;
        }
    }
}
