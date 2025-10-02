using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing.Processing;

namespace OpenAuth
{
    public class MathCaptchaHelper
    {
        private readonly Random _random = new Random();

        private const string FontFamily = "Arial";

        public string Question { get; private set; }

        public int CorrectAnswer { get; private set; }

        public string GenerateImage(int width = 120, int height = 40)
        {
            GenerateQuestion();

            using var image = new Image<Rgba32>(width, height);

            // 背景色
            image.Mutate(ctx => ctx.BackgroundColor(Color.White));

            // 添加噪点和文本
            image.Mutate(ctx =>
            {
                AddNoise(ctx, width, height);
                DrawText(ctx, width, height);
            });


            return ImageToBase64(image);
        }


        private string ImageToBase64(Image<Rgba32> image)
        {
            using var memoryStream = new MemoryStream();
            image.SaveAsPng(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            return "data:image/png;base64," + Convert.ToBase64String(imageBytes);
        }


        private void GenerateQuestion()
        {
            int num1 = _random.Next(10, 50);
            int num2 = _random.Next(10, 50);

            if (_random.Next(0, 2) == 0)
            {
                Question = $"{num1} + {num2} = ?";
                CorrectAnswer = num1 + num2;
            }
            else
            {
                if (num1 < num2)
                {
                    (num1, num2) = (num2, num1); // 交换确保大数在前
                }
                Question = $"{num1} - {num2} = ?";
                CorrectAnswer = num1 - num2;
            }
        }
        // 生成随机颜色
        private Color GetRandomNoiseColor()
        {
            // 生成偏浅的随机颜色（避免与黑色文字混淆）
            return Color.FromRgb(
                (byte)_random.Next(20, 160),
                (byte)_random.Next(20, 160),
                (byte)_random.Next(20, 160)
            );
        }

        // 添加噪点干扰
        private void AddNoise(IImageProcessingContext ctx, int width, int height)
        {

            // 随机点（每个点颜色不同）
            for (int i = 0; i < 50; i++)
            {
                int x = _random.Next(0, width);
                int y = _random.Next(0, height);
                ctx.Fill(GetRandomNoiseColor(), new RectangleF(x, y, 1, 1));
            }

            // 随机线（每条线颜色不同）
            for (int i = 0; i < 3; i++)
            {
                int x1 = _random.Next(0, width);
                int y1 = _random.Next(0, height);
                int x2 = _random.Next(0, width);
                int y2 = _random.Next(0, height);
                ctx.DrawLine(GetRandomNoiseColor(), 1f, new PointF(x1, y1), new PointF(x2, y2));
            }

            // 添加随机弧线增加干扰（可选）
            for (int i = 0; i < 2; i++)
            {
                int centerX = _random.Next(0, width);
                int centerY = _random.Next(0, height);
                int radius = _random.Next(5, 15);
                float startAngle = (float)_random.NextDouble() * 360;
                float endAngle = startAngle + (float)_random.Next(90, 270);

                var path = new PathBuilder()
                    .AddArc(new PointF(centerX, centerY), radius, radius, 0, startAngle, endAngle)
                    .Build();

                ctx.Draw(GetRandomNoiseColor(), 1f, path);
            }


        }

        // 绘制文本
        private void DrawText(IImageProcessingContext ctx, int width, int height)
        {
            var fontCollection = new FontCollection();
            fontCollection.AddSystemFonts();

            if (fontCollection.TryGet(FontFamily, out var family))
            {
              
            }
            else
            {
                family = fontCollection.Families.FirstOrDefault();
            }

            var font = family.CreateFont(16, FontStyle.Bold);


            var textOptions = new RichTextOptions(font)
            {
                Origin = new PointF(width / 2f, height / 2f),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var textOptions1 = new RichTextOptions(font)
            {
                Origin = new PointF(width / 2f + 1, height / 2f + 1),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
             
            // 文字阴影效果
            ctx.DrawText(
                textOptions1,
                Question,
                Color.LightGray);

            ctx.DrawText(
                textOptions,
                Question,
                Color.Black);
        }
    }
}
