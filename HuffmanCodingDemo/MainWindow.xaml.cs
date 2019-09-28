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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HuffmanCodingDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Ellipse ellipse = new Ellipse() { Width = 100, Height = 100, Fill = new SolidColorBrush() { Color = Colors.Black } };
            Canvas.SetLeft(ellipse, 100);
            Canvas.SetTop(ellipse, 100);
            canvas.Children.Add(ellipse);

        }
    }
}
