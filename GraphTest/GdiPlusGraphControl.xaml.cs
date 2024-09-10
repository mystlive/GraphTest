using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GraphTest
{
    public partial class GdiPlusGraphControl : UserControl
    {
        // ★修正ポイント1: 描画完了イベントを追加
        public event EventHandler<long> DrawCompleted;

        private float zoomFactor = 1.0f; // 拡大縮小の倍率

        public GdiPlusGraphControl()
        {
            InitializeComponent();
            this.Loaded += GdiPlusGraphControl_Loaded;
            this.MouseWheel += GdiPlusGraphControl_MouseWheel; // マウスホイールイベントを追加
            this.SizeChanged += GdiPlusGraphControl_SizeChanged; // ★サイズ変更時に再描画
        }

        private void GdiPlusGraphControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DrawGraph();
        }

        private void GdiPlusGraphControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            DrawGraph(); // ★サイズ変更時に再描画
        }

        // マウスホイールのイベントハンドラ
        private void GdiPlusGraphControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // マウスホイールの方向に応じて拡大縮小倍率を変更
            zoomFactor *= (e.Delta > 0) ? 1.1f : 0.9f;
            DrawGraph(); // 再描画
        }

        private void DrawGraph()
        {
            // 現在のUserControlのサイズに合わせて描画領域を調整
            int width = (int)this.ActualWidth;
            int height = (int)this.ActualHeight;

            if (width <= 0 || height <= 0)
                return; // サイズが有効でない場合は描画しない

            // ランダムデータ生成と描画時間の計測
            Random rand = new Random();
            int pointCount = 30000;
            Point[] points = new Point[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                points[i] = new Point(rand.Next(width), rand.Next(height)); // ★幅と高さを使用
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using (Bitmap bmp = new Bitmap(width, height)) // ★動的なサイズ
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    g.ScaleTransform(zoomFactor, zoomFactor); // 拡大縮小を適用

                    using (Pen pen = new Pen(Color.Blue, 1))
                    {
                        for (int i = 0; i < pointCount - 1; i++)
                        {
                            g.DrawRectangle(pen, points[i].X, points[i].Y, 1, 1); // 点を描画
                            g.DrawLine(pen, points[i], points[i + 1]); // 線を描画
                        }
                    }
                }

                // 描画結果をImageコントロールに表示
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    ms.Seek(0, SeekOrigin.Begin);

                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = ms;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();

                    RenderedImage.Source = bitmapImage;
                }
            }

            stopwatch.Stop();
            //            System.Windows.MessageBox.Show($"GDI+描画時間: {stopwatch.ElapsedMilliseconds} ms");
            DrawCompleted?.Invoke(this, stopwatch.ElapsedMilliseconds);

        }
    }
}
