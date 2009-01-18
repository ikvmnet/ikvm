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

import java.sql.Connection;
import java.sql.DatabaseMetaData;
import java.sql.ResultSet;
import java.sql.RowIdLifetime;
import java.sql.SQLException;

import cli.System.Data.*;
import cli.System.Data.Common.*;
import cli.System.Data.Odbc.*;

/**
 * @author Volker Berlin
 */
public class JdbcOdbcDatabaseMetaData implements DatabaseMetaData{

    private final DbConnection netConn;


    public JdbcOdbcDatabaseMetaData(DbConnection netConn){
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


    public boolean autoCommitFailureClosesAllResultSets() throws SQLException{
        // TODO Auto-generated method stub
        return false;
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
            String attributeNamePattern) throws SQLException{
        // TODO Auto-generated method stub
        return null;
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


    public ResultSet getClientInfoProperties() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getColumnPrivileges(String catalog, String schema, String table, String columnNamePattern)
            throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getColumns(String catalog, String schemaPattern, String tableNamePattern, String columnNamePattern)
            throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public Connection getConnection() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getCrossReference(String parentCatalog, String parentSchema, String parentTable,
            String foreignCatalog, String foreignSchema, String foreignTable) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public int getDatabaseMajorVersion() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getDatabaseMinorVersion() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public String getDatabaseProductName() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getDatabaseProductVersion() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public int getDefaultTransactionIsolation() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getDriverMajorVersion(){
        // TODO Auto-generated method stub
        return 0;
    }


    public int getDriverMinorVersion(){
        // TODO Auto-generated method stub
        return 0;
    }


    public String getDriverName() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getDriverVersion() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getExportedKeys(String catalog, String schema, String table) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getExtraNameCharacters() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getFunctionColumns(String catalog, String schemaPattern, String functionNamePattern,
            String columnNamePattern) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getFunctions(String catalog, String schemaPattern, String functionNamePattern) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getIdentifierQuoteString() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getImportedKeys(String catalog, String schema, String table) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getIndexInfo(String catalog, String schema, String table, boolean unique, boolean approximate)
            throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public int getJDBCMajorVersion() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public int getJDBCMinorVersion() throws SQLException{
        // TODO Auto-generated method stub
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
        // TODO Auto-generated method stub
        return null;
    }


    public String getProcedureTerm() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getProcedures(String catalog, String schemaPattern, String procedureNamePattern)
            throws SQLException{
        // the description of the restrictions can you request with GetSchema("Restrictions")
        String[] restrictions = new String[]{catalog, schemaPattern, procedureNamePattern};
        DataTable data = netConn.GetSchema(OdbcMetaDataCollectionNames.Procedures, restrictions);
        return new JdbcOdbcDTResultSet(data);
    }


    public int getResultSetHoldability() throws SQLException{
        // TODO Auto-generated method stub
        return 0;
    }


    public RowIdLifetime getRowIdLifetime() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getSQLKeywords() throws SQLException{
        // TODO Auto-generated method stub
        return null;
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


    public ResultSet getSchemas(String catalog, String schemaPattern) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getSearchStringEscape() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public String getStringFunctions() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getSuperTables(String catalog, String schemaPattern, String tableNamePattern) throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getSuperTypes(String catalog, String schemaPattern, String typeNamePattern) throws SQLException{
        // TODO Auto-generated method stub
        return null;
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
        // TODO Auto-generated method stub
        return null;
    }


    public String getTimeDateFunctions() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getTypeInfo() throws SQLException{
        // TODO Auto-generated method stub
        return null;
    }


    public ResultSet getUDTs(String catalog, String schemaPattern, String typeNamePattern, int[] types)
            throws SQLException{
        // TODO Auto-generated method stub
        return null;
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


    public boolean locatorsUpdateCopy() throws SQLException{
        // TODO Auto-generated method stub
        return false;
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
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsGetGeneratedKeys() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsGroupBy() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsGroupByBeyondSelect() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsGroupByUnrelated() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsIntegrityEnhancementFacility() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsLikeEscapeClause() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsLimitedOuterJoins() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsMinimumSQLGrammar() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsMixedCaseIdentifiers() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsMixedCaseQuotedIdentifiers() throws SQLException{
        // TODO Auto-generated method stub
        return false;
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
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsOuterJoins() throws SQLException{
        // TODO Auto-generated method stub
        return false;
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


    public boolean supportsResultSetHoldability(int holdability) throws SQLException{
        // TODO Auto-generated method stub
        return false;
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


    public boolean supportsStatementPooling() throws SQLException{
        // TODO Auto-generated method stub
        return false;
    }


    public boolean supportsStoredFunctionsUsingCallSyntax() throws SQLException{
        // TODO Auto-generated method stub
        return false;
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

}
