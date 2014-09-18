#region 文档说明
// ================================================================================================
// Copyright (c) 1991-2006 Ifmsoft Development Co.,LTD.
// All Rights Reserved.
//
//         File Name: SqlHelper.cs
//       Description: 提供针对 SQL Server 2000 数据库访问的方法。
//                    对于原有实现仅进行代码注视汉化，代码并未改动，
//                    扩充的方法均有如下标识：
//                    [yyyy-mm-dd Bpusoft Created]
//
//           Creator: 高翌翔
//     Creation Date: 2006-04-05
//
//          Modifier: 高翌翔
// Modification Date: 2006-06-30
//      Illustration: 当查询语句执行时间超过缺省设置 30 秒后 SqlCommand 会抛出
//                    异常信息，而 SqlHelper 无法重新设置 CommandTimeout 属性，
//                    因此添加 CustomCommandTimeout 类属性，并修改方法
//                    PrepareCommand(... ...)
// 
//          Modifier: 
// Modification Date: 
//      Illustration: 
// 
// ================================================================================================
// 用于 .NET 的微软数据访问应用程序块
// http://msdn.microsoft.com/library/en-us/dnbda/html/daab-rm.asp
//
// 该文件包含 SqlHelper 和 SqlHelperParameterCache 两个类的实现。
//
// 更多信息详见 Accessory\Data_Access_Application_Block_Implementation_Overview.chm
// ================================================================================================
// 发布历史
// 版本	描述
// 2.0	添加对于 FillDataset, UpdateDataset 和 "Param" helper 方法的支持。
// ================================================================================================
// Copyright (C) 2000-2001 Microsoft Corporation
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
// ================================================================================================
#endregion 文档说明

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace ProtocolsManage.Common
{
	/// <summary>
	/// SqlHelper 类有意了封装一些对于 SqlClient 共通使用的高性能、可扩展的最佳实践。
	/// 该类为密封类，不能被继承。
	/// </summary>
    public static class SqlHelper
    {
        /// <summary>
        /// 用户指定的命令执行时间（CommandTimeout，单位：秒），缺省值为 0，
        /// 若需设置该属性，用户必须在每次调用 SqlHelper 的方法前设置，
        /// 形如：SqlHelper.CustomCommandTimeout = 120。
        /// </summary>
        public static int CustomCommandTimeout = 0;

        #region 私有实用方法和构造函数 private utility methods & constructors

	    /// <summary>
        /// 该方法用于将 SqlParameters[] 参数数组绑定到一个 SqlCommand 上。
        ///
        /// 该方法将为任何一个方向为 InputOutput 输入/输出且值为 null 的参数赋一个 DBNull.Value 值。
        ///
        /// 该行为将阻止使用默认值，但是这对于一个有意设置的纯粹的 output 输出参数（源自 InputOutput），
        /// 而用户并未提供输入值得情况并不太通用。
        /// </summary>
        /// <param name="command">即将添加参数的 command 命令</param>
        /// <param name="commandParameters">一个被添加到 command 命令的 SqlParameter[] 参数数组</param>
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if( command == null ) throw new ArgumentNullException( "command" );
            if( commandParameters != null )
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if( p != null )
                    {
                        // 检查输入的 output 输出值是否被赋值
                        if ( ( p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input ) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        /// <summary>
        /// 该方法将一个数据行各列的值赋给一个 SqlParameter[] 参数数组。
        /// </summary>
        /// <param name="commandParameters">即将被赋值的 SqlParameter[] 参数数组</param>
        /// <param name="dataRow">该数据行用于存放存储过程的参数值</param>
        private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
            {
                // 如果未获得任何数据，则直接返回
                return;
            }

            int i = 0;
            // 设置参数值
            foreach(SqlParameter commandParameter in commandParameters)
            {
                // 检查参数名
                if( commandParameter.ParameterName == null ||
                    commandParameter.ParameterName.Length <= 1 )
                {
                    throw new Exception( string.Format(
                        "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
                        i, commandParameter.ParameterName ) );
                }

                // 由于参数名前包含 @ 符号，因此使用 commandParameter.ParameterName.Substring(1)
                if (dataRow.Table.Columns.IndexOf( commandParameter.ParameterName.Substring(1)) != -1 )
                {
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                }

                i++;
            }
        }

        /// <summary>
        /// 该方法将一个数组的值赋给一个 SqlParameter[] 参数数组。
        /// </summary>
        /// <param name="commandParameters">即将被赋值的 SqlParameter[] 参数数组</param>
        /// <param name="parameterValues">object 对象数组，其中存放着即将被分配值</param>
        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // 如果未获得任何数据，则直接返回
                return;
            }

            // 我们必须保证值的数量与参数的数量相等
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // 遍历 SqlParameter[] 参数数组，将值数组中将相应位置的值赋给参数
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // 如果当前数组值继承自 IDbDataParameter 接口，那么为这些值分配 Value 属性
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if( paramInstance.Value == null )
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }

        /// <summary>
        /// 该方法为提供的 command 命令打开（如果必要）并分配一个连接 connection、事务 transaction、
        /// 命令类型 command type 及其 参数 parameters。
        /// </summary>
        /// <param name="command">准备处理的命令 SqlCommand</param>
        /// <param name="connection">一个有效的连接 SqlConnection，在其上执行这个命令 command</param>
        /// <param name="transaction">一个有效的事务 SqlTransaction，或者是 'null'</param>
        /// <param name="commandType">命令类型 CommandType （存储过程 stored procedure、文本 text 等等）</param>
        /// <param name="commandText">存储过程名 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个赋给该命令的参数数组，或者如果不需要参数则为'null'</param>
        /// <param name="mustCloseConnection"><c>true</c> 如果 connection 连接是被该方法打开的就是 true；否则为 false</param>
        private static void PrepareCommand( SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection )
        {
            if ( command == null ) throw new ArgumentNullException( "command" );
            if ( commandText == null || commandText.Length == 0 ) throw new ArgumentNullException( "commandText" );

            // 如果提供的连接 connection 没有打开，那么我们将打开它。
            if ( connection.State != ConnectionState.Open )
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // 为该命令分配 connection 连接
            command.Connection = connection;

            // 设置该命令的文本（存储过程名 或 SQL 语句）
            command.CommandText = commandText;

            /*
             * 若设置类属性 CustomCommandTimeout，则根据当前设置修改 SqlCommand 实例的 CommandTimeout 属性，
             * 之后清空 CustomCommandTimeout。
             * 
             * 若未设置类属性 CustomCommandTimeout，则使用 SqlCommand 实例的 CommandTimeout 的缺省属性。
             */
            if ( CustomCommandTimeout > 0 )
            {
                command.CommandTimeout = CustomCommandTimeout;
                CustomCommandTimeout = 0;
            }

            // 如果我们提供了一个事务，则赋给该命令
            if ( transaction != null )
            {
                if( transaction.Connection == null )
                {
                    throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
                }
                command.Transaction = transaction;
            }

            // 设置命令类型 command type
            command.CommandType = commandType;

            // 如果提供了参数，则绑定命令参数 commandParameters
            if ( commandParameters != null )
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        #endregion 私有实用方法和构造函数 private utility methods & constructors


        #region 执行无果语句 ExecuteNonQuery

        #region Bpusoft Created

        /// <summary>
        /// 对于连接字符串中指定的数据库执行一个 SqlCommand 命令
        /// （该命令不返回任何结果集，也不接受任何参数）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery( connString, strSql );
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery(string connectionString, string sqlText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteNonQuery( connectionString, CommandType.Text, sqlText, (SqlParameter[])null );
        }

        /// <summary>
        /// 对于连接字符串中指定的数据库使用提供的参数执行一个 SqlCommand 命令
        /// （该命令不返回任何结果集）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery( string connectionString, string sqlText, SqlParameter[] commandParameters )
        {
            // 调用重载方法
            return ExecuteNonQuery( connectionString, CommandType.Text, sqlText, commandParameters );
        }

        /// <summary>
        /// 对于指定的的 SqlConnection 连接实例执行一个 SqlCommand 命令
        /// （该命令不返回任何结果集，也不接受任何参数）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery( conn, strSql );
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery( SqlConnection connection, string sqlText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteNonQuery( connection, CommandType.Text, sqlText, (SqlParameter[])null );
        }

        /// <summary>
        /// 对于指定的的 SqlConnection 连接实例使用提供的参数执行一个 SqlCommand 命令
        /// （该命令不返回任何结果集）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery( conn, strSql, commandParameters );
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery( SqlConnection connection, string sqlText, SqlParameter[] commandParameters )
        {
            // 调用重载方法
            return ExecuteNonQuery( connection, CommandType.Text, sqlText, commandParameters );
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务执行一个 SqlCommand 命令（不返回结果集，也不接受任何参数）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery( trans, sqlText );
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery( SqlTransaction transaction, string sqlText )
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteNonQuery( transaction, CommandType.Text, sqlText, (SqlParameter[])null );
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数执行一个 SqlCommand 命令（不返回结果集）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery( trans, sqlText, commandParameters );
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery( SqlTransaction transaction, string sqlText, SqlParameter[] commandParameters )
        {
            // 调用重载方法
            return ExecuteNonQuery( transaction, CommandType.Text, sqlText, commandParameters );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static DataTable GetSchemaByTableName( string connectionString, string tableName )
        {
            string sqlText = string.Format("SELECT * FROM {0}", tableName);

            return GetSchema(connectionString, sqlText);
        }

        /// <summary>
        /// 填充表结构。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="sqlText">用于获取表结构的 SELECT 语句</param>
        /// <returns></returns>
        public static DataTable GetSchema( string connectionString, string sqlText )
        {
            DataTable dtTarget = new DataTable();

            using( SqlConnection conn = new SqlConnection(connectionString) )
            {
                SqlDataAdapter da = new SqlDataAdapter( sqlText, conn ); 

                conn.Open();

                da.FillSchema(dtTarget, SchemaType.Source);
            }

            return dtTarget;
        }

        #endregion Bpusoft Created

        /// <summary>
        /// 对于连接字符串中指定的数据库执行一个 SqlCommand 命令
        /// （该命令不返回任何结果集，也不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteNonQuery(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于连接字符串中指定的数据库使用提供的参数执行一个 SqlCommand 命令
        /// （该命令不返回任何结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );

            // 创建并打开一个 SqlConnection，并且用完后释放该连接所占用的所有资源。
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 调用重载方法，将 connection string 连接字符串替换为一个 connection 连接实例
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// 使用提供的参数，对于在连接字符串中指定的数据库通过 SqlCommand 执行一个存储过程 （返回值的不是结果集）。
        /// （当每个存储过程被第一次调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // 将提供的值基于参数的顺序分配给这些参数
                AssignParameterValues(commandParameters, parameterValues);

                // 调用一个重载方法，该方法可以接受一个 SqlParameter[] 参数数组
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteNonQuery( connectionString, CommandType.StoredProcedure, spName );
            }
        }

        /// <summary>
        /// 对于指定的的 SqlConnection 连接实例执行一个 SqlCommand 命令
        /// （该命令不返回任何结果集，也不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于指定的的 SqlConnection 连接实例使用提供的参数执行一个 SqlCommand 命令
        /// （该命令不返回任何结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery( SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters )
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );

            // 创建一个命令以备执行
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );

            // 最后，执行命令
            int retval = cmd.ExecuteNonQuery();

            // 从命令 command 对象中移出 SqlParameters 参数，以便 commandParameters 可以被再次使用
            cmd.Parameters.Clear();
            if( mustCloseConnection )
                connection.Close();
            return retval;
        }

        /// <summary>
        /// 对于指定的 SqlConnection 数据库连接使用提供的参数通过 SqlCommand 执行一个存储过程 （不返回结果集）。
        ///	（当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // 将提供的值基于参数的顺序分配给这些参数
                AssignParameterValues(commandParameters, parameterValues);

                // 调用一个重载方法，该方法可以接受一个 SqlParameter[] 参数数组
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务执行一个 SqlCommand 命令（不返回结果集，也不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery( SqlTransaction transaction, CommandType commandType, string commandText )
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数执行一个 SqlCommand 命令（不返回结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

            // 创建一个命令以备执行
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );

            // 最后，执行命令
            int retval = cmd.ExecuteNonQuery();

            // 从命令 command 对象中移出 SqlParameters 参数，以便 commandParameters 可以被再次使用
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        ///	对于指定的事务 SqlTransaction 使用提供的参数通过 SqlCommand 执行一个存储过程 （不返回结果集）。
        ///	（当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int result = ExecuteNonQuery(conn, trans, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // 将提供的值基于参数的顺序分配给这些参数
                AssignParameterValues(commandParameters, parameterValues);

                // 调用一个重载方法，该方法可以接受一个 SqlParameter[] 参数数组
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion 执行无果语句 ExecuteNonQuery

        #region 执行数据集 ExecuteDataset

        /// <summary>
        /// 对于连接字符串中指定的数据库执行一个 SqlCommand 命令，
        /// 并返回一个结果集（该命令不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个 dataset 数据集，其中包含由 command 命令产生的结果集。</returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于连接字符串中指定的数据库使用提供的参数执行一个 SqlCommand 命令，
        /// 并返回一个结果集。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个 dataset 数据集，其中包含由 command 命令产生的结果集。</returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );

            // 创建并打开一个 SqlConnection，并且用完后释放该连接所占用的所有资源。
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 调用重载方法，将 connection string 连接字符串替换为一个 connection 连接实例
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// 对于在连接字符串中指定的数据库使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程</param>
        /// <returns>一个 dataset 数据集，其中包含由 command 命令产生的结果集</returns>
        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // 将提供的值基于参数的顺序分配给这些参数
                AssignParameterValues(commandParameters, parameterValues);

                // 调用一个重载方法，该方法可以接受一个 SqlParameter[] 参数数组
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接执行一个 SqlCommand 命令（返回一个结果集，但不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个 dataset 数据集，其中包含由 command 命令产生的结果集。</returns>
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteDataset(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接使用提供的参数执行一个 SqlCommand 命令（返回一个结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个 dataset 数据集，其中包含由 command 命令产生的结果集。</returns>
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );

            // 创建一个命令以备执行
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );

            // 创建 DataAdapter 和 DataSet
            using( SqlDataAdapter da = new SqlDataAdapter(cmd) )
            {
                DataSet ds = new DataSet();

                // 使用用缺省值命名的 DataTable 数据表填充 DataSet 数据集
                da.Fill(ds);

                // 从命令 command 对象中移出 SqlParameters 参数，以便 commandParameters 可以被再次使用
                cmd.Parameters.Clear();

                if( mustCloseConnection )
                    connection.Close();

                // 返回 dataset 数据集
                return ds;
            }
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        /// <returns>一个 dataset 数据集，其中包含由 command 命令产生的结果集。</returns>
        public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // 将提供的值基于参数的顺序分配给这些参数
                AssignParameterValues(commandParameters, parameterValues);

                // 调用一个重载方法，该方法可以接受一个 SqlParameter[] 参数数组
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务执行一个 SqlCommand 命令（返回一个结果集，但不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个 dataset 数据集，其中包含由 command 命令产生的结果集。</returns>
        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteDataset(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数执行一个 SqlCommand 命令（返回一个结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个 dataset 数据集，其中包含由 command 命令产生的结果集。</returns>
        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

            // 创建一个命令以备执行
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );

            // 创建 DataAdapter 和 DataSet
            using( SqlDataAdapter da = new SqlDataAdapter(cmd) )
            {
                DataSet ds = new DataSet();

                // 使用用缺省值命名的 DataTable 数据表填充 DataSet 数据集
                da.Fill(ds);

                // 从命令 command 对象中移出 SqlParameters 参数，以便 commandParameters 可以被再次使用
                cmd.Parameters.Clear();

                // 返回 dataset 数据集
                return ds;
            }
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        /// <returns>一个 dataset 数据集，其中包含由 command 命令产生的结果集。</returns>
        public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // 将提供的值基于参数的顺序分配给这些参数
                AssignParameterValues(commandParameters, parameterValues);

                // 调用一个重载方法，该方法可以接受一个 SqlParameter[] 参数数组
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }


        #endregion 执行数据集 ExecuteDataset

        #region 执行数据表 ExecuteDataTable [2006-04-06 Bpusoft Created]

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sqlText"></param>
        public static void ExecuteNonQuery( IDbConnection connection, string sqlText )
        {
            if(connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            IDbCommand command = connection.CreateCommand();
            command.ExecuteNonQuery( sqlText );
        }
        */

        /// <summary>
        /// 对于连接字符串中指定的数据库使用提供的参数执行一个 SqlCommand 命令，
        /// 并返回一个数据表。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个 datatable 数据表，其中包含由 command 命令产生的结果集</returns>
        public static DataTable ExecuteDataTable( string connectionString, string sqlText, SqlParameter[] commandParameters )
        {
            return ExecuteDataset( connectionString, CommandType.Text, sqlText, commandParameters ).Tables[0];
        }

        /// <summary>
        /// 对于连接字符串中指定的数据库执行一个 SqlCommand 命令，
        /// 并返回一个数据表（该命令不接受任何参数）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <returns>一个 datatable 数据表，其中包含由 command 命令产生的结果集。</returns>
        public static DataTable ExecuteDataTable( string connectionString, string sqlText )
        {
            return ExecuteDataset( connectionString, CommandType.Text, sqlText ).Tables[0];
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接使用提供的参数执行一个 SqlCommand 命令
        /// （返回一个结果集）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个 datatable 数据表，其中包含由 command 命令产生的结果集</returns>
        public static DataTable ExecuteDataTable( SqlConnection connection, string sqlText, SqlParameter[] commandParameters )
        {
            return ExecuteDataset( connection, CommandType.Text, sqlText, commandParameters ).Tables[0];
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接执行一个 SqlCommand 命令
        /// （返回一个结果集，但不接受任何参数）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <returns>一个 datatable 数据表，其中包含由 command 命令产生的结果集</returns>
        public static DataTable ExecuteDataTable( SqlConnection connection, string sqlText)
        {
            return ExecuteDataset( connection, CommandType.Text, sqlText ).Tables[0];
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数执行一个 SqlCommand 命令
        /// （返回一个结果集）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个 datatable 数据表，其中包含由 command 命令产生的结果集</returns>
        public static DataTable ExecuteDataTable( SqlTransaction transaction, string sqlText, SqlParameter[] commandParameters )
        {
            return ExecuteDataset( transaction, CommandType.Text, sqlText, commandParameters ).Tables[0];
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务执行一个 SqlCommand 命令
        /// （返回一个结果集，但不接受任何参数）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <returns>一个 datatable 数据表，其中包含由 command 命令产生的结果集</returns>
        public static DataTable ExecuteDataTable( SqlTransaction transaction, string sqlText )
        {
            return ExecuteDataset( transaction, CommandType.Text, sqlText ).Tables[0];
        }

        #endregion 执行数据表 ExecuteDataTable [2006-04-06 Bpusoft Created]

        #region 执行只读查询 ExecuteReader

        /// <summary>
        /// 该枚举用于指明连接实例 connection 是由调用者提供的，还是由 SqlHelper 创建的，
        /// 以便我们在调用 ExecuteReader() 时可以设置适当的 CommandBehavior 命令行为。
        /// </summary>
        private enum SqlConnectionOwnership
        {
            /// <summary>
            /// SqlHelper 拥有并管理 connection 连接实例。
            /// </summary>
            Internal,
            /// <summary>
            /// 调用者 拥有并管理 connection 连接实例。
            /// </summary>
            External
        }

        /// <summary>
        /// 创建并准备一个 SqlCommand 命令，然后以适当的 CommandBehavior 命令行为调用 ExecuteReader。
        /// </summary>
        /// <remarks>
        /// 如果我们创建并打开连接实例，那么我们希望当 DataReader 被关闭后关闭该连接实例。
        ///
        /// 如果调用提供了连接实例，那么我们希望将它留给调用者去处理。
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例，在其上执行此命令</param>
        /// <param name="transaction">一个有效的 SqlTransaction 事务，或者是 'null'</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个与该命令关联的 SqlParameter[] 参数数组或者如果无须参数则为 'null'</param>
        /// <param name="connectionOwnership">指明 connection 连接实例参数是由调用者提供的，还是由 SqlHelper 创建的</param>
        /// <returns>一个 SqlDataReader，包含该命令产生的结果集</returns>
        private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );

            bool mustCloseConnection = false;
            // 创建一个命令以备执行
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );

                // 创建一个 SqlDataReader 实例
                SqlDataReader dataReader;

                // 以适当的 CommandBehavior 命令行为调用 ExecuteReader
                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }

                // 从命令 command 对象中移出 SqlParameters 参数，以便 commandParameters 可以被再次使用
                /*
                 * 注意：这里有一个问题，当该 reader 被关闭时，output 输出参数的值会丢失，
                 *      所以如果将这些参数与 command 命令分离，然后由 SqlReader 可以设置它们的值。
                 *      当这种情况发生时，这些参数可以再次被其他 command 命令使用。
                 */
                bool canClear = true;
                foreach(SqlParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }

                if (canClear)
                {
                    cmd.Parameters.Clear();
                }

                return dataReader;
            }
            catch
            {
                if( mustCloseConnection )
                    connection.Close();
                throw;
            }
        }

        /// <summary>
        /// 对于连接字符串中指定的数据库执行一个 SqlCommand 命令，
        /// 并返回一个结果集（该命令不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个 SqlDataReader，包含该 command 命令产生的结果集</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteReader(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于连接字符串中指定的数据库使用提供的参数执行一个 SqlCommand 命令，
        /// 返回一个 SqlDataReader。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个 SqlDataReader，包含该 command 命令产生的结果集</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                // 调用私有重载方法，使用内建连接实例替换连接字符串
                return ExecuteReader(connection, null, commandType, commandText, commandParameters,SqlConnectionOwnership.Internal);
            }
            catch
            {
                // 如果我们返回 SqlDatReader 失败，那么我们必须自己关闭连接实例
                if( connection != null ) connection.Close();
                throw;
            }

        }

        /// <summary>
        /// 对于在连接字符串中指定的数据库使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// SqlDataReader dr = ExecuteReader(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        /// <returns>一个 SqlDataReader，包含该 command 命令产生的结果集</returns>
        public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接执行一个 SqlCommand 命令（返回一个结果集，但不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个 SqlDataReader，包含该 command 命令产生的结果集</returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteReader(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接使用提供的参数执行一个 SqlCommand 命令（返回一个结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个 SqlDataReader，包含该 command 命令产生的结果集</returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            // 调用私有重载方法，该方法接受一个 null 值的事务和一个外建连接实例
            return ExecuteReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// SqlDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        /// <returns>一个 SqlDataReader，包含该 command 命令产生的结果集</returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务执行一个 SqlCommand 命令（返回一个结果集，但不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个 SqlDataReader，包含该 command 命令产生的结果集</returns>
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteReader(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数执行一个 SqlCommand 命令（返回一个结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个 SqlDataReader，包含该 command 命令产生的结果集</returns>
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

            // 调用重载方法，指明 connection 连接实例是由调用者提供的
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// SqlDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        /// <returns>一个 SqlDataReader，包含该 command 命令产生的结果集</returns>
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion 执行只读查询 ExecuteReader

        #region 执行单值查询 ExecuteScalar

        #region Bpusoft Created

        /// <summary>
        /// 对于连接字符串中指定的数据库执行一个 SqlCommand 命令，
        /// 并返回一个 1x1 的结果集（该命令不接受任何参数）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar( string connectionString, string sqlText )
        {
            return ExecuteScalar( connectionString, CommandType.Text, sqlText );
        }

        /// <summary>
        /// 对于连接字符串中指定的数据库使用提供的参数执行一个 SqlCommand 命令，
        /// 并返回一个 1x1 的结果集。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar( string connectionString, string sqlText, SqlParameter[] commandParameters )
        {
            return ExecuteScalar( connectionString, CommandType.Text, sqlText, commandParameters );
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接执行一个 SqlCommand 命令（返回一个 1x1 的结果集，但不接受任何参数）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar( SqlConnection connection, string sqlText )
        {
            return ExecuteScalar( connection, CommandType.Text, sqlText );
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接使用提供的参数执行一个 SqlCommand 命令（返回一个 1x1 的结果集）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar( SqlConnection connection, string sqlText, SqlParameter[] commandParameters )
        {
            return ExecuteScalar( connection, CommandType.Text, sqlText, commandParameters );
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务执行一个 SqlCommand 命令（返回一个 1x1 的结果集，但不接受任何参数）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar( SqlTransaction transaction, string sqlText )
        {
            return ExecuteScalar( transaction, CommandType.Text, sqlText );
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数执行一个 SqlCommand 命令（返回一个 1x1 的结果集）。
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="sqlText">T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar( SqlTransaction transaction, string sqlText, SqlParameter[] commandParameters )
        {
            return ExecuteScalar( transaction, CommandType.Text, sqlText, commandParameters );
        }

        #endregion Bpusoft Created

        /// <summary>
        /// 对于连接字符串中指定的数据库执行一个 SqlCommand 命令，
        /// 并返回一个 1x1 的结果集（该命令不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteScalar(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于连接字符串中指定的数据库使用提供的参数执行一个 SqlCommand 命令，
        /// 并返回一个 1x1 的结果集。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar( string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters )
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            // 创建并打开一个 SqlConnection，并且用完后释放该连接所占用的所有资源。
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 调用重载方法，将 connection string 连接字符串替换为一个 connection 连接实例
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// 对于在连接字符串中指定的数据库使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个 1x1 的结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int orderCount = (int)ExecuteScalar(connString, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // 将提供的值基于参数的顺序分配给这些参数
                AssignParameterValues(commandParameters, parameterValues);

                // 调用一个重载方法，该方法可以接受一个 SqlParameter[] 参数数组
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接执行一个 SqlCommand 命令（返回一个 1x1 的结果集，但不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteScalar(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接使用提供的参数执行一个 SqlCommand 命令（返回一个 1x1 的结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );

            // 创建一个命令以备执行
            SqlCommand cmd = new SqlCommand();

            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );

            // 执行 command 命令并返回结果集
            object retval = cmd.ExecuteScalar();

            // 从命令 command 对象中移出 SqlParameters 参数，以便 commandParameters 可以被再次使用
            cmd.Parameters.Clear();

            if( mustCloseConnection )
                connection.Close();

            return retval;
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接使用提供的参数执行一个 SqlCommand 命令（返回一个 1x1 的结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalarByCmd(SqlConnection connection, CommandType commandType, string commandText,SqlCommand cmd, params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // 创建一个命令以备执行
            //SqlCommand cmd = new SqlCommand();

            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // 执行 command 命令并返回结果集
            object retval = cmd.ExecuteScalar();

            // 从命令 command 对象中移出 SqlParameters 参数，以便 commandParameters 可以被再次使用
            cmd.Parameters.Clear();

            if (mustCloseConnection)
                connection.Close();

            return retval;
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个 1x1 的结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // 将提供的值基于参数的顺序分配给这些参数
                AssignParameterValues(commandParameters, parameterValues);

                // 调用一个重载方法，该方法可以接受一个 SqlParameter[] 参数数组
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务执行一个 SqlCommand 命令（返回一个 1x1 的结果集，但不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteScalar(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数执行一个 SqlCommand 命令（返回一个 1x1 的结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

            // 创建一个命令以备执行
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );

            // 执行 command 命令并返回结果集
            object retval = cmd.ExecuteScalar();

            // 从命令 command 对象中移出 SqlParameters 参数，以便 commandParameters 可以被再次使用
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个 1x1 的结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从参数缓存中取出（或者找到它们并放入缓存）该存储过程的参数
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // 将提供的值基于参数的顺序分配给这些参数
                AssignParameterValues(commandParameters, parameterValues);

                // 调用一个重载方法，该方法可以接受一个 SqlParameter[] 参数数组
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }


        #endregion 执行单值查询 ExecuteScalar

        #region 执行 XML 只读查询 ExecuteXmlReader
        /// <summary>
        /// 对于指定的 SqlConnection 连接执行一个 SqlCommand 命令（返回一个结果集，但不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">使用 "FOR XML AUTO" 的存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个 XmlReader，其中包含由 command 命令产生的结果集</returns>
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteXmlReader(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接使用提供的参数执行一个 SqlCommand 命令（返回一个结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">使用 "FOR XML AUTO" 的存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个 XmlReader，其中包含由 command 命令产生的结果集</returns>
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );

            bool mustCloseConnection = false;
            // 创建一个命令以备执行
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );

                // 创建 DataAdapter 和 DataSet
                XmlReader retval = cmd.ExecuteXmlReader();

                // 从命令 command 对象中移出 SqlParameters 参数，以便 commandParameters 可以被再次使用
                cmd.Parameters.Clear();

                return retval;
            }
            catch
            {
                if( mustCloseConnection )
                    connection.Close();
                throw;
            }
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// XmlReader r = ExecuteXmlReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="spName">使用 "FOR XML AUTO" 的存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        /// <returns>一个 XmlReader，其中包含由 command 命令产生的结果集</returns>
        public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // 将提供的值基于参数的顺序分配给这些参数
                AssignParameterValues(commandParameters, parameterValues);

                // 调用一个重载方法，该方法可以接受一个 SqlParameter[] 参数数组
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务执行一个 SqlCommand 命令（返回一个结果集，但不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">使用 "FOR XML AUTO" 的存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个 XmlReader，其中包含由 command 命令产生的结果集</returns>
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // 通过为 SqlParameter[] 参数提供 null 来调用重载方法
            return ExecuteXmlReader(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数执行一个 SqlCommand 命令（返回一个结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">使用 "FOR XML AUTO" 的存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <returns>一个 XmlReader，其中包含由 command 命令产生的结果集</returns>
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

            // 创建一个命令以备执行
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );

            // 创建 DataAdapter 和 DataSet
            XmlReader retval = cmd.ExecuteXmlReader();

            // 从命令 command 对象中移出 SqlParameters 参数，以便 commandParameters 可以被再次使用
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// XmlReader r = ExecuteXmlReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        /// <returns>一个 dataset 数据集，其中包含由 command 命令产生的结果集。</returns>
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // 将提供的值基于参数的顺序分配给这些参数
                AssignParameterValues(commandParameters, parameterValues);

                // 调用一个重载方法，该方法可以接受一个 SqlParameter[] 参数数组
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion 执行 XML 只读查询 ExecuteXmlReader

        #region 填充数据集 FillDataset
        /// <summary>
        /// 对于连接字符串中指定的数据库执行一个 SqlCommand 命令，
        /// 并返回一个结果集（该命令不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="dataSet">一个 dataset 数据集，其中将包含由 command 命令产生的结果集</param>
        /// <param name="tableNames">该数组将用于创建表映射，以便通过用户定义的表名（可能就是实际的表名）引用 DataTable</param>
        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );

            // 创建并打开一个 SqlConnection，并且用完后释放该连接所占用的所有资源。
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 调用重载方法，将 connection string 连接字符串替换为一个 connection 连接实例
                FillDataset(connection, commandType, commandText, dataSet, tableNames);
            }
        }

        /// <summary>
        /// 对于连接字符串中指定的数据库使用提供的参数执行一个 SqlCommand 命令
        /// （该命令不返回任何结果集，也不接受任何）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        /// <param name="dataSet">一个 dataset 数据集，其中将包含由 command 命令产生的结果集</param>
        /// <param name="tableNames">该数组将用于创建表映射，以便通过用户定义的表名（可能就是实际的表名）引用 DataTable</param>
        public static void FillDataset(string connectionString, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            // 创建并打开一个 SqlConnection，并且用完后释放该连接所占用的所有资源。
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 调用重载方法，将 connection string 连接字符串替换为一个 connection 连接实例
                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
        }

        /// <summary>
        /// 对于在连接字符串中指定的数据库使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, 24);
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataSet">一个 dataset 数据集，其中将包含由 command 命令产生的结果集</param>
        /// <param name="tableNames">该数组将用于创建表映射，以便通过用户定义的表名（可能就是实际的表名）引用 DataTable</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        public static void FillDataset(string connectionString, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            // 创建并打开一个 SqlConnection，并且用完后释放该连接所占用的所有资源。
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 调用重载方法，将 connection string 连接字符串替换为一个 connection 连接实例
                FillDataset (connection, spName, dataSet, tableNames, parameterValues);
            }
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接执行一个 SqlCommand 命令（返回一个结果集，但不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="dataSet">一个 dataset 数据集，其中将包含由 command 命令产生的结果集</param>
        /// <param name="tableNames">该数组将用于创建表映射，以便通过用户定义的表名（可能就是实际的表名）引用 DataTable</param>
        public static void FillDataset(SqlConnection connection, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接使用提供的参数执行一个 SqlCommand 命令（返回一个结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="dataSet">一个 dataset 数据集，其中将包含由 command 命令产生的结果集</param>
        /// <param name="tableNames">该数组将用于创建表映射，以便通过用户定义的表名（可能就是实际的表名）引用 DataTable</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        public static void FillDataset(SqlConnection connection, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// FillDataset(conn, "GetOrders", ds, new string[] {"orders"}, 24, 36);
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataSet">一个 dataset 数据集，其中将包含由 command 命令产生的结果集</param>
        /// <param name="tableNames">该数组将用于创建表映射，以便通过用户定义的表名（可能就是实际的表名）引用 DataTable</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        public static void FillDataset(SqlConnection connection, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if ( connection == null ) throw new ArgumentNullException( "connection" );
            if (dataSet == null ) throw new ArgumentNullException( "dataSet" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // 将提供的值基于参数的顺序分配给这些参数
                AssignParameterValues(commandParameters, parameterValues);

                // 调用一个重载方法，该方法可以接受一个 SqlParameter[] 参数数组
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务执行一个 SqlCommand 命令（返回一个结果集，但不接受任何参数）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="dataSet">一个 dataset 数据集，其中将包含由 command 命令产生的结果集</param>
        /// <param name="tableNames">该数组将用于创建表映射，以便通过用户定义的表名（可能就是实际的表名）引用 DataTable</param>
        public static void FillDataset(SqlTransaction transaction, CommandType commandType,
            string commandText,
            DataSet dataSet, string[] tableNames)
        {
            FillDataset (transaction, commandType, commandText, dataSet, tableNames, null);
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数执行一个 SqlCommand 命令（返回一个结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="dataSet">一个 dataset 数据集，其中将包含由 command 命令产生的结果集</param>
        /// <param name="tableNames">该数组将用于创建表映射，以便通过用户定义的表名（可能就是实际的表名）引用 DataTable</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        public static void FillDataset(SqlTransaction transaction, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用提供的参数通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// 注：该方法不能访问 output 参数或者是存储过程的返回值参数。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// FillDataset(trans, "GetOrders", ds, new string[]{"orders"}, 24, 36);
        /// </remarks>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataSet">一个 dataset 数据集，其中将包含由 command 命令产生的结果集</param>
        /// <param name="tableNames">该数组将用于创建表映射，以便通过用户定义的表名（可能就是实际的表名）引用 DataTable</param>
        /// <param name="parameterValues">一个对象数组，其中的值将被作为输入值赋给存储过程。</param>
        public static void FillDataset(SqlTransaction transaction, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // 将提供的值基于参数的顺序分配给这些参数
                AssignParameterValues(commandParameters, parameterValues);

                // 调用一个重载方法，该方法可以接受一个 SqlParameter[] 参数数组
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else
            {
                // 否则我们可以仅调用该存储过程，而无需任何参数
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        /// <summary>
        /// 私有的辅助方法，该方法对于指定的 SqlTransaction 事务和 SqlConnection 连接实例
        /// 使用提供的参数执行一个 SqlCommand 命令（返回一个结果集）。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例</param>
        /// <param name="transaction">一个有效的 SqlTransaction 事务</param>
        /// <param name="commandType">命令类型（存储过程 CommandType.StoredProcedure，文本 CommandType.Text 等）</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="dataSet">一个 dataset 数据集，其中将包含由 command 命令产生的结果集</param>
        /// <param name="tableNames">该数组将用于创建表映射，以便通过用户定义的表名（可能就是实际的表名）引用 DataTable</param>
        /// <param name="commandParameters">一个用于在命令中执行的 SqlParameter[] 数组</param>
        private static void FillDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );

            // 创建一个命令以备执行
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );

            // 创建 DataAdapter 和 DataSet
            using( SqlDataAdapter dataAdapter = new SqlDataAdapter(command) )
            {
                // 添加由用户指定的表映射
                if (tableNames != null && tableNames.Length > 0)
                {
                    string tableName = "Table";
                    for (int index=0; index < tableNames.Length; index++)
                    {
                        if( tableNames[index] == null || tableNames[index].Length == 0 ) throw new ArgumentException( "The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames" );
                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName += (index + 1).ToString();
                    }
                }

                // 使用用缺省值命名的 DataTable 数据表填充 DataSet 数据集
                dataAdapter.Fill(dataSet);

                // 从命令 command 对象中移出 SqlParameters 参数，以便 commandParameters 可以被再次使用
                command.Parameters.Clear();
            }

            if( mustCloseConnection )
                connection.Close();
        }

        #endregion 填充数据集 FillDataset

        #region 更新数据集 UpdateDataset

        /// <summary>
        /// 对于在数据集中每次插入、更新、删除行执行各自的 command 命令。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order");
        /// </remarks>
        /// <param name="insertCommand">一个有效的用于向数据源中插入新记录的 transact-SQL 语句或者存储过程</param>
        /// <param name="deleteCommand">一个有效的用于从数据源中删除记录的 transact-SQL 语句或者存储过程</param>
        /// <param name="updateCommand">一个有效的用于在数据源中更新记录的 transact-SQL 语句或者存储过程</param>
        /// <param name="dataSet">用于更新数据源的 DataSet 数据集</param>
        /// <param name="tableName">用于表映射的源表的名称</param>
        public static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
        {
            if( insertCommand == null ) throw new ArgumentNullException( "insertCommand" );
            if( deleteCommand == null ) throw new ArgumentNullException( "deleteCommand" );
            if( updateCommand == null ) throw new ArgumentNullException( "updateCommand" );
            if( tableName == null || tableName.Length == 0 ) throw new ArgumentNullException( "tableName" );

            // 创建一个 SqlDataAdapter，并在用完后释放所占用的资源
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
            {
                // 设置 SqlDataAdapter 命令
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;

                // 更新数据源被改变的 DataSet 数据集
                dataAdapter.Update (dataSet, tableName);

                // 提交对于 DataSet 数据集全部的改变
                dataSet.AcceptChanges();
            }
        }

        #endregion 更新数据集 UpdateDataset

        #region 创建 Command 命令 CreateCommand

        /// <summary>
        /// 通过允许提供存储过程以及可选参数来简化 SqlCommand 的创建。
        /// </summary>
        /// <remarks>
        /// 示例：
        /// SqlCommand command = CreateCommand(conn, "AddCustomer", "CustomerID", "CustomerName");
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="sourceColumns">一个字符串数组，被赋予该存储过程的数据源列的列名</param>
        /// <returns>一个有效的 SqlCommand 对象</returns>
        public static SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 创建一个 SqlCommand 实例
            SqlCommand cmd = new SqlCommand( spName, connection );
            cmd.CommandType = CommandType.StoredProcedure;

            // 如果我们收到了参数值，那么我们需要指出它们的去向
            if ((sourceColumns != null) && (sourceColumns.Length > 0))
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // 基于参数的顺序，将提供的 SourceColumn 数据源列赋给这些参数
                for (int index=0; index < sourceColumns.Length; index++)
                    commandParameters[index].SourceColumn = sourceColumns[index];

                // 将找到的参数绑定到 SqlCommand 对象上
                AttachParameters (cmd, commandParameters);
            }

            return cmd;
        }

        #endregion 创建 Command 命令 CreateCommand

        #region 执行强类型参数的无果语句 ExecuteNonQueryTypedParams

        /// <summary>
        /// 对于连接字符串中指定的数据库使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （不返回结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQueryTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接实例使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （不返回结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="connection">一个有效的 SqlConnection 连接实例 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQueryTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （不返回结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="transaction">一个有效的 SqlTransaction 事务 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个整数，表明该命令所影响的行数。</returns>
        public static int ExecuteNonQueryTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion 执行强类型参数的无果语句 ExecuteNonQueryTypedParams

        #region 执行强类型参数的数据集 ExecuteDatasetTypedParams

        /// <summary>
        /// 对于连接字符串中指定的数据库使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个 dataset 数据集，其中包含由 command 命令产生的结果集。</returns>
        public static DataSet ExecuteDatasetTypedParams(string connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if ( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接实例使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="connection">一个有效的 SqlConnection 连接实例 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个 dataset 数据集，其中包含由 command 命令产生的结果集。</returns>
        public static DataSet ExecuteDatasetTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="transaction">一个有效的 SqlTransaction 事务 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个 dataset 数据集，其中包含由 command 命令产生的结果集。</returns>
        public static DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion 执行强类型参数的数据集 ExecuteDatasetTypedParams

        #region 执行强类型参数的只读查询 ExecuteReaderTypedParams

        /// <summary>
        /// 对于连接字符串中指定的数据库使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个 SqlDataReader，包含该 command 命令产生的结果集</returns>
        public static SqlDataReader ExecuteReaderTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if ( dataRow != null && dataRow.ItemArray.Length > 0 )
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接实例使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="connection">一个有效的 SqlConnection 连接实例 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个 SqlDataReader，包含该 command 命令产生的结果集</returns>
        public static SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="transaction">一个有效的 SqlTransaction 事务 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个 SqlDataReader，包含该 command 命令产生的结果集</returns>
        public static SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if( dataRow != null && dataRow.ItemArray.Length > 0 )
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion 执行强类型参数的只读查询 ExecuteReaderTypedParams

        #region 执行强类型参数的单值查询 ExecuteScalarTypedParams

        /// <summary>
        /// 对于连接字符串中指定的数据库使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （返回一个 1x1 的结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalarTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlConnection 连接实例使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （返回一个 1x1 的结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="connection">一个有效的 SqlConnection 连接实例 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalarTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （返回一个 1x1 的结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="transaction">一个有效的 SqlTransaction 事务 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个对象，其中包含由 command 命令产生的 1x1 结果集的值</returns>
        public static object ExecuteScalarTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }
        
        #endregion 执行强类型参数的单值查询 ExecuteScalarTypedParams

        #region 执行强类型参数的 XML 只读查询 ExecuteXmlReaderTypedParams

        /// <summary>
        /// 对于指定的 SqlConnection 连接实例使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="connection">一个有效的 SqlConnection 连接实例 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个 XmlReader，其中包含由 command 命令产生的结果集</returns>
        public static XmlReader ExecuteXmlReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 对于指定的 SqlTransaction 事务使用 dataRow 各列的值作为该存储过程的参数值
        /// 通过 SqlCommand 执行一个存储过程 （返回一个结果集）。
        /// （当每个存储过程第一次被调用时）该方法将在数据库中为存储过程查找参数，并基于参数的顺序赋值。
        /// </summary>
        /// <param name="transaction">一个有效的 SqlTransaction 事务 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="dataRow">用于盛放存储过程参数值得 DataRow 数据行</param>
        /// <returns>一个 XmlReader，其中包含由 command 命令产生的结果集</returns>
        public static XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // 如果该行有值，那么该存储过程必须被初始化
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // 从参数缓存中取出该存储过程的参数数组（或者找到它们并放到缓存中）
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // 设置参数值
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        
        #endregion 执行强类型参数的 XML 只读查询 ExecuteXmlReaderTypedParams

    }

    /// <summary>
    ///	SqlHelperParameterCache 提供了一些函数，
    ///	用于平衡在运行时存储过程参数的静态缓存与查找存储过程参数的能力。
    /// </summary>
    public sealed class SqlHelperParameterCache
    {
        #region 私有方法、变量以及构造函数 private methods, variables, and constructors

        /// <summary>
        /// 由于该类仅提供一些静态方法，因此设置默认构造函数为 private，
        /// 以阻止使用 "new SqlHelperParameterCache()" 来创建实例。
        /// </summary>
        private SqlHelperParameterCache() {}

        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 在运行时为一个存储过程解析匹配的的 SqlParameter[] 参数数组。
        /// </summary>
        /// <param name="connection">一个有效的 SqlConnection 连接实例 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="includeReturnValueParameter">是否包含它们的 return value parameter 返回值参数</param>
        /// <returns>查找到的 SqlParameter[] 参数数组</returns>
        private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            SqlCommand cmd = new SqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();

            if (!includeReturnValueParameter)
            {
                // 移除返回值参数
                cmd.Parameters.RemoveAt(0);
            }

            SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];

            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // 使用 DBNull.Value 初始化参数
            foreach (SqlParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }

        /// <summary>
        /// 深度拷贝缓存的 SqlParameter[] 数组。
        /// </summary>
        /// <param name="originalParameters">原始的参数数组</param>
        /// <returns>深度拷贝的参数数组</returns>
        private static SqlParameter[] CloneParameters( SqlParameter[] originalParameters )
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        #endregion 私有方法、变量以及构造函数 private methods, variables, and constructors

        #region 缓存函数 caching functions

        /// <summary>
        /// 将 SqlParameter[] 参数数组添加到缓存中。
        /// </summary>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <param name="commandParameters">一个将被缓存的 SqlParameter[] 参数数组。</param>
        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( commandText == null || commandText.Length == 0 ) throw new ArgumentNullException( "commandText" );

            string hashKey = connectionString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }

        /// <summary>
        /// 从缓存中取回 SqlParameter[] 参数数组。
        /// </summary>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="commandText">存储过程名称 或 T-SQL 命令</param>
        /// <returns>一个 SqlParameter[] 参数数组</returns>
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( commandText == null || commandText.Length == 0 ) throw new ArgumentNullException( "commandText" );

            string hashKey = connectionString + ":" + commandText;

            SqlParameter[] cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }

        #endregion 缓存函数 caching functions

        #region 参数查找函数 Parameter Discovery Functions

        /// <summary>
        /// 为该存储过程取回一个匹配的参数数组。
        /// </summary>
        /// <remarks>
        /// 该方法将在数据库中查询这些信息，并存储到缓存中以备将来使用。
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <returns>一个 SqlParameter[] 参数数组</returns>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        /// <summary>
        /// 为该存储过程取回一个匹配的参数数组。
        /// </summary>
        /// <remarks>
        /// 该方法将在数据库中查询这些信息，并存储到缓存中以备将来使用。
        /// </remarks>
        /// <param name="connectionString">一个有效的用于建立 SqlConnection 的连接字符串</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="includeReturnValueParameter">一个布尔值，指示返回值参数是否应该包括在结果中</param>
        /// <returns>一个 SqlParameter[] 参数数组</returns>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// 为该存储过程取回一个匹配的参数数组。
        /// </summary>
        /// <remarks>
        /// 该方法将在数据库中查询这些信息，并存储到缓存中以备将来使用。
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例 object</param>
        /// <param name="spName">存储过程名</param>
        /// <returns>一个 SqlParameter[] 参数数组</returns>
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
        {
            return GetSpParameterSet(connection, spName, false);
        }

        /// <summary>
        /// 为该存储过程取回一个匹配的参数数组。
        /// </summary>
        /// <remarks>
        /// 该方法将在数据库中查询这些信息，并存储到缓存中以备将来使用。
        /// </remarks>
        /// <param name="connection">一个有效的 SqlConnection 连接实例 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="includeReturnValueParameter">一个布尔值，指示返回值参数是否应该包括在结果中</param>
        /// <returns>一个 SqlParameter[] 参数数组</returns>
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            using (SqlConnection clonedConnection = (SqlConnection)((ICloneable)connection).Clone())
            {
                return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// 为该存储过程取回一个匹配的参数数组。
        /// </summary>
        /// <param name="connection">一个有效的 SqlConnection 连接实例 object</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="includeReturnValueParameter">一个布尔值，指示返回值参数是否应该包括在结果中</param>
        /// <returns>一个 SqlParameter[] 参数数组</returns>
        private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter":"");

            SqlParameter[] cachedParameters;

            cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                paramCache[hashKey] = spParameters;
                cachedParameters = spParameters;
            }

            return CloneParameters(cachedParameters);
        }

        #endregion 参数查找函数 Parameter Discovery Functions
    }
}
