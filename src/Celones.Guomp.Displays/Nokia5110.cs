using System.Device.Pwm;
using Celones.Device;
using Celones.Guomp.Extensions;
using SkiaSharp;

namespace Celones.Guomp.Displays
{
    public class Nokia5110 : Display
    {
        private readonly Pcd8544 m_ctl;
        private readonly PwmChannel m_bl;

        private double m_contrast;

        public override int Width => m_ctl.DramSizeX;
        public override int Height => m_ctl.DramSizeY * 8;

        public override double Brightness
        {
            get => m_bl.DutyCycle;
            set => m_bl.DutyCycle = value;
        }

        public override double Contrast
        {
            get => m_contrast;
            set {
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
            
        }

        public override void Initialize()
        {
            m_ctl.Initialize();
            m_bl.Start();

            Brightness = 1.0;
            Contrast = 1.0;
        }
        
        public override void Clear() {
            for (var index = 0; index < m_ctl.DramSizeX * m_ctl.DramSizeY; index++) {
                //m_ctl.Write(0x00);
            }

            Canvas.Clear(SKColor.Empty);
        }

        public override void Update(SKRectI rect)
        {
            var pixels = Surface.Snapshot().PeekPixels();
            rect.Intersect(new SKRectI(0, 0, Width, Height));

            var segmentEnd = (int)Math.Ceiling((rect.Top + rect.Height) / 8.0);
            for(var segment = rect.Top / 8; segment < segmentEnd; segment++) {
                var column = rect.Left;
                var columnEnd = column + rect.Width;
                m_ctl.Write(Pcd8544.Instruction.SetXAddress(column));
                m_ctl.Write(Pcd8544.Instruction.SetYAddress(segment));

                while(column < columnEnd)
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
    }
}
