namespace IKVM.Reflection.Emit
{

    static class LabelExtensions
    {

        public static int GetLabelValue(this Label self) => self.Id;

    }

}
