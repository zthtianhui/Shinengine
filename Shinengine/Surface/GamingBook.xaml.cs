﻿using Shinengine.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Shinengine.Data.DataStream;
using System.IO;

using Shinengine.Media;
using System.Media;
using SharpDX.Direct2D1;
using System.Threading;
using SharpDX;

using D2DBitmap = SharpDX.Direct2D1.Bitmap;
using DataStream = Shinengine.Data.DataStream;
using SharpDX.Mathematics.Interop;
using System.Windows.Media.Animation;
using System.Linq;
using System.Diagnostics;
using Shinengine.Theatre;

namespace Shinengine.Surface
{
    public class MeasureSize
    {
        /// <summary>
        /// 宽度
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// 构造
        /// </summary>
        public MeasureSize() { }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public MeasureSize(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
    /// <summary>
    /// GamingBook.xaml 的交互逻辑
    /// </summary>
    public partial class GamingBook : Page
    {
        static public FormattedText MeasureTextWidth(TextBlock target, double fontSize, string str)
        {

            FormattedText formattedText = new
                   FormattedText(str, System.Globalization.CultureInfo.CurrentCulture,
                   FlowDirection.LeftToRight,
                   new Typeface(target.FontFamily, target.FontStyle,
                   target.FontWeight, target.FontStretch),
                   fontSize, target.Foreground);


            return formattedText;

        }
        string Preparation = "";  //显示最后18行
        private void CommitText(int start_line = 18)
        {
            List<string> lines = Preparation[0..^1].Split('\n').ToList();
            restIlt.Maximum = lines.Count - 18 < 0 ? 0 : lines.Count - 18;
            if (lines.Count <= start_line)
            {
                _Contents.Text = string.Join("\n", lines);
                return;
            }
            List<string> _lines = new List<string>();
            for (int i = lines.Count - start_line; i - (lines.Count - start_line) < 18; i++)
            {
                if (i >= lines.Count)
                    break;
                _lines.Add(lines[i]);
            }

            _Contents.Text = string.Join("\n", _lines);
            return;
        }
        readonly ManualResetEvent call_next = new ManualResetEvent(false);

        SoundPlayer bPlayer = null;
        DataStream ms_mp;

        readonly Window _main_window = null;
        public GamingBook(Window main_window)
        {
            _main_window = main_window;
            InitializeComponent();
        }
        PageInfo np;
        public void Inint(DataStream stram)
        {
            restIlt.Maximum = 0;
            np = stram.GetSignalPage(0);
            ms_mp = stram;

            Book.Background = new System.Windows.Media.SolidColorBrush(Colors.Black);
            Page.Opacity = 0;
            Illustration.Opacity = 0;


            Illustration.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/" + np.Illustration));
            bPlayer = new SoundPlayer
            {
                SoundLocation = np.BackgroundMusic
            };
            bPlayer.Play();
            Preparation += np.Contents[0] + "\n";
            CommitText();

            Storyboard m_proj = new Storyboard();
            Storyboard m_proj_2 = new Storyboard();

            DoubleAnimation dam_proj = new DoubleAnimation
            {
                From = 1,
                To = 0.25,
                Duration = TimeSpan.FromSeconds(2.5),
                FillBehavior = FillBehavior.HoldEnd
            };

            DoubleAnimation dam_proj_2 = new DoubleAnimation
            {
                From = 0.25,
                To = 1,
                Duration = TimeSpan.FromSeconds(2.5),
                FillBehavior = FillBehavior.HoldEnd
            };

            m_proj.Children.Add(dam_proj);
            m_proj_2.Children.Add(dam_proj_2);

            Storyboard.SetTarget(m_proj, PageEdge);
            Storyboard.SetTarget(m_proj_2, PageEdge);

            Storyboard.SetTargetProperty(m_proj, new PropertyPath("(Opacity)"));
            Storyboard.SetTargetProperty(m_proj_2, new PropertyPath("(Opacity)"));

            m_proj.Completed += (e, v) => { m_proj_2.Begin(); };
            m_proj_2.Completed += (e, v) => { m_proj.Begin(); };

            m_proj.Begin();
            return;
        }

        public void Start(int page_count)
        {
            var m_mutil = new Thread(() =>
            {
                EasyAmal am_il = new EasyAmal(Illustration, "(Opacity)", 0.0, 1.0, 1.8);
                EasyAmal am_pg = new EasyAmal(Page, "(Opacity)", 0.0, 1.0, 0.6);

                am_il.Start(false);
                am_pg.Start(false);


                int ral_page = 0;
                for (int i = 1; ; i++)
                {

                    if (np.Contents.Count == i)
                    {
                        i = 0;
                        ral_page++;

                        if (ral_page >= page_count)
                            break;
                        call_next.WaitOne();
                        np = ms_mp.GetSignalPage(ral_page);


                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            Preparation += "\n";
                            CommitText();
                        }));
                        if (bPlayer.SoundLocation != np.BackgroundMusic)
                        {
                            bPlayer.Stop();
                            bPlayer.Dispose();
                            bPlayer = new SoundPlayer
                            {
                                SoundLocation = np.BackgroundMusic
                            };
                            bPlayer.Play();
                        }

                        ManualResetEvent wait_sp = new ManualResetEvent(false);

                        double vara = 1.0, varb = 0.0;
                        double increment = 1 / (0.6 * 30);

                        Illustration.Dispatcher.Invoke(new Action(() =>
                        {

                            var converta = Stage.BitmapImage2Bitmap(Illustration.Source as BitmapImage);
                            var convertb = Stage.BitmapImage2Bitmap(new BitmapImage(new Uri("pack://siteoforigin:,,,/" + np.Illustration)));


                            var videoCtrl = new Direct2DImage(new Size2((int)Illustration.Width, (int)Illustration.Height), 30)
                            {
                                Loadedsouce = null
                            };

                            D2DBitmap ral_picA = Stage.ConvertFromSystemBitmap(converta, videoCtrl.m_d2d_info.View);
                            D2DBitmap ral_picB = Stage.ConvertFromSystemBitmap(convertb, videoCtrl.m_d2d_info.View);

                            videoCtrl.FirstDraw += (t, v, b, s) =>
                            {
                                t.View.BeginDraw();

                                t.View.DrawBitmap(ral_picA,
                         new RawRectangleF(0, 0, b, s),
                          1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear,
                          new RawRectangleF(0, 0, converta.Width, converta.Height));

                                t.View.EndDraw();
                                return;
                            };

                            videoCtrl.DrawProc += (t, v, b, s) =>
                            {
                                if (vara < 0 || varb > 1)
                                {
                                    wait_sp.Set();

                                    ral_picA.Dispose();
                                    ral_picB.Dispose();

                                    converta.Dispose();
                                    convertb.Dispose();
                                    return DrawProcResult.Death;
                                }
                                var bmplist = v as List<D2DBitmap>;

                                t.View.BeginDraw();

                                t.View.DrawBitmap(ral_picA,
                         new RawRectangleF(0, 0, b, s),
                          (float)vara, SharpDX.Direct2D1.BitmapInterpolationMode.Linear,
                          new RawRectangleF(0, 0, converta.Width, converta.Height));
                                t.View.DrawBitmap(ral_picB,
                        new RawRectangleF(0, 0, b, s),
                         (float)varb, SharpDX.Direct2D1.BitmapInterpolationMode.Linear,
                         new RawRectangleF(0, 0, convertb.Width, convertb.Height));

                                t.View.EndDraw();

                                //vara -= increment;
                                varb += increment;
                                return DrawProcResult.Normal;
                            };
                            videoCtrl.DrawStartup(Illustration);
                        }));

                        wait_sp.WaitOne();
                        //     Illustration.Dispatcher.Invoke(new Action(()=> { Illustration.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/" + np.Illustration)); })); 
                    }

                    call_next.WaitOne();
                    call_next.Reset();
                    _Contents.Dispatcher.Invoke(new Action(() =>
                    {
                        restIlt.Value = 0;
                    }));
                    string load_printed = "";
                    foreach (var c in np.Contents[i])
                    {

                        _Contents.Dispatcher.Invoke(new Action(() =>
                        {
                            load_printed += c;
                            var ap_l = MeasureTextWidth(_Contents, _Contents.FontSize, load_printed);
                            if (ap_l.Width > _Contents.Width)
                            {
                                _Contents.Dispatcher.Invoke(new Action(() => { Preparation += '\n'; CommitText(); }));
                                load_printed = "";
                            }
                            Preparation += c;
                            CommitText();
                        }));
                        Thread.Sleep(30);
                    }
                    _Contents.Dispatcher.Invoke(new Action(() =>
                    {
                        Preparation += "\n";
                        CommitText();

                    }));

                }
            })
            {
                IsBackground = true
            };
            m_mutil.Start();
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Page_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                call_next.Set();
        }

        private void RestIlt_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CommitText((int)(restIlt.Value + 18));
        }

        private void Page_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (restIlt.Value < restIlt.Maximum)
                    restIlt.Value += 1;
            }
            else
            {
                if (restIlt.Value > restIlt.Minimum)
                    restIlt.Value -= 1;
                else
                {
                    call_next.Set();
                }
            }
        }
    }
}
