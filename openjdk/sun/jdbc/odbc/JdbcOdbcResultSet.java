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
import java.sql.*;

import cli.System.Data.Common.*;

/**
 * This JDBC Driver is a wrapper to the ODBC.NET Data Provider. This ResultSet based on a DataReader.
 */
public class JdbcOdbcResultSet extends JdbcOdbcObject implements ResultSet{

    private DbDataReader reader;

    private final JdbcOdbcStatement statement;

    private final int holdability;

    private final int concurrency;

    private int fetchSize;

    private int row;

    private final int resultSetType;

    private ResultSetMetaData metaData;


    /**
     * Create a ResultSet that based on a DbDataReader
     * 
     * @param statement
     *            the statement for getStatement(), can be null
     * @param reader
     *            the reader for the data access, if it null then the resultset is closed.
     */
    public JdbcOdbcResultSet(JdbcOdbcStatement statement, DbDataReader reader){
        this.statement = statement;
        this.reader = reader;
        this.resultSetType = TYPE_FORWARD_ONLY;
        this.concurrency = CONCUR_READ_ONLY;
        this.holdability = HOLD_CURSORS_OVER_COMMIT;
    }


    /**
     * A constructor for extended classes. All methods that use the reader must be overridden if you use this
     * constructor.
     * 
     * @param statement
     *            the statement for getStatement(), can be null
     * @param resultSetType
     *            a result set type; one of ResultSet.TYPE_FORWARD_ONLY, ResultSet.TYPE_SCROLL_INSENSITIVE, or
     *            ResultSet.TYPE_SCROLL_SENSITIVE
     * @param concurrency
     *            a concurrency type; one of ResultSet.CONCUR_READ_ONLY or ResultSet.CONCUR_UPDATABLE
     */
    protected JdbcOdbcResultSet(JdbcOdbcStatement statement, int resultSetType, int concurrency){
        this.statement = statement;
        this.reader = null;
        this.resultSetType = resultSetType;
        this.concurrency = concurrency;
        this.holdability = HOLD_CURSORS_OVER_COMMIT;
    }


    public boolean absolute(int rowPosition) throws SQLException{
        throwForwardOnly();
        return false; // for Compiler
    }


    public void afterLast() throws SQLException{
        throwForwardOnly();
    }


    public void beforeFirst() throws SQLException{
        throwForwardOnly();
    }


    public void cancelRowUpdates() throws SQLException{
        throwReadOnly();
    }


    public void clearWarnings() throws SQLException{
        // TODO Auto-generated method stub

    }


    public void close(){
        reader = null;
        statement.closeReaderIfPossible();
    }


    public void deleteRow() throws SQLException{
        throwReadOnly();
    }


    @Override
    public int findColumn(String columnLabel) throws SQLException{
        try{
            return getReader().GetOrdinal(columnLabel) + 1;
        }catch(ArrayIndexOutOfBoundsException ex){
            throw new SQLException("Column '" + columnLabel + "' not found.", "S0022", ex);
        }
    }


    public boolean first() throws SQLException{
        throwForwardOnly();
        return false; // for compiler
    }


    public int getConcurrency(){
        return concurrency;
    }


    public String getCursorName() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public int getFetchDirection(){
        return FETCH_UNKNOWN;
    }


    public int getFetchSize(){
        return fetchSize;
    }


    public int getHoldability(){
        return holdability;
    }


    public ResultSetMetaData getMetaData() throws SQLException{
        if(metaData == null){
            metaData = new JdbcOdbcResultSetMetaData(getReader());
        }
        return metaData;
    }


    public int getRow() throws SQLException{
        getReader(); // checking for is closed
        return row;
    }


    public Statement getStatement(){
        return statement;
    }


    public int getType(){
        return resultSetType;
    }


    public SQLWarning getWarnings() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public void insertRow() throws SQLException{
        throwReadOnly();
    }


    public boolean isAfterLast() throws SQLException{
        throwForwardOnly();
        return false; // only for compiler
    }


    public boolean isBeforeFirst() throws SQLException{
        throwForwardOnly();
        return false; // only for compiler
    }


    public boolean isClosed(){
        return reader == null;
    }


    public boolean isFirst() throws SQLException{
        throwForwardOnly();
        return false; // only for compiler
    }


    public boolean isLast() throws SQLException{
        throwForwardOnly();
        return false; // only for compiler
    }


    public boolean last() throws SQLException{
        throwForwardOnly();
        return false; // only for compiler
    }


    public void moveToCurrentRow() throws SQLException{
        throwReadOnly();
    }


    public void moveToInsertRow() throws SQLException{
        throwReadOnly();
    }


    public boolean next() throws SQLException{
        DbDataReader dataReader = getReader();
        //if we after the last row then we close the reader
        //to prevent an error on repeating call of next() after the end
        //that we check also get_IsClosed()
        if(!dataReader.get_IsClosed() && dataReader.Read()){
            row++;
            return true;
        }
        row = 0;
        statement.closeReaderIfPossible();
        return false;
    }


    public boolean previous() throws SQLException{
        throwForwardOnly();
        return false; // only for compiler
    }


    public void refreshRow() throws SQLException{
        throwForwardOnly();
    }


    public boolean relative(int rowPositions) throws SQLException{
        throwForwardOnly();
        return false; // only for compiler
    }


    public boolean rowDeleted() throws SQLException{
        throwReadOnly();
        return false; // only for compiler
    }


    public boolean rowInserted() throws SQLException{
        throwReadOnly();
        return false; // only for compiler
    }


    public boolean rowUpdated() throws SQLException{
        throwReadOnly();
        return false; // only for compiler
    }


    public void setFetchDirection(int direction){
        // ignore it
    }


    public void setFetchSize(int rows){
        // ignore it
    }


    public void updateArray(int columnIndex, Array x) throws SQLException{
        throwReadOnly();
    }


    public void updateArray(String columnLabel, Array x) throws SQLException{
        throwReadOnly();
    }


    public void updateAsciiStream(int columnIndex, InputStream x, int length) throws SQLException{
        throwReadOnly();
    }


    public void updateAsciiStream(String columnLabel, InputStream x, int length) throws SQLException{
        throwReadOnly();
    }


    public void updateAsciiStream(int columnIndex, InputStream x, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateAsciiStream(String columnLabel, InputStream x, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateAsciiStream(int columnIndex, InputStream x) throws SQLException{
        throwReadOnly();
    }


    public void updateAsciiStream(String columnLabel, InputStream x) throws SQLException{
        throwReadOnly();
    }


    public void updateBigDecimal(int columnIndex, BigDecimal x) throws SQLException{
        throwReadOnly();
    }


    public void updateBigDecimal(String columnLabel, BigDecimal x) throws SQLException{
        throwReadOnly();
    }


    public void updateBinaryStream(int columnIndex, InputStream x, int length) throws SQLException{
        throwReadOnly();
    }


    public void updateBinaryStream(String columnLabel, InputStream x, int length) throws SQLException{
        throwReadOnly();
    }


    public void updateBinaryStream(int columnIndex, InputStream x, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateBinaryStream(String columnLabel, InputStream x, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateBinaryStream(int columnIndex, InputStream x) throws SQLException{
        throwReadOnly();
    }


    public void updateBinaryStream(String columnLabel, InputStream x) throws SQLException{
        throwReadOnly();
    }


    public void updateBlob(int columnIndex, Blob x) throws SQLException{
        throwReadOnly();
    }


    public void updateBlob(String columnLabel, Blob x) throws SQLException{
        throwReadOnly();
    }


    public void updateBlob(int columnIndex, InputStream inputStream, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateBlob(String columnLabel, InputStream inputStream, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateBlob(int columnIndex, InputStream inputStream) throws SQLException{
        throwReadOnly();
    }


    public void updateBlob(String columnLabel, InputStream inputStream) throws SQLException{
        throwReadOnly();
    }


    public void updateBoolean(int columnIndex, boolean x) throws SQLException{
        throwReadOnly();
    }


    public void updateBoolean(String columnLabel, boolean x) throws SQLException{
        throwReadOnly();
    }


    public void updateByte(int columnIndex, byte x) throws SQLException{
        throwReadOnly();
    }


    public void updateByte(String columnLabel, byte x) throws SQLException{
        throwReadOnly();
    }


    public void updateBytes(int columnIndex, byte[] x) throws SQLException{
        throwReadOnly();
    }


    public void updateBytes(String columnLabel, byte[] x) throws SQLException{
        throwReadOnly();
    }


    public void updateCharacterStream(int columnIndex, Reader x, int length) throws SQLException{
        throwReadOnly();
    }


    public void updateCharacterStream(String columnLabel, Reader reader, int length) throws SQLException{
        throwReadOnly();
    }


    public void updateCharacterStream(int columnIndex, Reader x, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateCharacterStream(String columnLabel, Reader reader, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateCharacterStream(int columnIndex, Reader x) throws SQLException{
        throwReadOnly();
    }


    public void updateCharacterStream(String columnLabel, Reader reader) throws SQLException{
        throwReadOnly();
    }


    public void updateClob(int columnIndex, Clob x) throws SQLException{
        throwReadOnly();
    }


    public void updateClob(String columnLabel, Clob x) throws SQLException{
        throwReadOnly();
    }


    public void updateClob(int columnIndex, Reader reader, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateClob(String columnLabel, Reader reader, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateClob(int columnIndex, Reader reader) throws SQLException{
        throwReadOnly();
    }


    public void updateClob(String columnLabel, Reader reader) throws SQLException{
        throwReadOnly();
    }


    public void updateDate(int columnIndex, Date x) throws SQLException{
        throwReadOnly();
    }


    public void updateDate(String columnLabel, Date x) throws SQLException{
        throwReadOnly();
    }


    public void updateDouble(int columnIndex, double x) throws SQLException{
        throwReadOnly();
    }


    public void updateDouble(String columnLabel, double x) throws SQLException{
        throwReadOnly();
    }


    public void updateFloat(int columnIndex, float x) throws SQLException{
        throwReadOnly();
    }


    public void updateFloat(String columnLabel, float x) throws SQLException{
        throwReadOnly();
    }


    public void updateInt(int columnIndex, int x) throws SQLException{
        throwReadOnly();
    }


    public void updateInt(String columnLabel, int x) throws SQLException{
        throwReadOnly();
    }


    public void updateLong(int columnIndex, long x) throws SQLException{
        throwReadOnly();
    }


    public void updateLong(String columnLabel, long x) throws SQLException{
        throwReadOnly();
    }


    public void updateNCharacterStream(int columnIndex, Reader x, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateNCharacterStream(String columnLabel, Reader reader, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateNCharacterStream(int columnIndex, Reader x) throws SQLException{
        throwReadOnly();
    }


    public void updateNCharacterStream(String columnLabel, Reader reader) throws SQLException{
        throwReadOnly();
    }


    public void updateNClob(int columnIndex, NClob clob) throws SQLException{
        throwReadOnly();
    }


    public void updateNClob(String columnLabel, NClob clob) throws SQLException{
        throwReadOnly();
    }


    public void updateNClob(int columnIndex, Reader reader, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateNClob(String columnLabel, Reader reader, long length) throws SQLException{
        throwReadOnly();
    }


    public void updateNClob(int columnIndex, Reader reader) throws SQLException{
        throwReadOnly();
    }


    public void updateNClob(String columnLabel, Reader reader) throws SQLException{
        throwReadOnly();
    }


    public void updateNString(int columnIndex, String string) throws SQLException{
        throwReadOnly();
    }


    public void updateNString(String columnLabel, String string) throws SQLException{
        throwReadOnly();
    }


    public void updateNull(int columnIndex) throws SQLException{
        throwReadOnly();
    }


    public void updateNull(String columnLabel) throws SQLException{
        throwReadOnly();
    }


    public void updateObject(int columnIndex, Object x, int scaleOrLength) throws SQLException{
        throwReadOnly();
    }


    public void updateObject(int columnIndex, Object x) throws SQLException{
        throwReadOnly();
    }


    public void updateObject(String columnLabel, Object x, int scaleOrLength) throws SQLException{
        throwReadOnly();
    }


    public void updateObject(String columnLabel, Object x) throws SQLException{
        throwReadOnly();
    }


    public void updateRef(int columnIndex, Ref x) throws SQLException{
        throwReadOnly();
    }


    public void updateRef(String columnLabel, Ref x) throws SQLException{
        throwReadOnly();
    }


    public void updateRow() throws SQLException{
        throwReadOnly();
    }


    public void updateRowId(int columnIndex, RowId x) throws SQLException{
        throwReadOnly();
    }


    public void updateRowId(String columnLabel, RowId x) throws SQLException{
        throwReadOnly();
    }


    public void updateSQLXML(int columnIndex, SQLXML xmlObject) throws SQLException{
        throwReadOnly();
    }


    public void updateSQLXML(String columnLabel, SQLXML xmlObject) throws SQLException{
        throwReadOnly();
    }


    public void updateShort(int columnIndex, short x) throws SQLException{
        throwReadOnly();
    }


    public void updateShort(String columnLabel, short x) throws SQLException{
        throwReadOnly();
    }


    public void updateString(int columnIndex, String x) throws SQLException{
        throwReadOnly();
    }


    public void updateString(String columnLabel, String x) throws SQLException{
        throwReadOnly();
    }


    public void updateTime(int columnIndex, Time x) throws SQLException{
        throwReadOnly();
    }


    public void updateTime(String columnLabel, Time x) throws SQLException{
        throwReadOnly();
    }


    public void updateTimestamp(int columnIndex, Timestamp x) throws SQLException{
        throwReadOnly();
    }


    public void updateTimestamp(String columnLabel, Timestamp x) throws SQLException{
        throwReadOnly();
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


    private void throwForwardOnly() throws SQLException{
        throw new SQLException("ResultSet is forward only.", "24000");
    }


    private void throwReadOnly() throws SQLException{
        throw new SQLException("ResultSet is read only.", "24000");
    }


    /**
     * Check if this ResultSet is closed before access to the DbDataReader
     * 
     * @return
     * @throws SQLException
     */
    private DbDataReader getReader() throws SQLException{
        if(reader == null){
            throw new SQLException("ResultSet is closed.", "24000");
        }
        return reader;
    }


    /**
     * {@inheritDoc}
     */
    @Override
    protected Object getObjectImpl(int columnIndex) throws SQLException{
        try{
            DbDataReader datareader = getReader();
            try{
                return datareader.get_Item(columnIndex-1);
            }catch(ArrayIndexOutOfBoundsException aioobe){
                throw new SQLException( "Invalid column number ("+columnIndex+"). A number between 1 and "+datareader.get_FieldCount()+" is valid.", "S1002");
            }
        }catch(Throwable ex){
            throw JdbcOdbcUtils.createSQLException(ex);
        }
    }

}
