using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OxyPlot;
using OxyPlot.Series;

namespace GraphTest
{
    public partial class OxyPlotGraphControl : UserControl
    {
        public event EventHandler<long> DrawCompleted;

        public OxyPlotGraphControl()
        {
            InitializeComponent();
            SetupPlot();
        }

        private void SetupPlot()
        {
            // ランダムデータ生成と描画時間計測の開始
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // OxyPlotのPlotModelを作成
            var model = new PlotModel { Title = "OxyPlot Graph" };
            var series = new LineSeries();

            // ランダムデータを3万ポイント生成
            var rand = new Random();
            int pointCount = 30000;

            for (int i = 0; i < pointCount; i++)
            {
                series.Points.Add(new DataPoint(i, rand.NextDouble()));
            }

            // シリーズをPlotModelに追加
            model.Series.Add(series);

            // 描画を完了し、時間計測の終了
            PlotViewElement.Model = model;

            stopwatch.Stop();
            //            MessageBox.Show($"OxyPlot描画時間: {stopwatch.ElapsedMilliseconds} ms");
            DrawCompleted?.Invoke(this, stopwatch.ElapsedMilliseconds);
        }
    }
}
