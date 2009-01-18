/*
  Copyright (C) 2009 Volker Berlin (i-net software)

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
import java.util.HashMap;

import cli.System.Data.Common.DbException;
import cli.System.Data.Odbc.*;

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
        if(obj instanceof cli.System.Int32){
            return Integer.valueOf(CIL.unbox_int(obj));
        }
        if(obj instanceof cli.System.Int16){
            return Short.valueOf(CIL.unbox_short(obj));
        }
        if(obj instanceof cli.System.Byte){
            return Byte.valueOf(CIL.unbox_byte(obj));
        }
        if(obj instanceof cli.System.Decimal){
            return new BigDecimal(((cli.System.Decimal)obj).toString());
        }
        if(obj instanceof cli.System.DateTime){
            return new Timestamp(getJavaMillis((cli.System.DateTime)obj));
        }
        if(obj instanceof cli.System.TimeSpan){
            cli.System.TimeSpan ts = (cli.System.TimeSpan)obj;
            return new Time(ts.get_Hours(), ts.get_Minutes() - 1, ts.get_Seconds());
        }
        return obj;
    }

    /**
     * Get the milliseconds in the Java range from a .NET DateTime object.
     * @param dt the DateTime object
     * @return the milliseconds since 1970-01-01
     */
    public static long getJavaMillis(cli.System.DateTime dt){
        //calculation copied from System.currentTimeMillis()
        long january_1st_1970 = 62135596800000L;
        return dt.get_Ticks() / 10000L - january_1st_1970;
    }
    
    /**
     * The only valid Exception for JDBC is a SQLException.
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

}
