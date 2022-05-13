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

import ikvm.lang.CIL;

import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Types;

import cli.System.Convert;
import cli.System.DBNull;
import cli.System.Data.*;
import cli.System.Data.Common.*;
import cli.System.Data.Odbc.OdbcType;

/**
 * This JDBC Driver is a wrapper to the ODBC.NET Data Provider.
 */
public class JdbcOdbcResultSetMetaData implements ResultSetMetaData{

    private final DbDataReader reader;
    
    private final DataTable schema;
    
    JdbcOdbcResultSetMetaData(DbDataReader reader){
        this.reader = reader;
        schema = reader.GetSchemaTable();
    }
    
    public String getCatalogName(int column) throws SQLException{
        Object obj = getColumnMeta(column, "BaseCatalogName");
        if(obj == null || obj == DBNull.Value){
            return "";
        }
        return obj.toString();
    }


    public String getColumnClassName(int column) throws SQLException{
        String type = getColumnMeta(column, "DataType").toString();
        return JdbcOdbcUtils.getJavaClassName(type);
    }


    public int getColumnCount(){
        return schema.get_Rows().get_Count();
    }


    public int getColumnDisplaySize(int column) throws SQLException{
        int precision = getPrecision(column);
        int type = CIL.unbox_int( getColumnMeta(column, "ProviderType") );
        switch(type){
            case OdbcType.Decimal:
            case OdbcType.Numeric:
                return precision + (getScale(column) > 0 ? 2 : 1); // + sign and comma
        }
        return precision;
    }


    public String getColumnLabel(int column) throws SQLException{
        return getColumnMeta(column, "ColumnName").toString();
    }


    public String getColumnName(int column) throws SQLException{
        return getColumnMeta(column, "ColumnName").toString();
    }


    public int getColumnType(int column) throws SQLException{
        int type = CIL.unbox_int( getColumnMeta(column, "ProviderType") );
        switch(type){
            case OdbcType.VarChar:
                return Types.VARCHAR;
            case OdbcType.Char:
                return Types.CHAR;
            case OdbcType.Binary:
            case OdbcType.Timestamp:
                return Types.BINARY;
            case OdbcType.Bit:
                return Types.BOOLEAN;
            case OdbcType.TinyInt:
                return Types.TINYINT;
            case OdbcType.Date:
                return Types.DATE;
            case OdbcType.DateTime:
            case OdbcType.SmallDateTime:
                return Types.TIMESTAMP;
            case OdbcType.Decimal:
                return Types.DECIMAL;
            case OdbcType.Double:
                return Types.DOUBLE;
            case OdbcType.UniqueIdentifier:
                return Types.ROWID;
            case OdbcType.SmallInt:
                return Types.SMALLINT;
            case OdbcType.Int:
                return Types.INTEGER;
            case OdbcType.BigInt:
                return Types.BIGINT;
            case OdbcType.Real:
                return Types.FLOAT;
            case OdbcType.NVarChar:
                return Types.NVARCHAR;
            case OdbcType.NChar:
                return Types.NCHAR;
            case OdbcType.NText:
                return Types.LONGNVARCHAR;
            case OdbcType.Text:
                return Types.LONGVARCHAR;
            case OdbcType.Image:
                return Types.LONGVARBINARY;
            case OdbcType.Time:
                return Types.TIME;
            case OdbcType.Numeric:
                return Types.NUMERIC;
            case OdbcType.VarBinary:
                return Types.VARBINARY;
        }
        return Types.OTHER;
    }


    public String getColumnTypeName(int column) throws SQLException{
        try{
            return reader.GetDataTypeName(column - 1);
        }catch(ArrayIndexOutOfBoundsException ex){
            throw new SQLException("Invalid column number ("+column+"). A number between 1 and "+schema.get_Rows().get_Count()+" is valid.", "S1002");
        }catch(Throwable ex){
            throw JdbcOdbcUtils.createSQLException(ex);
        }
    }


    public int getPrecision(int column) throws SQLException{
        Object obj = getColumnMeta(column, "NumericPrecision");
        return Convert.ToInt32(obj);
    }


    public int getScale(int column) throws SQLException{
        Object obj = getColumnMeta(column, "NumericScale");
        return Convert.ToInt32(obj);
    }


    public String getSchemaName(int column) throws SQLException{
        Object obj = getColumnMeta(column, "BaseSchemaName");
        if(obj == null || obj == DBNull.Value){
            return "";
        }
        return obj.toString();
    }


    public String getTableName(int column) throws SQLException{
        Object obj = getColumnMeta(column, "BaseTableName");
        if(obj == null || obj == DBNull.Value){
            return "";
        }
        return obj.toString();
    }


    public boolean isAutoIncrement(int column) throws SQLException{
        Object obj = getColumnMeta(column, "IsAutoIncrement");
        return Convert.ToBoolean(obj);
    }


    public boolean isCaseSensitive(int column){
        return false;
    }


    public boolean isCurrency(int column) throws SQLException{
        return CIL.unbox_int( getColumnMeta(column, "ProviderType") ) == OdbcType.Decimal && getScale(column) == 4;
    }


    public boolean isDefinitelyWritable(int column){
        return false;
    }


    public int isNullable(int column) throws SQLException{
        Object obj = getColumnMeta(column, "AllowDBNull");
        return Convert.ToBoolean(obj) ? columnNullable : columnNoNulls;
    }


    public boolean isReadOnly(int column) throws SQLException{
        Object obj = getColumnMeta(column, "IsReadOnly");
        return Convert.ToBoolean(obj);
    }


    public boolean isSearchable(int column) throws SQLException{
        return !CIL.unbox_boolean( getColumnMeta(column, "IsLong") );
    }


    public boolean isSigned(int column) throws SQLException{
        int type = CIL.unbox_int( getColumnMeta(column, "ProviderType") );
        switch(type){
            case OdbcType.Numeric:
            case OdbcType.Decimal:
            case OdbcType.Double:
            case OdbcType.SmallInt:
            case OdbcType.Int:
            case OdbcType.BigInt:
            case OdbcType.Real:
                return true;
        }
        return false;
    }


    public boolean isWritable(int column) throws SQLException{
        Object obj = getColumnMeta(column, "IsReadOnly");
        return !Convert.ToBoolean(obj);
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
    
    private Object getColumnMeta(int column, String metaKey) throws SQLException{
        try{
            DataRow columnMeta = schema.get_Rows().get_Item(column-1);
            return columnMeta.get_Item(metaKey);
        }catch(ArrayIndexOutOfBoundsException ex){
            throw new SQLException("Invalid column number ("+column+"). A number between 1 and "+schema.get_Rows().get_Count()+" is valid.", "S1002");
        }
    }
}
