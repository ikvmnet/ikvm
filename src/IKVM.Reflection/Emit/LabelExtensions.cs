namespace IKVM.Reflection.Emit
{

    internal static class LabelExtensions
    {

        public static int GetLabelValue(this Label self)
        {
            return self.Id;
        }

    }

}
