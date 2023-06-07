using System;
using System.Reflection;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Implements <see cref="ReflectionMemberInfo"/> by reflecting against an existing .NET member.
    /// </summary>
    internal abstract class ReflectionMemberInfo : ReflectionEntityInfo, IManagedMemberInfo
    {

        readonly ReflectionTypeInfo declaringType;
        readonly MemberInfo member;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="member"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal ReflectionMemberInfo(ReflectionTypeInfo declaringType, MemberInfo member) :
            base(declaringType.Context)
        {
            this.declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            this.member = member ?? throw new ArgumentNullException(nameof(member));
        }

        public IManagedTypeInfo DeclaringType => declaringType;

        public string Name => member.Name;

    }

}
