using Eagle.Core.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace Eagle.Core.SqlQueries.DialectProvider
{

    /// <summary>
    /// The db provider factory.
    /// </summary>
    /// <remarks></remarks>
    public sealed class SqlQueryDialectProviderFactory
    {
        #region "private Memeber"

        private static SqlQueryDialectProviderBase defaultSqlQueryDialectProvider;

        private static Dictionary<string, SqlQueryDialectProviderBase> providerCache = 
            new Dictionary<string, SqlQueryDialectProviderBase>();

        private static readonly object lockObject = new object();

        private SqlQueryDialectProviderFactory() { }

        #endregion

        /// <summary>
        /// Creates the db provider.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="classTypeName">Name of the class.</param>
        /// <param name="connectionString">The conn STR.</param>
        /// <returns>The db provider.</returns>
        public static SqlQueryDialectProviderBase CreateSqlQueryDialectProviderFactory(string assemblyName, string classTypeName)
        {
            lock (lockObject)
            {
                string cacheKey = string.Concat(assemblyName, classTypeName);

                if (providerCache.ContainsKey(cacheKey))
                {
                    return providerCache[cacheKey];
                }
                else
                {
                    Assembly assembly = null;

                    if (string.IsNullOrEmpty(assemblyName))
                    {
                        assembly = typeof(SqlQueryDialectProviderFactory).Assembly;
                    }
                    else
                    {
                        assembly = Assembly.Load(assemblyName);
                    }

                    SqlQueryDialectProviderBase sqlQueryDialectProvider = (SqlQueryDialectProviderBase)assembly.CreateInstance(
                        classTypeName, true, BindingFlags.Default, null, null, null, null);

                    if (!providerCache.ContainsKey(cacheKey))
                    {
                        providerCache.Add(cacheKey, sqlQueryDialectProvider);
                    }

                    return sqlQueryDialectProvider;
                }
            }
        }

        public static SqlQueryDialectProviderBase CreateSqlQueryDialectProviderFactory(DatabaseType databaseType)
        {
            if (databaseType == DatabaseType.SqlServer)
            {
                return new SqlServerQueryDialectProvider(); 
            }
            else if (databaseType == DatabaseType.MySql)
            {
                return new MySqlQueryDialectProvider();
            }
            else if (databaseType == DatabaseType.Oracle)
            {
                return new OracleQueryDialectProvider();
            }
            else if (databaseType == DatabaseType.SqlLite)
            {
                return new SqlLiteQueryDialectProvider();
            }

            return new SqlServerQueryDialectProvider(); 
        }

        public static SqlQueryDialectProviderBase CreateSqlQueryDialectProviderFactory(IDbConnection dbConnection)
        {
            var dbConnectionTypeName = dbConnection.GetType().Name;

            DatabaseType databaseType = DatabaseType.SqlServer;

            if (dbConnectionTypeName.StartsWith("MySql", StringComparison.InvariantCultureIgnoreCase))
            {
                databaseType = DatabaseType.MySql;
            }
            else if (dbConnectionTypeName.StartsWith("Oracle", StringComparison.InvariantCultureIgnoreCase))
            {
                databaseType = DatabaseType.Oracle;
            }
            else if (dbConnectionTypeName.StartsWith("SQLite", StringComparison.InvariantCultureIgnoreCase))
            {
                databaseType = DatabaseType.SqlLite;
            }
            else if (dbConnectionTypeName.StartsWith("System.Data.SqlClient", StringComparison.InvariantCultureIgnoreCase))
            {
                databaseType = DatabaseType.SqlServer;
            }

            return CreateSqlQueryDialectProviderFactory(databaseType);
        }

        private static SqlQueryDialectProviderBase CreateSqlQueryDialectProviderFactory() 
        {
            string providerName = string.Empty;

            string[] assAndClass = providerName.Split(new char[] { ',' });

            try
            {
                if (assAndClass.Length.Equals(2))
                {
                    return CreateSqlQueryDialectProviderFactory(assAndClass[1].Trim(), assAndClass[0].Trim());
                }
                else
                {
                    return CreateSqlQueryDialectProviderFactory(string.Empty, providerName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SqlQueryDialectProviderBase Default
        {
            get
            {
                if (defaultSqlQueryDialectProvider == null)
                {
                    defaultSqlQueryDialectProvider = CreateSqlQueryDialectProviderFactory(DatabaseType.MySql);
                }

                return defaultSqlQueryDialectProvider;
            }
        }

    }

}
