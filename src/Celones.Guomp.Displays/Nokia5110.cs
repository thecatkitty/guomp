using Celones.Device;
using Celones.Guomp.Extensions;
using SkiaSharp;
using System.Device.Pwm;

namespace Celones.Guomp.Displays
{
    public class Nokia5110 : Display
    {
        private readonly Pcd8544 m_ctl;
        private readonly PwmChannel m_bl;

        private double m_contrast;

        private readonly SKPaint m_capturePaint;

        public override int Width => Pcd8544.DramSizeX;
        public override int Height => Pcd8544.DramSizeY * 8;

        public override double Brightness
        {
            get => m_bl.DutyCycle;
            set => m_bl.DutyCycle = value;
        }

        public override double Contrast
        {
            get => m_contrast;
            set
            {
                m_contrast = value;
                m_ctl.Write(Pcd8544.Instruction.SetOperationMode(instructionSet: Pcd8544.InstructionSet.Extended));
                m_ctl.Write(Pcd8544.Instruction.SetOperationVoltage((int)(60 * m_contrast)));
                m_ctl.Write(Pcd8544.Instruction.SetOperationMode(instructionSet: Pcd8544.InstructionSet.Basic));
            }
        }

        public Nokia5110(Pcd8544 controller, PwmChannel backLight)
        {
            m_ctl = controller;
            m_bl = backLight;
            
            m_capturePaint = new SKPaint
            {
                ColorFilter = SKColorFilter.CreateCompose(
                    SKColorFilter.CreateColorMatrix(new []
                    {
                        1, 0, 0, 0, 0,
                        0, 1, 0, 0, 0,
                        0, 0, 1, 0, 0,
                        -0.34f, -0.34f, -0.34f, 1, 0
                    }),
                    SKColorFilter.CreateHighContrast(true, SKHighContrastConfigInvertStyle.NoInvert, 1.0f))
            };
        }

        public override void Initialize()
        {
            m_ctl.Initialize();
            m_bl.Start();

            Brightness = 1.0;
            Contrast = 1.0;
        }

        public override void Clear()
        {
            for (var index = 0; index < Pcd8544.DramSizeX * Pcd8544.DramSizeY; index++)
            {
                m_ctl.Write(0x00);
            }

            Canvas.Clear(SKColor.Empty);
        }

        public override void Update(SKRectI rect)
        {
            var pixels = Surface.Snapshot().PeekPixels();
            rect.Intersect(new SKRectI(0, 0, Width, Height));

            var segmentEnd = (int)Math.Ceiling((rect.Top + rect.Height) / 8.0);
            for (var segment = rect.Top / 8; segment < segmentEnd; segment++)
            {
                var column = rect.Left;
                var columnEnd = column + rect.Width;
                m_ctl.Write(Pcd8544.Instruction.SetXAddress(column));
                m_ctl.Write(Pcd8544.Instruction.SetYAddress(segment));

                while (column < columnEnd)
                {
                    var data = 0;
                    for (var line = 0; line < 7; line++)
                    {
                        data |= pixels.GetMonochromePixel(column, segment * 8 + line) ? (1 << line) : 0;
                    }
                    m_ctl.Write((byte)data);
                    column++;
                }
            }
        }

        public override void Capture(string path)
        {

            var info = new SKImageInfo(Width, Height);
            var surface = SKSurface.Create(info);
            surface.Canvas.DrawSurface(Surface, 0, 0, m_capturePaint);
            File.WriteAllBytes(path, surface.Snapshot().Encode().ToArray());
        }
    }
}
