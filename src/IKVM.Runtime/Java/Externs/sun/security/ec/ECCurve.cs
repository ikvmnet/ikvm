using System.Security.Cryptography;

namespace IKVM.Java.Externs.sun.security.ec
{

#if NET47_OR_GREATER || NETCOREAPP
#else

    struct ECCurve
    {

        public enum ECCurveType : int
        {
            Implicit = 0,
            PrimeShortWeierstrass = 1,
            PrimeTwistedEdwards = 2,
            PrimeMontgomery = 3,
            Characteristic2 = 4,
            Named = 5,
        }

        public static ECCurve CreateFromOid(Oid curveOid)
        {
            ECCurve curve = default;
            curve.CurveType = ECCurveType.Named;
            curve.Oid = curveOid;
            return curve;
        }

        public ECCurveType CurveType;

        public Oid Oid { get; set; }

        public bool IsNamed => CurveType == ECCurve.ECCurveType.Named;

    }

#endif

}
