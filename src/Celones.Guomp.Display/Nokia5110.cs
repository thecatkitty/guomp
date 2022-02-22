using System;
using System.Device.Pwm;
using System.IO;
using Celones.Device;
using Celones.Guomp.Extensions;
using SkiaSharp;

namespace Celones.Guomp.Display
{
    public class Nokia5110
    {
        private Pcd8544 _ctl;
        private PwmChannel _bl;

        private double _contrast;

        private SKSurface _img;
        private SKCanvas _canvas;
        
        public int Width => _ctl.DramSizeX;
        public int Height => _ctl.DramSizeY * 8;

        public double Brightness
        {
            get => _bl.DutyCycle;
            set => _bl.DutyCycle = value;
        }

        public double Contrast
        {
            get => _contrast;
            set {
                _contrast = value;
                _ctl.Write(Pcd8544.Instruction.SetOperationMode(instructionSet: Pcd8544.InstructionSet.Extended));
                _ctl.Write(Pcd8544.Instruction.SetOperationVoltage((int)(60 * _contrast)));
                _ctl.Write(Pcd8544.Instruction.SetOperationMode(instructionSet: Pcd8544.InstructionSet.Basic));
            }
        }

        public SKCanvas Canvas => _canvas;

        public Nokia5110(Pcd8544 controller, PwmChannel backlight)
        {
            _ctl = controller;
            _bl = backlight;
            
            SKImageInfo info = new(Width, Height);
            _img = SKSurface.Create(info);
            _canvas = _img.Canvas;
        }

        public void Initialize()
        {
            _ctl.Initialize();
            _bl.Start();

            Brightness = 1.0;
            Contrast = 1.0;
        }
        
        public void Clear() {
            for (int index = 0; index < _ctl.DramSizeX * _ctl.DramSizeY; index++) {
                //_ctl.Write(0x00);
            }

            _canvas.Clear(SKColor.Empty);
        }

        public void Update() => Update(new SKRectI(0, 0, Width, Height));

        public void Update(SKRectI rect)
        {
            SKPixmap pixels = _img.Snapshot().PeekPixels();
            rect.Intersect(new SKRectI(0, 0, Width, Height));

            int segmentEnd = (int)Math.Ceiling((double)(rect.Top + rect.Height) / 8.0);
            for(int segment = (int)rect.Top / 8; segment < segmentEnd; segment++) {
                int column = (int)rect.Left;
                int columnEnd = column + (int)rect.Width;
                _ctl.Write(Device.Pcd8544.Instruction.SetXAddress(column));
                _ctl.Write(Device.Pcd8544.Instruction.SetYAddress(segment));

                while(column < columnEnd)
                {
                    int data = 0;
                    for (int line = 0; line < 7; line++)
                    {
                        data |= pixels.GetMonochromePixel(column, segment * 8 + line) ? (1 << line) : 0;
                    }
                    _ctl.Write((byte)data);
                    column++;
                }
            }
        }

        public void SaveScreenshot(string path)
        {
            SKImage snapshot = _img.Snapshot();
            SKData pngImage = snapshot.Encode();
            File.WriteAllBytes(path, pngImage.ToArray());
        } 
    }
}
