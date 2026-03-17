using System.Runtime.InteropServices;
using SixLabors.ImageSharp.PixelFormats;
using ImageSharp = SixLabors.ImageSharp.Image;
using ImageSharpImage = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>;

namespace LVGLSharp.Darwing
{
    public sealed class Image : IDisposable
    {
        private System.Drawing.Image? _drawingSource;
        private ImageSharpImage? _imageSharpSource;

        private Image(System.Drawing.Image source)
        {
            _drawingSource = source;
        }

        private Image(ImageSharpImage source)
        {
            _imageSharpSource = source;
        }

        public int Width
        {
            get
            {
                ThrowIfDisposed();

                return _drawingSource?.Width ?? _imageSharpSource!.Width;
            }
        }

        public int Height
        {
            get
            {
                ThrowIfDisposed();

                return _drawingSource?.Height ?? _imageSharpSource!.Height;
            }
        }

        /// <summary>
        /// Loads an image from the specified file.
        /// </summary>
        /// <param name="path">The file path of the image to load.</param>
        /// <returns>A platform-backed image instance.</returns>
        public static Image Load(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);

            return FromFile(path);
        }

        /// <summary>
        /// Creates an image from the specified file.
        /// </summary>
        /// <param name="path">The file path of the image to load.</param>
        /// <returns>A platform-backed image instance.</returns>
        public static Image FromFile(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);

            if (OperatingSystem.IsWindows())
            {
                return new Image(System.Drawing.Image.FromFile(path));
            }

            return new Image(ImageSharp.Load<Rgba32>(path));
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Image"/> to <see cref="Image"/>.
        /// </summary>
        /// <param name="image">The image instance to wrap.</param>
        public static implicit operator Image(System.Drawing.Image image)
        {
            ArgumentNullException.ThrowIfNull(image);

            return new Image(image);
        }

        internal byte[] ToLvglArgb8888Bytes()
        {
            ThrowIfDisposed();

            if (_drawingSource is not null)
            {
                return ToLvglArgb8888Bytes(_drawingSource);
            }

            var rgbaBytes = GC.AllocateUninitializedArray<byte>(Width * Height * Marshal.SizeOf<Rgba32>());
            var bgraBytes = GC.AllocateUninitializedArray<byte>(rgbaBytes.Length);
            _imageSharpSource!.CopyPixelDataTo(MemoryMarshal.Cast<byte, Rgba32>(rgbaBytes.AsSpan()));

            for (var offset = 0; offset < rgbaBytes.Length; offset += 4)
            {
                bgraBytes[offset] = rgbaBytes[offset + 2];
                bgraBytes[offset + 1] = rgbaBytes[offset + 1];
                bgraBytes[offset + 2] = rgbaBytes[offset];
                bgraBytes[offset + 3] = rgbaBytes[offset + 3];
            }

            return bgraBytes;
        }

        private static byte[] ToLvglArgb8888Bytes(System.Drawing.Image image)
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException("System.Drawing.Image is only supported by LVGLSharp.Darwing.Image on Windows.");
            }

            using var bitmap = new System.Drawing.Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(image, 0, 0, image.Width, image.Height);
            }

            var rectangle = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(rectangle, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var sourceBytes = GC.AllocateUninitializedArray<byte>(Math.Abs(bitmapData.Stride) * bitmap.Height);
                var bgraBytes = GC.AllocateUninitializedArray<byte>(bitmap.Width * bitmap.Height * 4);

                Marshal.Copy(bitmapData.Scan0, sourceBytes, 0, sourceBytes.Length);

                for (var y = 0; y < bitmap.Height; y++)
                {
                    var sourceRow = y * Math.Abs(bitmapData.Stride);
                    var destinationRow = y * bitmap.Width * 4;

                    Buffer.BlockCopy(sourceBytes, sourceRow, bgraBytes, destinationRow, bitmap.Width * 4);
                }

                return bgraBytes;
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private void ThrowIfDisposed()
        {
            if (_drawingSource is null && _imageSharpSource is null)
            {
                throw new ObjectDisposedException(nameof(Image));
            }
        }

        public void Dispose()
        {
            _drawingSource?.Dispose();
            _imageSharpSource?.Dispose();
            _drawingSource = null;
            _imageSharpSource = null;
        }
    }
}
