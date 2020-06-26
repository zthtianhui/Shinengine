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
using System.Windows.Controls;

namespace Shinengine.Surface
{
    /// <summary>
    /// Title.xaml 的交互逻辑
    /// </summary>
    public partial class Title : Page
    {
        #region tools
        bool nCanrun = true;

        [DllImport("Shinehelper.dll")]
        unsafe extern static public byte* getPCM();
        [DllImport("Shinehelper.dll")]
        extern public static bool waveInit(IntPtr hWnd, int channels, int sample_rate, int bits_per_sample, int size);
        [DllImport("Shinehelper.dll")]
        unsafe extern public static void waveWrite(byte* in_buf, int in_buf_len);
        [DllImport("Shinehelper.dll")]
        extern public static void waveClose();
        Direct2DImage DxBkGround = null;
        [DllImport("winmm")]
        static extern void timeBeginPeriod(int t);
        [DllImport("winmm")]
        static extern void timeEndPeriod(int t);
        #endregion
        unsafe public DrawProcResult DrawCallback(WicRenderTarget view, object Loadedsouce, int Width, int Height)
        {
            var video = Loadedsouce as VideoStreamDecoder;
            if (video == null)
                return DrawProcResult.Ignore;

            IntPtr dataPoint;
            int pitch;
            var res = video.TryDecodeNextFrame(out dataPoint, out pitch);
            if (!res)
            {
                DxBkGround = null;
                return DrawProcResult.Death;
            }
            var ImGc = new ImagingFactory();
            var WICBIT = new WICBitmap(ImGc, video.FrameSize.Width, video.FrameSize.Height, SharpDX.WIC.PixelFormat.Format32bppPBGRA, new DataRectangle(dataPoint, pitch));
            var BitSrc = D2DBitmap.FromWicBitmap(view, WICBIT);

            view.BeginDraw();
            view.DrawBitmap(BitSrc,
              new RawRectangleF(0, 0, Width, Height),
               1, SharpDX.Direct2D1.BitmapInterpolationMode.Linear,
               new RawRectangleF(0, 0, video.FrameSize.Width, video.FrameSize.Height));


            view.EndDraw();

            ImGc.Dispose();
            WICBIT.Dispose();

            BitSrc.Dispose();
            if (!nCanrun) return DrawProcResult.Death;
            return DrawProcResult.Commit;
        }
        public Title()
        {
            InitializeComponent();
            timeBeginPeriod(1);

            this.Background = new ImageBrush(new BitmapImage(new Uri("pack://siteoforigin:,,,/assets/CG/loading.png")));

            m_BGkMusic = new AudioPlayer("assets\\BGM\\pcpc006_bgm_01.wma", true);


            DxBkGround = new Direct2DImage(new Size2((int)BackGround.Width, (int)BackGround.Height), 30)
            {
                Loadedsouce = new VideoStreamDecoder("assets\\title.wmv")
            };

            DxBkGround.Disposed += (Loadedsouce, s) => { (Loadedsouce as VideoStreamDecoder).Dispose(); s.Dispose(); };
            DxBkGround.DrawProc += DrawCallback;

            DxBkGround.DrawStartup(BackGround);

            BkGrid.Unloaded += (e, v) =>
            {
                m_BGkMusic.canplay = false;
                nCanrun = false;
            };

        }

        public AudioPlayer m_BGkMusic = null;

    }
}
