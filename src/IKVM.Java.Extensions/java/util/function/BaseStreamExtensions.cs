using System.Collections.Generic;

using java.util.stream;

namespace java.util.function
{

    public static  class BaseStreamExtensions
    {

        public static IEnumerable<T> AsEnumerable<T>(this Stream stream)
        {
            return new StreamWrapper<T>(stream);
        }

    }

}
