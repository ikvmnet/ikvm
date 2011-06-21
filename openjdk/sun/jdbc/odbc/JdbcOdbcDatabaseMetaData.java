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

import ikvm.lang.CIL;

import java.sql.*;
import java.util.HashSet;

import cli.System.Data.*;
import cli.System.Data.Common.*;
import cli.System.Data.Odbc.*;

/**
 * @author Volker Berlin
 */
public class JdbcOdbcDatabaseMetaData implements DatabaseMetaData{

    private JdbcOdbcConnection jdbcConn;

    private final OdbcConnection netConn;

    private DataRow dataSourceInfo;

    public JdbcOdbcDatabaseMetaData(JdbcOdbcConnection jdbcConn, OdbcConnection netConn){
        this.jdbcConn = jdbcConn;
        this.netConn = netConn;
    }


    public boolean allProceduresAreCallable() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean allTablesAreSelectable() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean autoCommitFailureClosesAllResultSets(){
        throw new UnsupportedOperationException();
    }


    public boolean dataDefinitionCausesTransactionCommit() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean dataDefinitionIgnoredInTransactions() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean deletesAreDetected(int type) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean doesMaxRowSizeIncludeBlobs() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public ResultSet getAttributes(String catalog, String schemaPattern, String typeNamePattern,
            String attributeNamePattern){
        throw new UnsupportedOperationException();
    }


    public ResultSet getBestRowIdentifier(String catalog, String schema, String table, int scope, boolean nullable)
            throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getCatalogSeparator() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getCatalogTerm() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getCatalogs() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getClientInfoProperties(){
        throw new UnsupportedOperationException();
    }


    public ResultSet getColumnPrivileges(String catalog, String schema, String table, String columnNamePattern)
            throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getColumns(String catalog, String schemaPattern, String tableNamePattern, String columnNamePattern)
            throws SQLException{
        try{
            // the description of the restrictions can you request with GetSchema("Restrictions")
            String[] restrictions = new String[]{catalog, schemaPattern, tableNamePattern, columnNamePattern};
            DataTable data = netConn.GetSchema(OdbcMetaDataCollectionNames.Columns, restrictions);
            return new JdbcOdbcDTResultSet(data);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public Connection getConnection(){
        return jdbcConn;
    }


    public ResultSet getCrossReference(String parentCatalog, String parentSchema, String parentTable,
            String foreignCatalog, String foreignSchema, String foreignTable) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public int getDatabaseMajorVersion(){
        String version = netConn.get_ServerVersion().trim();
        for(int i = 0; i < version.length(); i++){
            char ch = version.charAt(i);
            if(ch < '0' || ch > '9'){
                return Integer.parseInt(version.substring(0, i));
            }
        }
        return Integer.parseInt(version);
    }


    public int getDatabaseMinorVersion(){
        String version = netConn.get_ServerVersion().trim();
        int idx = version.indexOf('.');
        if(idx < 0){
            return 0;
        }
        version = version.substring(idx + 1);
        for(int i = 0; i < version.length(); i++){
            char ch = version.charAt(i);
            if(ch < '0' || ch > '9'){
                return Integer.parseInt(version.substring(0, i));
            }
        }
        return Integer.parseInt(version);
    }


    public String getDatabaseProductName() throws SQLException{
        return String.valueOf(getInfo(DbMetaDataColumnNames.DataSourceProductName));
    }


    public String getDatabaseProductVersion(){
        return netConn.get_ServerVersion();
    }


    public int getDefaultTransactionIsolation(){
        return Connection.TRANSACTION_READ_COMMITTED;
    }


    public int getDriverMajorVersion(){
        return 2;
    }


    public int getDriverMinorVersion(){
        return 1;
    }


    public String getDriverName(){
        return "JDBC-ODBC Bridge (" + netConn.get_Driver() + ")";
    }


    public String getDriverVersion(){
        return "2.0001";
    }


    public ResultSet getExportedKeys(String catalog, String schema, String table) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getExtraNameCharacters(){
        return "";
    }


    public ResultSet getFunctionColumns(String catalog, String schemaPattern, String functionNamePattern,
            String columnNamePattern){
        throw new UnsupportedOperationException();
    }


    public ResultSet getFunctions(String catalog, String schemaPattern, String functionNamePattern){
        throw new UnsupportedOperationException();
    }


    public String getIdentifierQuoteString() throws SQLException{
        String quote = (String)getInfo(DbMetaDataColumnNames.QuotedIdentifierPattern);
        if(quote.length()>=2){
            char ch1 = quote.charAt(0);
            char ch2 = quote.charAt(quote.length()-1);
            if(ch1 == ch2){
                return quote.substring(0,1);
            }
        }
        return "\""; // ANSI SQL and should work with the most DBMS
    }


    public ResultSet getImportedKeys(String catalog, String schema, String table) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getIndexInfo(String catalog, String schema, String table, boolean unique, boolean approximate)
            throws SQLException{
        try{
            // the description of the restrictions can you request with GetSchema("Restrictions")
            String[] restrictions = new String[]{catalog, schema, table};
            DataTable data = netConn.GetSchema(OdbcMetaDataCollectionNames.Indexes, restrictions);
            return new JdbcOdbcDTResultSet(data);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public int getJDBCMajorVersion(){
        return 2;
    }


    public int getJDBCMinorVersion(){
        return 0;
    }


    public int getMaxBinaryLiteralLength() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxCatalogNameLength() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxCharLiteralLength() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxColumnNameLength() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxColumnsInGroupBy() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxColumnsInIndex() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxColumnsInOrderBy() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxColumnsInSelect() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxColumnsInTable() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxConnections() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxCursorNameLength() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxIndexLength() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxProcedureNameLength() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxRowSize() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxSchemaNameLength() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxStatementLength() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxStatements() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxTableNameLength() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxTablesInSelect() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getMaxUserNameLength() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public String getNumericFunctions() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getPrimaryKeys(String catalog, String schema, String table) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getProcedureColumns(String catalog, String schemaPattern, String procedureNamePattern,
            String columnNamePattern) throws SQLException{
        try{
            // the description of the restrictions can you request with GetSchema("Restrictions")
            String[] restrictions = new String[]{catalog, schemaPattern, procedureNamePattern, columnNamePattern};
            DataTable dt1 = netConn.GetSchema(OdbcMetaDataCollectionNames.ProcedureColumns, restrictions);
            DataTable dt2 = netConn.GetSchema(OdbcMetaDataCollectionNames.ProcedureParameters, restrictions);
            // concatenate the both DataTable
            DataRowCollection rows1 = dt1.get_Rows();
            DataRowCollection rows2 = dt2.get_Rows();
            for(int i = 0; i < rows2.get_Count(); i++){
                DataRow row = rows2.get_Item(i);
                rows1.Add(row.get_ItemArray());
            }
            return new JdbcOdbcDTResultSet(dt1);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public String getProcedureTerm() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getProcedures(String catalog, String schemaPattern, String procedureNamePattern)
            throws SQLException{
        try{
            // the description of the restrictions can you request with GetSchema("Restrictions")
            String[] restrictions = new String[]{catalog, schemaPattern, procedureNamePattern};
            DataTable data = netConn.GetSchema(OdbcMetaDataCollectionNames.Procedures, restrictions);
            return new JdbcOdbcDTResultSet(data);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public int getResultSetHoldability(){
        throw new UnsupportedOperationException();
    }


    public RowIdLifetime getRowIdLifetime(){
        throw new UnsupportedOperationException();
    }


    public String getSQLKeywords() throws SQLException{
        try{
            DataTable dt = netConn.GetSchema(DbMetaDataCollectionNames.ReservedWords);
            final DataRowCollection rows = dt.get_Rows();
            final int count = rows.get_Count();
            final StringBuilder builder = new StringBuilder();
            for(int i=0; i<count; i++){
                String word = (String)rows.get_Item(i).get_Item(0);
                if(builder.length() > 0){
                    builder.append(',');
                }
                builder.append(word);
            }
            return builder.toString();
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public int getSQLStateType() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public String getSchemaTerm() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getSchemas() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getSchemas(String catalog, String schemaPattern){
        throw new UnsupportedOperationException();
    }


    public String getSearchStringEscape() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getStringFunctions() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getSuperTables(String catalog, String schemaPattern, String tableNamePattern){
        throw new UnsupportedOperationException();
    }


    public ResultSet getSuperTypes(String catalog, String schemaPattern, String typeNamePattern){
        throw new UnsupportedOperationException();
    }


    public String getSystemFunctions() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getTablePrivileges(String catalog, String schemaPattern, String tableNamePattern)
            throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getTableTypes() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getTables(String catalog, String schemaPattern, String tableNamePattern, String[] types)
            throws SQLException{
        try{
            // the description of the restrictions can you request with GetSchema("Restrictions")
            String[] restrictions = new String[]{catalog, schemaPattern, tableNamePattern};
            DataTable dt1 = netConn.GetSchema(OdbcMetaDataCollectionNames.Tables, restrictions);
            DataTable dt2 = netConn.GetSchema(OdbcMetaDataCollectionNames.Views, restrictions);
            // concatenate the both DataTable
            DataRowCollection rows1 = dt1.get_Rows();
            DataRowCollection rows2 = dt2.get_Rows();
            for(int i = 0; i < rows2.get_Count(); i++){
                DataRow row = rows2.get_Item(i);
                rows1.Add(row.get_ItemArray());
            }
            if(types != null){
                RowLoop:
                // Filter the types
                for(int i = rows1.get_Count() - 1; i >= 0; i--){
                    DataRow row = rows1.get_Item(i);
                    Object tableType = row.get_Item("TABLE_TYPE");
                    for(String type : types){
                        if(type.equals(tableType)){
                            continue RowLoop;
                        }
                    }
                    rows1.RemoveAt(i);
                }
            }
            return new JdbcOdbcDTResultSet(dt1);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public String getTimeDateFunctions() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getTypeInfo() throws SQLException{
        try{
            //TODO Column Names and order are wrong
            DataTable data = netConn.GetSchema(DbMetaDataCollectionNames.DataTypes);
            return new JdbcOdbcDTResultSet(data);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


    public ResultSet getUDTs(String catalog, String schemaPattern, String typeNamePattern, int[] types){
        throw new UnsupportedOperationException();
    }


    public String getURL() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getUserName() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getVersionColumns(String catalog, String schema, String table) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public boolean insertsAreDetected(int type) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean isCatalogAtStart() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean isReadOnly() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean locatorsUpdateCopy(){
        throw new UnsupportedOperationException();
    }


    public boolean nullPlusNonNullIsNull() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean nullsAreSortedAtEnd() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean nullsAreSortedAtStart() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean nullsAreSortedHigh() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean nullsAreSortedLow() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean othersDeletesAreVisible(int type) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean othersInsertsAreVisible(int type) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean othersUpdatesAreVisible(int type) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean ownDeletesAreVisible(int type) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean ownInsertsAreVisible(int type) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean ownUpdatesAreVisible(int type) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean storesLowerCaseIdentifiers() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean storesLowerCaseQuotedIdentifiers() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean storesMixedCaseIdentifiers() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean storesMixedCaseQuotedIdentifiers() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean storesUpperCaseIdentifiers() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean storesUpperCaseQuotedIdentifiers() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsANSI92EntryLevelSQL() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsANSI92FullSQL() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsANSI92IntermediateSQL() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsAlterTableWithAddColumn() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsAlterTableWithDropColumn() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsBatchUpdates() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsCatalogsInDataManipulation() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsCatalogsInIndexDefinitions() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsCatalogsInPrivilegeDefinitions() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsCatalogsInProcedureCalls() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsCatalogsInTableDefinitions() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsColumnAliasing() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsConvert() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsConvert(int fromType, int toType) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsCoreSQLGrammar() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsCorrelatedSubqueries() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsDataDefinitionAndDataManipulationTransactions() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsDataManipulationTransactionsOnly() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsDifferentTableCorrelationNames() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsExpressionsInOrderBy() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsExtendedSQLGrammar() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsFullOuterJoins() throws SQLException{
        int join = CIL.unbox_int(getInfo(DbMetaDataColumnNames.SupportedJoinOperators));
        return (join & SupportedJoinOperators.FullOuter) > 0;
    }


    public boolean supportsGetGeneratedKeys() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsGroupBy() throws SQLException{
        int behavior = CIL.unbox_int(getInfo(DbMetaDataColumnNames.GroupByBehavior));
        return behavior != GroupByBehavior.NotSupported;
    }


    public boolean supportsGroupByBeyondSelect() throws SQLException{
        int behavior = CIL.unbox_int(getInfo(DbMetaDataColumnNames.GroupByBehavior));
        return behavior == GroupByBehavior.MustContainAll;
    }


    public boolean supportsGroupByUnrelated() throws SQLException{
        int behavior = CIL.unbox_int(getInfo(DbMetaDataColumnNames.GroupByBehavior));
        return behavior == GroupByBehavior.Unrelated;
    }


    public boolean supportsIntegrityEnhancementFacility() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsLikeEscapeClause() throws SQLException{
        return getSQLKeywords().toUpperCase().indexOf(",LIKE,") > 0;
    }


    public boolean supportsLimitedOuterJoins() throws SQLException{
        int join = CIL.unbox_int(getInfo(DbMetaDataColumnNames.SupportedJoinOperators));
        return join > 0;
    }


    public boolean supportsMinimumSQLGrammar() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsMixedCaseIdentifiers() throws SQLException{
        int identifierCase = CIL.unbox_int(getInfo(DbMetaDataColumnNames.IdentifierCase));
        return identifierCase == IdentifierCase.Sensitive;
    }


    public boolean supportsMixedCaseQuotedIdentifiers() throws SQLException{
        int identifierCase = CIL.unbox_int(getInfo(DbMetaDataColumnNames.QuotedIdentifierCase));
        return identifierCase == IdentifierCase.Sensitive;
    }


    public boolean supportsMultipleOpenResults() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsMultipleResultSets() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsMultipleTransactions() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsNamedParameters() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsNonNullableColumns() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsOpenCursorsAcrossCommit() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsOpenCursorsAcrossRollback() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsOpenStatementsAcrossCommit() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsOpenStatementsAcrossRollback() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsOrderByUnrelated() throws SQLException{
        return CIL.unbox_boolean( getInfo(DbMetaDataColumnNames.OrderByColumnsInSelect));
    }


    public boolean supportsOuterJoins() throws SQLException{
        int join = CIL.unbox_int(getInfo(DbMetaDataColumnNames.SupportedJoinOperators));
        return (join & SupportedJoinOperators.LeftOuter) > 0 || (join & SupportedJoinOperators.RightOuter) > 0;
    }


    public boolean supportsPositionedDelete() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsPositionedUpdate() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsResultSetConcurrency(int type, int concurrency) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsResultSetHoldability(int holdability){
        throw new UnsupportedOperationException();
    }


    public boolean supportsResultSetType(int type) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsSavepoints() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsSchemasInDataManipulation() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsSchemasInIndexDefinitions() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsSchemasInPrivilegeDefinitions() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsSchemasInProcedureCalls() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsSchemasInTableDefinitions() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsSelectForUpdate() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsStatementPooling(){
        throw new UnsupportedOperationException();
    }


    public boolean supportsStoredFunctionsUsingCallSyntax() throws SQLException{
        throw new UnsupportedOperationException();
    }


    public boolean supportsStoredProcedures() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsSubqueriesInComparisons() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsSubqueriesInExists() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsSubqueriesInIns() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsSubqueriesInQuantifieds() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsTableCorrelationNames() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsTransactionIsolationLevel(int level) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsTransactions() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsUnion() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsUnionAll() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean updatesAreDetected(int type) throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean usesLocalFilePerTable() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean usesLocalFiles() throws SQLException{
        // TODO Auto-generated method stub
        return false;
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

    private Object getInfo(String key) throws SQLException{
        try{
            if(dataSourceInfo == null){
                DataTable td = netConn.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);
                dataSourceInfo = td.get_Rows().get_Item(0);
            }
            return dataSourceInfo.get_Item(key);
        }catch(Throwable th){
            throw JdbcOdbcUtils.createSQLException(th);
        }
    }


	/**
	 * {@inheritDoc}
	 */
	public ResultSet getPseudoColumns(String catalog, String schemaPattern,
			String tableNamePattern, String columnNamePattern)
			throws SQLException {
		throw new SQLFeatureNotSupportedException();
	}


	/**
	 * {@inheritDoc}
	 */
	public boolean generatedKeyAlwaysReturned() throws SQLException {
		return false;
	}
}
