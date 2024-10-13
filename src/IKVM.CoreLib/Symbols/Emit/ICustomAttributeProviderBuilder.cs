namespace IKVM.CoreLib.Symbols.Emit
{

    interface ICustomAttributeProviderBuilder
    {

        /// <summary>
        /// Set a custom attribute on this object.
        /// </summary>
        /// <param name="attribute"></param>
        public void SetCustomAttribute(CustomAttribute attribute);

    }

}
