namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Extension methods for <see cref="Label"/>.
    /// </summary>
    static class LabelExtensions
    {

        /// <summary>
        /// Gets the label's index in the <see cref="ILGenerator"/> that defined it. This stands in for the internal
        /// <c>Label.GetLabelValue</c> method of the .NET runtime, which the code ported from it calls.
        /// </summary>
        /// <param name="self">The label.</param>
        /// <returns>The value of <see cref="Label.Id"/>.</returns>
        public static int GetLabelValue(this Label self) => self.Id;

    }

}
