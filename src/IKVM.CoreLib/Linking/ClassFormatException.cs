namespace IKVM.CoreLib.Linking
{

    internal class ClassFormatException : LinkingException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ClassFormatException()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public ClassFormatException(string message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg1"></param>
        public ClassFormatException(string format, object arg1) :
            base(string.Format(format, arg1))
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public ClassFormatException(string format, object arg1, object arg2) :
            base(string.Format(format, arg1, arg2))
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        public ClassFormatException(string format, object arg1, object arg2, object arg3) :
            base(string.Format(format, arg1, arg2, arg3))
        {

        }

    }

}