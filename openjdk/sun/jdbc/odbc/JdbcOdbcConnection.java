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

import cli.System.Data.*;
import cli.System.Data.Common.*;
import cli.System.Data.Odbc.*;

import java.sql.*;
import java.util.Map;
import java.util.Properties;

/**
 * This JDBC Driver is a wrapper to the ODBC.NET Data Provider
 */
public class JdbcOdbcConnection implements Connection{

    private final DbConnection netConn;

    private DbTransaction transaction;

    private int isolation = TRANSACTION_READ_COMMITTED;


    JdbcOdbcConnection(String connectString, Properties info) throws SQLException{
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
    }


    public void clearWarnings() throws SQLException{
        // TODO Auto-generated method stub

    }


    public void close() throws SQLException{
        try{
            netConn.Close();
        }catch(Exception ex){
            throw new SQLException(ex);
        }
    }


    public Array createArrayOf(String typeName, Object[] elements) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Blob createBlob() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Clob createClob() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public NClob createNClob() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public SQLXML createSQLXML() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Statement createStatement() throws SQLException{
        try{
            return new JdbcOdbcStatement(this, netConn.CreateCommand());
        }catch(Exception ex){
            throw new SQLException(ex);
        }
    }


    public Statement createStatement(int resultSetType, int resultSetConcurrency) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Statement createStatement(int resultSetType, int resultSetConcurrency, int resultSetHoldability)
            throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Struct createStruct(String typeName, Object[] attributes) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


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
        }catch(Exception ex){
            throw new SQLException(ex);
        }
    }


    public boolean getAutoCommit(){
        return transaction != null;
    }


    public void commit() throws SQLException{
        try{
            if(transaction == null){
                // auto commit == true
                return;
            }
            transaction.Commit();
            transaction = netConn.BeginTransaction(transaction.get_IsolationLevel());
        }catch(Exception ex){
            throw new SQLException(ex);
        }
    }


    public void rollback() throws SQLException{
        try{
            if(transaction == null){
                // auto commit == true
                return;
            }
            transaction.Rollback();
            transaction = netConn.BeginTransaction(transaction.get_IsolationLevel());
        }catch(Exception ex){
            throw new SQLException(ex);
        }
    }


    public void setTransactionIsolation(int level){
        isolation = level;
    }


    public int getTransactionIsolation(){
        return isolation;
    }


    public String getClientInfo(String name) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Properties getClientInfo() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public int getHoldability() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public DatabaseMetaData getMetaData() throws SQLException{
        // TODO Auto-generated method stub
        return new JdbcOdbcDatabaseMetaData(netConn);
    }


    public Map<String, Class<?>> getTypeMap() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public SQLWarning getWarnings() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public boolean isClosed() throws SQLException{
        return netConn.get_State().Value == ConnectionState.Closed;
    }


    public boolean isReadOnly() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean isValid(int timeout) throws SQLException{
        // TODO Auto-generated method stub
        return true;
    }


    public String nativeSQL(String sql) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public CallableStatement prepareCall(String sql) throws SQLException{
        return new JdbcOdbcCallableStatement(this, netConn.CreateCommand(), sql);
    }


    public CallableStatement prepareCall(String sql, int resultSetType, int resultSetConcurrency) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public CallableStatement prepareCall(String sql, int resultSetType, int resultSetConcurrency,
            int resultSetHoldability) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public PreparedStatement prepareStatement(String sql) throws SQLException{
        return new JdbcOdbcPreparedStatement(this, netConn.CreateCommand(), sql);
    }


    public PreparedStatement prepareStatement(String sql, int resultSetType, int resultSetConcurrency)
            throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public PreparedStatement prepareStatement(String sql, int resultSetType, int resultSetConcurrency,
            int resultSetHoldability) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public PreparedStatement prepareStatement(String sql, int autoGeneratedKeys) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public PreparedStatement prepareStatement(String sql, int[] columnIndexes) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public PreparedStatement prepareStatement(String sql, String[] columnNames) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public void releaseSavepoint(Savepoint savepoint) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void rollback(Savepoint savepoint) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setCatalog(String catalog) throws SQLException{
        netConn.ChangeDatabase(catalog);
    }


    public String getCatalog() throws SQLException{
        return netConn.get_Database();
    }


    public void setClientInfo(String name, String value) throws SQLClientInfoException{
        // TODO Auto-generated method stub

    }


    public void setClientInfo(Properties properties) throws SQLClientInfoException{
        // TODO Auto-generated method stub

    }


    public void setHoldability(int holdability) throws SQLException{
        // TODO Auto-generated method stub

    }


    public void setReadOnly(boolean readOnly) throws SQLException{
        // TODO Auto-generated method stub

    }


    public Savepoint setSavepoint(){
        throw new UnsupportedOperationException();
    }


    public Savepoint setSavepoint(String name){
        throw new UnsupportedOperationException();
    }


    public void setTypeMap(Map<String, Class<?>> map) throws SQLException{
        // TODO Auto-generated method stub

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
