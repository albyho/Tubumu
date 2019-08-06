using System;
using System.IO;
using SkiaSharp;

namespace Tubumu.Core.Utilities.Security
{
    /// <summary>
    /// 生成验证码的类
    /// </summary>
    public class ValidationCodeCreater
    {
        private string ValidationCode { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="codeLength"></param>
        /// <param name="validateCode"></param>
        public ValidationCodeCreater(int codeLength, out string validateCode)
        {
            if (codeLength < 1)
                throw new ArgumentOutOfRangeException(nameof(codeLength));

            ValidationCode = CreateValidationCode(codeLength);
            validateCode = ValidationCode;
        }

        /// <summary>
        /// CreateValidationCode
        /// </summary>
        /// <param name="codeLength"></param>
        /// <returns></returns>
        public string CreateValidationCode(int codeLength)
        {
            var chars = "1234567890qwertyuipasdfghjklzxcvbnm";
            var rand = new Random(Guid.NewGuid().GetHashCode());

            string result = null;
            for (int i = 0; i < codeLength; i++)
            {
                var r = rand.Next(chars.Length);

                result = string.Concat(result, chars[r]);
            }

            return result;
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        public byte[] CreateValidationCodeGraphic()
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());

            var randAngle = 40;
            var mapWidth = ValidationCode.Length * 18;
            var mapHeight = 28;

            using (var bitmap = new SKBitmap(mapWidth, mapHeight))
            {
                using (var canvas = new SKCanvas(bitmap))
                {
                    canvas.Clear(SKColors.AliceBlue);

                    var paint = new SKPaint() { Color = SKColors.LightGray, };
                    for (int i = 0; i < 50; i++)
                    {
                        int x = rand.Next(0, bitmap.Width);
                        int y = rand.Next(0, bitmap.Height);

                        canvas.DrawRect(new SKRect(x, y, x + 1, y + 1), paint);
                    }

                    var chars = ValidationCode.ToCharArray();
                    var colors = new[] { SKColors.Black, SKColors.Red, SKColors.DarkBlue, SKColors.Green, SKColors.Orange, SKColors.Brown, SKColors.DarkCyan, SKColors.Purple };
                    var fonts = new[]
                    {
                        SKTypeface.FromFamilyName("Verdana"),
                        SKTypeface.FromFamilyName("Microsoft Sans Serif"),
                        SKTypeface.FromFamilyName("Comic Sans MS"),
                        SKTypeface.FromFamilyName("Arial")
                    };

                    canvas.Translate(-4, 0);

                    for (int i = 0; i < chars.Length; i++)
                    {
                        int colorIndex = rand.Next(colors.Length);
                        int fontIndex = rand.Next(fonts.Length);

                        var fontColor = colors[colorIndex];
                        var foneSize = rand.Next(18, 25);
                        float angle = rand.Next(-randAngle, randAngle);

                        SKPoint point = new SKPoint(16, 28 / 2 + 4);

                        canvas.Translate(point);
                        canvas.RotateDegrees(angle);

                        var textPaint = new SKPaint()
                        {
                            TextAlign = SKTextAlign.Center,
                            Color = fontColor,
                            TextSize = foneSize,
                            IsVerticalText = true,
                            IsAntialias = true,
                            Typeface = fonts[fontIndex],

                            //IsAntialias = rand.Next(1) == 1 ? true : false,
                            //FakeBoldText = true,
                            //FilterQuality = SKFilterQuality.High,
                            //HintingLevel = SKPaintHinting.Full,

                            //IsEmbeddedBitmapText = true,                    
                            //LcdRenderText = true,
                            //Style = SKPaintStyle.StrokeAndFill,
                            //TextEncoding = SKTextEncoding.Utf8,
                        };

                        canvas.DrawText(chars[i].ToString(), new SKPoint(0, 0), textPaint);
                        canvas.RotateDegrees(-angle);
                        canvas.Translate(0, -point.Y);
                    }

                    using (var image = SKImage.FromBitmap(bitmap))
                    {
                        using (var ms = new MemoryStream())
                        {
                            image.Encode(SKEncodedImageFormat.Png, 90).SaveTo(ms);
                            return ms.ToArray();
                        }
                    }
                }
            }
        }
    }
}
