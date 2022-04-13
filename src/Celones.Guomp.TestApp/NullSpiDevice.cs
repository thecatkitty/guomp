using System.Device.Spi;

namespace Celones.Guomp.TestApp
{
    internal class NullSpiDevice : SpiDevice
    {
        public override byte ReadByte() => 0xFF;

        public override void Read(Span<byte> buffer) => buffer.Fill(0xFF);

        public override void WriteByte(byte value)
        {
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
        }

        public override void TransferFullDuplex(ReadOnlySpan<byte> writeBuffer, Span<byte> readBuffer) => readBuffer.Fill(0xFF);

        public override SpiConnectionSettings ConnectionSettings { get; } = new(-1);

        public static NullSpiDevice Create() => new();
    }
}
