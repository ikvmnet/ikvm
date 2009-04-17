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
