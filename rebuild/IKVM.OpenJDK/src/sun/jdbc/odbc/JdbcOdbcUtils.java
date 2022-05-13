/*
  Copyright (C) 2009, 2010 Volker Berlin (i-net software)
  Copyright (C) 2011 Karsten Heinrich (i-net software)

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
 */
package sun.jdbc.odbc;

import ikvm.lang.CIL;

import java.math.BigDecimal;
import java.sql.*;
import java.util.Calendar;
import java.util.HashMap;

import cli.System.DBNull;
import cli.System.TimeSpan;
import cli.System.Data.DbType;
import cli.System.Data.Common.DbException;
import cli.System.Data.Odbc.*;
import cli.System.Globalization.CultureInfo;

/**
 * @author Volker Berlin
 */
public class JdbcOdbcUtils{

    private static final HashMap<String, String> classNameMap = new HashMap<String, String>();
    static{
        classNameMap.put("System.String", "java.lang.String");
        classNameMap.put("System.Int16", "java.lang.Short");
        classNameMap.put("System.Int32", "java.lang.Integer");
        classNameMap.put("System.Int64", "java.lang.Long");
        classNameMap.put("System.Double", "java.lang.Double");
        classNameMap.put("System.Decimal", "java.math.BigDecimal");
        classNameMap.put("System.DateTime", "java.sql.Timestamp");
        classNameMap.put("System.TimeSpan", "java.sql.Time");
    }


    /**
     * Solve a mapping between .NET class names and the equivalent Java class names for
     * ResultSetMetaData.getColumnClassName
     * 
     * @param netClassName
     *            the .NET class name
     * @return the Java class name
     */
    public static String getJavaClassName(String netClassName){
        String javaClassName = classNameMap.get(netClassName);
        if(javaClassName != null){
            return javaClassName;
        }
        return "java.lang.Object";
    }


    /**
     * Convert a .NET Object in the equals Java Object.
     * 
     * @param obj
     *            the .NET Object
     * @return a Java Object
     */
    public static java.lang.Object convertNet2Java(java.lang.Object obj){
        if(obj instanceof cli.System.Int64){
            return Long.valueOf(CIL.unbox_long(obj));
        }
        if(obj instanceof cli.System.Int32){
            return Integer.valueOf(CIL.unbox_int(obj));
        }
        if(obj instanceof cli.System.Int16){
            return Short.valueOf(CIL.unbox_short(obj));
        }
        if(obj instanceof cli.System.Byte){
            return Byte.valueOf(CIL.unbox_byte(obj));
        }
        if(obj instanceof cli.System.Double){
            return Double.valueOf(CIL.unbox_double(obj));
        }
        if(obj instanceof cli.System.Single){
            return Float.valueOf(CIL.unbox_float(obj));
        }
        if(obj instanceof cli.System.Boolean){
            return Boolean.valueOf(CIL.unbox_boolean(obj));
        }
        if(obj instanceof cli.System.Decimal){
            return new BigDecimal(((cli.System.Decimal)obj).ToString(CultureInfo.get_InvariantCulture()));
        }
        if(obj instanceof cli.System.DateTime){
        	return convertDateTimeToTimestamp((cli.System.DateTime)obj);
        }
        if(obj instanceof cli.System.TimeSpan){
            cli.System.TimeSpan ts = (cli.System.TimeSpan)obj;
            return new Time(ts.get_Hours(), ts.get_Minutes(), ts.get_Seconds());
        }
        if(obj instanceof cli.System.DBNull){
            return null;
        }
        return obj;
    }

    /**
     * Converts a .NET DateTime to a Timestamp in the current Timezone
     * @param obj the dateTime
     * @return the conveted time stamp
     */
	public static Timestamp convertDateTimeToTimestamp( cli.System.DateTime obj) {
		long javaMillis = getJavaMillis(obj);
		int seconds = (int)(javaMillis / 1000);
		int nanos = (int)((javaMillis % 1000) * 1000000);
		return new Timestamp( 70, 0, 1, 0, 0, seconds, nanos );
	}


    /**
     * Convert a Java Object in the equals .NET Object.
     * 
     * @param obj
     *            Java Object
     * @param length
     *            the length of data if obj is a stream
     * @return .NET Object
     */
    public static Object convertJava2Net(Object obj, int length){
        // TODO use the length with streams
        return convertJava2Net(obj);
    }


    /**
     * Convert a Java Object in the equals .NET Object.
     * 
     * @param obj
     *            Java Object
     * @return a .NET Object
     */
    public static Object convertJava2Net(Object obj){
        if(obj == null){
            return DBNull.Value;
        }
        if(obj instanceof Double){
            return CIL.box_double(((Double)obj).doubleValue());
        }
        if(obj instanceof Float){
            return CIL.box_float(((Float)obj).floatValue());
        }
        if(obj instanceof Long){
            return CIL.box_long(((Long)obj).longValue());
        }
        if(obj instanceof Integer){
            return CIL.box_int(((Integer)obj).intValue());
        }
        if(obj instanceof Short){
            return CIL.box_short(((Short)obj).shortValue());
        }
        if(obj instanceof Byte){
            return CIL.box_byte(((Byte)obj).byteValue());
        }
        if(obj instanceof Boolean){
            return CIL.box_boolean(((Boolean)obj).booleanValue());
        }
        if(obj instanceof Time){
            Time ts = (Time)obj;
            return new TimeSpan(ts.getHours(), ts.getMinutes(), ts.getSeconds());
        }
        if(obj instanceof java.util.Date){
            long ticks = getNetTicks((java.util.Date)obj);
            return new cli.System.DateTime(ticks);
        }
        if(obj instanceof BigDecimal){
            return cli.System.Decimal.Parse(obj.toString(), CultureInfo.get_InvariantCulture());
        }
        return obj;
    }


    /**
     * Get the milliseconds in the Java range from a .NET DateTime object.
     * 
     * @param dt
     *            the DateTime object
     * @return the milliseconds since 1970-01-01
     */
    public static long getJavaMillis(cli.System.DateTime dt){
        // calculation copied from System.currentTimeMillis()
        long january_1st_1970 = 62135596800000L;
        return dt.get_Ticks() / 10000L - january_1st_1970;
    }


    /**
     * Get the ticks for a System.DateTime from a java.util.Date
     * 
     * @param date
     *            the java.util.Date
     * @return ticks
     */
    public static long getNetTicks(java.util.Date date){
        // inverse from getJavaMillis
        long january_1st_1970 = 62135596800000L;
        return (date.getTime() + january_1st_1970) * 10000L;
    }


    /**
     * Convert a local (current default) Date to a Date in the time zone of the given calendar. Do nothing if date or
     * calendar is null.
     * 
     * @param date
     *            the converting Date
     * @param cal
     *            the Calendar with the time zone
     */
    public static void convertLocalToCalendarDate(java.util.Date date, Calendar cal){
        if(date == null || cal == null){
            return;
        }
        cal.set(date.getYear() + 1900, date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date
                .getSeconds());
        long millis = cal.getTimeInMillis() / 1000 * 1000 + date.getTime() % 1000;
        date.setTime(millis);
    }


    /**
     * Convert a Date in the calendar time zone to a date in the local (current default) time zone. Do nothing if date
     * or calendar is null.
     * 
     * @param date
     * @param cal
     */
    public static void convertCalendarToLocalDate(java.util.Date date, Calendar cal){
        if(date == null || cal == null){
            return;
        }
        cal.setTimeInMillis(date.getTime());
        date.setYear(cal.get(Calendar.YEAR) - 1900);
        date.setMonth(cal.get(Calendar.MONTH));
        date.setDate(cal.get(Calendar.DAY_OF_MONTH));
        date.setHours(cal.get(Calendar.HOUR_OF_DAY));
        date.setMinutes(cal.get(Calendar.MINUTE));
        date.setSeconds(cal.get(Calendar.SECOND));
    }


    /**
     * The only valid Exception for JDBC is a SQLException. Here we create one based on the real exception.
     * 
     * @param th
     *            any Throwable that occur
     * @return a SQLException, never null
     */
    public static SQLException createSQLException(Throwable th){
        if(th instanceof SQLException){
            return (SQLException)th;
        }
        if(th instanceof OdbcException){
            SQLException sqlEx = null;
            OdbcErrorCollection errors = ((OdbcException)th).get_Errors();
            for(int e = 0; e < errors.get_Count(); e++){
                OdbcError err = errors.get_Item(e);
                SQLException newEx = new SQLException(err.get_Message(), err.get_SQLState(), err.get_NativeError());
                if(sqlEx == null){
                    sqlEx = newEx;
                }else{
                    sqlEx.setNextException(newEx);
                }
            }
            if(sqlEx != null){
                sqlEx.initCause(th);
                return sqlEx;
            }
        }
        if(th instanceof DbException){
            DbException dbEx = (DbException)th;
            return new SQLException(dbEx.get_Message(), "S1000", dbEx.get_ErrorCode(), th);
        }
        return new SQLException(th);
    }


    /**
     * Convert a value from java.sql.Types to a value from to a System.Data.DbType
     * 
     * @param type
     *            a JDBC type
     * @return a ADO.NET type
     * @throws SQLException
     *             if the type can not be converted
     */
    public static int convertJdbc2AdoNetType(int type) throws SQLException{
        switch(type){
            case Types.BIGINT:
                return DbType.Int64;
            case Types.BINARY:
            case Types.BLOB:
            case Types.LONGVARBINARY:
            case Types.VARBINARY:
                return DbType.Binary;
            case Types.BIT:
            case Types.BOOLEAN:
                return DbType.Boolean;
            case Types.CHAR:
                return DbType.AnsiStringFixedLength;
            case Types.CLOB:
            case Types.DATALINK:
            case Types.LONGVARCHAR:
            case Types.NULL: // we hope that the DBMS can map any NULL values from VARCHAR
            case Types.VARCHAR:
                return DbType.AnsiString;
            case Types.DATE:
                return DbType.Date;
            case Types.DECIMAL:
            case Types.NUMERIC:
                return DbType.Decimal;
            case Types.DOUBLE:
                return DbType.Double;
            case Types.FLOAT:
            case Types.REAL:
                return DbType.Single;
            case Types.INTEGER:
                return DbType.Int32;
            case Types.JAVA_OBJECT:
                return DbType.Object;
            case Types.LONGNVARCHAR:
            case Types.NCLOB:
            case Types.NVARCHAR:
                return DbType.String;
            case Types.NCHAR:
                return DbType.StringFixedLength;
            case Types.ROWID:
                return DbType.Guid;
            case Types.SMALLINT:
                return DbType.Int16;
            case Types.SQLXML:
                return DbType.Xml;
            case Types.TIME:
                return DbType.Time;
            case Types.TIMESTAMP:
                return DbType.DateTime;
            case Types.TINYINT:
                return DbType.Byte;
            case Types.ARRAY:
            case Types.DISTINCT:
            case Types.OTHER:
            case Types.REF:
            case Types.STRUCT:
                break;

        }
        throw new SQLException("Not supported JDBC type:" + type);
    }
}
