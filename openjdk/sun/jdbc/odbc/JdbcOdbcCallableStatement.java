/*
  Copyright (C) 2009, 2011 Volker Berlin (i-net software)

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

import cli.System.Data.*;
import cli.System.Data.Common.*;
import cli.System.Data.Odbc.*;



/**
 * @author Volker Berlin
 */
public class JdbcOdbcCallableStatement extends JdbcOdbcPreparedStatement implements CallableStatement{

    private final Parameters parameters = new Parameters();
    
    public JdbcOdbcCallableStatement(JdbcOdbcConnection jdbcConn, OdbcCommand command, String sql, int resultSetType, int resultSetConcurrency){
        super(jdbcConn, command, sql, resultSetType, resultSetConcurrency);
    }


    public final Array getArray(int parameterIndex){
        return parameters.getArray(parameterIndex);
    }


    public final Array getArray(String parameterName) throws SQLException{
        return parameters.getArray(parameterName);
    }


    public final BigDecimal getBigDecimal(int parameterIndex, int scale) throws SQLException{
        return parameters.getBigDecimal(parameterIndex, scale);
    }


    public final BigDecimal getBigDecimal(int parameterIndex) throws SQLException{
        return parameters.getBigDecimal(parameterIndex);
    }


    public final BigDecimal getBigDecimal(String parameterName) throws SQLException{
        return parameters.getBigDecimal(parameterName);
    }


    public final Blob getBlob(int parameterIndex){
        return parameters.getBlob(parameterIndex);
    }


    public final Blob getBlob(String parameterName) throws SQLException{
        return parameters.getBlob(parameterName);
    }


    public final boolean getBoolean(int parameterIndex) throws SQLException{
        return parameters.getBoolean(parameterIndex);
    }


    public final boolean getBoolean(String parameterName) throws SQLException{
        return parameters.getBoolean(parameterName);
    }


    public final byte getByte(int parameterIndex) throws SQLException{
        return parameters.getByte(parameterIndex);
    }


    public final byte getByte(String parameterName) throws SQLException{
        return parameters.getByte(parameterName);
    }


    public final byte[] getBytes(int parameterIndex) throws SQLException{
        return parameters.getBytes(parameterIndex);
    }


    public final byte[] getBytes(String parameterName) throws SQLException{
        return parameters.getBytes(parameterName);
    }


    public final Reader getCharacterStream(int parameterIndex) throws SQLException{
        return parameters.getCharacterStream(parameterIndex);
    }


    public final Reader getCharacterStream(String parameterName) throws SQLException{
        return parameters.getCharacterStream(parameterName);
    }


    public final Clob getClob(int parameterIndex){
        return parameters.getClob(parameterIndex);
    }


    public final Clob getClob(String parameterName) throws SQLException{
        return parameters.getClob(parameterName);
    }


    public final Date getDate(int parameterIndex, Calendar cal) throws SQLException{
        return parameters.getDate(parameterIndex, cal);
    }


    public final Date getDate(int parameterIndex) throws SQLException{
        return parameters.getDate(parameterIndex);
    }


    public final Date getDate(String parameterName, Calendar cal) throws SQLException{
        return parameters.getDate(parameterName, cal);
    }


    public final Date getDate(String parameterName) throws SQLException{
        return parameters.getDate(parameterName);
    }


    public final double getDouble(int parameterIndex) throws SQLException{
        return parameters.getDouble(parameterIndex);
    }


    public final double getDouble(String parameterName) throws SQLException{
        return parameters.getDouble(parameterName);
    }


    public final float getFloat(int parameterIndex) throws SQLException{
        return parameters.getFloat(parameterIndex);
    }


    public final float getFloat(String parameterName) throws SQLException{
        return parameters.getFloat(parameterName);
    }


    public final int getInt(int parameterIndex) throws SQLException{
        return parameters.getInt(parameterIndex);
    }


    public final int getInt(String parameterName) throws SQLException{
        return parameters.getInt(parameterName);
    }


    public final long getLong(int parameterIndex) throws SQLException{
        return parameters.getLong(parameterIndex);
    }


    public final long getLong(String parameterName) throws SQLException{
        return parameters.getLong(parameterName);
    }


    public final Reader getNCharacterStream(int parameterIndex) throws SQLException{
        return parameters.getNCharacterStream(parameterIndex);
    }


    public final Reader getNCharacterStream(String parameterName) throws SQLException{
        return parameters.getNCharacterStream(parameterName);
    }


    public final NClob getNClob(int parameterIndex){
        return parameters.getNClob(parameterIndex);
    }


    public final NClob getNClob(String parameterName) throws SQLException{
        return parameters.getNClob(parameterName);
    }


    public final String getNString(int parameterIndex) throws SQLException{
        return parameters.getNString(parameterIndex);
    }


    public final String getNString(String parameterName) throws SQLException{
        return parameters.getNString(parameterName);
    }


    public final Object getObject(int parameterIndex, Map<String, Class<?>> map){
        return parameters.getObject(parameterIndex, map);
    }


    public final Object getObject(int parameterIndex) throws SQLException{
        return parameters.getObject(parameterIndex);
    }


    public final Object getObject(String parameterName, Map<String, Class<?>> map) throws SQLException{
        return parameters.getObject(parameterName, map);
    }


    public final Object getObject(String parameterName) throws SQLException{
        return parameters.getObject(parameterName);
    }


    public final Ref getRef(int parameterIndex){
        return parameters.getRef(parameterIndex);
    }


    public final Ref getRef(String parameterName) throws SQLException{
        return parameters.getRef(parameterName);
    }


    public final RowId getRowId(int parameterIndex){
        return parameters.getRowId(parameterIndex);
    }


    public final RowId getRowId(String parameterName) throws SQLException{
        return parameters.getRowId(parameterName);
    }


    public final short getShort(int parameterIndex) throws SQLException{
        return parameters.getShort(parameterIndex);
    }


    public final short getShort(String parameterName) throws SQLException{
        return parameters.getShort(parameterName);
    }


    public final SQLXML getSQLXML(int parameterIndex){
        return parameters.getSQLXML(parameterIndex);
    }


    public final SQLXML getSQLXML(String parameterName) throws SQLException{
        return parameters.getSQLXML(parameterName);
    }


    public final String getString(int parameterIndex) throws SQLException{
        return parameters.getString(parameterIndex);
    }


    public final String getString(String parameterName) throws SQLException{
        return parameters.getString(parameterName);
    }


    public final Time getTime(int parameterIndex, Calendar cal) throws SQLException{
        return parameters.getTime(parameterIndex, cal);
    }


    public final Time getTime(int parameterIndex) throws SQLException{
        return parameters.getTime(parameterIndex);
    }


    public final Time getTime(String parameterName, Calendar cal) throws SQLException{
        return parameters.getTime(parameterName, cal);
    }


    public final Time getTime(String parameterName) throws SQLException{
        return parameters.getTime(parameterName);
    }


    public final Timestamp getTimestamp(int parameterIndex, Calendar cal) throws SQLException{
        return parameters.getTimestamp(parameterIndex, cal);
    }


    public final Timestamp getTimestamp(int parameterIndex) throws SQLException{
        return parameters.getTimestamp(parameterIndex);
    }


    public final Timestamp getTimestamp(String parameterName, Calendar cal) throws SQLException{
        return parameters.getTimestamp(parameterName, cal);
    }


    public final Timestamp getTimestamp(String parameterName) throws SQLException{
        return parameters.getTimestamp(parameterName);
    }


    public final URL getURL(int parameterIndex) throws SQLException{
        return parameters.getURL(parameterIndex);
    }


    public final URL getURL(String parameterName) throws SQLException{
        return parameters.getURL(parameterName);
    }


    public final boolean wasNull(){
        return parameters.wasNull();
    }


    public void registerOutParameter(int parameterIndex, int sqlType) throws SQLException{
        registerOutParameter(parameterIndex, sqlType, -1);
    }


    public void registerOutParameter(String parameterName, int sqlType) throws SQLException{
        registerOutParameter(parameters.findColumn(parameterName), sqlType);
    }


    public void registerOutParameter(int parameterIndex, int sqlType, int scaleOrLength) throws SQLException{
        DbParameter para = getPara(parameterIndex);
        int direction = para.get_Value() == null ? ParameterDirection.Output : ParameterDirection.InputOutput;
        para.set_Direction(ParameterDirection.wrap(direction));

        if(sqlType != Types.OTHER){
            para.set_DbType(DbType.wrap(JdbcOdbcUtils.convertJdbc2AdoNetType(sqlType)));
        }
        
        if(scaleOrLength >= 0){
            switch(sqlType){
                case Types.DECIMAL:
                case Types.NUMERIC:
                    para.set_Scale((byte)scaleOrLength);
            }
        }
    }


    public void registerOutParameter(int parameterIndex, int sqlType, String typeName){
        throw new UnsupportedOperationException();
    }


    public void registerOutParameter(String parameterName, int sqlType, int scale) throws SQLException{
        registerOutParameter(parameters.findColumn(parameterName), sqlType, scale);
    }


    public void registerOutParameter(String parameterName, int sqlType, String typeName) throws SQLException{
        registerOutParameter(parameters.findColumn(parameterName), sqlType, typeName);
    }


    public void setAsciiStream(String parameterName, InputStream x) throws SQLException{
        setAsciiStream(parameters.findColumn(parameterName), x);
    }


    public void setAsciiStream(String parameterName, InputStream x, int length) throws SQLException{
        setAsciiStream(parameters.findColumn(parameterName), x, length);
    }


    public void setAsciiStream(String parameterName, InputStream x, long length) throws SQLException{
        setAsciiStream(parameters.findColumn(parameterName), x, length);
    }


    public void setBigDecimal(String parameterName, BigDecimal x) throws SQLException{
        setBigDecimal(parameters.findColumn(parameterName), x);
    }


    public void setBinaryStream(String parameterName, InputStream x) throws SQLException{
        setBinaryStream(parameters.findColumn(parameterName), x);
    }


    public void setBinaryStream(String parameterName, InputStream x, int length) throws SQLException{
        setBinaryStream(parameters.findColumn(parameterName), x, length);
    }


    public void setBinaryStream(String parameterName, InputStream x, long length) throws SQLException{
        setBinaryStream(parameters.findColumn(parameterName), x, length);
    }


    public void setBlob(String parameterName, Blob x) throws SQLException{
        setBlob(parameters.findColumn(parameterName), x);
    }


    public void setBlob(String parameterName, InputStream x) throws SQLException{
        setBlob(parameters.findColumn(parameterName), x);
    }


    public void setBlob(String parameterName, InputStream x, long length) throws SQLException{
        setBlob(parameters.findColumn(parameterName), x, length);
    }


    public void setBoolean(String parameterName, boolean x) throws SQLException{
        setBoolean(parameters.findColumn(parameterName), x);
    }


    public void setByte(String parameterName, byte x) throws SQLException{
        setByte(parameters.findColumn(parameterName), x);
    }


    public void setBytes(String parameterName, byte[] x) throws SQLException{
        setBytes(parameters.findColumn(parameterName), x);
    }


    public void setCharacterStream(String parameterName, Reader x) throws SQLException{
        setCharacterStream(parameters.findColumn(parameterName), x);
    }


    public void setCharacterStream(String parameterName, Reader x, int length) throws SQLException{
        setCharacterStream(parameters.findColumn(parameterName), x, length);
    }


    public void setCharacterStream(String parameterName, Reader x, long length) throws SQLException{
        setCharacterStream(parameters.findColumn(parameterName), x, length);
    }


    public void setClob(String parameterName, Clob x) throws SQLException{
        setClob(parameters.findColumn(parameterName), x);
    }


    public void setClob(String parameterName, Reader x) throws SQLException{
        setClob(parameters.findColumn(parameterName), x);
    }


    public void setClob(String parameterName, Reader x, long length) throws SQLException{
        setClob(parameters.findColumn(parameterName), x, length);
    }


    public void setDate(String parameterName, Date x) throws SQLException{
        setDate(parameters.findColumn(parameterName), x);
    }


    public void setDate(String parameterName, Date x, Calendar cal) throws SQLException{
        setDate(parameters.findColumn(parameterName), x, cal);
    }


    public void setDouble(String parameterName, double x) throws SQLException{
        setDouble(parameters.findColumn(parameterName), x);
    }


    public void setFloat(String parameterName, float x) throws SQLException{
        setFloat(parameters.findColumn(parameterName), x);
    }


    public void setInt(String parameterName, int x) throws SQLException{
        setInt(parameters.findColumn(parameterName), x);
    }


    public void setLong(String parameterName, long x) throws SQLException{
        setLong(parameters.findColumn(parameterName), x);
    }


    public void setNCharacterStream(String parameterName, Reader x) throws SQLException{
        setNCharacterStream(parameters.findColumn(parameterName), x);
    }


    public void setNCharacterStream(String parameterName, Reader x, long length) throws SQLException{
        setNCharacterStream(parameters.findColumn(parameterName), x, length);
    }


    public void setNClob(String parameterName, NClob x) throws SQLException{
        setNClob(parameters.findColumn(parameterName), x);
    }


    public void setNClob(String parameterName, Reader x) throws SQLException{
        setNClob(parameters.findColumn(parameterName), x);
    }


    public void setNClob(String parameterName, Reader x, long length) throws SQLException{
        setNClob(parameters.findColumn(parameterName), x, length);
    }


    public void setNString(String parameterName, String x) throws SQLException{
        setNString(parameters.findColumn(parameterName), x);
    }


    public void setNull(String parameterName, int sqlType) throws SQLException{
        setNull(parameters.findColumn(parameterName), sqlType);
    }


    public void setNull(String parameterName, int sqlType, String typeName) throws SQLException{
        setNull(parameters.findColumn(parameterName), sqlType, typeName);
    }


    public void setObject(String parameterName, Object x) throws SQLException{
        setObject(parameters.findColumn(parameterName), x);
    }


    public void setObject(String parameterName, Object x, int targetSqlType) throws SQLException{
        setObject(parameters.findColumn(parameterName), x, targetSqlType);
    }


    public void setObject(String parameterName, Object x, int targetSqlType, int scaleOrLength) throws SQLException{
        setObject(parameters.findColumn(parameterName), x, targetSqlType, scaleOrLength);
    }


    public void setRowId(String parameterName, RowId x) throws SQLException{
        setRowId(parameters.findColumn(parameterName), x);
    }


    public void setSQLXML(String parameterName, SQLXML x) throws SQLException{
        setSQLXML(parameters.findColumn(parameterName), x);
    }


    public void setShort(String parameterName, short x) throws SQLException{
        setShort(parameters.findColumn(parameterName), x);
    }


    public void setString(String parameterName, String x) throws SQLException{
        setString(parameters.findColumn(parameterName), x);
    }


    public void setTime(String parameterName, Time x) throws SQLException{
        setTime(parameters.findColumn(parameterName), x);
    }


    public void setTime(String parameterName, Time x, Calendar cal) throws SQLException{
        setTime(parameters.findColumn(parameterName), x, cal);
    }


    public void setTimestamp(String parameterName, Timestamp x) throws SQLException{
        setTimestamp(parameters.findColumn(parameterName), x);
    }


    public void setTimestamp(String parameterName, Timestamp x, Calendar cal) throws SQLException{
        setTimestamp(parameters.findColumn(parameterName), x, cal);
    }


    public void setURL(String parameterName, URL x) throws SQLException{
        setURL(parameters.findColumn(parameterName), x);
    }

    private class Parameters extends JdbcOdbcObject{

        @Override
        public int findColumn(String parameterName) throws SQLException{
            try{
                DbParameterCollection params = command.get_Parameters();
                for(int i=0; i<params.get_Count();i++){
                    DbParameter param = params.get_Item(i);
                    if(parameterName.equalsIgnoreCase(param.get_ParameterName())){
                        return i+1;
                    }
                }
                throw new SQLException( "Parameter '"+parameterName+"' not found.", "S0022");
            }catch(Throwable th){
                throw JdbcOdbcUtils.createSQLException(th);
            }
        }

        @Override
        protected Object getObjectImpl(int parameterIndex) throws SQLException{
            return getPara(parameterIndex).get_Value();
        }
        
    }


    /**
     * {@inheritDoc}
     */
	public <T> T getObject(int parameterIndex, Class<T> type)
			throws SQLException {
		throw new SQLFeatureNotSupportedException();
	}


    /**
     * {@inheritDoc}
     */
	public <T> T getObject(String parameterName, Class<T> type)
			throws SQLException {
		throw new SQLFeatureNotSupportedException();
	}
}
