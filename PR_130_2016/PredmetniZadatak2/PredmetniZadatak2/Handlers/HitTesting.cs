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
using System.Windows.Media.Media3D;

namespace PredmetniZadatak2.Handlers
{
    class HitTesting
    {
        private ToolTip toolTipForAll = new ToolTip();
        Window window;
        Model3DGroup model3DGroup;
        Viewport3D viewport;
        HashSet<GeometryModel3D> previusModels;

        public HitTesting(Viewport3D viewpor, Model3DGroup model3DGroup, Window window)
        {
            previusModels = new HashSet<GeometryModel3D>();
            this.viewport = viewpor;
            this.window = window;
            this.model3DGroup = model3DGroup;
            viewpor.MouseLeftButtonDown += MouseLeftButtonDown;
            CreateToolTip();
        }
        private HitTestResultBehavior HitResult(HitTestResult result)
        {
            var hitResult = result as RayHitTestResult;
            var value = hitResult?.ModelHit.GetValue(FrameworkElement.TagProperty);
            if (value is Entity)
            {
                toolTipForAll.Content = value.ToString();
                toolTipForAll.IsOpen = true;
            }
            if (value is LineEntity)
            {
                RestoreColor();
                var line = value as LineEntity;
                var a = hitResult.ModelHit as GeometryModel3D;
             
                var first = model3DGroup.Children.FirstOrDefault(item => (item.GetValue(FrameworkElement.TagProperty) as Entity)?.Id == line.FirstEnd);
                // var first = model3DGroup.Children.FirstOrDefault(item => (item.GetValue(FrameworkElement.TagProperty) as Entity)?.counter >3);
                var second = model3DGroup.Children.FirstOrDefault(item => (item.GetValue(FrameworkElement.TagProperty) as Entity)?.Id == line.SecondEnd);
                var first3D = (first as GeometryModel3D);
                var second3D = (second as GeometryModel3D);
                if (first3D is object)
                {
                    first3D.Material = new DiffuseMaterial(Brushes.Red);
                    previusModels.Add(first3D);
                }
                if (second3D is object)
                {
                    second3D.Material = new DiffuseMaterial(Brushes.Red);
                    previusModels.Add(second3D);
                }
            }
            return HitTestResultBehavior.Stop;
        }
        private void RestoreColor()
        {
            foreach (var item in previusModels)
            {
                Entity value = (Entity)item.GetValue(FrameworkElement.TagProperty);
                if(value is SubstationEntity)
                {
                    item.Material = new DiffuseMaterial(Brushes.Orange);
                }
                else if(value is SwitchEntity)
                {
                    item.Material = new DiffuseMaterial(Brushes.Green);
                }
                else if(value is NodeEntity)
                {
                    item.Material = new DiffuseMaterial(Brushes.Blue);
                }
                else
                    item.Material = new DiffuseMaterial(Brushes.Black);
            }
            previusModels.Clear();
        }
        private void CreateToolTip()
        {
            toolTipForAll.StaysOpen = false;
            toolTipForAll.IsOpen = false;
            toolTipForAll.Background = Brushes.DarkGray;
            toolTipForAll.BorderBrush = Brushes.Black;
            toolTipForAll.Foreground = Brushes.Black;
        }
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            toolTipForAll.IsOpen = false;
            var mouseposition = e.GetPosition(viewport);
            var testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
            var testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);
            var pointparams = new PointHitTestParameters(mouseposition);
            var rayparams = new RayHitTestParameters(testpoint3D, testdirection);
            VisualTreeHelper.HitTest(viewport, null, HitResult, pointparams);
        }
    }
}
