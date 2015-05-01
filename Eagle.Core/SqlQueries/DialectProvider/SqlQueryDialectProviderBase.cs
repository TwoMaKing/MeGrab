using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Eagle.Core.SqlQueries.DialectProvider
{
    public abstract class SqlQueryDialectProviderBase : ISqlQueryDialectProvider
    {
        /// <summary>
        /// Discovers params from SQL text.
        /// E.g. insert into [user] values (@user_name, @user_email, @user_password). 
        /// Having 3 parameters: @user_name, @user_email, @user_password.
        /// </summary>
        /// <param name="sql">The full or part of SQL text.</param>
        /// <returns>The discovered params.</returns>
        public string[] DiscoverParams(string sql) 
        {
            if (sql == null)
            {
                return null;
            }

            Regex r = new Regex("\\" + this.ParameterPrefix + @"([\w\d_]+)");

            MatchCollection ms = r.Matches(sql);

            if (ms.Count == 0)
            {
                return null;
            }

            string[] paramNames = new string[ms.Count];
            for (int i = 0; i < ms.Count; i++)
            {
                paramNames[i] = ms[i].Value;
            }
            return paramNames;
        }

        /// <summary>
        /// Builds the name of the parameter. 
        /// E.g. for MS SQL add a '@' char at the begin with the column name. e.g. @user_name, @user_email
        /// insert into [user] values (@user_name, @user_email)
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string BuildParameterName(string name) 
        {
            name = name.Trim(this.ParameterLeftToken, this.ParameterRightToken);

            if (!name[0].Equals(this.ParameterPrefix))
            {
                return name.Insert(0, this.ParameterPrefix.ToString());
            }

            return name;
        }

        /// <summary>
        /// Builds the name of the column or table.
        /// E.g. for MS SQL add '[' and ']' at the left and right of the column name.
        /// e.g. [user_name], [user_email]
        /// update [user] set [user_name]=@user_name, [user_email]=@user_email where [user_id]=@user_id
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string BuildColumnName(string name) 
        {
            string buildedColumnName = string.Empty;

            if (name.Contains("."))
            {
                string[] splittedColumnSections = name.Split('.');

                for (int i = 0; i < splittedColumnSections.Length; ++i)
                {
                    string sectionName = splittedColumnSections[i];

                    if (!sectionName.StartsWith(this.ParameterLeftToken.ToString()))
                    {
                        sectionName = sectionName.Insert(0, this.ParameterLeftToken.ToString());
                    }

                    if (!sectionName.EndsWith(this.ParameterRightToken.ToString()))
                    {
                        sectionName = sectionName + this.ParameterRightToken.ToString();
                    }

                    if (i > 0)
                    {
                        buildedColumnName += "." + sectionName;
                    }
                }
            }
            else
            {
                buildedColumnName = name;

                if (!buildedColumnName.StartsWith(this.ParameterLeftToken.ToString()))
                {
                    buildedColumnName = buildedColumnName.Insert(0, this.ParameterLeftToken.ToString());
                }

                if (!buildedColumnName.EndsWith(this.ParameterRightToken.ToString()))
                {
                    buildedColumnName = buildedColumnName + this.ParameterRightToken.ToString();
                }
            }

            return buildedColumnName;
        }

        /// <summary>
        /// Creates the select statement for paging.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="topCount"></param>
        /// <param name="skipCount"></param>
        /// <param name="identityColumn"></param>
        /// <param name="identityColumnIsNumber"></param>
        /// <param name="groupBy"></param>
        /// <param name="includedColumns"></param>
        /// <returns></returns>
        public abstract string CreateSelectRangeStatement(string tableName,
                                                          string where,
                                                          string orderBy,
                                                          int topCount,
                                                          int skipCount,
                                                          string identityColumn,
                                                          bool identityColumnIsNumberOrSequence = true,
                                                          string groupBy = null,
                                                          params string[] includedColumns);

        public ISqlCriteriaExpression SqlCriteriaExpression()
        {
            return new SqlCriteriaExpression(this);
        }

        /// <summary>
        /// Gets the select sql script for the last inserted row auto ID statement.
        /// </summary>
        /// <value>The select last inserted row auto ID statement.</value>
        public abstract string SelectLastInsertedRowAutoIDStatement { get; }

        ///<summary>
        ///Gets the param prefix.
        /// </summary>
        /// <value>The param prefix.</value>
        public abstract char ParameterPrefix { get; }

        /// <summary>
        /// Gets the left token of parameter name. e.g. [, i.e. [post_id]
        /// </summary>
        public abstract char ParameterLeftToken { get; }

        /// <summary>
        /// Gets the right token of parameter name. e.g.], i.e. [post_id]
        /// </summary>
        public abstract char ParameterRightToken { get; }

        /// <summary>
        /// Gets the wild char token.
        /// </summary>
        public abstract char WildCharToken { get; }

        /// <summary>
        /// Gets the wild single char token.
        /// </summary>
        public abstract char WildSingleCharToken { get; }

    }

}

