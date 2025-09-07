using IKVM.Reflection;

namespace IKVM.CoreLib.IkvmReflection
{

    public static class IkvmReflectionExtensions
    {

        /// <summary>
        /// Gets the parameters types of the specified method or constructor.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Type[] GetParameterTypes(this MethodBase method)
        {
            var p = method.GetParameters();
            var a = p.Length > 0 ? new Type[p.Length] : [];
            for (int i = 0; i < p.Length; i++)
                a[i] = p[i].ParameterType;

            return a;
        }

    }

}
