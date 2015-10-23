using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Tesla.Data {
    public static class Database {
        private static string ConnectionString;
        private static string DbProviderName;
        private static Type DbConnectionType;
        private static ObjectActivator<IDbConnection> DbConnectionActivator;
        private static readonly ConcurrentStack<string> DatabasesStack = new ConcurrentStack<string>();

        public static void Initialize(string dbConnectionProvider, string connectionString = null) {
            DbProviderName = dbConnectionProvider;

            DbConnectionType = TypeDiscovery.TypeFromString(DbProviderName);
            DbConnectionActivator = TypeDiscovery.CreateActivator<IDbConnection>(DbConnectionType.GetConstructor(new[] { typeof(string) }));

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

        public static void UseDatabase(string dbName, IDbConnection connection = null) {
            if (connection == null) {
                using (connection = GetConnection()) {
                    using (var cmd = connection.CreateCommand()) {
                        cmd.CommandText = $"USE {dbName}";
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = $"USE {dbName}";
                cmd.ExecuteNonQuery();
            }
        }

        public static void PushDatabase(string newDbName, IDbConnection connection = null) {
            if (connection == null) {
                using (connection = GetConnection()) {
                    using (var cmd = connection.CreateCommand()) {
                        cmd.CommandText = "SELECT DATABASE()";
                        var currentDatabase = cmd.ExecuteScalar() as string;

                        if (currentDatabase == null) {
                            throw new ArgumentNullException("No current database selected.");
                        }

                        DatabasesStack.Push(currentDatabase);

                        cmd.CommandText = $"USE {newDbName}";
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = "SELECT DATABASE()";
                var currentDatabase = cmd.ExecuteScalar() as string;

                if (currentDatabase == null) {
                    throw new ArgumentNullException("No current database selected.");
                }

                DatabasesStack.Push(currentDatabase);

                cmd.CommandText = $"USE {newDbName}";
                cmd.ExecuteNonQuery();
            }
        }

        public static void PopDatabase(IDbConnection connection = null) {
            if (DatabasesStack.IsEmpty) {
                throw new ArgumentOutOfRangeException("Databases stack is already empty.");
            }

            var oldDatabase = string.Empty;

            if (!DatabasesStack.TryPop(out oldDatabase)) {
                throw new InvalidOperationException("Was not able to pop database from a stack (concurrency?).");
            }

            if (connection == null) {
                using (connection = GetConnection()) {
                    using (var cmd = connection.CreateCommand()) {
                        cmd.CommandText = $"USE {oldDatabase}";
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            using (var cmd = connection.CreateCommand()) {
                cmd.CommandText = $"USE {oldDatabase}";
                cmd.ExecuteNonQuery();
            }
        }
    }
}