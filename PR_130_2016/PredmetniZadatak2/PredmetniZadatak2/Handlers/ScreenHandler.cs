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
using System.Windows.Shapes;

namespace PredmetniZadatak2.Handlers
{
    public class ScreenHandler
    {
        public static List<Image> images = new List<Image>();
        public static double MinI = 672;
        public static double MinJ = 726;
        public static double MaxI = 811;
        public static double MaxJ = 951;
        
        public static Ellipse DrawSubstationImage(int indexI, int indexJ, Canvas myCanvas, SubstationEntity station)
        {
            Ellipse element = new Ellipse() { Width = 3, Height = 3, Fill = Brushes.LightGreen };
            element.ToolTip = $"ID: {station.Id}\nSubstation Entity\nName: {station.Name}";
            Canvas.SetLeft(element, ((double)indexJ - MinJ) / (MaxJ - MinJ) * myCanvas.Width);
            Canvas.SetTop(element, ((double)indexI - MinI) / (MaxI - MinI) * myCanvas.Height);

            return element;
        }

        public static Ellipse DrawNodeImage(int indexI, int indexJ, Canvas myCanvas, NodeEntity node)
        {
            Ellipse element = new Ellipse() { Width = 3, Height = 3, Fill = Brushes.Orange };
            element.ToolTip = $"ID: {node.Id}\nNode Entity\nName: {node.Name}";
            Canvas.SetLeft(element, ((double)indexJ - MinJ) / (MaxJ - MinJ) * myCanvas.Width);
            Canvas.SetTop(element, ((double)indexI - MinI) / (MaxI - MinI) * myCanvas.Height);

            return element;
        }

        public static Ellipse DrawSwitchImage(int indexI, int indexJ, Canvas myCanvas, SwitchEntity switchEntity)
        {
            Ellipse element = new Ellipse() { Width = 3, Height = 3, Fill = Brushes.LightSkyBlue };
            element.ToolTip = $"ID: {switchEntity.Id}\nSwitch Entity\nName: {switchEntity.Name}\nStatus: {switchEntity.Status}";
            Canvas.SetLeft(element, ((double)indexJ - MinJ) / (MaxJ - MinJ) * myCanvas.Width);
            Canvas.SetTop(element, ((double)indexI - MinI) / (MaxI - MinI) * myCanvas.Height);

            return element;
        }

        public static Line DrawLine(double x1, double y1, double x2, double y2, Canvas myCanvas, LineEntity lineEntity)
        {
            Line line = new Line()
            {
                X1 = x1,
                X2 = x2,
                Y1 = y1,
                Y2 = y2,
                StrokeThickness = 0.5,
                Stroke = Brushes.White
            };

            line.ToolTip = $"ID: {lineEntity.Id}\nLine Entity\nName:{lineEntity.Name}";
           
            return line;
        }     
    }
}
