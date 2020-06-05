using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace PredmetniZadatak2.Handlers
{
    public class Transformation
    {       
        #region Field
        Viewport3D viewport;
        private ScaleTransform3D skaliranje;
        int zoomMax = 20;
        int zoomMin = 1;
        int zoomCurrent = 1;
        private Point start = new Point();
        private Point diffOffset = new Point();
        AxisAngleRotation3D rotateX;
        AxisAngleRotation3D rotateY;
        private bool rotating;
        private Quaternion rotationDelta;
        private Quaternion rotation;
        TranslateTransform3D translate;
        Window window;
        #endregion

        #region Constuctor
        public Transformation(Viewport3D viewport, ScaleTransform3D skaliranje, Window window, TranslateTransform3D translate, AxisAngleRotation3D rotateX, AxisAngleRotation3D rotateY)
        {
            this.skaliranje = skaliranje;
            this.viewport = viewport;
            this.window = window;
            this.translate = translate;
            rotationDelta = Quaternion.Identity;
            this.rotateX = rotateX;
            this.rotateY = rotateY;
            this.rotation = new Quaternion(0, 0, 0, 1);
            Attach();
        }
        #endregion

        public void Attach()
        {
            viewport.PreviewMouseWheel += viewport_MouseWheel;
            viewport.MouseLeftButtonDown += MouseLeftButtonDown;
            viewport.MouseLeftButtonUp += MouseLeftButtonUp;
            viewport.MouseMove += MouseMove;
            viewport.MouseDown += MiddleButtonDown;
            viewport.MouseUp += MiddleButtonUp;
        }

        #region Zoom
        private void viewport_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scaleX;
            double scaleY;
            double scaleZ;

           // System.Windows.Point p = e.MouseDevice.GetPosition(this);
            if (e.Delta > 0 && zoomCurrent < zoomMax)
            {
                scaleX = skaliranje.ScaleX + 0.1;
                scaleY = skaliranje.ScaleY + 0.1;
                scaleZ = skaliranje.ScaleZ + 0.1;

                zoomCurrent++;

                skaliranje.CenterX = 5;
                skaliranje.CenterY = 3.5;

                skaliranje.ScaleX = scaleX;
                skaliranje.ScaleY = scaleY;
                skaliranje.ScaleZ = scaleZ;

            }
            else if (e.Delta <= 0 && zoomCurrent > zoomMin)
            {
                scaleX = skaliranje.ScaleX - 0.1;
                scaleY = skaliranje.ScaleY - 0.1;
                scaleZ = skaliranje.ScaleZ - 0.1;

                zoomCurrent--;

                skaliranje.CenterX = 5;
                skaliranje.CenterY = 3.5;

                skaliranje.ScaleX = scaleX;
                skaliranje.ScaleY = scaleY;
                skaliranje.ScaleZ = scaleZ;
            }
        }
        #endregion

        #region Move and rotation
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewport.CaptureMouse();
            start = e.GetPosition(window);

            diffOffset.X = translate.OffsetX;
            diffOffset.Y = translate.OffsetY;
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            viewport.ReleaseMouseCapture();
        }
        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (viewport.IsMouseCaptured)
            {
                Point end = e.GetPosition(window);
                double offsetX = end.X - start.X;
                double offsetY = end.Y - start.Y;
                double w = window.Width;
                double h = window.Height;
                double translateX = (offsetX * 100) / w;
                double translateY = -(offsetY * 100) / h;
                var q = rotation;
                if (rotating)
                {
                    var angleY = (rotateY.Angle + -translateX) % 360;
                    var angleX = (rotateX.Angle + translateY) % 360;
                    if (-80 < angleY && angleY < 80)
                    {
                        rotateY.Angle = angleY;
                    }
                    if (-20 < angleX && angleX < 135)
                    {
                        rotateX.Angle = angleX;
                    }
                    start = end;
                }
                else
                {
                    translate.OffsetX = diffOffset.X + (translateX / (100 * skaliranje.ScaleX) * 5);
                    translate.OffsetY = diffOffset.Y + (translateY / (100 * skaliranje.ScaleX) * 5);
                    translate.OffsetZ = translate.OffsetZ;
                }
            }
        }
       
        private void MiddleButtonDown(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                rotating = true;
                viewport.CaptureMouse();
                start = e.GetPosition(window);

                diffOffset.X = translate.OffsetX;
                diffOffset.Y = translate.OffsetY;
            }
        }
        private void MiddleButtonUp(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Released)
            {
                rotating = false;
                viewport.ReleaseMouseCapture();
            }
        }
        #endregion

    }
}
