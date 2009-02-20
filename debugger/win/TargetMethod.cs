using System;
using System.Collections.Generic;
using System.Text;
using Debugger.MetaData;
using Debugger.Wrappers.CorSym;
using Debugger.Wrappers;
using Debugger.Expressions;

namespace ikvm.debugger.win
{
    class TargetMethod
    {
        private readonly MethodInfo methodInfo;

        internal TargetMethod(MethodInfo methodInfo){
            this.methodInfo = methodInfo;
        }

        internal String Name
        {
            get { return methodInfo.FullName; }
        }

        internal int MethodId
        {
            get { return (int)methodInfo.MetadataToken; }
        }

        internal String JniSignature
        {
            get {
                StringBuilder signature = new StringBuilder();
                signature.Append(methodInfo.ParameterCount + "-" + methodInfo.Module.MetaData.GetGenericParamCount(methodInfo.MetadataToken));
                for(int i=0; i<methodInfo.ParameterCount; i++)
                {
                    Expression expr = methodInfo.GetExpressionForParameter(i);
                    signature.Append(methodInfo.GetParameterName(i) + "-" + expr + "-" + expr.Code + "-" + expr.CodeTail);
                    signature.Append('|');
                }
                return signature.ToString(); 
            }
        }

        internal String GenericSignature
        {
            get
            {
                return "";//TODO
            }
        }

        internal int AccessFlags
        {
            get
            {
                // http://java.sun.com/docs/books/jvms/first_edition/html/ClassFile.doc.html#12725
                if (methodInfo.IsPublic)
                {
                    return 0x0001;
                }

                if (methodInfo.IsProtected)
                {
                    return 0x0004;
                }

                if (methodInfo.IsPrivate)
                {
                    return 0x0002;
                }
                return 0;
            }
        }

    }
}
