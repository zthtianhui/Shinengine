﻿using SharpDX.Mathematics.Interop;
using System;
using System.Windows;
using System.Windows.Input;

using System.Runtime.InteropServices;


using SharpDX;
using SharpDX.Direct2D1;
using System.Windows.Media.Imaging;
using ImageBrush = System.Windows.Media.ImageBrush;
using System.Windows.Interop;

using WICBitmap = SharpDX.WIC.Bitmap;
using D2DBitmap = SharpDX.Direct2D1.Bitmap;
using SharpDX.WIC;
using System.Windows.Media;
using System.Media;

using Shinengine.Media;
using Color = System.Windows.Media.Color;

using NAudio.Wave;
using System.Threading;
using System.Drawing;
using SharpDX.DXGI;
using Shinengine.Data;
using System.Windows.Controls;
using System.Collections.Generic;
using FFmpeg.AutoGen;
using Image = System.Windows.Controls.Image;
using Shinengine.Scripts;
using System.DirectoryServices.ActiveDirectory;

namespace Shinengine.Surface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       static public void resizeToGrid(UIElementCollection grid, double width_rate, double height_rate)
        {
            
            foreach (var i in grid)
            {
                var _i = i as UIElement;
                if (_i.GetType() == typeof(System.Windows.Controls.Grid))
                {
                    var _i_r = _i as Grid;
                    if (_i_r.Name == "foreg" || _i_r.Name == "Page" || _i_r.Name == "character_usage" || _i_r.Name.Contains("save")) 
                    {
                        _i_r.Width = _i_r.Width * width_rate;
                        _i_r.Height = _i_r.Height * height_rate;

                        var new_margin = new Thickness(_i_r.Margin.Left * width_rate, _i_r.Margin.Top * height_rate, _i_r.Margin.Right * width_rate, _i_r.Margin.Bottom * height_rate);
                        _i_r.Margin = new_margin;
                    }
                    resizeToGrid(_i_r.Children, width_rate, height_rate);
                    continue;
                }
                if(_i is Image)
                {
                    var _i_r = _i as Image;
                    _i_r.Width = _i_r.Width * width_rate;
                    _i_r.Height = _i_r.Height * height_rate;

                    var new_margin = new Thickness(_i_r.Margin.Left * width_rate, _i_r.Margin.Top * height_rate, _i_r.Margin.Right * width_rate, _i_r.Margin.Bottom * height_rate);
                    _i_r.Margin = new_margin;
                }
                else if (_i is Button)
                {
                    var _i_r = _i as Button;
                    _i_r.Width = _i_r.Width * width_rate;
                    _i_r.Height = _i_r.Height * height_rate;

                    var new_margin = new Thickness(_i_r.Margin.Left * width_rate, _i_r.Margin.Top * height_rate, _i_r.Margin.Right * width_rate, _i_r.Margin.Bottom * height_rate);
                    _i_r.Margin = new_margin;
                }
                else if (_i is Canvas)
                {
                    var _i_r = _i as Canvas;
                    _i_r.Width = _i_r.Width * width_rate;
                    _i_r.Height = _i_r.Height * height_rate;

                    var new_margin = new Thickness(_i_r.Margin.Left * width_rate, _i_r.Margin.Top * height_rate, _i_r.Margin.Right * width_rate, _i_r.Margin.Bottom * height_rate);
                    _i_r.Margin = new_margin;
                }
                else if (_i is TextBlock)
                {
                    var _i_r = _i as TextBlock;
                    _i_r.Width = _i_r.Width * width_rate;
                    _i_r.Height = _i_r.Height * height_rate;

                    _i_r.FontSize = _i_r.FontSize * height_rate;

                    var new_margin = new Thickness(_i_r.Margin.Left * width_rate, _i_r.Margin.Top * height_rate, _i_r.Margin.Right * width_rate, _i_r.Margin.Bottom * height_rate);
                    _i_r.Margin = new_margin;
                }
                else if (_i is Slider)
                {
                    var _i_r = _i as Slider;
                    _i_r.Width = _i_r.Width * width_rate;
                    _i_r.Height = _i_r.Height * height_rate;

                    var new_margin = new Thickness(_i_r.Margin.Left * width_rate, _i_r.Margin.Top * height_rate, _i_r.Margin.Right * width_rate, _i_r.Margin.Bottom * height_rate);
                    _i_r.Margin = new_margin;
                }
                else if(_i is StackPanel)
                {
                    var _i_r = _i as StackPanel;
                    _i_r.Width = _i_r.Width * width_rate;
                    _i_r.Height = _i_r.Height * height_rate;

                    var new_margin = new Thickness(_i_r.Margin.Left * width_rate, _i_r.Margin.Top * height_rate, _i_r.Margin.Right * width_rate, _i_r.Margin.Bottom * height_rate);
                    _i_r.Margin = new_margin;

                    resizeToGrid(_i_r.Children, width_rate, height_rate);
                }

            }
        }
       static  private void resizeEvt(Grid page,Size2 oldSize, Size2 newSize)
        {
            double width_rate = (double)newSize.Width / (double)oldSize.Width;
            double height_rate = (double)newSize.Height / (double)oldSize.Height;

            resizeToGrid(page.Children, width_rate, height_rate);
        }
        static public Title title = null;
        static public  GamingBook bookMode = null;
        static public GamingTheatre theatreMode = null;
        static public Setting settere = null;
        static public SaveLoad sldata = null;

        static public MainWindow m_window = null;

        StaticCharacter.ChangeableAreaInfo[] m_Infos = null;
    

        public MainWindow()
        {
            InitializeComponent();
            m_window = this;
            if (SharedSetting.FullS)
            {

                this.WindowStyle = WindowStyle.None;
                this.WindowState = System.Windows.WindowState.Maximized;
            }

            FFmpegBinariesHelper.RegisterFFmpegBinaries();
            return;
        }
        static public GamingBook switchToBookmode()
        {
            GamingBook mbp = new GamingBook(m_window);
            mbp.Inint(new Data.DataStream("Book1.xml"));
            mbp.Start(0);

            bookMode = mbp;
            if (SharedSetting.FullS)
            {
                resizeEvt(bookMode.Book, new Size2(1280, 720), new Size2((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight));

            }
            return mbp; 
        }
        static public Title switchToTitle()
        {
            Title mlp = new Title();
            mlp.setting.Click += (e, v) =>
            {
                settere = new Setting(new BitmapImage(new Uri("pack://siteoforigin:,,,/assets/CG/10.png")), null, null);
                if (SharedSetting.FullS)
                {
                    resizeEvt(settere.mpOi, new Size2(1280, 720), new Size2((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight));
                }
                settere.foreg.MouseRightButtonUp += (e, v) =>
                {
                    switchToTitle();
                    m_window.Content = title.Content;
                };
                settere.fullandwindow.Click += (e, v) =>
                {
                    SharedSetting.FullS = !SharedSetting.FullS;
                    if (SharedSetting.FullS)
                    {
                        m_window.WindowStyle = WindowStyle.None;
                        m_window.WindowState = System.Windows.WindowState.Maximized;
                        resizeEvt(settere.mpOi, new Size2(1280, 720), new Size2((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight));
                    }
                    else
                    {
                        m_window.WindowStyle = WindowStyle.SingleBorderWindow;
                        m_window.WindowState = System.Windows.WindowState.Normal;
                        resizeEvt(settere.mpOi, new Size2((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight), new Size2(1280, 720));
                    }

                };
                m_window.Content = settere.Content;
            };
            mlp.EnExit.Click += (e, v) =>
            {
                m_window.Close();
            };
            mlp.ExitButton.Click += (e, v) =>
            {
                m_window.Close();
            };
            mlp.StartButton.Click += (e, v) =>
            {

                mlp.StartButton.IsEnabled = false;
                mlp.Background = new System.Windows.Media.SolidColorBrush(Colors.White);
                EasyAmal maa = null;

                maa = new EasyAmal(title.BkGrid, "(Opacity)", 1.0, 0.0, SharedSetting.switchSpeed / 2, (e, v) =>
                    {
                        theatreMode = switchToSignalTheatre();
                        theatreMode.m_theatre.saved_frame = 0;
                        theatreMode.Start(Chapter1.Chapter1Script, () =>
                        {
                            title = switchToTitle();
                            m_window.Content = title.Content;

                            theatreMode.m_logo.Dispose();
                            theatreMode.m_theatre.SetBackgroundMusic();
                            theatreMode.m_theatre.Exit();

                            if (theatreMode != null)
                                theatreMode = null;
                        });
                        m_window.Content = theatreMode.Content;

                        maa.stbd.Stop();
                    });
                maa.Start(true);
            };
            title = mlp;
            if (SharedSetting.FullS)
            {
                resizeEvt(title.BkGrid, new Size2(1280, 720), new Size2((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight));

            }

            return mlp;
        }

        static public GamingTheatre switchToSignalTheatre()
        {


            GamingTheatre m_game = new GamingTheatre();
            if (SharedSetting.FullS)
            {
                resizeEvt(m_game.SBK, new Size2(1280, 720), new Size2((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight));
            }
            m_game.Init(m_window);
            m_game.toTitle.Click += (e, v) =>
            {
                title = switchToTitle();
                m_window.Content = title.Content;

                theatreMode.m_logo.Dispose();
                theatreMode.m_theatre.SetBackgroundMusic();
                theatreMode.m_theatre.Exit();

                if (theatreMode != null)
                    theatreMode = null;
            };
            m_game.SaveLoad.Click += (e, v) =>
            {
                if (GamingTheatre.isSkiping)
                {
                    return;
                }
                GamingTheatre.AutoMode = false;
                if (m_game.ShowIn.Children.Contains(m_game.auto_icon))
                {
                    m_game.ShowIn.Children.Remove(m_game.auto_icon);
                }

                var m_thread_intp = new Thread(() =>
                {
                    EasyAmal mpos = new EasyAmal(m_game.SBK, "(Opacity)", 1.0, 0.0, SharedSetting.switchSpeed);
                    mpos.Start(false);/// hide tview

                    m_game.Dispatcher.Invoke(new Action(() =>
                    {
                        SaveLoad mst = new SaveLoad(0, m_game.m_theatre.saved_frame, m_game.m_theatre.stage.last_save);
                        if (SharedSetting.FullS)
                        {
                            resizeEvt(mst.Forgan, new Size2(1280, 720), new Size2((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight));
                        }
                        sldata = mst;

                        bool canFocue = false;
                        sldata.exitlpg.Click += (e, v) => 
                        {
                            if (canFocue) return;
                            canFocue = true;
                            new Thread(() =>
                            {

                                EasyAmal mpos2 = new EasyAmal(sldata.Forgan, "(Opacity)", 1.0, 0.0, SharedSetting.switchSpeed, (e, c) =>
                                {
                                    m_window.Content = m_game.Content;
                                    EasyAmal mpos = new EasyAmal(m_game.SBK, "(Opacity)", 0.0, 1.0, SharedSetting.switchSpeed);
                                    mpos.Start(true);/// hide tview
                                    sldata = null;
                                });
                                mpos2.Start(true);
                            }).Start();

                        };
                        sldata.Forgan.MouseRightButtonUp += (e, v) =>
                        {
                            if (canFocue) return;
                            canFocue = true;
                            new Thread(() =>
                            {

                                EasyAmal mpos2 = new EasyAmal(sldata.Forgan, "(Opacity)", 1.0, 0.0, SharedSetting.switchSpeed, (e, c) =>
                                {
                                    m_window.Content = m_game.Content;
                                    EasyAmal mpos = new EasyAmal(m_game.SBK, "(Opacity)", 0.0, 1.0, SharedSetting.switchSpeed);
                                    mpos.Start(true);/// hide tview
                                    sldata = null;
                                });
                                mpos2.Start(true);
                            }).Start();

                        };

                        EasyAmal mpos = new EasyAmal(mst.Forgan, "(Opacity)", 0.0, 1.0, SharedSetting.switchSpeed);
                        mpos.Start(true);

                        m_window.Content = mst.Content;
                    }));

                });
                m_thread_intp.IsBackground = true;
                m_thread_intp.Start();
            };
            m_game.setting.Click += (e, v) =>
            {
                if (GamingTheatre.isSkiping)
                {
                    return;
                }
                GamingTheatre.AutoMode = false;
                if (m_game.ShowIn.Children.Contains(m_game.auto_icon))
                {
                    m_game.ShowIn.Children.Remove(m_game.auto_icon);
                }
                #region 获取上一次绘图
                var lpic = m_game.m_theatre.stage.last_save;

                var mbps = new WriteableBitmap((int)m_game.BG.Width, (int)m_game.BG.Height, 72, 72, System.Windows.Media.PixelFormats.Pbgra32, null);
                mbps.Lock();

                var mic_lock = lpic.Lock(BitmapLockFlags.Read);
                unsafe
                {
                    Direct2DImage.RtlMoveMemory((void*)mbps.BackBuffer, (void*)mic_lock.Data.DataPointer, mbps.PixelHeight * mbps.BackBufferStride);
                }
                mic_lock.Dispose();
                mbps.AddDirtyRect(new Int32Rect(0, 0, (int)m_game.BG.Width, (int)m_game.BG.Height));
                mbps.Unlock();
                #endregion
                var m_thread_intp = new Thread(() =>
                {
                    m_game.m_theatre.usage.Hide(null, false);
                    m_game.Dispatcher.Invoke(new Action(() =>
                    {
                        Setting mst = new Setting(mbps, m_game.m_theatre.m_player, m_game.m_theatre.m_em_player);
                        if (SharedSetting.FullS)
                        {
                            resizeEvt(mst.mpOi, new Size2(1280, 720), new Size2((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight));
                        }
                        settere = mst;
                        settere.fullandwindow.IsEnabled = false;
                        EasyAmal mpos = new EasyAmal(settere.foreg, "(Opacity)", 0.0, 1.0, SharedSetting.switchSpeed);

                        bool canFocue = false;
                        settere.exitlpg.Click += (e, v) => 
                        {
                            if (canFocue) return;
                            canFocue = true;
                            new Thread(() =>
                            {

                                EasyAmal mpos2 = new EasyAmal(settere.foreg, "(Opacity)", 1.0, 0.0, SharedSetting.switchSpeed, (e, c) =>
                                {
                                    m_window.Content = m_game.Content;
                                    m_game.m_theatre.usage.Show(null, true);
                                    settere = null;
                                });
                                mpos2.Start(true);
                            }).Start();
                        };
                        settere.mpOi.MouseRightButtonUp += (e, v) =>
                        {
                            if (canFocue) return;
                            canFocue = true;
                            new Thread(() =>
                            {

                                EasyAmal mpos2 = new EasyAmal(settere.foreg, "(Opacity)", 1.0, 0.0, SharedSetting.switchSpeed, (e, c) =>
                                {
                                    m_window.Content = m_game.Content;
                                    m_game.m_theatre.usage.Show(null, true);
                                    settere = null;
                                });
                                mpos2.Start(true);
                            }).Start();

                        };
                        mpos.Start(true);
                        m_window.Content = mst.Content;
                    }));

                });
                m_thread_intp.IsBackground = true;
                m_thread_intp.Start();

            };

            m_game.EnExit.Click += (e, v) =>
            {
                m_window.Close();
            };
          
            m_game.BG.Opacity = 0;
            m_game.Usage.Opacity = 0;

           

            return m_game;


        }


        private void BkGrid_Loaded(object sender, RoutedEventArgs e)
        {
            switchToTitle();
            this.Content = title.Content;

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            PInvoke.Kernel32.ExitProcess(0);
        }

        private void mmKeyDown(object sender, KeyEventArgs e)
        {
            if (theatreMode == null)
                return;
            if (settere != null || theatreMode.isBakcloging)
            {
                return;
            }
            if (e.Key == Key.LeftCtrl)
                GamingTheatre.isSkiping = true;

            if (theatreMode.m_theatre.call_next != null) theatreMode.m_theatre.call_next.Set();
            theatreMode.Menu.IsEnabled = false;
            if (!theatreMode.ShowIn.Children.Contains(theatreMode.skip_icon))
            {
                theatreMode.ShowIn.Children.Add(theatreMode.skip_icon);
            }


        }

        private void mmKeyUp(object sender, KeyEventArgs e)
        {
            if (theatreMode == null)
                return;
            if (settere != null || theatreMode.isBakcloging)
            {
                return;
            }
            if (e.Key == Key.LeftCtrl)
                GamingTheatre.isSkiping = false;
            if (theatreMode != null)
            {
                theatreMode.Menu.IsEnabled = true;
                if (theatreMode.ShowIn.Children.Contains(theatreMode.skip_icon))
                {
                    theatreMode.ShowIn.Children.Remove(theatreMode.skip_icon);
                }
            }
        }
    }
}