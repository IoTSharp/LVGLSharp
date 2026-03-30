namespace LVGLSharp;

public static class DisplayBufferSizeHelper
{
    /// <summary>
    /// Calculates the total pixel count for the specified surface size.
    /// </summary>
    public static int GetPixelCount(int width, int height)
    {
        ValidateDimensions(width, height);
        return checked(width * height);
    }

    /// <summary>
    /// Calculates the byte size required for a full-frame LVGL RGB565 draw buffer.
    /// </summary>
    public static uint GetRgb565DrawBufferByteSize(int width, int height)
    {
        return checked((uint)(GetPixelCount(width, height) * sizeof(ushort)));
    }

    /// <summary>
    /// Calculates the byte size required for a 32-bit host framebuffer.
    /// </summary>
    public static int GetColor32FrameBufferByteSize(int width, int height)
    {
        return checked(GetPixelCount(width, height) * sizeof(uint));
    }

    private static void ValidateDimensions(int width, int height)
    {
        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }
    }
}
