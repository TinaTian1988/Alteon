using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Core.Common
{
    public class ImageHelper
    {
        public static bool MakeCenterThumbnail(string origPath, string destPath, int width, int height)
        {
            try
            {
                using (Image image = Image.FromFile(origPath))
                {
                    int originalWidth = image.Width;
                    int originalHeight = image.Height;
                    double ratio = Math.Max((originalWidth / (double)width < originalHeight / (double)height)
                                        ? originalWidth / (double)width
                                        : originalHeight / (double)height, 1);

                    var cutSize = new Size((int)Math.Round(width * ratio, 0), (int)Math.Round(height * ratio, 0));

                    int x = (int)Math.Round((double)(originalWidth - cutSize.Width) / 2, 0);
                    int y = (int)Math.Round((double)(originalHeight - cutSize.Height) / 2, 0); ;

                    var bitmap = new Bitmap(width, height);
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.Clear(Color.White);

                        var destRect = new Rectangle(0, 0, width, height);
                        var originalRect = new Rectangle(x, y, cutSize.Width, cutSize.Height);
                        graphics.DrawImage(image, destRect, originalRect, GraphicsUnit.Pixel);
                    }
                    #region wjy20131004修改
                    // 以下代码为保存图片时,设置压缩质量
                    EncoderParameters encoderParams = new EncoderParameters();
                    long[] quality = new long[1];
                    quality[0] = 100;
                    EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                    encoderParams.Param[0] = encoderParam;
                    //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.
                    ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo jpegICI = null;
                    for (int i = 0; i < arrayICI.Length; i++)
                    {
                        if (arrayICI[i].FormatDescription.Equals("JPEG"))
                        {
                            jpegICI = arrayICI[i];
                            //设置JPEG编码
                            break;
                        }
                    }
                    if (jpegICI != null)
                    {
                        bitmap.Save(destPath, jpegICI, encoderParams);
                    }

                    #endregion
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.White);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath + "t", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
    }
}
