using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;

using IKVM.Internal;

namespace IKVM.Runtime
{

#if IMPORTER == false && EXPORTER == false

    /// <summary>
    /// Provides utilities for working with dynamic methods.
    /// </summary>
    static class DynamicMethodUtil
    {

        /// <summary>
        /// Creates a new <see cref="DynamicMethod"/> owned by the appropriate metadata element.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="nonPublic"></param>
        /// <param name="returnType"></param>
        /// <param name="paramTypes"></param>
        /// <returns></returns>
        [SecuritySafeCritical]
        internal static DynamicMethod Create(string name, Type owner, bool nonPublic, Type returnType, Type[] paramTypes)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
#if NETFRAMEWORK
            return CreateFramework(name, owner, nonPublic, returnType, paramTypes);
#else
            return CreateCore(name, owner, nonPublic, returnType, paramTypes);
#endif
#endif
        }


#if NETFRAMEWORK

        static readonly Lazy<Module> dynamicModule = new Lazy<Module>(CreateDynamicModule, true);

        /// <summary>
        /// Creates the <see cref="Module"/> used for dynamic methods. This module has to be security critical.
        /// </summary>
        /// <returns></returns>
        static Module CreateDynamicModule()
        {
            var name = RuntimeUtil.IsMono ? "__DynamicMethodModule" : "__<DynamicMethodModule>";
            var dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(name), AssemblyBuilderAccess.RunAndCollect);
            return dynamicAssembly.DefineDynamicModule(name);
        }

        /// <summary>
        /// Creates a new <see cref="DynamicMethod"/> while targeting .NET Framework.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="nonPublic"></param>
        /// <param name="returnType"></param>
        /// <param name="paramTypes"></param>
        /// <returns></returns>
        static DynamicMethod CreateFramework(string name, Type owner, bool nonPublic, Type returnType, Type[] paramTypes)
        {
            try
            {
                return new DynamicMethod(name, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, returnType, paramTypes, dynamicModule.Value, true);
            }
            catch (SecurityException)
            {
                return new DynamicMethod(name, returnType, paramTypes, nonPublic);
            }
        }

#else

    /// <summary>
    /// Creates a new <see cref="DynamicMethod"/> while targeting .NET Core.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="owner"></param>
    /// <param name="nonPublic"></param>
    /// <param name="returnType"></param>
    /// <param name="paramTypes"></param>
    /// <returns></returns>
    static DynamicMethod CreateCore(string name, Type owner, bool nonPublic, Type returnType, Type[] paramTypes)
    {
        if (ReflectUtil.CanOwnDynamicMethod(owner))
            return new DynamicMethod(name, returnType, paramTypes, owner);
        else
            return new DynamicMethod(name, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, returnType, paramTypes, owner.Module, true);
    }

#endif

    }

#endif

}