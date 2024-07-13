using System;

using javax.script;

namespace IKVM.Tests.Java.javax.script
{

    public class SharedObject
    {

        // Public fields
        public string publicString = "PublicString";
        public string[] publicStringArray = ["ArrayString[0]", "ArrayString[1]", "ArrayString[2]", "ArrayString[3]"];
        public Person publicObject = new Person(256);
        public Person[] publicObjectArray = [new Person(4), new Person(-422), new Person(14)];
        public bool publicBoolean = true;
        public bool[] publicBooleanArray = [true, false, false, true];
        public global::java.lang.Boolean publicBooleanBox = new global::java.lang.Boolean(true);
        public long publicLong = 933333333333333333L;
        public long[] publicLongArray = [99012333333333L, -124355555L, 89777777777L];
        public global::java.lang.Long publicLongBox = new global::java.lang.Long(9333333333L);
        public int publicInt = 2076543123;
        public int[] publicIntArray = [0, 1, 1, 2, 3, 5, 8, 13, 21, 34];
        public global::java.lang.Integer publicIntBox = new global::java.lang.Integer(20765123);
        public byte publicByte = unchecked((byte)-128);
        public byte[] publicByteArray = [1, 2, 4, 8, 16, 32, 64, 127, unchecked((byte)-128)];
        public global::java.lang.Byte publicByteBox = new global::java.lang.Byte(127);
        public short publicShort = 32000;
        public short[] publicShortArray = [3240, 8900, -16789, 1, 12];
        public global::java.lang.Short publicShortBox = new global::java.lang.Short(global::java.lang.Short.MIN_VALUE);
        public float publicFloat = 0.7f;
        public float[] publicFloatArray = [-32.01f, 89.3f, -1.3e8f, 3.1f];
        public global::java.lang.Float publicFloatBox = new global::java.lang.Float(1.377e4f);
        public double publicDouble = 1.34e20;
        public double[] publicDoubleArray = [0.75e80, 8e-43, 1.000077, 0.123e10];
        public global::java.lang.Double publicDoubleBox = new global::java.lang.Double(1.4e-19);
        public char publicChar = 'A';
        public char[] publicCharArray = "Hello Nashorn".ToCharArray();
        public global::java.lang.Character publicCharBox = new global::java.lang.Character('B');

        // Public static fields
        public static string publicStaticString = "PublicStaticString";
        public static string[] publicStaticStringArray = ["StaticArrayString[0]", "StaticArrayString[1]", "StaticArrayString[2]", "StaticArrayString[3]"];
        public static Person publicStaticObject = new Person(512);
        public static Person[] publicStaticObjectArray = { new Person(40), new Person(-22), new Person(18) };
        public static bool publicStaticBoolean = true;
        public static bool[] publicStaticBooleanArray = { false, false, false, true };
        public static global::java.lang.Boolean publicStaticBooleanBox = new global::java.lang.Boolean(true);
        public static long publicStaticLong = 13333333333333333L;
        public static long[] publicStaticLongArray = { 19012333333333L, -224355555L, 39777777777L };
        public static global::java.lang.Long publicStaticLongBox = new global::java.lang.Long(9333333334L);
        public static int publicStaticInt = 207654323;
        public static int[] publicStaticIntArray = { 5, 8, 13, 21, 34 };
        public static global::java.lang.Integer publicStaticIntBox = new global::java.lang.Integer(2075123);
        public static byte publicStaticByte = unchecked((byte)-12);
        public static byte[] publicStaticByteArray = { 16, 32, 64, 127, unchecked((byte)-128) };
        public static global::java.lang.Byte publicStaticByteBox = new global::java.lang.Byte(17);
        public static short publicStaticShort = 320;
        public static short[] publicStaticShortArray = { 1240, 900, -1789, 100, 12 };
        public static global::java.lang.Short publicStaticShortBox = new global::java.lang.Short(-16777);
        public static float publicStaticFloat = 7.7e8f;
        public static float[] publicStaticFloatArray = { -131.01f, 189.3f, -31.3e8f, 3.7f };
        public static global::java.lang.Float publicStaticFloatBox = new global::java.lang.Float(1.37e4f);
        public static double publicStaticDouble = 1.341e20;
        public static double[] publicStaticDoubleArray = { 0.75e80, 0.123e10, 8e-43, 1.000077 };
        public static global::java.lang.Double publicStaticDoubleBox = new global::java.lang.Double(1.41e-12);
        public static char publicStaticChar = 'C';
        public static char[] publicStaticCharArray = "Nashorn".ToCharArray();
        public static global::java.lang.Character publicStaticCharBox = new global::java.lang.Character('D');

        // Public final fields
        public readonly string publicFinalString = "PublicFinalString";
        public readonly string[] publicFinalStringArray = ["FinalArrayString[0]", "FinalArrayString[1]", "FinalArrayString[2]", "FinalArrayString[3]"];
        public readonly Person publicFinalObject = new Person(1024);
        public readonly Person[] publicFinalObjectArray = [new Person(-900), new Person(1000), new Person(180)];
        public readonly bool publicFinalBoolean = true;
        public readonly bool[] publicFinalBooleanArray = [false, false, true, false];
        public readonly global::java.lang.Boolean publicFinalBooleanBox = new global::java.lang.Boolean(true);
        public readonly long publicFinalLong = 13353333333333333L;
        public readonly long[] publicFinalLongArray = [1901733333333L, -2247355555L, 3977377777L];
        public readonly global::java.lang.Long publicFinalLongBox = new global::java.lang.Long(9377333334L);
        public readonly int publicFinalInt = 20712023;
        public readonly int[] publicFinalIntArray = [50, 80, 130, 210, 340];
        public readonly global::java.lang.Integer publicFinalIntBox = new global::java.lang.Integer(207512301);
        public readonly byte publicFinalByte = unchecked((byte)-7);
        public readonly byte[] publicFinalByteArray = [1, 3, 6, 17, unchecked((byte)-128)];
        public readonly global::java.lang.Byte publicFinalByteBox = new global::java.lang.Byte(19);
        public readonly short publicFinalShort = 31220;
        public readonly short[] publicFinalShortArray = [12240, 9200, -17289, 1200, 12];
        public readonly global::java.lang.Short publicFinalShortBox = new global::java.lang.Short(-26777);
        public readonly float publicFinalFloat = 7.72e8f;
        public readonly float[] publicFinalFloatArray = [-131.012f, 189.32f, -31.32e8f, 3.72f];
        public readonly global::java.lang.Float publicFinalFloatBox = new global::java.lang.Float(1.372e4f);
        public readonly double publicFinalDouble = 1.3412e20;
        public readonly double[] publicFinalDoubleArray = [0.725e80, 0.12e10, 8e-3, 1.00077];
        public readonly global::java.lang.Double publicFinalDoubleBox = new global::java.lang.Double(1.412e-12);
        public readonly char publicFinalChar = 'E';
        public readonly char[] publicFinalCharArray = "Nashorn hello".ToCharArray();
        public readonly global::java.lang.Character publicFinalCharBox = new global::java.lang.Character('F');

        // Public static final fields
        public static readonly string publicStaticFinalString = "PublicStaticFinalString";
        public static readonly string[] publicStaticFinalStringArray = ["StaticFinalArrayString[0]", "StaticFinalArrayString[1]", "StaticFinalArrayString[2]", "StaticFinalArrayString[3]"];
        public static readonly Person publicStaticFinalObject = new Person(2048);
        public static readonly Person[] publicStaticFinalObjectArray = [new Person(-9), new Person(110), new Person(global::java.lang.Integer.MAX_VALUE)];
        public static readonly bool publicStaticFinalBoolean = true;
        public static readonly bool[] publicStaticFinalBooleanArray = [false, true, false, false];
        public static readonly global::java.lang.Boolean publicStaticFinalBooleanBox = new global::java.lang.Boolean(true);
        public static readonly long publicStaticFinalLong = 8333333333333L;
        public static readonly long[] publicStaticFinalLongArray = [19017383333L, -2247358L, 39773787L];
        public static readonly global::java.lang.Long publicStaticFinalLongBox = new global::java.lang.Long(9377388334L);
        public static readonly int publicStaticFinalInt = 207182023;
        public static readonly int[] publicStaticFinalIntArray = [1308, 210, 340];
        public static readonly global::java.lang.Integer publicStaticFinalIntBox = new global::java.lang.Integer(2078301);
        public static readonly byte publicStaticFinalByte = unchecked((byte)-70);
        public static readonly byte[] publicStaticFinalByteArray = [17, unchecked((byte)-128), 81];
        public static readonly global::java.lang.Byte publicStaticFinalByteBox = new global::java.lang.Byte(91);
        public static readonly short publicStaticFinalShort = 8888;
        public static readonly short[] publicStaticFinalShortArray = [8240, 9280, -1289, 120, 812];
        public static readonly global::java.lang.Short publicStaticFinalShortBox = new global::java.lang.Short(-26);
        public static readonly float publicStaticFinalFloat = 0.72e8f;
        public static readonly float[] publicStaticFinalFloatArray = [-8131.012f, 9.32f, -138.32e8f, 0.72f];
        public static readonly global::java.lang.Float publicStaticFinalFloatBox = new global::java.lang.Float(1.2e4f);
        public static readonly double publicStaticFinalDouble = 1.8e12;
        public static readonly double[] publicStaticFinalDoubleArray = [8.725e80, 0.82e10, 18e-3, 1.08077];
        public static readonly global::java.lang.Double publicStaticFinalDoubleBox = new global::java.lang.Double(1.5612e-13);
        public static readonly char publicStaticFinalChar = 'K';
        public static readonly char[] publicStaticFinalCharArray = "StaticString".ToCharArray();
        public static readonly global::java.lang.Character publicStaticFinalCharBox = new global::java.lang.Character('L');

        // Special vars
        public volatile bool volatileBoolean = true;

        [NonSerialized]
        public bool transientBoolean = true;

        // For methods testing
        public bool isAccessed = false;
        public volatile bool isFinished = false;

        private ScriptEngine engine;

        public ScriptEngine getEngine()
        {
            return engine;
        }

        public void setEngine(ScriptEngine engine)
        {
            this.engine = engine;
        }

        public void voidMethod()
        {
            isAccessed = true;
        }

        public bool booleanMethod(bool arg)
        {
            return !arg;
        }

        public global::java.lang.Boolean booleanBoxingMethod(global::java.lang.Boolean arg)
        {
            return new global::java.lang.Boolean(!arg.booleanValue());
        }

        public bool[] booleanArrayMethod(bool[] arg)
        {
            bool[] res = new bool[arg.Length];
            for (int i = 0; i < arg.Length; i++)
            {
                res[i] = !arg[i];
            }
            return res;
        }

        public int intMethod(int arg)
        {
            return arg + arg;
        }

        public global::java.lang.Integer intBoxingMethod(global::java.lang.Integer arg)
        {
            return new global::java.lang.Integer(arg.intValue() + arg.intValue());
        }

        public int[] intArrayMethod(int[] arg)
        {
            int[] res = new int[arg.Length];
            for (int i = 0; i < arg.Length; i++)
            {
                res[i] = arg[i] * 2;
            }
            return res;
        }

        public long longMethod(long arg)
        {
            return arg + arg;
        }

        public global::java.lang.Long longBoxingMethod(global::java.lang.Long arg)
        {
            return new global::java.lang.Long(arg.longValue() + arg.longValue());
        }

        public long[] longArrayMethod(long[] arg)
        {
            long[] res = new long[arg.Length];
            for (int i = 0; i < arg.Length; i++)
            {
                res[i] = arg[i] * 2;
            }
            return res;
        }

        public byte byteMethod(byte arg)
        {
            return (byte)(arg + arg);
        }

        public global::java.lang.Byte byteBoxingMethod(global::java.lang.Byte arg)
        {
            return new global::java.lang.Byte((byte)(arg.byteValue() + arg.byteValue()));
        }

        public byte[] byteArrayMethod(byte[] arg)
        {
            byte[] res = new byte[arg.Length];
            for (int i = 0; i < arg.Length; i++)
            {
                res[i] = (byte)(arg[i] * 2);
            }
            return res;
        }

        public char charMethod(char arg)
        {
            return global::java.lang.Character.toUpperCase(arg);
        }

        public global::java.lang.Character charBoxingMethod(global::java.lang.Character arg)
        {
            return new global::java.lang.Character(global::java.lang.Character.toUpperCase(arg.charValue()));
        }

        public char[] charArrayMethod(char[] arg)
        {
            char[] res = new char[arg.Length];
            for (int i = 0; i < arg.Length; i++)
            {
                res[i] = global::java.lang.Character.toUpperCase(arg[i]);
            }
            return res;
        }

        public short shortMethod(short arg)
        {
            return (short)(arg + arg);
        }

        public global::java.lang.Short shortBoxingMethod(global::java.lang.Short arg)
        {
            return new global::java.lang.Short((short)(arg.shortValue() + arg.shortValue()));
        }

        public short[] shortArrayMethod(short[] arg)
        {
            short[] res = new short[arg.Length];
            for (int i = 0; i < arg.Length; i++)
            {
                res[i] = (short)(arg[i] * 2);
            }
            return res;
        }

        public float floatMethod(float arg)
        {
            return arg + arg;
        }

        public global::java.lang.Float floatBoxingMethod(global::java.lang.Float arg)
        {
            return new global::java.lang.Float(arg.floatValue() + arg.floatValue());
        }

        public float[] floatArrayMethod(float[] arg)
        {
            float[] res = new float[arg.Length];
            for (int i = 0; i < arg.Length; i++)
            {
                res[i] = arg[i] * 2;
            }
            return res;
        }

        public double doubleMethod(double arg)
        {
            return arg + arg;
        }

        public global::java.lang.Double doubleBoxingMethod(global::java.lang.Double arg)
        {
            return new global::java.lang.Double(arg.doubleValue() + arg.doubleValue());
        }

        public double[] doubleArrayMethod(double[] arg)
        {
            double[] res = new double[arg.Length];
            for (int i = 0; i < arg.Length; i++)
            {
                res[i] = arg[i] * 2;
            }
            return res;
        }

        public string stringMethod(string str)
        {
            return str + str;
        }

        public string[] stringArrayMethod(string[] arr)
        {
            int l = arr.Length;
            string[] res = new string[l];
            for (int i = 0; i < l; i++)
            {
                res[i] = arr[l - i - 1];
            }
            return res;
        }

        public Person[] objectArrayMethod(Person[] arr)
        {
            Person[] res = new Person[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                res[i] = new Person(i + 100);
            }
            return res;
        }

        public Person objectMethod(Person t)
        {
            t.id *= 2;
            return t;
        }

        public int twoParamMethod(long l, double d)
        {
            return (int)(l + d);
        }

        public int threeParamMethod(short s, long l, char c)
        {
            return (int)(s + l + c);
        }

        public Person[] twoObjectParamMethod(Person arg1, Person arg2)
        {
            return new Person[] { arg2, arg1 };
        }

        public Person[] threeObjectParamMethod(Person arg1, Person arg2, Person arg3)
        {
            return new Person[] { arg3, arg2, arg1 };
        }

        public Person[] eightObjectParamMethod(Person arg1, Person arg2, Person arg3, Person arg4, Person arg5, Person arg6, Person arg7, Person arg8)
        {
            return new Person[] { arg8, arg7, arg6, arg5, arg4, arg3, arg2, arg1 };
        }

        public Person[] nineObjectParamMethod(Person arg1, Person arg2, Person arg3, Person arg4, Person arg5, Person arg6, Person arg7, Person arg8, Person arg9)
        {
            return new Person[] { arg9, arg8, arg7, arg6, arg5, arg4, arg3, arg2, arg1 };
        }

        public Person[] methodObjectEllipsis(params Person[] args)
        {
            int l = args.Length;
            Person[] res = new Person[l];
            for (int i = 0; i < l; i++)
            {
                res[i] = args[l - i - 1];
            }
            return res;
        }

        public Person[] methodPrimitiveEllipsis(params int[] args)
        {
            int l = args.Length;
            Person[] res = new Person[l];
            for (int i = 0; i < l; i++)
            {
                res[i] = new Person(args[i]);
            }
            return res;
        }

        public object[] methodMixedEllipsis(params object[] args)
        {
            return args;
        }

        public object[] methodObjectWithEllipsis(string arg, params int[] args)
        {
            object[] res = new object[args.Length + 1];
            res[0] = arg;
            for (int i = 0; i < args.Length; i++)
            {
                res[i + 1] = args[i];
            }
            return res;
        }

        public object[] methodPrimitiveWithEllipsis(int arg, params long[] args)
        {
            object[] res = new object[args.Length + 1];
            res[0] = arg;
            for (int i = 0; i < args.Length; i++)
            {
                res[i + 1] = args[i];
            }
            return res;
        }

        public object[] methodMixedWithEllipsis(string arg1, int arg2, params object[] args)
        {
            object[] res = new object[args.Length + 2];
            res[0] = arg1;
            res[1] = arg2;
            global::java.lang.System.arraycopy(args, 0, res, 2, args.Length);
            return res;
        }

        public void methodStartsThread()
        {
            isFinished = false;

            global::java.lang.Thread t = new global::java.lang.Thread(new IKVM.Java.Extensions.java.lang.DelegateRunnable(() =>
            {
                try
                {
                    global::java.lang.Thread.sleep(1000);
                    isFinished = true;
                }
                catch (global::java.lang.InterruptedException e)
                {
                    e.printStackTrace();
                }
            }));

            t.start();
        }

        public string overloadedMethodDoubleVSint(int arg)
        {
            return "int";
        }

        public string overloadedMethodDoubleVSint(double arg)
        {
            return "double";
        }

        public int overloadedMethod(int arg)
        {
            return arg * 2;
        }

        public int overloadedMethod(string arg)
        {
            return arg.Length;
        }

        public int overloadedMethod(bool arg)
        {
            return (arg) ? 1 : 0;
        }

        public int overloadedMethod(Person arg)
        {
            return arg.id * 2;
        }

        public int firstLevelMethodInt(int arg)
        {
            return (int)((Invocable)engine).invokeFunction("secondLevelMethodInt", arg);
        }

        public int thirdLevelMethodInt(int arg)
        {
            return arg * 5;
        }

        public int firstLevelMethodInteger(global::java.lang.Integer arg)
        {
            return (int)((Invocable)engine).invokeFunction("secondLevelMethodInteger", arg);
        }

        public int thirdLevelMethodInteger(global::java.lang.Integer arg)
        {
            return arg.intValue() * 10;
        }

        public Person firstLevelMethodObject(Person p)
        {
            return (Person)((Invocable)engine).invokeFunction("secondLevelMethodObject", p);
        }

        public Person thirdLevelMethodObject(Person p)
        {
            p.id *= 10;
            return p;
        }

    }

}