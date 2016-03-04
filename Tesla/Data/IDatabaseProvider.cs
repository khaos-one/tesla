using System.Data;

namespace Tesla.Data {
    public interface IDatabaseProvider {
        IDbConnection GetConnection(string connectionString);
        IDbCommand CreateCommand(string sql, IDbConnection connection = null, IDbTransaction transaction = null);
    }
}
