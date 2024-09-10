using ScottPlot;
using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace GraphTest
{
    public partial class ScottPlotGraphControl : UserControl
    {
        public event EventHandler<long> DrawCompleted;

        public ScottPlotGraphControl()
        {
            InitializeComponent();
            SetupPlot();
        }

        private void SetupPlot()
        {
            Random rand = new Random();
            int pointCount = 30000;
            double[] dataX = new double[pointCount];
            double[] dataY = new double[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                dataX[i] = i;
                dataY[i] = rand.NextDouble() * 200;
            }

            // ★最新バージョンの ScottPlot では AddScatter の代わりに Add.Scatter を使用
            ScottPlotView.Plot.Clear(); // 既存データをクリア
            ScottPlotView.Plot.Add.Scatter(dataX, dataY); // スキャッタプロットを追加

            ScottPlotView.Plot.Title("ScottPlot Graph");
            ScottPlotView.Plot.XLabel("X Axis");
            ScottPlotView.Plot.YLabel("Y Axis");

            ScottPlotView.Refresh(); // プロットを更新して描画
        }

    }
}
