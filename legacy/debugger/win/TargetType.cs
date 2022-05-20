using System;
using System.Collections.Generic;
using System.Text;
using Debugger.MetaData;

namespace ikvm.debugger.win
{
    internal class TargetType
    {
        private static int typeIdCounter;
        private static Dictionary<int, TargetType> typeList = new Dictionary<int, TargetType>();

        private readonly int typeId;
        private readonly DebugType type;

        internal TargetType(DebugType type)
        {
            this.typeId = ++typeIdCounter;
            this.type = type;
            typeList.Add(typeId, this);
        }

        internal static TargetType GetTargetType(int typeId)
        {
            return typeList[typeId];
        }

        internal int TypeId
        {
            get { return typeId; }
        }

        internal bool Identical(DebugType type)
        {
            return this.type.Equals(type);
        }

        internal String GetJniSignature()
        {
            //TODO if it is not a class
            return 'L' + type.Name.Replace('.', '/') + ';';
        }

        internal IList<TargetMethod> GetMethods()
        {
            List<TargetMethod> result = new List<TargetMethod>();
            IList<MethodInfo> methods = type.GetMethods(BindingFlags.All);
            foreach (MethodInfo method in methods)
            {
                result.Add(new TargetMethod(method));
            }
            return result;
        }
    }


}
