using SkiaSharp;

namespace Celones.Guomp
{
    public abstract class Display
    {
        public SKSurface Surface { get; }

        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract double Brightness { get; set; }
        public abstract double Contrast { get; set; }
        public SKCanvas Canvas { get; }

        protected Display()
        {
            SKImageInfo info = new(Width, Height);
            Surface = SKSurface.Create(info);
            Canvas = Surface.Canvas;
        }

        public abstract void Initialize();
        public abstract void Clear();
        public void Update() => Update(new SKRectI(0, 0, Width, Height));
        public abstract void Update(SKRectI rect);
        public virtual void Capture(string path) => File.WriteAllBytes(path, Surface.Snapshot().Encode().ToArray());
    }
}
