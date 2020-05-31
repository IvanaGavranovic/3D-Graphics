using PredmetniZadatak2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PredmetniZadatak2.Handlers
{
    public class GridHandler
    {
        public static Dictionary<UInt64, Entity> Entities = new Dictionary<UInt64, Entity>();
        static List<int> IndexesI = new List<int>();
        static List<int> IndexesJ = new List<int>();

        public static UInt64[,] MakeGrid()
        {
            UInt64[,] grid = new UInt64[1000, 1000];

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    grid[i, j] = 0;
                }
            }
            return grid;
        }

        public static NetworkModel LoadNetworkModel(NetworkModel networkModel, UInt64[,] grid, Canvas myCanvas, MouseButtonEventHandler increase)
        {
            int counter = 0;

            networkModel = XmlHandler.Load<NetworkModel>(@"..\..\Geographic.xml");

            // SUBSTATIONS
            for (int i = 0; i < networkModel.Substations.Count; i++)
            {
                double decimalX, decimalY;
                CoordinatesHandler.ToLatLon(networkModel.Substations[i].X, networkModel.Substations[i].Y, 34, out decimalX, out decimalY);

                int indexI, indexJ;
                CoordinatesHandler.FromCoordsToIndex(decimalX, decimalY, out indexI, out indexJ);

                if (grid[indexI, indexJ] == 0)
                {
                    grid[indexI, indexJ] = networkModel.Substations[i].Id;

                    networkModel.Substations[i].Row = indexI;
                    networkModel.Substations[i].Column = indexJ;
                    IndexesI.Add(indexI);
                    IndexesJ.Add(indexJ);

                    Entities.Add(networkModel.Substations[i].Id, networkModel.Substations[i]);
                }

                Ellipse image = ScreenHandler.DrawSubstationImage(indexI, indexJ, myCanvas, networkModel.Substations[i]);
                image.MouseLeftButtonDown += increase;
                networkModel.Substations[i].Shape = image;
                myCanvas.Children.Add(image);
            }

            // NODES
            for (int i = 0; i < networkModel.Nodes.Count; i++)
            {
                double decimalX, decimalY;
                CoordinatesHandler.ToLatLon(networkModel.Nodes[i].X, networkModel.Nodes[i].Y, 34, out decimalX, out decimalY);

                int indexI, indexJ;
                CoordinatesHandler.FromCoordsToIndex(decimalX, decimalY, out indexI, out indexJ);

                if (grid[indexI, indexJ] == 0)
                {
                    grid[indexI, indexJ] = networkModel.Nodes[i].Id;
                }
                else
                {
                    bool spaceFound = FindFreeSpaceForNode(networkModel, grid, indexI, indexJ, i, out indexI, out indexJ);

                    grid[indexI, indexJ] = networkModel.Nodes[i].Id;

                    if (!spaceFound)
                    {
                        counter++;
                    }
                }
                networkModel.Nodes[i].Row = indexI;
                networkModel.Nodes[i].Column = indexJ;
                IndexesI.Add(indexI);
                IndexesJ.Add(indexJ);

                Entities.Add(networkModel.Nodes[i].Id, networkModel.Nodes[i]);

                Ellipse image = ScreenHandler.DrawNodeImage(indexI, indexJ, myCanvas, networkModel.Nodes[i]);
                image.MouseLeftButtonDown += increase;
                networkModel.Nodes[i].Shape = image;
                myCanvas.Children.Add(image);
            }

            // SWITCHES
            for (int i = 0; i < networkModel.Switches.Count; i++)
            {
                double decimalX, decimalY;
                CoordinatesHandler.ToLatLon(networkModel.Switches[i].X, networkModel.Switches[i].Y, 34, out decimalX, out decimalY);
                networkModel.Switches[i].X = decimalX;
                networkModel.Switches[i].Y = decimalY;

                int indexI, indexJ;
                CoordinatesHandler.FromCoordsToIndex(decimalX, decimalY, out indexI, out indexJ);

                if (grid[indexI, indexJ] == 0)
                {
                    grid[indexI, indexJ] = networkModel.Switches[i].Id;
                }
                else
                {
                    bool spaceFound = FindFreeSpaceForSwitch(networkModel, grid, indexI, indexJ, i, out indexI, out indexJ);

                    grid[indexI, indexJ] = networkModel.Switches[i].Id;

                    if (!spaceFound)
                    {
                        counter++;
                    }
                }

                networkModel.Switches[i].Row = indexI;
                networkModel.Switches[i].Column = indexJ;
                IndexesI.Add(indexI);
                IndexesJ.Add(indexJ);

                Entities.Add(networkModel.Switches[i].Id, networkModel.Switches[i]);

                Ellipse image = ScreenHandler.DrawSwitchImage(indexI, indexJ, myCanvas, networkModel.Switches[i]);
                image.MouseLeftButtonDown += increase;
                networkModel.Switches[i].Shape = image;
                myCanvas.Children.Add(image);
            }

            int minI = IndexesI.Min();          // 672
            int minJ = IndexesJ.Min();          // 726
            int maxI = IndexesI.Max();          // 811
            int maxJ = IndexesJ.Max();          // 951

            return networkModel;
        }
  
      
        public static bool FindFreeSpaceForNode(NetworkModel networkModel, UInt64[,] grid, int indexI, int indexJ, int i, out int freeI, out int freeJ)
        {
            bool spaceFound = false;
            int step = 1;

            while (!spaceFound)
            {
                if (grid[indexI, (indexJ - step) % 1000] == 0)
                {
                    indexJ = (indexJ - step) % 1000;
                    grid[indexI, indexJ] = networkModel.Nodes[i].Id;
                    spaceFound = true;
                }
                else if (grid[(indexI - step) % 1000, (indexJ - step) % 1000] == 0)
                {
                    indexI = (indexI - step) % 1000;
                    indexJ = (indexJ - step) % 1000;
                    grid[indexI, indexJ] = networkModel.Nodes[i].Id;
                    spaceFound = true;
                }
                else if (grid[(indexI - step) % 1000, indexJ] == 0)
                {
                    indexI = (indexI - step) % 1000;
                    grid[indexI, indexJ] = networkModel.Nodes[i].Id;
                    spaceFound = true;
                }
                else if (grid[indexI, (indexJ + step) % 1000] == 0)
                {
                    indexJ = (indexJ + step) % 1000;
                    grid[indexI, indexJ] = networkModel.Nodes[i].Id;
                    spaceFound = true;
                }
                else if (grid[(indexI + step) % 1000, (indexJ + step) % 1000] == 0)
                {
                    indexI = (indexI + step) % 1000;
                    indexJ = (indexJ + step) % 1000;
                    grid[indexI, indexJ] = networkModel.Nodes[i].Id;
                    spaceFound = true;
                }
                else if (grid[(indexI + step) % 1000, indexJ] == 0)
                {
                    indexI = (indexI + step) % 1000;
                    grid[indexI, indexJ] = networkModel.Nodes[i].Id;
                    spaceFound = true;
                }
                if (++step == 16)
                {
                    break;
                }
            }

            freeI = indexI;
            freeJ = indexJ;

            return spaceFound;
        }

        public static bool FindFreeSpaceForSwitch(NetworkModel networkModel, UInt64[,] grid, int indexI, int indexJ, int i, out int freeI, out int freeJ)
        {
            bool spaceFound = false;
            int step = 1;

            while (!spaceFound)
            {
                if (grid[indexI, (indexJ - step) % 1000] == 0)
                {
                    indexJ = (indexJ - step) % 1000;
                    grid[indexI, indexJ] = networkModel.Switches[i].Id;
                    spaceFound = true;
                }
                else if (grid[(indexI - step) % 1000, (indexJ - step) % 1000] == 0)
                {
                    indexI = (indexI - step) % 1000;
                    indexJ = (indexJ - step) % 1000;
                    grid[indexI, indexJ] = networkModel.Switches[i].Id;
                    spaceFound = true;
                }
                else if (grid[(indexI - step) % 1000, indexJ] == 0)
                {
                    indexI = (indexI - step) % 1000;
                    grid[indexI, indexJ] = networkModel.Switches[i].Id;
                    spaceFound = true;
                }
                else if (grid[indexI, (indexJ + step) % 1000] == 0)
                {
                    indexJ = (indexJ + step) % 1000;
                    grid[indexI, indexJ] = networkModel.Switches[i].Id;
                    spaceFound = true;
                }
                else if (grid[(indexI + step) % 1000, (indexJ + step) % 1000] == 0)
                {
                    indexI = (indexI + step) % 1000;
                    indexJ = (indexJ + step) % 1000;
                    grid[indexI, indexJ] = networkModel.Switches[i].Id;
                    spaceFound = true;
                }
                else if (grid[(indexI + step) % 1000, indexJ] == 0)
                {
                    indexI = (indexI + step) % 1000; ;
                    grid[indexI, indexJ] = networkModel.Switches[i].Id;
                    spaceFound = true;
                }

                if (++step == 16)
                {
                    break;
                }
            }

            freeI = indexI;
            freeJ = indexJ;

            return spaceFound;
        }
    }
}
