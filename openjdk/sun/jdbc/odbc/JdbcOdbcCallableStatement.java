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

import cli.System.Data.Common.*;



/**
 * @author Volker Berlin
 */
public class JdbcOdbcCallableStatement extends JdbcOdbcPreparedStatement implements CallableStatement{

    public JdbcOdbcCallableStatement(JdbcOdbcConnection jdbcConn, DbCommand command, String sql){
        super(jdbcConn, command, sql);
        // TODO Auto-generated constructor stub
    }


    public Array getArray(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Array getArray(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public BigDecimal getBigDecimal(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public BigDecimal getBigDecimal(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public BigDecimal getBigDecimal(int parameterIndex, int scale) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Blob getBlob(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Blob getBlob(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public boolean getBoolean(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean getBoolean(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public byte getByte(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public byte getByte(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public byte[] getBytes(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public byte[] getBytes(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Reader getCharacterStream(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Reader getCharacterStream(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Clob getClob(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Clob getClob(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Date getDate(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Date getDate(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Date getDate(int parameterIndex, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Date getDate(String parameterName, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public double getDouble(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public double getDouble(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public float getFloat(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public float getFloat(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getInt(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getInt(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public long getLong(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public long getLong(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public Reader getNCharacterStream(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Reader getNCharacterStream(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public NClob getNClob(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public NClob getNClob(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getNString(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getNString(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Object getObject(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Object getObject(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Object getObject(int parameterIndex, Map<String, Class<?>> map) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Object getObject(String parameterName, Map<String, Class<?>> map) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Ref getRef(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Ref getRef(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public RowId getRowId(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public RowId getRowId(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public SQLXML getSQLXML(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public SQLXML getSQLXML(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public short getShort(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public short getShort(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public String getString(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getString(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Time getTime(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Time getTime(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Time getTime(int parameterIndex, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Time getTime(String parameterName, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Timestamp getTimestamp(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Timestamp getTimestamp(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Timestamp getTimestamp(int parameterIndex, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Timestamp getTimestamp(String parameterName, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public URL getURL(int parameterIndex) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public URL getURL(String parameterName) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public void registerOutParameter(int parameterIndex, int sqlType) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void registerOutParameter(String parameterName, int sqlType) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void registerOutParameter(int parameterIndex, int sqlType, int scale) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void registerOutParameter(int parameterIndex, int sqlType, String typeName) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void registerOutParameter(String parameterName, int sqlType, int scale) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void registerOutParameter(String parameterName, int sqlType, String typeName) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setAsciiStream(String parameterName, InputStream x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setAsciiStream(String parameterName, InputStream x, int length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setAsciiStream(String parameterName, InputStream x, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setBigDecimal(String parameterName, BigDecimal x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setBinaryStream(String parameterName, InputStream x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setBinaryStream(String parameterName, InputStream x, int length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setBinaryStream(String parameterName, InputStream x, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setBlob(String parameterName, Blob x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setBlob(String parameterName, InputStream inputStream) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setBlob(String parameterName, InputStream inputStream, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setBoolean(String parameterName, boolean x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setByte(String parameterName, byte x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setBytes(String parameterName, byte[] x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setCharacterStream(String parameterName, Reader reader) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setCharacterStream(String parameterName, Reader reader, int length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setCharacterStream(String parameterName, Reader reader, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setClob(String parameterName, Clob x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setClob(String parameterName, Reader reader) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setClob(String parameterName, Reader reader, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setDate(String parameterName, Date x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setDate(String parameterName, Date x, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setDouble(String parameterName, double x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setFloat(String parameterName, float x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setInt(String parameterName, int x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setLong(String parameterName, long x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setNCharacterStream(String parameterName, Reader value) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setNCharacterStream(String parameterName, Reader value, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setNClob(String parameterName, NClob value) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setNClob(String parameterName, Reader reader) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setNClob(String parameterName, Reader reader, long length) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setNString(String parameterName, String value) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setNull(String parameterName, int sqlType) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setNull(String parameterName, int sqlType, String typeName) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setObject(String parameterName, Object x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setObject(String parameterName, Object x, int targetSqlType) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setObject(String parameterName, Object x, int targetSqlType, int scale) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setRowId(String parameterName, RowId x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setSQLXML(String parameterName, SQLXML xmlObject) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setShort(String parameterName, short x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setString(String parameterName, String x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setTime(String parameterName, Time x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setTime(String parameterName, Time x, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setTimestamp(String parameterName, Timestamp x) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setTimestamp(String parameterName, Timestamp x, Calendar cal) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setURL(String parameterName, URL val) throws SQLException{
        // TODO Auto-generated method stub

    }


    public boolean wasNull() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }

}
