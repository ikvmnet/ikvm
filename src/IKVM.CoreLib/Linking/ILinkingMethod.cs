using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Linking
{

    internal interface ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> : ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        /// <summary>
        /// Initiates a link of the method with the specified mode.
        /// </summary>
        /// <param name="mode"></param>
        void Link(LoadMode mode);

    }

}
