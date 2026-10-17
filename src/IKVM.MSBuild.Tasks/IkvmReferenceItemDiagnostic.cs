namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// A problem with an <see cref="IkvmReferenceItem"/>: the resource holding its message and error code, and the
    /// message's arguments.
    /// </summary>
    internal class IkvmReferenceItemDiagnostic
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="args"></param>
        public IkvmReferenceItemDiagnostic(string resource, params object[] args)
        {
            Resource = resource;
            Args = args;
        }

        /// <summary>
        /// Name of the resource holding the message and its error code.
        /// </summary>
        public string Resource { get; }

        /// <summary>
        /// Arguments of the message.
        /// </summary>
        public object[] Args { get; }

    }

}
