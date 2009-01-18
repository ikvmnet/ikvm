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
            case DbType.Decimal:
            case DbType.Currency:
            case DbType.VarNumeric:
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
            case DbType.AnsiString:
                return Types.VARCHAR;
            case DbType.AnsiStringFixedLength:
                return Types.CHAR;
            case DbType.Binary:
                return Types.BINARY;
            case DbType.Boolean:
                return Types.BOOLEAN;
            case DbType.Byte:
            case DbType.SByte:
                return Types.TINYINT;
            case DbType.Date:
                return Types.DATE;
            case DbType.DateTime:
            case DbType.DateTime2:
            case DbType.DateTimeOffset:
                return Types.TIMESTAMP;
            case DbType.Decimal:
            case DbType.Currency:
                return Types.DECIMAL;
            case DbType.Double:
                return Types.DOUBLE;
            case DbType.Guid:
                return Types.ROWID;
            case DbType.Int16:
            case DbType.UInt16:
                return Types.SMALLINT;
            case DbType.Int32:
            case DbType.UInt32:
                return Types.INTEGER;
            case DbType.Int64:
            case DbType.UInt64:
                return Types.BIGINT;
            case DbType.Single:
                return Types.FLOAT;
            case DbType.String:
                return Types.NVARCHAR;
            case DbType.StringFixedLength:
                return Types.NCHAR;
            case DbType.Time:
                return Types.TIME;
            case DbType.VarNumeric:
                return Types.NUMERIC;
            case DbType.Xml:
                return Types.SQLXML;
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
        return CIL.unbox_int( getColumnMeta(column, "ProviderType") ) == DbType.Currency;
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
            case DbType.Currency:
            case DbType.Decimal:
            case DbType.Double:
            case DbType.Int16:
            case DbType.Int32:
            case DbType.Int64:
            case DbType.SByte:
            case DbType.Single:
            case DbType.VarNumeric:
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
