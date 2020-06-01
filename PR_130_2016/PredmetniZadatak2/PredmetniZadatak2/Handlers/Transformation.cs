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
        private Vector3D center;
        private bool centered;
        private bool enabled;
        private Point point;
        private bool rotating;
        private Quaternion rotation;
        private Quaternion rotationDelta;
        private double scale;
        private double scaleDelta;
        private bool scaling;
        private List<Viewport3D> slaves;
        private Vector3D translate;
        private Vector3D translateDelta;


        ScrollViewer scrollViewer;
        Window window;
        Viewport3D viewport;

        #region SCROLL AND MOVE

        public Transformation(FrameworkElement element, ScrollViewer scrollViewer, Viewport3D viewport)
        {
            this.window = (Window)element;
            this.scrollViewer = scrollViewer;
            this.viewport = viewport;

            Reset();

            Attach();
            Slaves.Add(viewport);
            Enabled = true;
        }

        public List<Viewport3D> Slaves
        {
            get { return slaves ?? (slaves = new List<Viewport3D>()); }
            set { slaves = value; }
        }

        public bool Enabled
        {
            get { return enabled && (slaves != null) && (slaves.Count > 0); }
            set { enabled = value; }
        }

        public void Attach()
        {
            window.MouseMove += MouseMoveHandler;
            window.MouseRightButtonDown += MouseDownHandler;
            window.MouseRightButtonUp += MouseUpHandler;
            window.MouseDown += MiddleButtonDownHandler;
            scrollViewer.PreviewMouseLeftButtonDown += scrollViewerPreviewMouseLeftButtonDown;
            scrollViewer.PreviewMouseMove += scrollViewerPreviewMouseMove;
            scrollViewer.PreviewMouseLeftButtonUp += scrollViewerPreviewMouseLeftButtonUp;
            scrollViewer.PreviewMouseWheel += scrollViewerPreviewMouseWheel;
        }

        public void Detach(FrameworkElement element)
        {
            element.MouseMove -= MouseMoveHandler;
            element.MouseRightButtonDown -= MouseDownHandler;
            element.MouseRightButtonUp -= MouseUpHandler;
            element.MouseDown += MiddleButtonDownHandler;
            scrollViewer.PreviewMouseLeftButtonDown -= scrollViewerPreviewMouseLeftButtonDown;
            scrollViewer.PreviewMouseMove -= scrollViewerPreviewMouseMove;
            scrollViewer.PreviewMouseLeftButtonUp -= scrollViewerPreviewMouseLeftButtonUp;
            scrollViewer.PreviewMouseWheel -= scrollViewerPreviewMouseWheel;
        }


        Point scrollMousePoint = new Point();
        double hOff = 1;
        double vEff = 1;
        public void scrollViewerPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            scrollMousePoint = e.GetPosition(scrollViewer);
            hOff = scrollViewer.HorizontalOffset;
            vEff = scrollViewer.VerticalOffset;
            scrollViewer.CaptureMouse();
        }
        private void scrollViewerPreviewMouseMove(object sender, MouseEventArgs e)
        {

            if (scrollViewer.IsMouseCaptured)
            {
                scrollViewer.ScrollToHorizontalOffset(hOff + (scrollMousePoint.X - e.GetPosition(scrollViewer).X));
                scrollViewer.ScrollToVerticalOffset(vEff + (scrollMousePoint.Y - e.GetPosition(scrollViewer).Y));
            }
        }

        private void scrollViewerPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.ReleaseMouseCapture();
        }


        private void scrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point mouseAtImage = e.GetPosition(viewport);
            Point mouseAtScrollViewer = e.GetPosition(scrollViewer);

            ScaleTransform st = viewport.LayoutTransform as ScaleTransform;
            if (st == null)
            {
                st = new ScaleTransform();
                viewport.LayoutTransform = st;
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
            window.UpdateLayout();
            #endregion

            Vector offset = viewport.TranslatePoint(mouseAtImage, scrollViewer) - mouseAtScrollViewer;
            scrollViewer.ScrollToHorizontalOffset(offset.X);
            scrollViewer.ScrollToVerticalOffset(offset.Y);
            window.UpdateLayout();

            e.Handled = true;
        }

        private void UpdateSlaves(Quaternion q, double s, Vector3D t)
        {
            try
            {
                if (slaves != null)
                {
                    foreach (var i in slaves)
                    {
                        var mv = i.Children[0] as ModelVisual3D;
                        var t3Dg = mv.Transform as Transform3DGroup;

                        var groupScaleTransform = t3Dg.Children[0] as ScaleTransform3D;
                        var groupRotateTransform = t3Dg.Children[1] as RotateTransform3D;
                        var groupTranslateTransform = t3Dg.Children[2] as TranslateTransform3D;

                        groupScaleTransform.ScaleX = s;
                        groupScaleTransform.ScaleY = s;
                        groupScaleTransform.ScaleZ = s;
                        groupRotateTransform.Rotation = new AxisAngleRotation3D(q.Axis, q.Angle);
                        groupTranslateTransform.OffsetX = t.X;
                        groupTranslateTransform.OffsetY = t.Y;
                        groupTranslateTransform.OffsetZ = t.Z;
                    }
                }
            }
            catch { }
        }

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (!Enabled) return;
            e.Handled = true;

            var el = (UIElement)sender;

            if (el.IsMouseCaptured)
            {
                var delta = point - e.MouseDevice.GetPosition(el);
                var t = new Vector3D();

                delta /= 2;
                var q = rotation;

                if (rotating)
                {
                    // We can redefine this 2D mouse delta as a 3D mouse delta
                    // where "into the screen" is Z
                    var mouse = new Vector3D(delta.X, -delta.Y, 0);
                    var axis = Vector3D.CrossProduct(mouse, new Vector3D(0, 0, 1));
                    var len = axis.Length;
                    if (len < 0.00001 || scaling)
                        rotationDelta = new Quaternion(new Vector3D(0, 0, 1), 0);
                    else
                        rotationDelta = new Quaternion(axis, len);

                    q = rotationDelta * rotation;
                }
                else
                {
                    delta /= 20;
                    translateDelta.X = delta.X * -1;
                    translateDelta.Y = delta.Y;
                }

                t = translate + translateDelta;

                UpdateSlaves(q, scale * scaleDelta, t);
            }
        }

        private void MouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            if (!Enabled) return;
            e.Handled = true;


            if (Keyboard.IsKeyDown(Key.F1))
            {
                Reset();
                return;
            }

            var el = (UIElement)sender;
            point = e.MouseDevice.GetPosition(el);
            // Initialize the center of rotation to the lookatpoint
            if (!centered)
            {
                var camera = (ProjectionCamera)slaves[0].Camera;
                center = camera.LookDirection;
                centered = true;
            }

            scaling = (e.MiddleButton == MouseButtonState.Pressed);

            rotating = Keyboard.IsKeyDown(Key.Space) == false;

            el.CaptureMouse();
        }

        private void MouseUpHandler(object sender, MouseButtonEventArgs e)
        {
            if (!enabled) return;
            e.Handled = true;

            // Stuff the current initial + delta into initial so when we next move we
            // start at the right place.
            if (rotating)
                rotation = rotationDelta * rotation;
            else
            {
                translate += translateDelta;
                translateDelta.X = 0;
                translateDelta.Y = 0;
            }

            //scale = scaleDelta*scale;
            var el = (UIElement)sender;
            el.ReleaseMouseCapture();
        }

        private void Reset()
        {
            rotation = new Quaternion(0, 0, 0, 1);
            scale = 1;
            translate.X = 0;
            translate.Y = 0;
            translate.Z = 0;
            translateDelta.X = 0;
            translateDelta.Y = 0;
            translateDelta.Z = 0;

            // Clear delta too, because if reset is called because of a double click then the mouse
            // up handler will also be called and this way it won't do anything.
            rotationDelta = Quaternion.Identity;
            scaleDelta = 1;
            UpdateSlaves(rotation, scale, translate);
        }

        #endregion

        #region HIT TESTING

        // lista modela
        public static ArrayList models = new ArrayList();
        private GeometryModel3D hitgeo;

        private void MiddleButtonDownHandler(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton.Equals(MouseButtonState.Pressed))
            {
                System.Windows.Point mouseposition = e.GetPosition(viewport);
                Point3D testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
                Vector3D testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);

                PointHitTestParameters pointparams =
                         new PointHitTestParameters(mouseposition);
                RayHitTestParameters rayparams =
                         new RayHitTestParameters(testpoint3D, testdirection);

                //test for a result in the Viewport3D     
                hitgeo = null;
                VisualTreeHelper.HitTest(viewport, null, HTResult, pointparams);
            }
        }

        private HitTestResultBehavior HTResult(System.Windows.Media.HitTestResult rawresult)
        {

            RayHitTestResult rayResult = rawresult as RayHitTestResult;

            if (rayResult != null)
            {

                DiffuseMaterial darkSide =
                     new DiffuseMaterial(new SolidColorBrush(
                     System.Windows.Media.Colors.Red));
                bool gasit = false;
                for (int i = 0; i < models.Count; i++)
                {
                    if ((GeometryModel3D)models[i] == rayResult.ModelHit)
                    {
                        hitgeo = (GeometryModel3D)rayResult.ModelHit;
                        gasit = true;

                        // tooltip za pritisnuti entitet
                        MessageBox.Show(MapHandler.Entities[i].ToString());
                    }
                }
                if (!gasit)
                {
                    hitgeo = null;
                }
            }

            return HitTestResultBehavior.Stop;
        }

        /*private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < models.Count; i++)
            {
                ((GeometryModel3D)models[i]).Material = blue;
            }
            MiddleButtonDownHandler(viewport, e);
        }*/

        #endregion
    }
}
