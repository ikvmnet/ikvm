package IKVM.Internal;

@cli.System.ComponentModel.EditorBrowsableAttribute.Annotation(value = cli.System.ComponentModel.EditorBrowsableState.__Enum.Never)
final class RuntimeInit {

    @ikvm.lang.ModuleInitializer
    static void Init() {
        cli.System.GC.KeepAlive(ikvm.runtime.Util.getInstanceTypeFromClass(cli.IKVM.Runtime.JVM.class));
    }

}
