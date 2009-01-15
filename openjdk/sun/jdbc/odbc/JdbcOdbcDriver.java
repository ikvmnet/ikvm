/*
 * Created on 07.01.2009
 */
package sun.jdbc.odbc;

import java.sql.*;
import java.util.Properties;

/**
 * This JDBC Driver is a wrapper to the ODBC.NET Data Provider
 */
public class JdbcOdbcDriver implements Driver{

    private static final String URL_PREFIX = "jdbc:odbc:";

    static{
        try{
            DriverManager.registerDriver(new JdbcOdbcDriver());
        }catch(SQLException ex){
            throw new ExceptionInInitializerError(ex);
        }
    }


    /**
     * {@inheritDoc}
     */
    public boolean acceptsURL(String url){
        return url.startsWith(URL_PREFIX);
    }


    /**
     * {@inheritDoc}
     */
    public Connection connect(String url, Properties info) throws SQLException{
        if(!acceptsURL(url)){
            return null;
        }
        String connectString = url.substring(URL_PREFIX.length());
        return new JdbcOdbcConnection(connectString, info);
    }


    /**
     * {@inheritDoc}
     */
    public int getMajorVersion(){
        return 2;
    }


    /**
     * {@inheritDoc}
     */
    public int getMinorVersion(){
        return 1;
    }


    /**
     * {@inheritDoc}
     */
    public DriverPropertyInfo[] getPropertyInfo(String url, Properties info) throws SQLException{
        return new DriverPropertyInfo[0];
    }


    /**
     * {@inheritDoc}
     */
    public boolean jdbcCompliant(){
        return true;
    }

}
