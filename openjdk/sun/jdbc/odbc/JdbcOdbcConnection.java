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

import cli.System.Data.*;
import cli.System.Data.Common.*;
import cli.System.Data.Odbc.*;

import java.sql.*;
import java.util.Map;
import java.util.Properties;
import java.util.concurrent.Executor;

/**
 * This JDBC Driver is a wrapper to the ODBC.NET Data Provider
 */
public class JdbcOdbcConnection implements Connection{

    private final OdbcConnection netConn;

    private DbTransaction transaction;

    private int isolation = TRANSACTION_READ_COMMITTED;


    JdbcOdbcConnection(String connectString, Properties info) throws SQLException{
        try{
            boolean isDSN = connectString.indexOf('=') < 0;
            StringBuilder connStr = new StringBuilder();
            if(isDSN){
                connStr.append("DSN=");
            }
            connStr.append(connectString);

            String uid = info.getProperty("user");
            String pwd = info.getProperty("password");

            if(uid != null){
                connStr.append(";UID=").append(uid);
            }
            if(pwd != null){
                connStr.append(";PWD=").append(pwd);
            }

            netConn = new OdbcConnection(connStr.toString());

            netConn.Open();
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    @Override
	public void clearWarnings() throws SQLException{
        // TODO Auto-generated method stub

    }


    @Override
	public void close() throws SQLException{
        try{
            netConn.Close();
        }catch(Throwable ex){
            throw JdbcOdbcUtils.createSQLException(ex);
        }
    }


    @Override
	public Array createArrayOf(String typeName, Object[] elements){
        throw new UnsupportedOperationException();
    }


    @Override
	public Blob createBlob(){
        throw new UnsupportedOperationException();
    }


    @Override
	public Clob createClob(){
        throw new UnsupportedOperationException();
    }


    @Override
	public NClob createNClob(){
        throw new UnsupportedOperationException();
    }


    @Override
	public SQLXML createSQLXML(){
        throw new UnsupportedOperationException();
    }


    @Override
	public Statement createStatement() throws SQLException{
        return createStatement(ResultSet.TYPE_FORWARD_ONLY, ResultSet.CONCUR_READ_ONLY);
    }


    @Override
	public Statement createStatement(int resultSetType, int resultSetConcurrency) throws SQLException{
        try{
            return new JdbcOdbcStatement(this, netConn.CreateCommand(), resultSetType, resultSetConcurrency);
        }catch(Throwable ex){
            throw JdbcOdbcUtils.createSQLException(ex);
        }
    }


    @Override
	public Statement createStatement(int resultSetType, int resultSetConcurrency, int resultSetHoldability){
        throw new UnsupportedOperationException();
    }


    @Override
	public Struct createStruct(String typeName, Object[] attributes){
        throw new UnsupportedOperationException();
    }


    @Override
	public void setAutoCommit(boolean autoCommit) throws SQLException{
        try{
            if(autoCommit && transaction != null){
                return; // no change
            }
            if(!autoCommit && transaction == null){
                return; // no change
            }
            int level;
            switch(isolation){
                case TRANSACTION_READ_COMMITTED:
                    level = IsolationLevel.ReadUncommitted;
                    break;
                case TRANSACTION_READ_UNCOMMITTED:
                    level = IsolationLevel.ReadCommitted;
                    break;
                case TRANSACTION_REPEATABLE_READ:
                    level = IsolationLevel.RepeatableRead;
                    break;
                case TRANSACTION_SERIALIZABLE:
                    level = IsolationLevel.Serializable;
                    break;
                default:
                    level = IsolationLevel.ReadCommitted;
            }
            if(autoCommit){
                transaction = netConn.BeginTransaction(IsolationLevel.wrap(level));
            }else{
                transaction.Commit();
                transaction = null;
            }
        }catch(Throwable ex){
            throw JdbcOdbcUtils.createSQLException(ex);
        }
    }


    @Override
	public boolean getAutoCommit(){
        return transaction != null;
    }


    @Override
	public void commit() throws SQLException{
        try{
            if(transaction == null){
                // auto commit == true
                return;
            }
            transaction.Commit();
            transaction = netConn.BeginTransaction(transaction.get_IsolationLevel());
        }catch(Throwable ex){
            throw JdbcOdbcUtils.createSQLException(ex);
        }
    }


    @Override
	public void rollback() throws SQLException{
        try{
            if(transaction == null){
                // auto commit == true
                return;
            }
            transaction.Rollback();
            transaction = netConn.BeginTransaction(transaction.get_IsolationLevel());
        }catch(Throwable ex){
            throw JdbcOdbcUtils.createSQLException(ex);
        }
    }


    @Override
	public void setTransactionIsolation(int level){
        isolation = level;
    }


    @Override
	public int getTransactionIsolation(){
        return isolation;
    }


    @Override
	public String getClientInfo(String name){
        throw new UnsupportedOperationException();
    }


    @Override
	public Properties getClientInfo(){
        throw new UnsupportedOperationException();
    }


    @Override
	public int getHoldability(){
        throw new UnsupportedOperationException();
    }


    @Override
	public DatabaseMetaData getMetaData(){
        return new JdbcOdbcDatabaseMetaData(this, netConn);
    }


    @Override
	public Map<String, Class<?>> getTypeMap(){
        throw new UnsupportedOperationException();
    }


    @Override
	public SQLWarning getWarnings() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    @Override
	public boolean isClosed() throws SQLException{
        return netConn.get_State().Value == ConnectionState.Closed;
    }


    @Override
	public boolean isReadOnly() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    @Override
	public boolean isValid(int timeout) throws SQLException{
        throw new UnsupportedOperationException();
    }


    @Override
	public String nativeSQL(String sql) throws SQLException{
        // TODO Auto-generated method stub
        return sql;
    }


    @Override
	public CallableStatement prepareCall(String sql) throws SQLException{
        return prepareCall(sql, ResultSet.TYPE_FORWARD_ONLY, ResultSet.CONCUR_READ_ONLY);
    }


    @Override
	public CallableStatement prepareCall(String sql, int resultSetType, int resultSetConcurrency) throws SQLException{
        try{
            return new JdbcOdbcCallableStatement(this, netConn.CreateCommand(), sql, resultSetType,
                    resultSetConcurrency);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    @Override
	public CallableStatement prepareCall(String sql, int resultSetType, int resultSetConcurrency,
            int resultSetHoldability){
        throw new UnsupportedOperationException();
    }


    @Override
	public PreparedStatement prepareStatement(String sql) throws SQLException{
        return prepareStatement(sql, ResultSet.TYPE_FORWARD_ONLY, ResultSet.CONCUR_READ_ONLY);
    }


    @Override
	public PreparedStatement prepareStatement(String sql, int resultSetType, int resultSetConcurrency)
            throws SQLException{
        try{
            return new JdbcOdbcPreparedStatement(this, netConn.CreateCommand(), sql, resultSetType,
                    resultSetConcurrency);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    @Override
	public PreparedStatement prepareStatement(String sql, int resultSetType, int resultSetConcurrency,
            int resultSetHoldability){
        throw new UnsupportedOperationException();
    }


    @Override
	public PreparedStatement prepareStatement(String sql, int autoGeneratedKeys){
        throw new UnsupportedOperationException();
    }


    @Override
	public PreparedStatement prepareStatement(String sql, int[] columnIndexes){
        throw new UnsupportedOperationException();
    }


    @Override
	public PreparedStatement prepareStatement(String sql, String[] columnNames){
        throw new UnsupportedOperationException();
    }


    @Override
	public void releaseSavepoint(Savepoint savepoint){
        throw new UnsupportedOperationException();
    }


    @Override
	public void rollback(Savepoint savepoint){
        throw new UnsupportedOperationException();
    }


    @Override
	public void setCatalog(String catalog) throws SQLException{
        try{
            netConn.ChangeDatabase(catalog);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    @Override
	public String getCatalog(){
        return netConn.get_Database();
    }


    @Override
	public void setClientInfo(String name, String value){
        throw new UnsupportedOperationException();
    }


    @Override
	public void setClientInfo(Properties properties){
        throw new UnsupportedOperationException();
    }


    @Override
	public void setHoldability(int holdability){
        throw new UnsupportedOperationException();
    }


    @Override
	public void setReadOnly(boolean readOnly) throws SQLException{
        // TODO Auto-generated method stub

    }


    @Override
	public Savepoint setSavepoint(){
        throw new UnsupportedOperationException();
    }


    @Override
	public Savepoint setSavepoint(String name){
        throw new UnsupportedOperationException();
    }


	@Override
	public void setTypeMap(Map<String, Class<?>> map){
        throw new UnsupportedOperationException();
    }


    @Override
	public boolean isWrapperFor(Class<?> iface){
        return iface.isAssignableFrom(this.getClass());
    }


    @Override
	public <T>T unwrap(Class<T> iface) throws SQLException{
        if(isWrapperFor(iface)){
            return (T)this;
        }
        throw new SQLException(this.getClass().getName() + " does not implements " + iface.getName() + ".", "01000");
    }


    /**
     * {@inheritDoc}
     */
	public void setSchema(String schema) throws SQLException {
	}


    /**
     * {@inheritDoc}
     */
	public String getSchema() throws SQLException {
		return null;
	}


    /**
     * {@inheritDoc}
     */
	public void abort(Executor executor) throws SQLException {
		throw new SQLFeatureNotSupportedException();
	}


    /**
     * {@inheritDoc}
     */
	public void setNetworkTimeout(Executor executor, int milliseconds)
			throws SQLException {
		throw new SQLFeatureNotSupportedException();
	}


    /**
     * {@inheritDoc}
     */
	public int getNetworkTimeout() throws SQLException {
		throw new SQLFeatureNotSupportedException();
	}
}
