using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Tesla.Data {
    public static class Database {
        private static string ConnectionString;
            //ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

        private static string DbProviderName;

        private static readonly Type DbConnectionType = TypeDiscovery.TypeFromString(DbProviderName);
        private static readonly ObjectActivator<IDbConnection> DbConnectionActivator = 
            TypeDiscovery.CreateActivator<IDbConnection>(DbConnectionType.GetConstructor(new[] { typeof(string) }));
            //TypeDiscovery.InterfaceFromString<IDbConnection>(DbProviderName);

        public static void Initialize(string dbConnectionProvider, string connectionString = null) {
            DbProviderName = dbConnectionProvider;

            if (connectionString != null) {
                ConnectionString = connectionString;
            }
        }

        public static IDbConnection GetConnection(string connectionString = null) {
            if (connectionString == null) {
                connectionString = ConnectionString;
            }

            var connection = DbConnectionActivator(connectionString);
            connection.Open();
            return connection;
        }

        public static DataResult Query(string sql, Dictionary<string, object> parameters = null, IDbConnection connection = null) {
            if (connection == null) {
                using (connection = GetConnection()) {
                    using (var cmd = connection.CreateCommand()) {
                        cmd.CommandText = sql;
                        cmd.Assign(parameters);

                        using (var reader = cmd.ExecuteReader()) {
                            return new DataResult(reader);
                        }
                    }
                }
            }

            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = sql;
                cmd.Assign(parameters);

                using (var reader = cmd.ExecuteReader()) {
                    return new DataResult(reader);
                }
            }
        }

        public static IDataReader GetReader(string sql, IDbConnection connection, Dictionary<string, object> parameters = null) {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Assign(parameters);

            return cmd.ExecuteReader();
        }

        public static int NonQuery(string sql, Dictionary<string, object> parameters = null, IDbConnection connection = null) {
            if (connection == null) {
                using (connection = GetConnection()) {
                    using (var cmd = connection.CreateCommand()) {
                        cmd.CommandText = sql;
                        cmd.Assign(parameters);

                        return cmd.ExecuteNonQuery();
                    }
                }
            }

            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = sql;
                cmd.Assign(parameters);

                return cmd.ExecuteNonQuery();
            }
        }

        public static ulong InsertIdentity(string sql, Dictionary<string, object> parameters = null,
            IDbConnection connection = null) {
            if (connection == null) {
                using (connection = GetConnection()) {
                    using (var cmd = connection.CreateCommand()) {
                        cmd.CommandText = sql.Trim(';') + "; SELECT @@IDENTITY;";
                        cmd.Assign(parameters);
                        return (ulong)cmd.ExecuteScalar();
                    }
                }
            }

            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = sql.Trim(';') + "; SELECT @@IDENTITY;";
                cmd.Assign(parameters);

                return (ulong)cmd.ExecuteScalar();
            }
        }

        public static object ScalarQuery(string sql, Dictionary<string, object> parameters = null, IDbConnection connection = null) {
            if (connection == null) {
                using (connection = GetConnection()) {
                    using (var cmd = connection.CreateCommand()) {
                        cmd.CommandText = sql;
                        cmd.Assign(parameters);

                        return cmd.ExecuteScalar();
                    }
                }
            }

            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = sql;
                cmd.Assign(parameters);

                return cmd.ExecuteScalar();
            }
        }
    }
}