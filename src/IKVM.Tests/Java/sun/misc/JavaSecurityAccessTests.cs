using FluentAssertions;

using java.security;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using sun.misc;

namespace IKVM.Tests.Java.sun.misc
{

    [TestClass]
    public class JavaSecurityAccessTests
    {

        class NullDomainCombiner : DomainCombiner
        {

            public ProtectionDomain[] combine(ProtectionDomain[] currentDomains, ProtectionDomain[] assignedDomains)
            {
                return currentDomains;
            }

        }

        class DomainCombinerPrivilegedTestAction : PrivilegedAction
        {

            readonly DomainCombiner dc;

            public DomainCombinerPrivilegedTestAction(DomainCombiner dc)
            {
                this.dc = dc;
            }

            public object run()
            {
                return dc == AccessController.getContext().getDomainCombiner();
            }

        }

        /// <summary>
        /// Make sure that JavaSecurityAccess.doIntersectionPrivilege() is not dropping the information about the domain combiner of the stack ACC.
        /// Imported from jdk.java/security/ProtectionDomain/PreserveCombinerTests
        /// </summary>
        [TestMethod]
        public void ShouldPreserveDomainCombiner()
        {
            var dc = new NullDomainCombiner();
            var saved = AccessController.getContext();
            var stack = new AccessControlContext(AccessController.getContext(), dc);
            var ret = (bool)SharedSecrets.getJavaSecurityAccess().doIntersectionPrivilege(new DomainCombinerPrivilegedTestAction(dc), stack, saved);
            ret.Should().BeTrue();
        }

    }

}
