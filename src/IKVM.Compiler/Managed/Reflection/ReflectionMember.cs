using System;
using System.Collections.Generic;
using System.Reflection;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Implements <see cref="ReflectionMember"/> by reflecting against an existing .NET member.
    /// </summary>
    internal abstract class ReflectionMember : ReflectionBase, IManagedMember
    {

        readonly ReflectionType declaringType;
        readonly MemberInfo member;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="member"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal ReflectionMember(ReflectionType declaringType, MemberInfo member) :
            base(declaringType.Context)
        {
            this.declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            this.member = member ?? throw new ArgumentNullException(nameof(member));
        }

        public IManagedType DeclaringType => declaringType;

        public string Name => member.Name;

        public IReadOnlyList<IManagedCustomAttribute> CustomAttributes => throw new NotImplementedException();

    }

}
