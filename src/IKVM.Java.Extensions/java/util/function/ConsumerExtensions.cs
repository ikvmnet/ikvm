using System;

namespace java.util.function
{

    public static class ConsumerExtensions
    {

        /// <summary>
        /// Returns a <see cref="Consumer"/> implementation that invokes the given <see cref="Action{TArg}"/>.
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Consumer ToConsumer<TArg>(this Action<TArg> action)
        {
            return new DelegateConsumer<TArg>(action);
        }

    }

}
