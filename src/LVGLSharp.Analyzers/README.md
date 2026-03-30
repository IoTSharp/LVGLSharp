# LVGLSharp.Analyzers

`LVGLSharp.Analyzers` contains the Roslyn analyzers used by the LVGLSharp repository.

## What it checks

- runtime package reference combinations
- repository-specific usage patterns that are easy to get wrong
- guidance surfaced during build instead of at runtime

This library is normally consumed transitively through `LVGLSharp.Forms` and is not part of the main public NuGet publishing workflow.

Docs and package guidance: <https://lvglsharp.net/en/nuget.html>  
Repository: <https://github.com/IoTSharp/LVGLSharp>
