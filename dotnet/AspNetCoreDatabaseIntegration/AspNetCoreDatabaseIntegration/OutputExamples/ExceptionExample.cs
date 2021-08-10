using System.ComponentModel;

namespace AspNetCoreDatabaseIntegration.OutputExamples
{
    public class ExceptionExample
    {
        [DefaultValue(@"Mcrosoft.Data.SqlClient.SqlException (0x80131904): A TOP N or FETCH rowcount value may not be negative.
at Microsoft.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
at Microsoft.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
at Microsoft.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
at Microsoft.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)...")]
        public string Exception { get; set; }
    }
}
