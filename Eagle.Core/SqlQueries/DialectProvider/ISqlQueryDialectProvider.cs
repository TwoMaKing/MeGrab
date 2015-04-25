using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Core.SqlQueries.DialectProvider
{
    public interface ISqlQueryDialectProvider
    {
        /// <summary>
        /// Discovers params from SQL text.
        /// E.g. insert into [user] values (@user_name, @user_email, @user_password). 
        /// Having 3 parameters: @user_name, @user_email, @user_password.
        /// </summary>
        /// <param name="sql">The full or part of SQL text.</param>
        /// <returns>The discovered params.</returns>
        string[] DiscoverParams(string sql);

        /// <summary>
        /// Builds the name of the parameter. 
        /// E.g. for MS SQL add a '@' char at the begin with the column name. e.g. @user_name, @user_email
        /// insert into [user] values (@user_name, @user_email)
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        string BuildParameterName(string name);

        /// <summary>
        /// Builds the name of the column or table.
        /// E.g. for MS SQL add '[' and ']' at the left and right of the column name.
        /// e.g. [user_name], [user_email]
        /// update [user] set [user_name]=@user_name, [user_email]=@user_email where [user_id]=@user_id
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        string BuildColumnName(string name);

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
        string CreateSelectRangeStatement(string tableName,
                                          string where,
                                          string orderBy,
                                          int topCount,
                                          int skipCount,
                                          string identityColumn,
                                          bool identityColumnIsNumberOrSequence = true,
                                          string groupBy = null,
                                          params string[] includedColumns);

        /// <summary>
        /// Gets the instance of ISqlCriteriaExpression.
        /// </summary>
        /// <returns>The instance of ISqlCriteriaExpression</returns>
        ISqlCriteriaExpression SqlCriteriaExpression();

        /// <summary>
        /// Gets the select sql script for the last inserted row auto ID statement.
        /// </summary>
        /// <value>The select last inserted row auto ID statement.</value>
       string SelectLastInsertedRowAutoIDStatement { get; }

        ///<summary>
        ///Gets the param prefix.
        /// </summary>
        /// <value>The param prefix.</value>
        char ParameterPrefix { get; }

        /// <summary>
        /// Gets the left token of parameter name. e.g. [, i.e. [post_id]
        /// </summary>
        char ParameterLeftToken { get; }

        /// <summary>
        /// Gets the right token of parameter name. e.g.], i.e. [post_id]
        /// </summary>
        char ParameterRightToken { get; }

        /// <summary>
        /// Gets the wild char token.
        /// </summary>
        char WildCharToken { get; }

        /// <summary>
        /// Gets the wild single char token.
        /// </summary>
       char WildSingleCharToken { get; }
    }
}
