using System;

using IKVM.Runtime.Accessors;
using IKVM.Runtime.Accessors.Java.Lang;
using IKVM.Runtime.Vfs;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using Type = IKVM.Reflection.Type;
#else
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Runtime
{

    static partial class JVM
    {

        /// <summary>
        /// Internal services of the JVM.
        /// </summary>
        /// <remarks>
        /// JVM.Internal exists so that static initialization of JVM does not cause initialization of internal state.
        /// </remarks>
        internal static class Internal
        {

            internal const string JarClassList = "--ikvm-classes--/";

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

            internal static readonly RuntimeContextOptions contextOptions = new RuntimeContextOptions();
            internal static readonly RuntimeContext context = new RuntimeContext(contextOptions, new Resolver(), false);
            internal static readonly VfsTable vfs = VfsTable.BuildDefaultTable(new VfsRuntimeContext(context), Properties.HomePath);
            internal static readonly Lazy<object> systemThreadGroup = new Lazy<object>(() => ThreadGroupAccessor.Init());
            internal static readonly Lazy<object> mainThreadGroup = new Lazy<object>(() => ThreadGroupAccessor.Init(null, SystemThreadGroup, "main"));

            static AccessorCache baseAccessors;
            static ThreadGroupAccessor threadGroupAccessor;
            static SystemAccessor systemAccessor;

            internal static AccessorCache BaseAccessors => AccessorCache.Get(ref baseAccessors, context.Resolver.ResolveBaseAssembly());

            internal static ThreadGroupAccessor ThreadGroupAccessor => BaseAccessors.Get(ref threadGroupAccessor);

            internal static SystemAccessor SystemAccessor => BaseAccessors.Get(ref systemAccessor);

#endif

        }

    }

}
