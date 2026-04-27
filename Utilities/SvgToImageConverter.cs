using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Svg.Skia;
using SkiaSharp;
using System.Net;
using System.IO;

namespace GearOS.Utilities
{
    public class SvgToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string url = value as string;
            if (string.IsNullOrEmpty(url)) return null;

            try
            {
                using (var client = new WebClient())
                {
                    byte[] data = client.DownloadData(url);

                    // Gestion des PNG (Images Produits)
                    if (url.ToLower().EndsWith(".png"))
                    {
                        using (var stream = new MemoryStream(data))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.StreamSource = stream;
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            bitmap.Freeze();
                            return bitmap;
                        }
                    }

                    // Gestion des SVG (Logos et Icônes)
                    using (var stream = new MemoryStream(data))
                    {
                        var svg = new SKSvg();
                        if (svg.Load(stream) == null) return null;

                        int width = (int)Math.Max(svg.Picture.CullRect.Width, 100);
                        int height = (int)Math.Max(svg.Picture.CullRect.Height, 100);

                        var bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
                        bitmap.Lock();

                        var info = new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
                        using (var surface = SKSurface.Create(info, bitmap.BackBuffer, bitmap.BackBufferStride))
                        {
                            surface.Canvas.Clear(SKColors.Transparent);
                            surface.Canvas.DrawPicture(svg.Picture);
                        }

                        bitmap.AddDirtyRect(new System.Windows.Int32Rect(0, 0, width, height));
                        bitmap.Unlock();
                        bitmap.Freeze();
                        return bitmap;
                    }
                }
            }
            catch { return null; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}