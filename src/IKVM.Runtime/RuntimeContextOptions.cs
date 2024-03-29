﻿namespace IKVM.Runtime
{

    internal class RuntimeContextOptions
    {

#if NETFRAMEWORK
        internal const string DefaultDynamicAssemblySuffixAndPublicKey = "-ikvm-runtime-injected, PublicKey=00240000048000009400000006020000002400005253413100040000010001009D674F3D63B8D7A4C428BD7388341B025C71AA61C6224CD53A12C21330A3159D300051FE2EED154FE30D70673A079E4529D0FD78113DCA771DA8B0C1EF2F77B73651D55645B0A4294F0AF9BF7078432E13D0F46F951D712C2FCF02EB15552C0FE7817FC0AED58E0984F86661BF64D882F29B619899DD264041E7D4992548EB9E";
#else
        internal const string DefaultDynamicAssemblySuffixAndPublicKey = "-ikvm-runtime-injected";
#endif

        readonly string dynamicAssemblySuffixAndPublicKey;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="dynamicAssemblySuffixAndPublicKey"></param>
        public RuntimeContextOptions(string dynamicAssemblySuffixAndPublicKey = null)
        {
            this.dynamicAssemblySuffixAndPublicKey = dynamicAssemblySuffixAndPublicKey ?? DefaultDynamicAssemblySuffixAndPublicKey;
        }

        /// <summary>
        /// Gets the suffix and public key to add to dynamically generated assemblies.
        /// </summary>
        public string DynamicAssemblySuffixAndPublicKey => dynamicAssemblySuffixAndPublicKey;

    }

}
