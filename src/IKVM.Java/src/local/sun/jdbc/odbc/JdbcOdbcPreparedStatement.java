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
import java.sql.Array;
import java.sql.Blob;
import java.sql.Clob;
import java.sql.Date;
import java.sql.NClob;
import java.sql.ParameterMetaData;
import java.sql.PreparedStatement;
import java.sql.Ref;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.RowId;
import java.sql.SQLException;
import java.sql.SQLXML;
import java.sql.Time;
import java.sql.Timestamp;
import java.sql.Types;
import java.util.Calendar;

import cli.System.Data.*;
import cli.System.Data.Common.*;
import cli.System.Data.Odbc.*;

/**
 * @author Volker Berlin
 */
public class JdbcOdbcPreparedStatement extends JdbcOdbcStatement implements PreparedStatement{

    public JdbcOdbcPreparedStatement(JdbcOdbcConnection jdbcConn, OdbcCommand command, String sql, int resultSetType, int resultSetConcurrency){
        super(jdbcConn, command, resultSetType, resultSetConcurrency);
        command.set_CommandText(sql);
        command.Prepare();
    }


    public void addBatch() throws SQLException{
        // TODO Auto-generated method stub

    }


    public void clearParameters(){
        DbParameterCollection params = command.get_Parameters();
        params.Clear();
    }


    public boolean execute() throws SQLException{
        return super.execute(null);
    }


    public ResultSet executeQuery() throws SQLException{
        return super.executeQuery(null);
    }


    public int executeUpdate() throws SQLException{
        return super.executeUpdate(null);
    }


    public ResultSetMetaData getMetaData() throws SQLException{
        ResultSet rs = getResultSet();
        if(rs != null){
            rs.getMetaData();
        }
        DbDataReader reader = command.ExecuteReader(CommandBehavior.wrap(CommandBehavior.SchemaOnly));
        JdbcOdbcResultSetMetaData metadata = new JdbcOdbcResultSetMetaData(reader);
        reader.Close();
        return metadata;
    }


    public ParameterMetaData getParameterMetaData(){
        throw new UnsupportedOperationException();
    }


    public void setArray(int parameterIndex, Array x) throws SQLException{
        setObject(parameterIndex, x, Types.ARRAY);
    }


    public void setAsciiStream(int parameterIndex, InputStream x) throws SQLException{
        setObject(parameterIndex, x, Types.LONGVARCHAR);
    }


    public void setAsciiStream(int parameterIndex, InputStream x, int length) throws SQLException{
        setObject(parameterIndex, x, Types.LONGVARCHAR, length);
    }


    public void setAsciiStream(int parameterIndex, InputStream x, long length) throws SQLException{
        setObject(parameterIndex, x, Types.LONGVARCHAR, (int)length);
    }


    public void setBigDecimal(int parameterIndex, BigDecimal x) throws SQLException{
        setObject(parameterIndex, x, Types.DECIMAL);
    }


    public void setBinaryStream(int parameterIndex, InputStream x) throws SQLException{
        setObject(parameterIndex, x, Types.LONGVARBINARY);
    }


    public void setBinaryStream(int parameterIndex, InputStream x, int length) throws SQLException{
        setObject(parameterIndex, x, Types.LONGVARBINARY, length);
    }


    public void setBinaryStream(int parameterIndex, InputStream x, long length) throws SQLException{
        setObject(parameterIndex, x, Types.LONGVARBINARY, (int)length);
    }


    public void setBlob(int parameterIndex, Blob x) throws SQLException{
        setObject(parameterIndex, x, Types.BLOB);
    }


    public void setBlob(int parameterIndex, InputStream x) throws SQLException{
        setObject(parameterIndex, x, Types.BLOB);
    }


    public void setBlob(int parameterIndex, InputStream x, long length) throws SQLException{
        setObject(parameterIndex, x, Types.BLOB, (int)length);
    }


    public void setBoolean(int parameterIndex, boolean x) throws SQLException{
        setObject(parameterIndex, Boolean.valueOf(x), Types.BOOLEAN);
    }


    public void setByte(int parameterIndex, byte x) throws SQLException{
        setObject(parameterIndex, Byte.valueOf(x), Types.TINYINT);
    }


    public void setBytes(int parameterIndex, byte[] x) throws SQLException{
        setObject(parameterIndex, x, Types.BINARY);
    }


    public void setCharacterStream(int parameterIndex, Reader x) throws SQLException{
        setObject(parameterIndex, x, Types.LONGVARCHAR);
    }


    public void setCharacterStream(int parameterIndex, Reader x, int length) throws SQLException{
        setObject(parameterIndex, x, Types.NCLOB, length);
    }


    public void setCharacterStream(int parameterIndex, Reader x, long length) throws SQLException{
        setObject(parameterIndex, x, Types.LONGVARCHAR, (int)length);
    }


    public void setClob(int parameterIndex, Clob x) throws SQLException{
        setObject(parameterIndex, x, Types.CLOB);
    }


    public void setClob(int parameterIndex, Reader x) throws SQLException{
        setObject(parameterIndex, x, Types.CLOB);
    }


    public void setClob(int parameterIndex, Reader x, long length) throws SQLException{
        setObject(parameterIndex, x, Types.CLOB, (int)length);
    }


    public void setDate(int parameterIndex, Date x) throws SQLException{
        setObject(parameterIndex, x, Types.DATE);
    }


    public void setDate(int parameterIndex, Date x, Calendar cal) throws SQLException{
        JdbcOdbcUtils.convertCalendarToLocalDate(x, cal);
        setObject(parameterIndex, x, Types.DATE);
    }


    public void setDouble(int parameterIndex, double x) throws SQLException{
        setObject(parameterIndex, Double.valueOf(x), Types.DOUBLE);
    }


    public void setFloat(int parameterIndex, float x) throws SQLException{
        setObject(parameterIndex, Float.valueOf(x), Types.FLOAT);
    }


    public void setInt(int parameterIndex, int x) throws SQLException{
        setObject(parameterIndex, Integer.valueOf(x), Types.INTEGER);
    }


    public void setLong(int parameterIndex, long x) throws SQLException{
        setObject(parameterIndex, Long.valueOf(x), Types.BIGINT);
    }


    public void setNCharacterStream(int parameterIndex, Reader x) throws SQLException{
        setObject(parameterIndex, x, Types.LONGNVARCHAR);
    }


    public void setNCharacterStream(int parameterIndex, Reader x, long length) throws SQLException{
        setObject(parameterIndex, x, Types.LONGNVARCHAR, (int)length);
    }


    public void setNClob(int parameterIndex, NClob x) throws SQLException{
        setObject(parameterIndex, x, Types.NCLOB);
    }


    public void setNClob(int parameterIndex, Reader x) throws SQLException{
        setObject(parameterIndex, x, Types.NCLOB);
    }


    public void setNClob(int parameterIndex, Reader x, long length) throws SQLException{
        setObject(parameterIndex, x, Types.NCLOB, (int)length);
    }


    public void setNString(int parameterIndex, String x) throws SQLException{
        setObject(parameterIndex, x, Types.NVARCHAR);
    }


    public void setNull(int parameterIndex, int sqlType) throws SQLException{
        setObject(parameterIndex, null, sqlType);
    }


    public void setNull(int parameterIndex, int sqlType, String typeName) throws SQLException{
        setObject(parameterIndex, null, sqlType);
    }


    public void setObject(int parameterIndex, Object x) throws SQLException{
        setObject(parameterIndex, x, Types.OTHER, -1);
    }


    public void setObject(int parameterIndex, Object x, int targetSqlType) throws SQLException{
        setObject(parameterIndex, x, targetSqlType, -1);
    }


    public void setObject(int parameterIndex, Object x, int targetSqlType, int scaleOrLength) throws SQLException{
        DbParameter para = getPara(parameterIndex);
        para.set_Value(JdbcOdbcUtils.convertJava2Net(x, scaleOrLength));
        if(para.get_Direction().Value == ParameterDirection.Output){
            para.set_Direction(ParameterDirection.wrap(ParameterDirection.InputOutput));
        }
        
        if(targetSqlType != Types.OTHER){
            para.set_DbType(DbType.wrap(JdbcOdbcUtils.convertJdbc2AdoNetType(targetSqlType)));
        }
        
        if(scaleOrLength >= 0){
            switch(targetSqlType){
                case Types.DECIMAL:
                case Types.NUMERIC:
                    para.set_Scale((byte)scaleOrLength);
            }
        }
    }


    public void setRef(int parameterIndex, Ref x) throws SQLException{
        setObject(parameterIndex, x, Types.REF);
    }


    public void setRowId(int parameterIndex, RowId x) throws SQLException{
        setObject(parameterIndex, x, Types.ROWID);
    }


    public void setSQLXML(int parameterIndex, SQLXML x) throws SQLException{
        setObject(parameterIndex, x, Types.SQLXML);
    }


    public void setShort(int parameterIndex, short x) throws SQLException{
        setObject(parameterIndex, Short.valueOf(x), Types.SMALLINT);
    }


    public void setString(int parameterIndex, String x) throws SQLException{
        setObject(parameterIndex, x, Types.VARCHAR);
    }


    public void setTime(int parameterIndex, Time x) throws SQLException{
        setObject(parameterIndex, x, Types.TIME);
    }


    public void setTime(int parameterIndex, Time x, Calendar cal) throws SQLException{
        JdbcOdbcUtils.convertCalendarToLocalDate(x, cal);
        setObject(parameterIndex, x, Types.TIME);
    }


    public void setTimestamp(int parameterIndex, Timestamp x) throws SQLException{
        setObject(parameterIndex, x, Types.TIMESTAMP);
    }


    public void setTimestamp(int parameterIndex, Timestamp x, Calendar cal) throws SQLException{
        JdbcOdbcUtils.convertCalendarToLocalDate(x, cal);
        setObject(parameterIndex, x, Types.TIMESTAMP);
    }


    public void setURL(int parameterIndex, URL x) throws SQLException{
        setObject(parameterIndex, x, Types.DATALINK);
    }


    public void setUnicodeStream(int parameterIndex, InputStream x, int length) throws SQLException{
        setObject(parameterIndex, x, Types.LONGNVARCHAR, length);
    }


    /**
     * Get the DbParameter from the current command. If the parameter does not exits in the collection then add it.
     * 
     * @param parameterIndex
     *            The JDBC parameter index starting with 1
     * @return the DbParameter for the index.
     * @throws SQLException
     *             If any error occur.
     */
    protected DbParameter getPara(int parameterIndex) throws SQLException{
        try{
            DbParameterCollection params = command.get_Parameters();
            while(params.get_Count() < parameterIndex){
                params.Add(command.CreateParameter());
            }
            return params.get_Item(parameterIndex - 1);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }
}
