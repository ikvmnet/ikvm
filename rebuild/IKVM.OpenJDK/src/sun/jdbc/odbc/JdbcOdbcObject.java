/*
  Copyright (C) 2009 Volker Berlin (i-net software)
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

import java.io.ByteArrayInputStream;
import java.io.InputStream;
import java.io.Reader;
import java.io.StringReader;
import java.math.BigDecimal;
import java.net.URL;
import java.sql.Array;
import java.sql.Blob;
import java.sql.Clob;
import java.sql.Date;
import java.sql.NClob;
import java.sql.Ref;
import java.sql.RowId;
import java.sql.SQLException;
import java.sql.SQLXML;
import java.sql.Time;
import java.sql.Timestamp;
import java.util.Calendar;
import java.util.Map;

import cli.System.Convert;
import cli.System.DBNull;
import cli.System.IConvertible;
import cli.System.Int16;
import cli.System.Int32;
import cli.System.Int64;
import cli.System.Single;

/**
 * @author Volker Berlin
 */
public abstract class JdbcOdbcObject{

    private boolean wasNull;


    /**
     * Maps the given ResultSet column label to its ResultSet column index or the given CallableStatment parameter name
     * to the parameter index.
     * 
     * @param columnLabel
     *            the label for the column specified with the SQL AS clause. If the SQL AS clause was not specified,
     *            then the label is the name of the column
     * @return the column index of the given column name
     * @throws SQLException
     *             if the ResultSet object does not contain a column labeled columnLabel, a database access error occurs
     *             or this method is called on a closed result set
     */
    public abstract int findColumn(String columnLabel) throws SQLException;


    /**
     * Read an Object from the current row store at the current row on the given column.
     * 
     * @param columnIndex
     *            a JDBC column index starting with 1
     * @return a .NET Object, DBNull or null
     * @throws SQLException
     *             if the result is closed or any other error occur.
     */
    protected abstract Object getObjectImpl(int columnIndex) throws SQLException;


    /**
     * Read an Object from the current row store at the current row on the given column. Set the flag wasNull.
     * 
     * @param columnIndex
     *            a JDBC column index starting with 1
     * @return a .NET Object or null
     * @throws SQLException
     *             if the result is closed or any other error occur.
     */
    private final Object getObjectSetWasNull(int columnIndex) throws SQLException{
        Object obj = getObjectImpl(columnIndex);
        if(obj == null || obj == DBNull.Value){
            wasNull = true;
            return null;
        }
        wasNull = false;
        return obj;
    }


    public final Array getArray(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public final Array getArray(String columnLabel) throws SQLException{
        return getArray(findColumn(columnLabel));
    }


    public final InputStream getAsciiStream(int columnIndex) throws SQLException{
        try{
            String str = getString(columnIndex);
            if(str == null){
                return null;
            }
            return new ByteArrayInputStream(str.getBytes("Ascii"));
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final InputStream getAsciiStream(String columnLabel) throws SQLException{
        return getAsciiStream(findColumn(columnLabel));
    }


    public final BigDecimal getBigDecimal(int columnIndex, int scale) throws SQLException{
        BigDecimal dec = getBigDecimal(columnIndex);
        if(dec == null){
            return null;
        }
        if(dec.scale() != scale){
            return dec.setScale(scale, BigDecimal.ROUND_HALF_EVEN);
        }
        return dec;
    }


    public final BigDecimal getBigDecimal(String columnLabel, int scale) throws SQLException{
        return getBigDecimal(findColumn(columnLabel), scale);
    }


    public final BigDecimal getBigDecimal(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectSetWasNull(columnIndex);
            if(wasNull){
                return null;
            }
            String str = obj.toString();
            return new BigDecimal(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final BigDecimal getBigDecimal(String columnLabel) throws SQLException{
        return getBigDecimal(findColumn(columnLabel));
    }


    public final InputStream getBinaryStream(int columnIndex) throws SQLException{
        byte[] data = getBytes(columnIndex);
        if(data == null){
            return null;
        }
        return new ByteArrayInputStream(data);
    }


    public final InputStream getBinaryStream(String columnLabel) throws SQLException{
        return getBinaryStream(findColumn(columnLabel));
    }


    public final Blob getBlob(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public final Blob getBlob(String columnLabel) throws SQLException{
        return getBlob(findColumn(columnLabel));
    }


    public final boolean getBoolean(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectSetWasNull(columnIndex);
            if(wasNull){
                return false;
            }
            if(obj instanceof IConvertible){
                return Convert.ToBoolean(obj);
            }
            String str = obj.toString();
            if(str.length() > 0){
                // special handling for boolean representation in old databases
                char ch = str.charAt(0);
                if(ch == 'T' || ch == 't'){
                    return true;
                }
                if(ch == 'F' || ch == 'f'){
                    return true;
                }
            }
            return cli.System.Boolean.Parse(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final boolean getBoolean(String columnLabel) throws SQLException{
        return getBoolean(findColumn(columnLabel));
    }


    public final byte getByte(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectSetWasNull(columnIndex);
            if(wasNull){
                return 0;
            }
            if(obj instanceof IConvertible){
                return Convert.ToByte(obj);
            }
            String str = obj.toString();
            return cli.System.Byte.Parse(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final byte getByte(String columnLabel) throws SQLException{
        return getByte(findColumn(columnLabel));
    }


    public final byte[] getBytes(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectSetWasNull(columnIndex);
            if(wasNull){
                return null;
            }
            if(obj instanceof byte[]){
                return (byte[])obj;
            }
            String str = obj.toString();
            return str.getBytes(); // which encoding?
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final byte[] getBytes(String columnLabel) throws SQLException{
        return getBytes(findColumn(columnLabel));
    }


    public final Reader getCharacterStream(int columnIndex) throws SQLException{
        String str = getString(columnIndex);
        if(str == null){
            return null;
        }
        return new StringReader(str);
    }


    public final Reader getCharacterStream(String columnLabel) throws SQLException{
        return getCharacterStream(findColumn(columnLabel));
    }


    public final Clob getClob(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public final Clob getClob(String columnLabel) throws SQLException{
        return getClob(findColumn(columnLabel));
    }


    public final Date getDate(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectSetWasNull(columnIndex);
            if(wasNull){
                return null;
            }
            if(obj instanceof cli.System.DateTime){
                cli.System.DateTime dt = (cli.System.DateTime)obj;
                return new Date(dt.get_Year() - 1900, dt.get_Month() - 1, dt.get_Day());
            }
            String str = obj.toString();
            return Date.valueOf(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final Date getDate(String columnLabel) throws SQLException{
        return getDate(findColumn(columnLabel));
    }


    public final Date getDate(int columnIndex, Calendar cal) throws SQLException{
        Date date = getDate(columnIndex);
        JdbcOdbcUtils.convertLocalToCalendarDate(date, cal);
        return date;
    }


    public final Date getDate(String columnLabel, Calendar cal) throws SQLException{
        return getDate(findColumn(columnLabel), cal);
    }


    public final double getDouble(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectSetWasNull(columnIndex);
            if(wasNull){
                return 0;
            }
            if(obj instanceof IConvertible){
                return Convert.ToDouble(obj);
            }
            String str = obj.toString();
            return cli.System.Double.Parse(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final double getDouble(String columnLabel) throws SQLException{
        return getDouble(findColumn(columnLabel));
    }


    public final float getFloat(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectSetWasNull(columnIndex);
            if(wasNull){
                return 0;
            }
            if(obj instanceof IConvertible){
                return Convert.ToSingle(obj);
            }
            String str = obj.toString();
            return Single.Parse(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final float getFloat(String columnLabel) throws SQLException{
        return getFloat(findColumn(columnLabel));
    }


    public final int getInt(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectSetWasNull(columnIndex);
            if(wasNull){
                return 0;
            }
            if(obj instanceof IConvertible){
                return Convert.ToInt32(obj);
            }
            String str = obj.toString();
            return Int32.Parse(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final int getInt(String columnLabel) throws SQLException{
        return getInt(findColumn(columnLabel));
    }


    public final long getLong(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectSetWasNull(columnIndex);
            if(wasNull){
                return 0;
            }
            if(obj instanceof IConvertible){
                return Convert.ToInt64(obj);
            }
            String str = obj.toString();
            return Int64.Parse(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final long getLong(String columnLabel) throws SQLException{
        return getLong(findColumn(columnLabel));
    }


    public final Reader getNCharacterStream(int columnIndex) throws SQLException{
        return getCharacterStream(columnIndex);
    }


    public final Reader getNCharacterStream(String columnLabel) throws SQLException{
        return getCharacterStream(columnLabel);
    }


    public final NClob getNClob(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public final NClob getNClob(String columnLabel) throws SQLException{
        return getNClob(findColumn(columnLabel));
    }


    public final String getNString(int columnIndex) throws SQLException{
        return getString(columnIndex);
    }


    public final String getNString(String columnLabel) throws SQLException{
        return getString(columnLabel);
    }


    public final Object getObject(int columnIndex) throws SQLException{
        return JdbcOdbcUtils.convertNet2Java(getObjectSetWasNull(columnIndex));
    }


    public final Object getObject(String columnLabel) throws SQLException{
        return getObject(findColumn(columnLabel));
    }


    public final Object getObject(int columnIndex, Map<String, Class<?>> map){
        throw new UnsupportedOperationException();
    }


    public final Object getObject(String columnLabel, Map<String, Class<?>> map) throws SQLException{
        return getObject(findColumn(columnLabel), map);
    }


    public final Ref getRef(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public final Ref getRef(String columnLabel) throws SQLException{
        return getRef(findColumn(columnLabel));
    }


    public final RowId getRowId(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public final RowId getRowId(String columnLabel) throws SQLException{
        return getRowId(findColumn(columnLabel));
    }


    public final SQLXML getSQLXML(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public final SQLXML getSQLXML(String columnLabel) throws SQLException{
        return getSQLXML(findColumn(columnLabel));
    }


    public final short getShort(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectSetWasNull(columnIndex);
            if(wasNull){
                return 0;
            }
            if(obj instanceof IConvertible){
                return Convert.ToInt16(obj);
            }
            String str = obj.toString();
            return Int16.Parse(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final short getShort(String columnLabel) throws SQLException{
        return getShort(findColumn(columnLabel));
    }


    public final String getString(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectSetWasNull(columnIndex);
            if(wasNull){
                return null;
            }
            return obj.toString();
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final String getString(String columnLabel) throws SQLException{
        return getString(findColumn(columnLabel));
    }


    public final Time getTime(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectSetWasNull(columnIndex);
            if(wasNull){
                return null;
            }
            if(obj instanceof cli.System.DateTime){
                cli.System.DateTime dt = (cli.System.DateTime)obj;
                return new Time(dt.get_Hour(), dt.get_Minute() - 1, dt.get_Second());
            }
            if(obj instanceof cli.System.TimeSpan){
                cli.System.TimeSpan ts = (cli.System.TimeSpan)obj;
                return new Time(ts.get_Hours(), ts.get_Minutes() - 1, ts.get_Seconds());
            }
            String str = obj.toString();
            return Time.valueOf(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final Time getTime(String columnLabel) throws SQLException{
        return getTime(findColumn(columnLabel));
    }


    public final Time getTime(int columnIndex, Calendar cal) throws SQLException{
        Time time = getTime(columnIndex);
        JdbcOdbcUtils.convertLocalToCalendarDate(time, cal);
        return time;
    }


    public final Time getTime(String columnLabel, Calendar cal) throws SQLException{
        return getTime(findColumn(columnLabel), cal);
    }


    public final Timestamp getTimestamp(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectSetWasNull(columnIndex);
            if(wasNull){
                return null;
            }
            if(obj instanceof cli.System.DateTime){
            	return JdbcOdbcUtils.convertDateTimeToTimestamp((cli.System.DateTime)obj);
            }
            String str = obj.toString();
            return Timestamp.valueOf(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final Timestamp getTimestamp(String columnLabel) throws SQLException{
        return getTimestamp(findColumn(columnLabel));
    }


    public final Timestamp getTimestamp(int columnIndex, Calendar cal) throws SQLException{
        Timestamp ts = getTimestamp(columnIndex);
        JdbcOdbcUtils.convertLocalToCalendarDate(ts, cal);
        return ts;
    }


    public final Timestamp getTimestamp(String columnLabel, Calendar cal) throws SQLException{
        return getTimestamp(findColumn(columnLabel), cal);
    }


    public final URL getURL(int columnIndex) throws SQLException{
        try{
            String url = getString(columnIndex);
            if(wasNull){
                return null;
            }
            return new URL(url);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final URL getURL(String columnLabel) throws SQLException{
        return getURL(findColumn(columnLabel));
    }


    public final InputStream getUnicodeStream(int columnIndex) throws SQLException{
        try{
            return new ByteArrayInputStream(getString(columnIndex).getBytes("UTF16"));
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public final InputStream getUnicodeStream(String columnLabel) throws SQLException{
        return getUnicodeStream(findColumn(columnLabel));
    }


    public final boolean wasNull(){
        return wasNull;
    }

}
