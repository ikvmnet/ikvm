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

import java.io.ByteArrayInputStream;
import java.io.InputStream;
import java.io.Reader;
import java.io.StringReader;
import java.io.UnsupportedEncodingException;
import java.math.BigDecimal;
import java.net.URL;
import java.sql.*;
import java.util.Calendar;
import java.util.Map;

import cli.System.Convert;
import cli.System.DBNull;
import cli.System.IConvertible;
import cli.System.Int16;
import cli.System.Int32;
import cli.System.Int64;
import cli.System.OverflowException;
import cli.System.Single;
import cli.System.TimeSpan;
import cli.System.Data.DataTable;
import cli.System.Data.SchemaType;
import cli.System.Data.Common.DbDataAdapter;
import cli.System.Data.Common.DbDataReader;
import cli.System.Data.Odbc.OdbcDataAdapter;

/**
 * This JDBC Driver is a wrapper to the ODBC.NET Data Provider. This ResultSet based on a DataReader.
 */
public class JdbcOdbcResultSet implements ResultSet{

    private final DbDataReader reader;

    private final JdbcOdbcStatement statement;

    private boolean wasNull;

    private final int holdability;

    private final int concurrency;

    private int fetchSize;

    private int row;

    private final int resultSetType;


    public JdbcOdbcResultSet(JdbcOdbcStatement statement, DbDataReader reader){
        this.statement = statement;
        this.reader = reader;
        holdability = HOLD_CURSORS_OVER_COMMIT;
        concurrency = CONCUR_READ_ONLY;
        resultSetType = TYPE_FORWARD_ONLY;
    }


    public boolean absolute(int row) throws SQLException{
        throwReadOnly();
        return false; // for Compiler
    }


    public void afterLast() throws SQLException{
        throwReadOnly();
    }


    public void beforeFirst() throws SQLException{
        throwReadOnly();
    }


    public void cancelRowUpdates() throws SQLException{
        throwReadOnly();
    }


    public void clearWarnings() throws SQLException{
        // TODO Auto-generated method stub

    }


    public void close() throws SQLException{
        // TODO Auto-generated method stub

    }


    public void deleteRow() throws SQLException{
        throwReadOnly();
    }


    public int findColumn(String columnLabel) throws SQLException{
        return reader.GetOrdinal(columnLabel) + 1;
    }


    public boolean first() throws SQLException{
        throwReadOnly();
        return false; // for compiler
    }


    public Array getArray(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public Array getArray(String columnLabel) throws SQLException{
        return getArray(findColumn(columnLabel));
    }


    public InputStream getAsciiStream(int columnIndex) throws SQLException{
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


    public InputStream getAsciiStream(String columnLabel) throws SQLException{
        return getAsciiStream(findColumn(columnLabel));
    }


    public BigDecimal getBigDecimal(int columnIndex, int scale) throws SQLException{
        BigDecimal dec = getBigDecimal(columnIndex);
        if(dec == null){
            return null;
        }
        if(dec.scale() != scale){
            return dec.setScale(scale, BigDecimal.ROUND_HALF_EVEN);
        }
        return dec;
    }


    public BigDecimal getBigDecimal(String columnLabel, int scale) throws SQLException{
        return getBigDecimal(findColumn(columnLabel), scale);
    }


    public BigDecimal getBigDecimal(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
            if(wasNull){
                return null;
            }
            String str = obj.toString();
            return new BigDecimal(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public BigDecimal getBigDecimal(String columnLabel) throws SQLException{
        return getBigDecimal(findColumn(columnLabel));
    }


    public InputStream getBinaryStream(int columnIndex) throws SQLException{
        byte[] data = getBytes(columnIndex);
        if(data == null){
            return null;
        }
        return new ByteArrayInputStream(data);
    }


    public InputStream getBinaryStream(String columnLabel) throws SQLException{
        return getBinaryStream(findColumn(columnLabel));
    }


    public Blob getBlob(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public Blob getBlob(String columnLabel) throws SQLException{
        return getBlob(findColumn(columnLabel));
    }


    public boolean getBoolean(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
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


    public boolean getBoolean(String columnLabel) throws SQLException{
        return getBoolean(findColumn(columnLabel));
    }


    public byte getByte(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
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


    public byte getByte(String columnLabel) throws SQLException{
        return getByte(findColumn(columnLabel));
    }


    public byte[] getBytes(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
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


    public byte[] getBytes(String columnLabel) throws SQLException{
        return getBytes(findColumn(columnLabel));
    }


    public Reader getCharacterStream(int columnIndex) throws SQLException{
        String str = getString(columnIndex);
        if(str == null){
            return null;
        }
        return new StringReader(str);
    }


    public Reader getCharacterStream(String columnLabel) throws SQLException{
        return getCharacterStream(findColumn(columnLabel));
    }


    public Clob getClob(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public Clob getClob(String columnLabel) throws SQLException{
        return getClob(findColumn(columnLabel));
    }


    public int getConcurrency(){
        return concurrency;
    }


    public String getCursorName() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Date getDate(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
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


    public Date getDate(String columnLabel) throws SQLException{
        return getDate(findColumn(columnLabel));
    }


    public Date getDate(int columnIndex, Calendar cal) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
            if(wasNull){
                return null;
            }
            if(obj instanceof cli.System.DateTime){
                cal.setTimeInMillis(JdbcOdbcUtils.getJavaMillis((cli.System.DateTime)obj));
                int year = cal.get(Calendar.YEAR) - 1900;
                int month = cal.get(Calendar.MONTH) - 1;
                int day = cal.get(Calendar.DAY_OF_MONTH);
                return new Date(year, month, day);
            }
            String str = obj.toString();
            return Date.valueOf(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public Date getDate(String columnLabel, Calendar cal) throws SQLException{
        return getDate(findColumn(columnLabel), cal);
    }


    public double getDouble(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
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


    public double getDouble(String columnLabel) throws SQLException{
        return getDouble(findColumn(columnLabel));
    }


    public int getFetchDirection(){
        return FETCH_UNKNOWN;
    }


    public int getFetchSize(){
        return fetchSize;
    }


    public float getFloat(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
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


    public float getFloat(String columnLabel) throws SQLException{
        return getFloat(findColumn(columnLabel));
    }


    public int getHoldability(){
        return holdability;
    }


    public int getInt(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
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


    public int getInt(String columnLabel) throws SQLException{
        return getInt(findColumn(columnLabel));
    }


    public long getLong(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
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


    public long getLong(String columnLabel) throws SQLException{
        return getLong(findColumn(columnLabel));
    }


    public ResultSetMetaData getMetaData(){
        return new JdbcOdbcResultSetMetaData(reader);
    }


    public Reader getNCharacterStream(int columnIndex) throws SQLException{
        return getCharacterStream(columnIndex);
    }


    public Reader getNCharacterStream(String columnLabel) throws SQLException{
        return getCharacterStream(columnLabel);
    }


    public NClob getNClob(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public NClob getNClob(String columnLabel) throws SQLException{
        return getNClob(findColumn(columnLabel));
    }


    public String getNString(int columnIndex) throws SQLException{
        return getString(columnIndex);
    }


    public String getNString(String columnLabel) throws SQLException{
        return getString(columnLabel);
    }


    public Object getObject(int columnIndex) throws SQLException{
        return JdbcOdbcUtils.convertNet2Java(getObjectImpl(columnIndex));
    }


    public Object getObject(String columnLabel) throws SQLException{
        return getObject(findColumn(columnLabel));
    }


    public Object getObject(int columnIndex, Map<String, Class<?>> map){
        throw new UnsupportedOperationException();
    }


    public Object getObject(String columnLabel, Map<String, Class<?>> map) throws SQLException{
        return getObject(findColumn(columnLabel), map);
    }


    public Ref getRef(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public Ref getRef(String columnLabel) throws SQLException{
        return getRef(findColumn(columnLabel));
    }


    public int getRow(){
        return row;
    }


    public RowId getRowId(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public RowId getRowId(String columnLabel) throws SQLException{
        return getRowId(findColumn(columnLabel));
    }


    public SQLXML getSQLXML(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public SQLXML getSQLXML(String columnLabel) throws SQLException{
        return getSQLXML(findColumn(columnLabel));
    }


    public short getShort(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
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


    public short getShort(String columnLabel) throws SQLException{
        return getShort(findColumn(columnLabel));
    }


    public Statement getStatement(){
        return statement;
    }


    public String getString(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
            if(wasNull){
                return null;
            }
            return obj.toString();
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public String getString(String columnLabel) throws SQLException{
        return getString(findColumn(columnLabel));
    }


    public Time getTime(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
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


    public Time getTime(String columnLabel) throws SQLException{
        return getTime(findColumn(columnLabel));
    }


    public Time getTime(int columnIndex, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Time getTime(String columnLabel, Calendar cal) throws SQLException{
        return getTime(findColumn(columnLabel), cal);
    }


    public Timestamp getTimestamp(int columnIndex) throws SQLException{
        try{
            Object obj = getObjectImpl(columnIndex);
            if(wasNull){
                return null;
            }
            if(obj instanceof cli.System.DateTime){
                cli.System.DateTime dt = (cli.System.DateTime)obj;
                return new Timestamp(JdbcOdbcUtils.getJavaMillis(dt));
            }
            String str = obj.toString();
            return Timestamp.valueOf(str);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public Timestamp getTimestamp(String columnLabel) throws SQLException{
        return getTimestamp(findColumn(columnLabel));
    }


    public Timestamp getTimestamp(int columnIndex, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Timestamp getTimestamp(String columnLabel, Calendar cal) throws SQLException{
        return getTimestamp(findColumn(columnLabel), cal);
    }


    public int getType(){
        return resultSetType;
    }


    public URL getURL(int columnIndex){
        throw new UnsupportedOperationException();
    }


    public URL getURL(String columnLabel) throws SQLException{
        return getURL(findColumn(columnLabel));
    }


    public InputStream getUnicodeStream(int columnIndex) throws SQLException{
        try{
            return new ByteArrayInputStream(getString(columnIndex).getBytes("UTF16"));
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public InputStream getUnicodeStream(String columnLabel) throws SQLException{
        return getUnicodeStream(findColumn(columnLabel));
    }


    public SQLWarning getWarnings() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public void insertRow() throws SQLException{
        throwReadOnly();
    }


    public boolean isAfterLast() throws SQLException{
        throwReadOnly();
        return false; // only for compiler
    }


    public boolean isBeforeFirst() throws SQLException{
        throwReadOnly();
        return false; // only for compiler
    }


    public boolean isClosed() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean isFirst() throws SQLException{
        throwReadOnly();
        return false; // only for compiler
    }


    public boolean isLast() throws SQLException{
        throwReadOnly();
        return false; // only for compiler
    }


    public boolean last() throws SQLException{
        throwReadOnly();
        return false; // only for compiler
    }


    public void moveToCurrentRow() throws SQLException{
        throwReadOnly();
    }


    public void moveToInsertRow() throws SQLException{
        throwReadOnly();
    }


    public boolean next() throws SQLException{
        if(reader.Read()){
            row++;
            return true;
        }
        row = 0;
        return false;
    }


    public boolean previous() throws SQLException{
        throwReadOnly();
        return false; // only for compiler
    }


    public void refreshRow() throws SQLException{
        throwReadOnly();
    }


    public boolean relative(int rows) throws SQLException{
        throwReadOnly();
        return false; // only for compiler
    }


    public boolean rowDeleted() throws SQLException{
        throwReadOnly();
        return false; // only for compiler
    }


    public boolean rowInserted() throws SQLException{
        throwReadOnly();
        return false; // only for compiler
    }


    public boolean rowUpdated() throws SQLException{
        throwReadOnly();
        return false; // only for compiler
    }


    public void setFetchDirection(int direction){
        // ignore it
    }


    public void setFetchSize(int rows){
        // ignore it
    }


    public void updateArray(int columnIndex, Array x) throws SQLException{
        throwReadOnly();
    }


    public void updateArray(String columnLabel, Array x) throws SQLException{
        throwReadOnly();
    }


    public void updateAsciiStream(int columnIndex, InputStream x, int length) throws SQLException{
        throwReadOnly();
    }


    public void updateAsciiStream(String columnLabel, InputStream x, int length) throws SQLException{
        throwReadOnly();
    }


    public void updateAsciiStream(int columnIndex, InputStream x, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateAsciiStream(String columnLabel, InputStream x, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateAsciiStream(int columnIndex, InputStream x) throws SQLException{
        throwReadOnly();
    }


    public void updateAsciiStream(String columnLabel, InputStream x) throws SQLException{
        throwReadOnly();
    }


    public void updateBigDecimal(int columnIndex, BigDecimal x) throws SQLException{
        throwReadOnly();
    }


    public void updateBigDecimal(String columnLabel, BigDecimal x) throws SQLException{
        throwReadOnly();
    }


    public void updateBinaryStream(int columnIndex, InputStream x, int length) throws SQLException{
        throwReadOnly();
    }


    public void updateBinaryStream(String columnLabel, InputStream x, int length) throws SQLException{
        throwReadOnly();
    }


    public void updateBinaryStream(int columnIndex, InputStream x, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateBinaryStream(String columnLabel, InputStream x, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateBinaryStream(int columnIndex, InputStream x) throws SQLException{
        throwReadOnly();
    }


    public void updateBinaryStream(String columnLabel, InputStream x) throws SQLException{
        throwReadOnly();
    }


    public void updateBlob(int columnIndex, Blob x) throws SQLException{
        throwReadOnly();
    }


    public void updateBlob(String columnLabel, Blob x) throws SQLException{
        throwReadOnly();
    }


    public void updateBlob(int columnIndex, InputStream inputStream, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateBlob(String columnLabel, InputStream inputStream, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateBlob(int columnIndex, InputStream inputStream) throws SQLException{
        throwReadOnly();
    }


    public void updateBlob(String columnLabel, InputStream inputStream) throws SQLException{
        throwReadOnly();
    }


    public void updateBoolean(int columnIndex, boolean x) throws SQLException{
        throwReadOnly();
    }


    public void updateBoolean(String columnLabel, boolean x) throws SQLException{
        throwReadOnly();
    }


    public void updateByte(int columnIndex, byte x) throws SQLException{
        throwReadOnly();
    }


    public void updateByte(String columnLabel, byte x) throws SQLException{
        throwReadOnly();
    }


    public void updateBytes(int columnIndex, byte[] x) throws SQLException{
        throwReadOnly();
    }


    public void updateBytes(String columnLabel, byte[] x) throws SQLException{
        throwReadOnly();
    }


    public void updateCharacterStream(int columnIndex, Reader x, int length) throws SQLException{
        throwReadOnly();
    }


    public void updateCharacterStream(String columnLabel, Reader reader, int length) throws SQLException{
        throwReadOnly();
    }


    public void updateCharacterStream(int columnIndex, Reader x, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateCharacterStream(String columnLabel, Reader reader, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateCharacterStream(int columnIndex, Reader x) throws SQLException{
        throwReadOnly();
    }


    public void updateCharacterStream(String columnLabel, Reader reader) throws SQLException{
        throwReadOnly();
    }


    public void updateClob(int columnIndex, Clob x) throws SQLException{
        throwReadOnly();
    }


    public void updateClob(String columnLabel, Clob x) throws SQLException{
        throwReadOnly();
    }


    public void updateClob(int columnIndex, Reader reader, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateClob(String columnLabel, Reader reader, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateClob(int columnIndex, Reader reader) throws SQLException{
        throwReadOnly();
    }


    public void updateClob(String columnLabel, Reader reader) throws SQLException{
        throwReadOnly();
    }


    public void updateDate(int columnIndex, Date x) throws SQLException{
        throwReadOnly();
    }


    public void updateDate(String columnLabel, Date x) throws SQLException{
        throwReadOnly();
    }


    public void updateDouble(int columnIndex, double x) throws SQLException{
        throwReadOnly();
    }


    public void updateDouble(String columnLabel, double x) throws SQLException{
        throwReadOnly();
    }


    public void updateFloat(int columnIndex, float x) throws SQLException{
        throwReadOnly();
    }


    public void updateFloat(String columnLabel, float x) throws SQLException{
        throwReadOnly();
    }


    public void updateInt(int columnIndex, int x) throws SQLException{
        throwReadOnly();
    }


    public void updateInt(String columnLabel, int x) throws SQLException{
        throwReadOnly();
    }


    public void updateLong(int columnIndex, long x) throws SQLException{
        throwReadOnly();
    }


    public void updateLong(String columnLabel, long x) throws SQLException{
        throwReadOnly();
    }


    public void updateNCharacterStream(int columnIndex, Reader x, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateNCharacterStream(String columnLabel, Reader reader, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateNCharacterStream(int columnIndex, Reader x) throws SQLException{
        throwReadOnly();
    }


    public void updateNCharacterStream(String columnLabel, Reader reader) throws SQLException{
        throwReadOnly();
    }


    public void updateNClob(int columnIndex, NClob clob) throws SQLException{
        throwReadOnly();
    }


    public void updateNClob(String columnLabel, NClob clob) throws SQLException{
        throwReadOnly();
    }


    public void updateNClob(int columnIndex, Reader reader, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateNClob(String columnLabel, Reader reader, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateNClob(int columnIndex, Reader reader) throws SQLException{
        throwReadOnly();
    }


    public void updateNClob(String columnLabel, Reader reader) throws SQLException{
        throwReadOnly();
    }


    public void updateNString(int columnIndex, String string) throws SQLException{
        throwReadOnly();
    }


    public void updateNString(String columnLabel, String string) throws SQLException{
        throwReadOnly();
    }


    public void updateNull(int columnIndex) throws SQLException{
        throwReadOnly();
    }


    public void updateNull(String columnLabel) throws SQLException{
        throwReadOnly();
    }


    public void updateObject(int columnIndex, Object x, int scaleOrLength) throws SQLException{
        throwReadOnly();
    }


    public void updateObject(int columnIndex, Object x) throws SQLException{
        throwReadOnly();
    }


    public void updateObject(String columnLabel, Object x, int scaleOrLength) throws SQLException{
        throwReadOnly();
    }


    public void updateObject(String columnLabel, Object x) throws SQLException{
        throwReadOnly();
    }


    public void updateRef(int columnIndex, Ref x) throws SQLException{
        throwReadOnly();
    }


    public void updateRef(String columnLabel, Ref x) throws SQLException{
        throwReadOnly();
    }


    public void updateRow() throws SQLException{
        throwReadOnly();
    }


    public void updateRowId(int columnIndex, RowId x) throws SQLException{
        throwReadOnly();
    }


    public void updateRowId(String columnLabel, RowId x) throws SQLException{
        throwReadOnly();
    }


    public void updateSQLXML(int columnIndex, SQLXML xmlObject) throws SQLException{
        throwReadOnly();
    }


    public void updateSQLXML(String columnLabel, SQLXML xmlObject) throws SQLException{
        throwReadOnly();
    }


    public void updateShort(int columnIndex, short x) throws SQLException{
        throwReadOnly();
    }


    public void updateShort(String columnLabel, short x) throws SQLException{
        throwReadOnly();
    }


    public void updateString(int columnIndex, String x) throws SQLException{
        throwReadOnly();
    }


    public void updateString(String columnLabel, String x) throws SQLException{
        throwReadOnly();
    }


    public void updateTime(int columnIndex, Time x) throws SQLException{
        throwReadOnly();
    }


    public void updateTime(String columnLabel, Time x) throws SQLException{
        throwReadOnly();
    }


    public void updateTimestamp(int columnIndex, Timestamp x) throws SQLException{
        throwReadOnly();
    }


    public void updateTimestamp(String columnLabel, Timestamp x) throws SQLException{
        throwReadOnly();
    }


    public boolean wasNull(){
        return wasNull;
    }


    public boolean isWrapperFor(Class<?> iface){
        return iface.isAssignableFrom(this.getClass());
    }


    public <T>T unwrap(Class<T> iface) throws SQLException{
        if(isWrapperFor(iface)){
            return (T)this;
        }
        throw new SQLException(this.getClass().getName() + " does not implements " + iface.getName() + ".", "01000");
    }


    protected Object getObjectImpl(int columnIndex) throws SQLException{
        try{
            columnIndex--;
            Object obj = reader.get_Item(columnIndex);
            if(obj == null || obj == DBNull.Value){
                wasNull = true;
                return null;
            }
            return obj;
        }catch(Throwable ex){
            throw JdbcOdbcUtils.createSQLException(ex);
        }
    }


    private void throwReadOnly() throws SQLException{
        throw new SQLException("ResultSet is read only.");
    }

}
