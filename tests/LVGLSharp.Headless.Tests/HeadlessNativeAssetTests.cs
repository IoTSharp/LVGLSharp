using Xunit;

namespace LVGLSharp.Headless.Tests;

public sealed class HeadlessNativeAssetTests
{
    [Fact]
    public void NativeAssets_AreCopiedToHeadlessTestOutput()
    {
        HeadlessTestEnvironment.AssertNativeAssetsCopiedToOutput();
    }
}
