namespace IKVM.CoreLib.Linking
{

    internal interface ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {



    }

}
