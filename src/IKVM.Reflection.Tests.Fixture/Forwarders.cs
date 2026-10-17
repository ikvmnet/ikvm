using System.Runtime.CompilerServices;

// forwards a type to another assembly, so the rendering and the copy cover exported types
[assembly: TypeForwardedTo(typeof(System.Collections.Generic.List<>))]
