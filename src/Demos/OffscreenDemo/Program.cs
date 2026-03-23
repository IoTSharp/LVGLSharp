using LVGLSharp.Runtime.Linux;
using System.Text;
using static LVGLSharp.Interop.LVGL;

namespace OffscreenDemo;

internal static unsafe class Program
{
    private static void Main(string[] args)
    {
        var outputPath = args.Length > 0
            ? args[0]
            : Environment.GetEnvironmentVariable("LVGLSHARP_OFFSCREEN_SNAPSHOT_PATH");

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            outputPath = Path.Combine(AppContext.BaseDirectory, "offscreen-snapshot.png");
        }

        using var view = new OffscreenView(480, 320, 96f);
        view.Open();

        var root = view.Root;
        if (root == null)
        {
            throw new InvalidOperationException("Offscreen root 创建失败。");
        }

        var label = lv_label_create(root);
        if (label == null)
        {
            throw new InvalidOperationException("Offscreen label 创建失败。");
        }

        var text = Encoding.UTF8.GetBytes("LVGLSharp Offscreen Snapshot\0");
        fixed (byte* textPtr = text)
        {
            lv_label_set_text(label, textPtr);
        }

        lv_obj_center(label);
        view.RenderFrame();

        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        view.SavePng(outputPath);
        Console.WriteLine($"Offscreen snapshot saved: {outputPath}");
    }
}