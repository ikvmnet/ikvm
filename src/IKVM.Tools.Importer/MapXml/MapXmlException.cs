using System;

namespace IKVM.Tools.Importer.MapXml
{

    class MapXmlException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public MapXmlException()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public MapXmlException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public MapXmlException(string message, Exception innerException) : base(message, innerException)
        {

        }

    }

}