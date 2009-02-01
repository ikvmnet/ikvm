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

import cli.System.Data.*;
import cli.System.Data.Odbc.*;

/**
 * @author Volker Berlin
 */
public class JdbcOdbcUpdateableResultSet extends JdbcOdbcDTResultSet{

    private final OdbcDataAdapter da;

    private final DataTable data;

    private DataRow insertRow;


    public JdbcOdbcUpdateableResultSet(OdbcCommand cmd){
        this(new DataTable(), cmd);
    }


    private JdbcOdbcUpdateableResultSet(DataTable data, OdbcCommand cmd){
        super(data, CONCUR_UPDATABLE);
        this.data = data;
        da = new OdbcDataAdapter(cmd);
        da.Fill(data);
        OdbcCommandBuilder cmdBldr = new OdbcCommandBuilder(da);
        cmdBldr.GetUpdateCommand(); // throw an exception if update is not possible, we want a very early exception
    }


    @Override
    protected DataRow getDataRow() throws SQLException{
        if(insertRow != null){
            return insertRow;
        }
        return super.getDataRow();
    }


    @Override
    protected void setDataRow() throws SQLException{
        insertRow = null;
        super.setDataRow();
    }


    @Override
    public void updateObject(int columnIndex, Object x, int scaleOrLength) throws SQLException{
        try{
            x = JdbcOdbcUtils.convertJava2Net(x, scaleOrLength);
            getDataRow().set_Item(columnIndex - 1, x);
        }catch(ArrayIndexOutOfBoundsException ex){
            throw new SQLException("Invalid column number (" + columnIndex + "). A number between 1 and "
                    + data.get_Columns().get_Count() + " is valid.", "S1002", ex);
        }
    }


    @Override
    public void updateRow() throws SQLException{
        if(insertRow != null){
            throw new SQLException("Cursor is on the insert row.");
        }
        try{
            da.Update(data);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    @Override
    public void deleteRow() throws SQLException{
        if(insertRow != null){
            throw new SQLException("Cursor is on the insert row.");
        }
        try{
            getDataRow().Delete(); // Delete the current row
            da.Update(data);
            setDataRow(); // set a new Current Row
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    @Override
    public void insertRow() throws SQLException{
        if(insertRow == null){
            throw new SQLException("Cursor is not on the insert row.");
        }
        try{
            getRows().Add(insertRow);
            insertRow = null;
            da.Update(data);
            last();
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }

    }


    @Override
    public void moveToInsertRow(){
        insertRow = data.NewRow();
    }


    @Override
    public void moveToCurrentRow(){
        insertRow = null;
    }


    @Override
    public void cancelRowUpdates() throws SQLException{
        getDataRow().CancelEdit();
    }


    @Override
    public boolean rowDeleted(){
        return false;
    }


    @Override
    public boolean rowInserted(){
        return false;
    }


    @Override
    public boolean rowUpdated(){
        return false;
    }
}
