using System;

namespace IKVM.Runtime.Accessors.Java.Lang
{

#if FIRST_PASS == false && EXPORTER == false && IMPORTER == false

    /// <summary>
    /// Provides runtime access to the 'java.lang.ProcessBuilderRedirect' type.
    /// </summary>
    internal sealed class ProcessBuilderRedirectAccessor : Accessor<object>
    {

        FieldAccessor<object> PIPE;
        FieldAccessor<object> INHERIT;

        MethodAccessor<Func<object, object>> file;
        MethodAccessor<Func<object, bool>> append;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public ProcessBuilderRedirectAccessor(AccessorTypeResolver resolver) :
            base(resolver, "java.lang.ProcessBuilder+Redirect")
        {

        }

        /// <summary>
        /// Gets the value of the PIPE field.
        /// </summary>
        /// <returns></returns>
        public object GetPipe() => GetField(ref PIPE, nameof(PIPE)).GetValue();

        /// <summary>
        /// Gets the value of the INHERIT field.
        /// </summary>
        /// <returns></returns>
        public object GetInherit() => GetField(ref INHERIT, nameof(INHERIT)).GetValue();

        /// <summary>
        /// Invokes the 'file' method.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public object InvokeFile(object self) => GetMethod(ref file, nameof(file), Resolve("java.io.File")).Invoker(self);

        /// <summary>
        /// Invokes the 'append' method.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public bool InvokeAppend(object self) => GetMethod(ref append, nameof(append), typeof(bool)).Invoker(self);

    }

#endif

}
