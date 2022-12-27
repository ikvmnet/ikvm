using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;

using IKVM.Internal;

static class DynamicMethodUtils
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
#if NETFRAMEWORK
        return CreateFramework(name, owner, nonPublic, returnType, paramTypes);
#else
        return CreateCore(name, owner, nonPublic, returnType, paramTypes);
#endif
    }


#if NETFRAMEWORK

    static readonly Lazy<Module> dynamicModule = new Lazy<Module>(CreateDynamicModule, true);

    /// <summary>
    /// Returns <c>true</c> if the current context has restricted member access.
    /// </summary>
    /// <returns></returns>
    static bool IsRestrictedMemberAccess()
    {
        try
        {
            new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess).Demand();
            return true;
        }
        catch (SecurityException)
        {
            return false;
        }
    }

    /// <summary>
    /// Creates the <see cref="Module"/> used for dynamic methods. This module has to be security critical.
    /// </summary>
    /// <returns></returns>
    static Module CreateDynamicModule()
    {
        var dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("__<IKVMDynamicMethodHolder>"), AssemblyBuilderAccess.RunAndCollect);
        return dynamicAssembly.DefineDynamicModule("__<IKVMDynamicMethodHolder>");
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
            // we don't have RestrictedMemberAccess, so we stick the dynamic method in our module and hope for the best
            // (i.e. that we're trying to access something with assembly access in an assembly that lets us)
            if (nonPublic && IsRestrictedMemberAccess() == false)
                return new DynamicMethod(name, returnType, paramTypes, typeof(DynamicMethodUtils).Module);

            // apparently we don't have full trust, so we try again with .NET 2.0 SP1 method
            // and we only request restrictSkipVisibility if it is required
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