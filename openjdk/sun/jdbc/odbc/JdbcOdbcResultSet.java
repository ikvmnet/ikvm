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

import java.io.InputStream;
import java.io.Reader;
import java.math.BigDecimal;
import java.net.URL;
import java.sql.*;
import java.util.Calendar;
import java.util.Map;

import cli.System.Data.Common.DbDataReader;

/**
 * This JDBC Driver is a wrapper to the ODBC.NET Data Provider
 */
public class JdbcOdbcResultSet implements ResultSet{

    private final DbDataReader reader;

    private final JdbcOdbcStatement statement;

    private boolean wasNull;


    public JdbcOdbcResultSet(JdbcOdbcStatement statement, DbDataReader reader){
        this.statement = statement;
        this.reader = reader;
    }


    public boolean absolute(int row) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public void afterLast() throws SQLException{
        // TODO Auto-generated method stub

    }


    public void beforeFirst() throws SQLException{
        // TODO Auto-generated method stub

    }


    public void cancelRowUpdates() throws SQLException{
        // TODO Auto-generated method stub

    }


    public void clearWarnings() throws SQLException{
        // TODO Auto-generated method stub

    }


    public void close() throws SQLException{
        // TODO Auto-generated method stub

    }


    public void deleteRow() throws SQLException{
        // TODO Auto-generated method stub

    }


    public int findColumn(String columnLabel) throws SQLException{
        return reader.GetOrdinal(columnLabel) + 1;
    }


    public boolean first() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public Array getArray(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Array getArray(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public InputStream getAsciiStream(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public InputStream getAsciiStream(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public BigDecimal getBigDecimal(int columnIndex, int scale) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public BigDecimal getBigDecimal(String columnLabel, int scale) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public BigDecimal getBigDecimal(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public BigDecimal getBigDecimal(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public InputStream getBinaryStream(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public InputStream getBinaryStream(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Blob getBlob(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Blob getBlob(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public boolean getBoolean(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean getBoolean(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public byte getByte(int columnIndex) throws SQLException{
        try{
            columnIndex--;
            if(reader.IsDBNull(columnIndex)){
                wasNull = true;
                return 0;
            }
            return reader.GetByte(columnIndex);
        }catch(Exception ex){
            throw new SQLException(ex);
        }
    }


    public byte getByte(String columnLabel) throws SQLException{
        return getByte(findColumn(columnLabel));
    }


    public byte[] getBytes(int columnIndex) throws SQLException{
        try{
            columnIndex--;
            if(reader.IsDBNull(columnIndex)){
                wasNull = true;
                return null;
            }
            int count = (int)reader.GetBytes(columnIndex, 0, null, 0, 0);
            byte[] buffer = new byte[count];
            reader.GetBytes(columnIndex, 0, buffer, 0, buffer.length);
            return buffer;
        }catch(Exception ex){
            throw new SQLException(ex);
        }
    }


    public byte[] getBytes(String columnLabel) throws SQLException{
        return getBytes(findColumn(columnLabel));
    }


    public Reader getCharacterStream(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Reader getCharacterStream(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Clob getClob(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Clob getClob(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public int getConcurrency() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public String getCursorName() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Date getDate(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Date getDate(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Date getDate(int columnIndex, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Date getDate(String columnLabel, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public double getDouble(int columnIndex) throws SQLException{
        try{
            columnIndex--;
            if(reader.IsDBNull(columnIndex)){
                wasNull = true;
                return 0;
            }
            return reader.GetDouble(columnIndex);
        }catch(Exception ex){
            throw new SQLException(ex);
        }
    }


    public double getDouble(String columnLabel) throws SQLException{
        return getDouble(findColumn(columnLabel));
    }


    public int getFetchDirection() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getFetchSize() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public float getFloat(int columnIndex) throws SQLException{
        try{
            columnIndex--;
            if(reader.IsDBNull(columnIndex)){
                wasNull = true;
                return 0;
            }
            return reader.GetFloat(columnIndex);
        }catch(Exception ex){
            throw new SQLException(ex);
        }
    }


    public float getFloat(String columnLabel) throws SQLException{
        return getFloat(findColumn(columnLabel));
    }


    public int getHoldability() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getInt(int columnIndex) throws SQLException{
        try{
            columnIndex--;
            if(reader.IsDBNull(columnIndex)){
                wasNull = true;
                return 0;
            }
            return reader.GetInt32(columnIndex);
        }catch(Exception ex){
            throw new SQLException(ex);
        }
    }


    public int getInt(String columnLabel) throws SQLException{
        return getInt(findColumn(columnLabel));
    }


    public long getLong(int columnIndex) throws SQLException{
        try{
            columnIndex--;
            if(reader.IsDBNull(columnIndex)){
                wasNull = true;
                return 0;
            }
            return reader.GetInt64(columnIndex);
        }catch(Exception ex){
            throw new SQLException(ex);
        }
    }


    public long getLong(String columnLabel) throws SQLException{
        return getLong(findColumn(columnLabel));
    }


    public ResultSetMetaData getMetaData(){
        return new JdbcOdbcResultSetMetaData(reader, reader.GetSchemaTable());
    }


    public Reader getNCharacterStream(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Reader getNCharacterStream(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public NClob getNClob(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public NClob getNClob(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getNString(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getNString(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Object getObject(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Object getObject(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Object getObject(int columnIndex, Map<String, Class<?>> map) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Object getObject(String columnLabel, Map<String, Class<?>> map) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Ref getRef(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Ref getRef(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public int getRow() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public RowId getRowId(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public RowId getRowId(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public SQLXML getSQLXML(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public SQLXML getSQLXML(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public short getShort(int columnIndex) throws SQLException{
        try{
            columnIndex--;
            if(reader.IsDBNull(columnIndex)){
                wasNull = true;
                return 0;
            }
            return reader.GetInt16(columnIndex);
        }catch(Exception ex){
            throw new SQLException(ex);
        }
    }


    public short getShort(String columnLabel) throws SQLException{
        return getShort(findColumn(columnLabel));
    }


    public Statement getStatement(){
        return statement;
    }


    public String getString(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getString(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Time getTime(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Time getTime(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Time getTime(int columnIndex, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Time getTime(String columnLabel, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Timestamp getTimestamp(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Timestamp getTimestamp(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Timestamp getTimestamp(int columnIndex, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Timestamp getTimestamp(String columnLabel, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public int getType() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public URL getURL(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public URL getURL(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public InputStream getUnicodeStream(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public InputStream getUnicodeStream(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public SQLWarning getWarnings() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public void insertRow() throws SQLException{
        // TODO Auto-generated method stub

    }


    public boolean isAfterLast() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean isBeforeFirst() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean isClosed() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean isFirst() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean isLast() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean last() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public void moveToCurrentRow() throws SQLException{
        // TODO Auto-generated method stub

    }


    public void moveToInsertRow() throws SQLException{
        // TODO Auto-generated method stub

    }


    public boolean next() throws SQLException{
        return reader.Read();
    }


    public boolean previous() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public void refreshRow() throws SQLException{
        // TODO Auto-generated method stub

    }


    public boolean relative(int rows) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean rowDeleted() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean rowInserted() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean rowUpdated() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public void setFetchDirection(int direction) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setFetchSize(int rows) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateArray(int columnIndex, Array x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateArray(String columnLabel, Array x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateAsciiStream(int columnIndex, InputStream x, int length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateAsciiStream(String columnLabel, InputStream x, int length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateAsciiStream(int columnIndex, InputStream x, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateAsciiStream(String columnLabel, InputStream x, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateAsciiStream(int columnIndex, InputStream x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateAsciiStream(String columnLabel, InputStream x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBigDecimal(int columnIndex, BigDecimal x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBigDecimal(String columnLabel, BigDecimal x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBinaryStream(int columnIndex, InputStream x, int length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBinaryStream(String columnLabel, InputStream x, int length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBinaryStream(int columnIndex, InputStream x, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBinaryStream(String columnLabel, InputStream x, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBinaryStream(int columnIndex, InputStream x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBinaryStream(String columnLabel, InputStream x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBlob(int columnIndex, Blob x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBlob(String columnLabel, Blob x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBlob(int columnIndex, InputStream inputStream, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBlob(String columnLabel, InputStream inputStream, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBlob(int columnIndex, InputStream inputStream) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBlob(String columnLabel, InputStream inputStream) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBoolean(int columnIndex, boolean x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBoolean(String columnLabel, boolean x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateByte(int columnIndex, byte x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateByte(String columnLabel, byte x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBytes(int columnIndex, byte[] x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateBytes(String columnLabel, byte[] x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateCharacterStream(int columnIndex, Reader x, int length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateCharacterStream(String columnLabel, Reader reader, int length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateCharacterStream(int columnIndex, Reader x, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateCharacterStream(String columnLabel, Reader reader, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateCharacterStream(int columnIndex, Reader x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateCharacterStream(String columnLabel, Reader reader) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateClob(int columnIndex, Clob x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateClob(String columnLabel, Clob x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateClob(int columnIndex, Reader reader, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateClob(String columnLabel, Reader reader, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateClob(int columnIndex, Reader reader) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateClob(String columnLabel, Reader reader) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateDate(int columnIndex, Date x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateDate(String columnLabel, Date x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateDouble(int columnIndex, double x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateDouble(String columnLabel, double x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateFloat(int columnIndex, float x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateFloat(String columnLabel, float x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateInt(int columnIndex, int x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateInt(String columnLabel, int x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateLong(int columnIndex, long x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateLong(String columnLabel, long x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNCharacterStream(int columnIndex, Reader x, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNCharacterStream(String columnLabel, Reader reader, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNCharacterStream(int columnIndex, Reader x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNCharacterStream(String columnLabel, Reader reader) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNClob(int columnIndex, NClob clob) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNClob(String columnLabel, NClob clob) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNClob(int columnIndex, Reader reader, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNClob(String columnLabel, Reader reader, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNClob(int columnIndex, Reader reader) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNClob(String columnLabel, Reader reader) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNString(int columnIndex, String string) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNString(String columnLabel, String string) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNull(int columnIndex) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateNull(String columnLabel) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateObject(int columnIndex, Object x, int scaleOrLength) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateObject(int columnIndex, Object x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateObject(String columnLabel, Object x, int scaleOrLength) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateObject(String columnLabel, Object x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateRef(int columnIndex, Ref x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateRef(String columnLabel, Ref x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateRow() throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateRowId(int columnIndex, RowId x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateRowId(String columnLabel, RowId x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateSQLXML(int columnIndex, SQLXML xmlObject) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateSQLXML(String columnLabel, SQLXML xmlObject) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateShort(int columnIndex, short x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateShort(String columnLabel, short x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateString(int columnIndex, String x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateString(String columnLabel, String x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateTime(int columnIndex, Time x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateTime(String columnLabel, Time x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateTimestamp(int columnIndex, Timestamp x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void updateTimestamp(String columnLabel, Timestamp x) throws SQLException{
        // TODO Auto-generated method stub

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

}
