using PredmetniZadatak2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace PredmetniZadatak2.Handlers
{
    public class ScreenHandler
    {
        public static GeometryModel3D Make3DCube(double x, double y, double z, double edge, EntityType type)
        {
            double temp = x;
            x = y;
            y = temp;

            // meshGeometry za kocku
            MeshGeometry3D cube = new MeshGeometry3D();

            // ivice kocke
            cube.Positions = new Point3DCollection
            {
                new Point3D(x, y, z), new Point3D(x + edge, y, z), new Point3D(x, y + edge, z), new Point3D(x + edge, y + edge, z),
                new Point3D(x, y, edge + z), new Point3D(x + edge, y, edge + z), new Point3D(x, y + edge, edge + z), new Point3D(x + edge, y + edge, edge + z)
            };

            // trouglovi
            List<int> listTriangles = new List<int>() { 2, 3, 1,  2, 1, 0,  7, 1, 3,  7, 5, 1,  6, 5, 7,  6, 4, 5,
                                                        6, 2, 4, 2, 0, 4,  2, 7, 3,  2, 6, 7,  0, 1, 5,  0, 5, 4};
            Int32Collection triangles = new Int32Collection(listTriangles);
            cube.TriangleIndices = triangles;

            // definisanje modela kocke
            GeometryModel3D cubeModel = new GeometryModel3D();
            cubeModel.Geometry = cube;

            // odabir boje u zavisnosti od tipa
            switch (type)
            {
                case EntityType.Substation:
                    cubeModel.Material = new DiffuseMaterial(Brushes.Red);
                    break;

                case EntityType.Node:
                    cubeModel.Material = new DiffuseMaterial(Brushes.Blue);
                    break;

                case EntityType.Switch:
                    cubeModel.Material = new DiffuseMaterial(Brushes.ForestGreen);
                    break;
            }

            return cubeModel;
        }

        public static void Draw3DCube(GeometryModel3D cubeModel, ModelVisual3D myModel)
        {
            // 3D model
            ModelVisual3D model = new ModelVisual3D();
            model.Content = cubeModel;

            myModel.Children.Add(model);
        }

        public static void DrawLine(double x1, double y1, double x2, double y2, ModelVisual3D myModel)
        {
            double temp1 = x1, temp2 = x2;
            x1 = y1;
            x2 = y2;
            y1 = temp1;
            y2 = temp2;

            // meshGeometry
            MeshGeometry3D line = new MeshGeometry3D();

            // ivice
            line.Positions = new Point3DCollection
            {
               new Point3D(x1, y1, 0.01), new Point3D(x2, y2, 0.01), new Point3D(x2 - 0.005, y2 - 0.005, 0.01), new Point3D(x1 - 0.005, y1 - 0.005, 0.01)
            };

            // trouglovi
            List<int> listTriangles = new List<int>() { 0, 1, 2, 0, 2, 3 };
            Int32Collection triangles = new Int32Collection(listTriangles);
            line.TriangleIndices = triangles;

            // definisanje modela
            GeometryModel3D lineModel = new GeometryModel3D();
            lineModel.Geometry = line;
            lineModel.Material = new DiffuseMaterial(Brushes.Black);

            // 3D model
            ModelVisual3D model = new ModelVisual3D();
            model.Content = lineModel;

            myModel.Children.Add(model);
        }
    }
}
