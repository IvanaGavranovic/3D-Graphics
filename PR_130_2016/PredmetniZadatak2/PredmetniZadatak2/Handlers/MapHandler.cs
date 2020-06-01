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

            // sirina opsega
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

                // preracunavanje u koordinate
                CoordinatesHandler.ToLatLon(networkModel.Substations[i].X, networkModel.Substations[i].Y, 34, out latitude, out longitude);
                networkModel.Substations[i].X = latitude;
                networkModel.Substations[i].Y = longitude;

                // preracunavanje u poziciju na mapi
                CoordinatesHandler.FromCoordinatesToMapPosition(latitude, longitude, out mapX, out mapY);
                networkModel.Substations[i].MapX = mapX;
                networkModel.Substations[i].MapY = mapY;

                // crtanje cvora koji je u opsegu mape
                if (mapX != -1 && mapY != -1)
                {
                    // provera da li postoji cvorova na toj lokaciji, redjanje na spratove
                    // u prvom prolazu se sigurno crta
                    double z = 0;

                    if (Points.Count != 0 || i != 0)
                    {
                        int index = 0;

                        // ako postoji objekat unutar definisanog precnika, uvecava se brojac i definise se visina na kojoj je potrebno nacrtati sledeci objekat
                        if (HasEntityInRadius(mapX, mapY, out index))
                        {
                            //Points[new Tuple<double, double>(mapX, mapY)].Count++;
                            //z = 0.05 * Points[new Tuple<double, double>(mapX, mapY)].Count;
                            Points[index].Count++;
                            z = 0.05 * Points[index].Count;
                        }
                        else
                        {
                            z = 0;
                            //Points.Add(new Tuple<double, double>(mapX, mapY), new PointStack(mapX, mapY, 0));
                            Points.Add(new PointStack(mapX, mapY, 0));
                        }
                    }
                    else
                    {
                        z = 0;
                        //EntitiesDict.Add(new Tuple<double, double>(roundLat, roundLog), 0);
                        //Points.Add(new Tuple<double, double>(mapX, mapY), new PointStack(mapX, mapY, 0));
                        Points.Add(new PointStack(mapX, mapY, 0));
                    }

                    // kreiranje 3D modela kocke i dodavanje u ArrayList models, zbog Hit Testing-a
                    GeometryModel3D model3D = ScreenHandler.Make3DCube(mapX, mapY, z, 0.05, EntityType.Substation);
                    Transformation.models.Add(model3D);

                    // dodavanje entiteta u recnik, kljuc je njegov redni broj u okviru ArrayList-e models
                    Entities.Add(Transformation.models.Count - 1, networkModel.Substations[i]);

                    ScreenHandler.Draw3DCube(model3D, myModel);
                }
            }

            // NODES
            for (int i = 0; i < networkModel.Nodes.Count; i++)
            {
                double latitude, longitude, mapX, mapY;

                // preracunavanje u koordinate
                CoordinatesHandler.ToLatLon(networkModel.Nodes[i].X, networkModel.Nodes[i].Y, 34, out latitude, out longitude);
                networkModel.Nodes[i].X = latitude;
                networkModel.Nodes[i].Y = longitude;

                // preracunavanje u poziciju na mapi
                CoordinatesHandler.FromCoordinatesToMapPosition(latitude, longitude, out mapX, out mapY);
                networkModel.Nodes[i].MapX = mapX;
                networkModel.Nodes[i].MapY = mapY;

                // crtanje c vora koji je u opsegu mape
                if (mapX != -1 && mapY != -1)
                {
                    /// provera da li postoji cvorova na toj lokaciji, redjanje na spratove
                    // u prvom prolazu se sigurno crta
                    double z = 0;

                    if (i != 0)
                    {
                        int index = 0;

                        // ako postoji objekat unutar definisanog precnika, uvecava se brojac i definise se visina na kojoj je potrebno nacrtati sledeci objekat
                        if (HasEntityInRadius(mapX, mapY, out index))
                        {
                            //z = 0.05 * EntitiesDict[new Tuple<double, double>(roundLat, roundLog)];
                            //Points[new Tuple<double, double>(mapX, mapY)].Count++;
                            Points[index].Count++;
                            z = 0.05 * Points[index].Count;
                        }
                        else
                        {
                            z = 0;
                            //Points.Add(new Tuple<double, double>(mapX, mapY), new PointStack(mapX, mapY, 0));
                            Points.Add(new PointStack(mapX, mapY, 0));
                        }
                    }
                    else
                    {
                        z = 0;
                        //EntitiesDict.Add(new Tuple<double, double>(roundLat, roundLog), 0);
                        //Points.Add(new Tuple<double, double>(mapX, mapY), new PointStack(mapX, mapY, 0));
                        Points.Add(new PointStack(mapX, mapY, 0));
                    }

                    // kreiranje 3D modela kocke i dodavanje u ArrayList models, zbog Hit Testing-a
                    GeometryModel3D model3D = ScreenHandler.Make3DCube(mapX, mapY, z, 0.05, EntityType.Node);
                    Transformation.models.Add(model3D);

                    // dodavanje entiteta u recnik, kljuc je njegov redni broj u okviru ArrayList-e models
                    Entities.Add(Transformation.models.Count - 1, networkModel.Nodes[i]);

                    ScreenHandler.Draw3DCube(model3D, myModel);
                }
            }

            // SWITCHES
            for (int i = 0; i < networkModel.Switches.Count; i++)
            {
                double latitude, longitude, mapX, mapY;

                // preracunavanje u koordinate
                CoordinatesHandler.ToLatLon(networkModel.Switches[i].X, networkModel.Switches[i].Y, 34, out latitude, out longitude);
                networkModel.Switches[i].X = latitude;
                networkModel.Switches[i].Y = longitude;

                // preracunavanje u poziciju na mapi
                CoordinatesHandler.FromCoordinatesToMapPosition(latitude, longitude, out mapX, out mapY);
                networkModel.Switches[i].MapX = mapX;
                networkModel.Switches[i].MapY = mapY;

                // crtanje cvora koji je u opsegu mape
                if (mapX != -1 && mapY != -1)
                {
                    // provera da li postoji cvorova na toj lokaciji, redjanje na spratove
                    // u prvom prolazu se sigurno crta
                    double z = 0;

                    if (i != 0)
                    {
                        int index = 0;

                        // ako postoji objekat unutar definisanog precnika, uvecava se brojac i definise se visina na kojoj je potrebno nacrtati sledeci objekat
                        if (HasEntityInRadius(mapX, mapY, out index))
                        {
                            //Points[new Tuple<double, double>(mapX, mapY)].Count++;
                            //z = 0.05 * Points[new Tuple<double, double>(mapX, mapY)].Count;
                            Points[index].Count++;
                            z = 0.05 * Points[index].Count;
                        }
                        else
                        {
                            z = 0;
                            //EntitiesDict.Add(new Tuple<double, double>(roundLat, roundLog), 0);
                            //Points.Add(new Tuple<double, double>(mapX, mapY), new PointStack(mapX, mapY, 0));
                            Points.Add(new PointStack(mapX, mapY, 0));
                        }
                    }
                    else
                    {
                        z = 0;
                        //EntitiesDict.Add(new Tuple<double, double>(roundLat, roundLog), 0);
                        //Points.Add(new Tuple<double, double>(mapX, mapY), new PointStack(mapX, mapY, 0));
                        Points.Add(new PointStack(mapX, mapY, 0));
                    }

                    // kreiranje 3D modela kocke i dodavanje u ArrayList models, zbog Hit Testing-a
                    GeometryModel3D model3D = ScreenHandler.Make3DCube(mapX, mapY, z, 0.05, EntityType.Switch);
                    Transformation.models.Add(model3D);

                    // dodavanje entiteta u recnik, kljuc je njegov redni broj u okviru ArrayList-e models
                    Entities.Add(Transformation.models.Count - 1, networkModel.Switches[i]);

                    ScreenHandler.Draw3DCube(model3D, myModel);
                }
            }

            // LINES
            int cnt = 0;

            for (int i = 0; i < networkModel.Lines.Count; i++)
            {
                double latitude1, longitude1, mapX1, mapY1, latitude2, longitude2, mapX2, mapY2;

                // prolazak kroz sve tacke jedne putanje
                for (int j = 0; j < networkModel.Lines[i].Vertices.Count - 1; j++)
                {
                    // preracunavanje u koordinate prve tacke
                    CoordinatesHandler.ToLatLon(networkModel.Lines[i].Vertices[j].X, networkModel.Lines[i].Vertices[j].Y, 34, out latitude1, out longitude1);
                    networkModel.Lines[i].Vertices[j].X = latitude1;
                    networkModel.Lines[i].Vertices[j].Y = longitude1;

                    // preracunavanje u poziciju na mapi
                    CoordinatesHandler.FromCoordinatesToMapPosition(latitude1, longitude1, out mapX1, out mapY1);

                    // preracunavanje u koordinate druge tacke
                    CoordinatesHandler.ToLatLon(networkModel.Lines[i].Vertices[j + 1].X, networkModel.Lines[i].Vertices[j + 1].Y, 34, out latitude2, out longitude2);
                    networkModel.Lines[i].Vertices[j + 1].X = latitude2;
                    networkModel.Lines[i].Vertices[j + 1].Y = longitude2;

                    // preracunavanje u poziciju na mapi
                    CoordinatesHandler.FromCoordinatesToMapPosition(latitude2, longitude2, out mapX2, out mapY2);

                    // provera da li su tacke u opsegu
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
