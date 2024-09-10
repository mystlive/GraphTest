using System;
using System.ComponentModel;
using System.Windows;

namespace GraphTest
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string drawTime;

        public string DrawTime
        {
            get { return drawTime; }
            set
            {
                drawTime = value;
                OnPropertyChanged("DrawTime");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        // WPFのグラフを表示
        private void ShowWpfGraph_Click(object sender, RoutedEventArgs e)
        {
            var wpfGraphControl = new StandardGraphControl();

            // ★修正ポイント3: WPFグラフの描画完了イベントをハンドリングして描画時間を表示
            wpfGraphControl.DrawCompleted += (s, drawTime) =>
            {
                DrawTime = $"描画時間: {drawTime} ms";
            };

            GraphContainer.Content = wpfGraphControl;
        }

        // OxyPlotのグラフを表示
        private void ShowOxyPlotGraph_Click(object sender, RoutedEventArgs e)
        {
            var oxyPlotGraphControl = new OxyPlotGraphControl();

            // ★修正ポイント4: OxyPlotの描画完了イベントをハンドリングして描画時間を表示
            oxyPlotGraphControl.DrawCompleted += (s, drawTime) =>
            {
                DrawTime = $"描画時間: {drawTime} ms";
            };

            GraphContainer.Content = oxyPlotGraphControl;
        }

        // GDI+グラフを表示
        private void ShowGdiGraph_Click(object sender, RoutedEventArgs e)
        {
            var gdiGraphControl = new GdiPlusGraphControl();

            // ★修正ポイント1: GDI+描画完了イベントをハンドリングして描画時間をステータスバーに表示
            gdiGraphControl.DrawCompleted += (s, drawTime) =>
            {
                DrawTime = $"描画時間: {drawTime} ms";
            };

            GraphContainer.Content = gdiGraphControl;
        }

        private void ShowScottPlotGraph_Click(object sender, RoutedEventArgs e)
        {
            var scottPlotGraphControl = new ScottPlotGraphControl();

            // ScottPlotグラフの描画完了イベントをハンドリングして描画時間を表示
            scottPlotGraphControl.DrawCompleted += (s, drawTime) =>
            {
                DrawTime = $"描画時間: {drawTime} ms";
            };

            GraphContainer.Content = scottPlotGraphControl;
        }
    }
}
