using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
//using System.Text;
using System.Threading.Tasks;

namespace AccountingEveryDay.Utility
{
    public static class Utility
    {
        public static Bitmap ConvertImg(string imagePath, int targetWidth = 1920, int targetHeight = 1080)
        {
            Image originalImage = Image.FromFile(imagePath);
            // 計算縮放比例
            float scale = Math.Min((float)targetWidth / originalImage.Width, (float)targetHeight / originalImage.Height);
            int newWidth = (int)(originalImage.Width * scale);
            int newHeight = (int)(originalImage.Height * scale);

            // 驗證尺寸
            if (newWidth <= 0 || newHeight <= 0)
            {
                throw new InvalidOperationException("計算出的圖片尺寸無效。");
            }

            Bitmap resizedBitmap = new Bitmap(newWidth, newHeight);
            Graphics graphics = Graphics.FromImage(resizedBitmap);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);

            MemoryStream ms = new MemoryStream();

            ImageCodecInfo jpegCodec = GetEncoder(ImageFormat.Jpeg);
            if (jpegCodec == null)
            {
                throw new InvalidOperationException("找不到 JPEG 編碼器。");
            }

            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 30L);
            resizedBitmap.Save(ms, jpegCodec, encoderParameters);
            return new Bitmap(ms);
        }

        /// <summary>
        /// 調整圖片大小並進行壓縮
        /// </summary>
        /// <param name="image">原始圖片</param>
        /// <param name="width">目標寬度</param>
        /// <param name="height">目標高度</param>
        /// <param name="quality">JPEG 壓縮品質 (1-100)</param>
        /// <returns>處理後的 Bitmap</returns>
        public static Bitmap ResizeAndCompressImage(Image image, int width, int height, long quality)
        {
            // 將傳入的 Image 包裝為新的 Bitmap，確保與原始資源脫離
            Image sourceImage = new Bitmap(image);

            // 創建調整大小後的 Bitmap
            var resizedBitmap = new Bitmap(width, height);

            // 在新的 Bitmap 上進行繪製
            Graphics graphics = Graphics.FromImage(resizedBitmap);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;

            // 繪製圖片到目標大小
            graphics.DrawImage(sourceImage, 0, 0, width, height);

            // 如果需要壓縮，進行 JPEG 壓縮處理
            if (quality > 0)
            {
                var ms = new MemoryStream();

                // 獲取 JPEG 編碼器
                ImageCodecInfo jpegCodec = GetEncoder(ImageFormat.Jpeg);
                if (jpegCodec == null)
                {
                    throw new InvalidOperationException("找不到 JPEG 編碼器。");
                }

                // 設置壓縮品質
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                // 壓縮並保存到 MemoryStream
                resizedBitmap.Save(ms, jpegCodec, encoderParameters);

                // 從 MemoryStream 讀取新的 Bitmap，確保壓縮過後的圖片與 MemoryStream 無關
                return new Bitmap(ms);

            }

            // 如果不需要壓縮，直接返回調整大小後的 Bitmap
            return resizedBitmap;

        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


        // 只做壓縮，不做儲存，返回壓縮後的 Bitmap
        public static void CompressAndSaveImage(Image image, string outputPath, long quality)
        {
            // 確保圖片不是 null
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (string.IsNullOrEmpty(outputPath)) throw new ArgumentException("Output path cannot be null or empty", nameof(outputPath));

            // 獲取 JPEG 編碼器
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            if (jpgEncoder == null) throw new InvalidOperationException("JPEG encoder not found");

            // 設定圖像品質參數
            System.Drawing.Imaging.Encoder qualityEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters encoderParameters = new EncoderParameters(1);
            EncoderParameter encoderParameter = new EncoderParameter(qualityEncoder, quality);
            encoderParameters.Param[0] = encoderParameter;

            // 使用 Graphics 繪製壓縮圖片
            using (Bitmap compressedBitmap = new Bitmap(image.Width, image.Height))
            {
                using (Graphics g = Graphics.FromImage(compressedBitmap))
                {
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // 繪製原始圖像到新 Bitmap
                    g.DrawImage(image, 0, 0, image.Width, image.Height);
                }

                // 儲存壓縮圖片到指定路徑
                compressedBitmap.Save(outputPath, jpgEncoder, encoderParameters);
            }
        }








    }
}
