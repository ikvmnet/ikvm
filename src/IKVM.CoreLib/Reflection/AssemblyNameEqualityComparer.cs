using System;
using System.Collections.Generic;
using System.Reflection;

namespace IKVM.CoreLib.Reflection
{

    class AssemblyNameEqualityComparer : IEqualityComparer<AssemblyName>
    {

        /// <summary>
        /// Gets the default instance of this comparer.
        /// </summary>
        public static readonly AssemblyNameEqualityComparer Instance = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public AssemblyNameEqualityComparer()
        {

        }


        /// <inheritdoc />
        public bool Equals(AssemblyName? x, AssemblyName? y)
        {
            // this expects non-null AssemblyName
            if (x == null || y == null)
                return false;

            if (ReferenceEquals(x, y))
                return true;

            if (x.Name != null && y.Name != null)
            {
                if (string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase) != 0)
                    return false;
            }
            else if (!(x.Name == null && y.Name == null))
            {
                return false;
            }

            if (x.Version != null && y.Version != null)
            {
                if (x.Version != y.Version)
                    return false;
            }
            else if (!(x.Version == null && y.Version == null))
            {
                return false;
            }

            if (x.CultureInfo != null && y.CultureInfo != null)
            {
                if (!x.CultureInfo.Equals(y.CultureInfo))
                    return false;
            }
            else if (!(x.CultureInfo == null && y.CultureInfo == null))
            {
                return false;
            }

            var xArray = x.GetPublicKeyToken();
            var yArray = y.GetPublicKeyToken();
            if (!IsSameKeyToken(xArray, yArray))
                return false;

            return true;
        }

        /// <inheritdoc />
        public int GetHashCode(AssemblyName obj)
        {
            int hashcode = 0;

            if (obj.Name != null)
                hashcode ^= obj.Name.GetHashCode();

            if (obj.Version != null)
                hashcode ^= obj.Version.GetHashCode();

            if (obj.CultureInfo != null)
                hashcode ^= obj.CultureInfo.GetHashCode();

            var objArray = obj.GetPublicKeyToken();
            if (objArray != null)
            {
                // distinguishing no PKToken from "PKToken = null" which is an array of length=0
                hashcode ^= objArray.Length.GetHashCode() + 1;
                if (objArray.Length > 0)
                    hashcode ^= BitConverter.ToUInt64(objArray, 0).GetHashCode();
            }

            return hashcode;
        }

        static bool IsSameKeyToken(byte[]? reqKeyToken, byte[]? curKeyToken)
        {
            bool isSame = false;

            if (reqKeyToken == null && curKeyToken == null)
            {
                // Both Key Tokens are not set, treat them as same.
                isSame = true;
            }
            else if (reqKeyToken != null && curKeyToken != null)
            {
                // Both KeyTokens are set. 
                if (reqKeyToken.Length == curKeyToken.Length)
                {
                    isSame = true;
                    for (int i = 0; i < reqKeyToken.Length; i++)
                    {
                        if (reqKeyToken[i] != curKeyToken[i])
                        {
                            isSame = false;
                            break;
                        }
                    }
                }
            }

            return isSame;
        }

    }

}
