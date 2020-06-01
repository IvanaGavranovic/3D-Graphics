using PredmetniZadatak2.Classes;
using PredmetniZadatak2.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PredmetniZadatak2
{
    public partial class MainWindow : Window
    {
        private GeometryModel3D hitgeo;
        private DiffuseMaterial blue = new DiffuseMaterial() { Brush = Brushes.LightPink };

        public MainWindow()
        {
            InitializeComponent();

            NetworkModel networkModel = new NetworkModel();
            networkModel = MapHandler.LoadModelToMap(networkModel, MyModel);

            Transformation transformation = new Transformation(this, scrollViewer, mainViewport);

        }

        #region SKROL I POMERANJE

        System.Windows.Point scrollMousePoint = new System.Windows.Point();
        double hOff = 1;
        double vEff = 1;
        private void scrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            scrollMousePoint = e.GetPosition(scrollViewer);
            hOff = scrollViewer.HorizontalOffset;
            vEff = scrollViewer.VerticalOffset;
            scrollViewer.CaptureMouse();
        }

        private void scrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton.Equals(MouseButtonState.Pressed)) { MessageBox.Show("Middle button clicked"); }

            if (scrollViewer.IsMouseCaptured)
            {
                scrollViewer.ScrollToHorizontalOffset(hOff + (scrollMousePoint.X - e.GetPosition(scrollViewer).X));
                scrollViewer.ScrollToVerticalOffset(vEff + (scrollMousePoint.Y - e.GetPosition(scrollViewer).Y));
            }
        }

        private void scrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.ReleaseMouseCapture();
        }


        private void scrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            System.Windows.Point mouseAtImage = e.GetPosition(mainViewport);
            System.Windows.Point mouseAtScrollViewer = e.GetPosition(scrollViewer);

            ScaleTransform st = mainViewport.LayoutTransform as ScaleTransform;
            if (st == null)
            {
                st = new ScaleTransform();
                mainViewport.LayoutTransform = st;
            }

            if (e.Delta > 0)
            {
                st.ScaleX = st.ScaleY = st.ScaleX * 1.25;
                if (st.ScaleX > 64) st.ScaleX = st.ScaleY = 64;
            }
            else
            {
                st.ScaleX = st.ScaleY = st.ScaleX / 1.25;
                if (st.ScaleX < 1) st.ScaleX = st.ScaleY = 1;
            }
            #region [this step is critical for offset]
            scrollViewer.ScrollToHorizontalOffset(0);
            scrollViewer.ScrollToVerticalOffset(0);
            this.UpdateLayout();
            #endregion

            Vector offset = mainViewport.TranslatePoint(mouseAtImage, scrollViewer) - mouseAtScrollViewer;
            scrollViewer.ScrollToHorizontalOffset(offset.X);
            scrollViewer.ScrollToVerticalOffset(offset.Y);
            this.UpdateLayout();

            e.Handled = true;
        }
        #endregion
    }
}
