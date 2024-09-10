using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace GraphTest
{
    /// <summary>
    /// StandardGraphControl.xaml の相互作用ロジック
    /// </summary>
    public partial class StandardGraphControl : UserControl
    {
        public event EventHandler<long> DrawCompleted;

        public StandardGraphControl()
        {
            InitializeComponent();
            this.Loaded += StandardGraphControl_Loaded;
            //            this.MouseWheel += StandardGraphControl_MouseWheel;
            this.PreviewMouseWheel += StandardGraphControl_MouseWheel;

        }

        private void StandardGraphControl_Loaded(object sender, RoutedEventArgs e)
        {
            DrawGraph();
        }

        private void DrawGraph()
        {
            Random rand = new Random();
            int pointCount = 30000;
            Point[] points = new Point[pointCount];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < pointCount; i++)
            {
                points[i] = new Point(rand.NextDouble() * DrawingCanvas.ActualWidth, rand.NextDouble() * DrawingCanvas.ActualHeight);
            }

            // 描画処理開始
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                for (int i = 0; i < pointCount - 1; i++)
                {
                    dc.DrawLine(new Pen(Brushes.Blue, 1), points[i], points[i + 1]);
                }
            }

            DrawingCanvas.Children.Clear();
            DrawingCanvas.Children.Add(new VisualHost(drawingVisual));

            stopwatch.Stop();
            //            MessageBox.Show($"WPF標準描画時間: {stopwatch.ElapsedMilliseconds} ms");
            DrawCompleted?.Invoke(this, stopwatch.ElapsedMilliseconds);

        }

        private class VisualHost : FrameworkElement
        {
            private readonly Visual _visual;

            public VisualHost(Visual visual)
            {
                _visual = visual;
                this.AddVisualChild(_visual);
            }

            protected override int VisualChildrenCount => 1;

            protected override Visual GetVisualChild(int index)
            {
                return _visual;
            }
        }

        private void StandardGraphControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // マウスホイールの方向に応じて拡大縮小
            double zoomFactor = (e.Delta > 0) ? 1.1 : 0.9;
            CanvasScaleTransform.ScaleX *= zoomFactor;
            CanvasScaleTransform.ScaleY *= zoomFactor;

            e.Handled = true;
        }
    }
}
