﻿using PredmetniZadatak2.Classes;
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
        Transformation transformation;
        HitTesting hitTestService;

        public MainWindow()
        {
            InitializeComponent();

            NetworkModel networkModel = new NetworkModel();
            networkModel = MapHandler.LoadModelToMap(networkModel, MyModel);

            transformation = new Transformation(viewport, skaliranje, this, translate, rotateX, rotateY);

            hitTestService = new HitTesting(viewport, model3DGroup, this);
        }

    }
}
