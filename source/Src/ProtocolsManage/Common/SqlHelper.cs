#region �ĵ�˵��
// ================================================================================================
// Copyright (c) 1991-2006 Ifmsoft Development Co.,LTD.
// All Rights Reserved.
//
//         File Name: SqlHelper.cs
//       Description: �ṩ��� SQL Server 2000 ���ݿ���ʵķ�����
//                    ����ԭ��ʵ�ֽ����д���ע�Ӻ��������벢δ�Ķ���
//                    ����ķ����������±�ʶ��
//                    [yyyy-mm-dd Bpusoft Created]
//
//           Creator: ������
//     Creation Date: 2006-04-05
//
//          Modifier: ������
// Modification Date: 2006-06-30
//      Illustration: ����ѯ���ִ��ʱ�䳬��ȱʡ���� 30 ��� SqlCommand ���׳�
//                    �쳣��Ϣ���� SqlHelper �޷��������� CommandTimeout ���ԣ�
//                    ������ CustomCommandTimeout �����ԣ����޸ķ���
//                    PrepareCommand(... ...)
// 
//          Modifier: 
// Modification Date: 
//      Illustration: 
// 
// ================================================================================================
// ���� .NET ��΢�����ݷ���Ӧ�ó����
// http://msdn.microsoft.com/library/en-us/dnbda/html/daab-rm.asp
//
// ���ļ����� SqlHelper �� SqlHelperParameterCache �������ʵ�֡�
//
// ������Ϣ��� Accessory\Data_Access_Application_Block_Implementation_Overview.chm
// ================================================================================================
// ������ʷ
// �汾	����
// 2.0	��Ӷ��� FillDataset, UpdateDataset �� "Param" helper ������֧�֡�
// ================================================================================================
// Copyright (C) 2000-2001 Microsoft Corporation
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
// ================================================================================================
#endregion �ĵ�˵��

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace ProtocolsManage.Common
{
	/// <summary>
	/// SqlHelper �������˷�װһЩ���� SqlClient ��ͨʹ�õĸ����ܡ�����չ�����ʵ����
	/// ����Ϊ�ܷ��࣬���ܱ��̳С�
	/// </summary>
    public static class SqlHelper
    {
        /// <summary>
        /// �û�ָ��������ִ��ʱ�䣨CommandTimeout����λ���룩��ȱʡֵΪ 0��
        /// �������ø����ԣ��û�������ÿ�ε��� SqlHelper �ķ���ǰ���ã�
        /// ���磺SqlHelper.CustomCommandTimeout = 120��
        /// </summary>
        public static int CustomCommandTimeout = 0;

        #region ˽��ʵ�÷����͹��캯�� private utility methods & constructors

	    /// <summary>
        /// �÷������ڽ� SqlParameters[] ��������󶨵�һ�� SqlCommand �ϡ�
        ///
        /// �÷�����Ϊ�κ�һ������Ϊ InputOutput ����/�����ֵΪ null �Ĳ�����һ�� DBNull.Value ֵ��
        ///
        /// ����Ϊ����ֹʹ��Ĭ��ֵ�����������һ���������õĴ���� output ���������Դ�� InputOutput����
        /// ���û���δ�ṩ����ֵ���������̫ͨ�á�
        /// </summary>
        /// <param name="command">������Ӳ����� command ����</param>
        /// <param name="commandParameters">һ������ӵ� command ����� SqlParameter[] ��������</param>
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if( command == null ) throw new ArgumentNullException( "command" );
            if( commandParameters != null )
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if( p != null )
                    {
                        // �������� output ���ֵ�Ƿ񱻸�ֵ
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
        /// �÷�����һ�������и��е�ֵ����һ�� SqlParameter[] �������顣
        /// </summary>
        /// <param name="commandParameters">��������ֵ�� SqlParameter[] ��������</param>
        /// <param name="dataRow">�����������ڴ�Ŵ洢���̵Ĳ���ֵ</param>
        private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
            {
                // ���δ����κ����ݣ���ֱ�ӷ���
                return;
            }

            int i = 0;
            // ���ò���ֵ
            foreach(SqlParameter commandParameter in commandParameters)
            {
                // ��������
                if( commandParameter.ParameterName == null ||
                    commandParameter.ParameterName.Length <= 1 )
                {
                    throw new Exception( string.Format(
                        "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
                        i, commandParameter.ParameterName ) );
                }

                // ���ڲ�����ǰ���� @ ���ţ����ʹ�� commandParameter.ParameterName.Substring(1)
                if (dataRow.Table.Columns.IndexOf( commandParameter.ParameterName.Substring(1)) != -1 )
                {
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                }

                i++;
            }
        }

        /// <summary>
        /// �÷�����һ�������ֵ����һ�� SqlParameter[] �������顣
        /// </summary>
        /// <param name="commandParameters">��������ֵ�� SqlParameter[] ��������</param>
        /// <param name="parameterValues">object �������飬���д���ż���������ֵ</param>
        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // ���δ����κ����ݣ���ֱ�ӷ���
                return;
            }

            // ���Ǳ��뱣ֵ֤��������������������
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // ���� SqlParameter[] �������飬��ֵ�����н���Ӧλ�õ�ֵ��������
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // �����ǰ����ֵ�̳��� IDbDataParameter �ӿڣ���ôΪ��Щֵ���� Value ����
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
        /// �÷���Ϊ�ṩ�� command ����򿪣������Ҫ��������һ������ connection������ transaction��
        /// �������� command type ���� ���� parameters��
        /// </summary>
        /// <param name="command">׼����������� SqlCommand</param>
        /// <param name="connection">һ����Ч������ SqlConnection��������ִ��������� command</param>
        /// <param name="transaction">һ����Ч������ SqlTransaction�������� 'null'</param>
        /// <param name="commandType">�������� CommandType ���洢���� stored procedure���ı� text �ȵȣ�</param>
        /// <param name="commandText">�洢������ �� T-SQL ����</param>
        /// <param name="commandParameters">һ������������Ĳ������飬�����������Ҫ������Ϊ'null'</param>
        /// <param name="mustCloseConnection"><c>true</c> ��� connection �����Ǳ��÷����򿪵ľ��� true������Ϊ false</param>
        private static void PrepareCommand( SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection )
        {
            if ( command == null ) throw new ArgumentNullException( "command" );
            if ( commandText == null || commandText.Length == 0 ) throw new ArgumentNullException( "commandText" );

            // ����ṩ������ connection û�д򿪣���ô���ǽ�������
            if ( connection.State != ConnectionState.Open )
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // Ϊ��������� connection ����
            command.Connection = connection;

            // ���ø�������ı����洢������ �� SQL ��䣩
            command.CommandText = commandText;

            /*
             * ������������ CustomCommandTimeout������ݵ�ǰ�����޸� SqlCommand ʵ���� CommandTimeout ���ԣ�
             * ֮����� CustomCommandTimeout��
             * 
             * ��δ���������� CustomCommandTimeout����ʹ�� SqlCommand ʵ���� CommandTimeout ��ȱʡ���ԡ�
             */
            if ( CustomCommandTimeout > 0 )
            {
                command.CommandTimeout = CustomCommandTimeout;
                CustomCommandTimeout = 0;
            }

            // ��������ṩ��һ�������򸳸�������
            if ( transaction != null )
            {
                if( transaction.Connection == null )
                {
                    throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
                }
                command.Transaction = transaction;
            }

            // ������������ command type
            command.CommandType = commandType;

            // ����ṩ�˲��������������� commandParameters
            if ( commandParameters != null )
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        #endregion ˽��ʵ�÷����͹��캯�� private utility methods & constructors


        #region ִ���޹���� ExecuteNonQuery

        #region Bpusoft Created

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ִ��һ�� SqlCommand ����
        /// ������������κν������Ҳ�������κβ�������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery( connString, strSql );
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery(string connectionString, string sqlText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteNonQuery( connectionString, CommandType.Text, sqlText, (SqlParameter[])null );
        }

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ����
        /// ������������κν��������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery( string connectionString, string sqlText, SqlParameter[] commandParameters )
        {
            // �������ط���
            return ExecuteNonQuery( connectionString, CommandType.Text, sqlText, commandParameters );
        }

        /// <summary>
        /// ����ָ���ĵ� SqlConnection ����ʵ��ִ��һ�� SqlCommand ����
        /// ������������κν������Ҳ�������κβ�������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery( conn, strSql );
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery( SqlConnection connection, string sqlText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteNonQuery( connection, CommandType.Text, sqlText, (SqlParameter[])null );
        }

        /// <summary>
        /// ����ָ���ĵ� SqlConnection ����ʵ��ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ����
        /// ������������κν��������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery( conn, strSql, commandParameters );
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery( SqlConnection connection, string sqlText, SqlParameter[] commandParameters )
        {
            // �������ط���
            return ExecuteNonQuery( connection, CommandType.Text, sqlText, commandParameters );
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ִ��һ�� SqlCommand ��������ؽ������Ҳ�������κβ�������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery( trans, sqlText );
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery( SqlTransaction transaction, string sqlText )
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteNonQuery( transaction, CommandType.Text, sqlText, (SqlParameter[])null );
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ��������ؽ��������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery( trans, sqlText, commandParameters );
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery( SqlTransaction transaction, string sqlText, SqlParameter[] commandParameters )
        {
            // �������ط���
            return ExecuteNonQuery( transaction, CommandType.Text, sqlText, commandParameters );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString">���ݿ������ַ���</param>
        /// <param name="tableName">����</param>
        /// <returns></returns>
        public static DataTable GetSchemaByTableName( string connectionString, string tableName )
        {
            string sqlText = string.Format("SELECT * FROM {0}", tableName);

            return GetSchema(connectionString, sqlText);
        }

        /// <summary>
        /// ����ṹ��
        /// </summary>
        /// <param name="connectionString">���ݿ������ַ���</param>
        /// <param name="sqlText">���ڻ�ȡ��ṹ�� SELECT ���</param>
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
        /// ���������ַ�����ָ�������ݿ�ִ��һ�� SqlCommand ����
        /// ������������κν������Ҳ�������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteNonQuery(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ����
        /// ������������κν��������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );

            // ��������һ�� SqlConnection������������ͷŸ�������ռ�õ�������Դ��
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // �������ط������� connection string �����ַ����滻Ϊһ�� connection ����ʵ��
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// ʹ���ṩ�Ĳ����������������ַ�����ָ�������ݿ�ͨ�� SqlCommand ִ��һ���洢���� ������ֵ�Ĳ��ǽ��������
        /// ����ÿ���洢���̱���һ�ε���ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // ���ṩ��ֵ���ڲ�����˳��������Щ����
                AssignParameterValues(commandParameters, parameterValues);

                // ����һ�����ط������÷������Խ���һ�� SqlParameter[] ��������
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteNonQuery( connectionString, CommandType.StoredProcedure, spName );
            }
        }

        /// <summary>
        /// ����ָ���ĵ� SqlConnection ����ʵ��ִ��һ�� SqlCommand ����
        /// ������������κν������Ҳ�������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ����ָ���ĵ� SqlConnection ����ʵ��ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ����
        /// ������������κν��������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery( SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters )
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );

            // ����һ�������Ա�ִ��
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );

            // ���ִ������
            int retval = cmd.ExecuteNonQuery();

            // ������ command �������Ƴ� SqlParameters �������Ա� commandParameters ���Ա��ٴ�ʹ��
            cmd.Parameters.Clear();
            if( mustCloseConnection )
                connection.Close();
            return retval;
        }

        /// <summary>
        /// ����ָ���� SqlConnection ���ݿ�����ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� �������ؽ��������
        ///	����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // ���ṩ��ֵ���ڲ�����˳��������Щ����
                AssignParameterValues(commandParameters, parameterValues);

                // ����һ�����ط������÷������Խ���һ�� SqlParameter[] ��������
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ִ��һ�� SqlCommand ��������ؽ������Ҳ�������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery( SqlTransaction transaction, CommandType commandType, string commandText )
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ��������ؽ��������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

            // ����һ�������Ա�ִ��
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );

            // ���ִ������
            int retval = cmd.ExecuteNonQuery();

            // ������ command �������Ƴ� SqlParameters �������Ա� commandParameters ���Ա��ٴ�ʹ��
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        ///	����ָ�������� SqlTransaction ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� �������ؽ��������
        ///	����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int result = ExecuteNonQuery(conn, trans, "PublishOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // ���ṩ��ֵ���ڲ�����˳��������Щ����
                AssignParameterValues(commandParameters, parameterValues);

                // ����һ�����ط������÷������Խ���һ�� SqlParameter[] ��������
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ִ���޹���� ExecuteNonQuery

        #region ִ�����ݼ� ExecuteDataset

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ִ��һ�� SqlCommand ���
        /// ������һ�������������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <returns>һ�� dataset ���ݼ������а����� command ��������Ľ������</returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ���
        /// ������һ���������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ�� dataset ���ݼ������а����� command ��������Ľ������</returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );

            // ��������һ�� SqlConnection������������ͷŸ�������ռ�õ�������Դ��
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // �������ط������� connection string �����ַ����滻Ϊһ�� connection ����ʵ��
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// �����������ַ�����ָ�������ݿ�ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢����</param>
        /// <returns>һ�� dataset ���ݼ������а����� command ��������Ľ����</returns>
        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // ���ṩ��ֵ���ڲ�����˳��������Щ����
                AssignParameterValues(commandParameters, parameterValues);

                // ����һ�����ط������÷������Խ���һ�� SqlParameter[] ��������
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ִ��һ�� SqlCommand �������һ������������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <returns>һ�� dataset ���ݼ������а����� command ��������Ľ������</returns>
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteDataset(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�����������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ�� dataset ���ݼ������а����� command ��������Ľ������</returns>
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );

            // ����һ�������Ա�ִ��
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );

            // ���� DataAdapter �� DataSet
            using( SqlDataAdapter da = new SqlDataAdapter(cmd) )
            {
                DataSet ds = new DataSet();

                // ʹ����ȱʡֵ������ DataTable ���ݱ���� DataSet ���ݼ�
                da.Fill(ds);

                // ������ command �������Ƴ� SqlParameters �������Ա� commandParameters ���Ա��ٴ�ʹ��
                cmd.Parameters.Clear();

                if( mustCloseConnection )
                    connection.Close();

                // ���� dataset ���ݼ�
                return ds;
            }
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        /// <returns>һ�� dataset ���ݼ������а����� command ��������Ľ������</returns>
        public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // ���ṩ��ֵ���ڲ�����˳��������Щ����
                AssignParameterValues(commandParameters, parameterValues);

                // ����һ�����ط������÷������Խ���һ�� SqlParameter[] ��������
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ִ��һ�� SqlCommand �������һ������������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <returns>һ�� dataset ���ݼ������а����� command ��������Ľ������</returns>
        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteDataset(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�����������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ�� dataset ���ݼ������а����� command ��������Ľ������</returns>
        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

            // ����һ�������Ա�ִ��
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );

            // ���� DataAdapter �� DataSet
            using( SqlDataAdapter da = new SqlDataAdapter(cmd) )
            {
                DataSet ds = new DataSet();

                // ʹ����ȱʡֵ������ DataTable ���ݱ���� DataSet ���ݼ�
                da.Fill(ds);

                // ������ command �������Ƴ� SqlParameters �������Ա� commandParameters ���Ա��ٴ�ʹ��
                cmd.Parameters.Clear();

                // ���� dataset ���ݼ�
                return ds;
            }
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        /// <returns>һ�� dataset ���ݼ������а����� command ��������Ľ������</returns>
        public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // ���ṩ��ֵ���ڲ�����˳��������Щ����
                AssignParameterValues(commandParameters, parameterValues);

                // ����һ�����ط������÷������Խ���һ�� SqlParameter[] ��������
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }


        #endregion ִ�����ݼ� ExecuteDataset

        #region ִ�����ݱ� ExecuteDataTable [2006-04-06 Bpusoft Created]

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
        /// ���������ַ�����ָ�������ݿ�ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ���
        /// ������һ�����ݱ�
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ�� datatable ���ݱ����а����� command ��������Ľ����</returns>
        public static DataTable ExecuteDataTable( string connectionString, string sqlText, SqlParameter[] commandParameters )
        {
            return ExecuteDataset( connectionString, CommandType.Text, sqlText, commandParameters ).Tables[0];
        }

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ִ��һ�� SqlCommand ���
        /// ������һ�����ݱ�����������κβ�������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <returns>һ�� datatable ���ݱ����а����� command ��������Ľ������</returns>
        public static DataTable ExecuteDataTable( string connectionString, string sqlText )
        {
            return ExecuteDataset( connectionString, CommandType.Text, sqlText ).Tables[0];
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ����
        /// ������һ�����������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ�� datatable ���ݱ����а����� command ��������Ľ����</returns>
        public static DataTable ExecuteDataTable( SqlConnection connection, string sqlText, SqlParameter[] commandParameters )
        {
            return ExecuteDataset( connection, CommandType.Text, sqlText, commandParameters ).Tables[0];
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ִ��һ�� SqlCommand ����
        /// ������һ������������������κβ�������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <returns>һ�� datatable ���ݱ����а����� command ��������Ľ����</returns>
        public static DataTable ExecuteDataTable( SqlConnection connection, string sqlText)
        {
            return ExecuteDataset( connection, CommandType.Text, sqlText ).Tables[0];
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ����
        /// ������һ�����������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ�� datatable ���ݱ����а����� command ��������Ľ����</returns>
        public static DataTable ExecuteDataTable( SqlTransaction transaction, string sqlText, SqlParameter[] commandParameters )
        {
            return ExecuteDataset( transaction, CommandType.Text, sqlText, commandParameters ).Tables[0];
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ִ��һ�� SqlCommand ����
        /// ������һ������������������κβ�������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <returns>һ�� datatable ���ݱ����а����� command ��������Ľ����</returns>
        public static DataTable ExecuteDataTable( SqlTransaction transaction, string sqlText )
        {
            return ExecuteDataset( transaction, CommandType.Text, sqlText ).Tables[0];
        }

        #endregion ִ�����ݱ� ExecuteDataTable [2006-04-06 Bpusoft Created]

        #region ִ��ֻ����ѯ ExecuteReader

        /// <summary>
        /// ��ö������ָ������ʵ�� connection ���ɵ������ṩ�ģ������� SqlHelper �����ģ�
        /// �Ա������ڵ��� ExecuteReader() ʱ���������ʵ��� CommandBehavior ������Ϊ��
        /// </summary>
        private enum SqlConnectionOwnership
        {
            /// <summary>
            /// SqlHelper ӵ�в����� connection ����ʵ����
            /// </summary>
            Internal,
            /// <summary>
            /// ������ ӵ�в����� connection ����ʵ����
            /// </summary>
            External
        }

        /// <summary>
        /// ������׼��һ�� SqlCommand ���Ȼ�����ʵ��� CommandBehavior ������Ϊ���� ExecuteReader��
        /// </summary>
        /// <remarks>
        /// ������Ǵ�����������ʵ������ô����ϣ���� DataReader ���رպ�رո�����ʵ����
        ///
        /// ��������ṩ������ʵ������ô����ϣ����������������ȥ����
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ����������ִ�д�����</param>
        /// <param name="transaction">һ����Ч�� SqlTransaction ���񣬻����� 'null'</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ�������������� SqlParameter[] �����������������������Ϊ 'null'</param>
        /// <param name="connectionOwnership">ָ�� connection ����ʵ���������ɵ������ṩ�ģ������� SqlHelper ������</param>
        /// <returns>һ�� SqlDataReader����������������Ľ����</returns>
        private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );

            bool mustCloseConnection = false;
            // ����һ�������Ա�ִ��
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );

                // ����һ�� SqlDataReader ʵ��
                SqlDataReader dataReader;

                // ���ʵ��� CommandBehavior ������Ϊ���� ExecuteReader
                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }

                // ������ command �������Ƴ� SqlParameters �������Ա� commandParameters ���Ա��ٴ�ʹ��
                /*
                 * ע�⣺������һ�����⣬���� reader ���ر�ʱ��output ���������ֵ�ᶪʧ��
                 *      �����������Щ������ command ������룬Ȼ���� SqlReader �����������ǵ�ֵ��
                 *      �������������ʱ����Щ���������ٴα����� command ����ʹ�á�
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
        /// ���������ַ�����ָ�������ݿ�ִ��һ�� SqlCommand ���
        /// ������һ�������������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <returns>һ�� SqlDataReader�������� command ��������Ľ����</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteReader(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ���
        /// ����һ�� SqlDataReader��
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ�� SqlDataReader�������� command ��������Ľ����</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                // ����˽�����ط�����ʹ���ڽ�����ʵ���滻�����ַ���
                return ExecuteReader(connection, null, commandType, commandText, commandParameters,SqlConnectionOwnership.Internal);
            }
            catch
            {
                // ������Ƿ��� SqlDatReader ʧ�ܣ���ô���Ǳ����Լ��ر�����ʵ��
                if( connection != null ) connection.Close();
                throw;
            }

        }

        /// <summary>
        /// �����������ַ�����ָ�������ݿ�ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// SqlDataReader dr = ExecuteReader(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        /// <returns>һ�� SqlDataReader�������� command ��������Ľ����</returns>
        public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ִ��һ�� SqlCommand �������һ������������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <returns>һ�� SqlDataReader�������� command ��������Ľ����</returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteReader(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�����������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ�� SqlDataReader�������� command ��������Ľ����</returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            // ����˽�����ط������÷�������һ�� null ֵ�������һ���⽨����ʵ��
            return ExecuteReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// SqlDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        /// <returns>һ�� SqlDataReader�������� command ��������Ľ����</returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ִ��һ�� SqlCommand �������һ������������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <returns>һ�� SqlDataReader�������� command ��������Ľ����</returns>
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteReader(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�����������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ�� SqlDataReader�������� command ��������Ľ����</returns>
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

            // �������ط�����ָ�� connection ����ʵ�����ɵ������ṩ��
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// SqlDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        /// <returns>һ�� SqlDataReader�������� command ��������Ľ����</returns>
        public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ִ��ֻ����ѯ ExecuteReader

        #region ִ�е�ֵ��ѯ ExecuteScalar

        #region Bpusoft Created

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ִ��һ�� SqlCommand ���
        /// ������һ�� 1x1 �Ľ����������������κβ�������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar( string connectionString, string sqlText )
        {
            return ExecuteScalar( connectionString, CommandType.Text, sqlText );
        }

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ���
        /// ������һ�� 1x1 �Ľ������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar( string connectionString, string sqlText, SqlParameter[] commandParameters )
        {
            return ExecuteScalar( connectionString, CommandType.Text, sqlText, commandParameters );
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ִ��һ�� SqlCommand �������һ�� 1x1 �Ľ���������������κβ�������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar( SqlConnection connection, string sqlText )
        {
            return ExecuteScalar( connection, CommandType.Text, sqlText );
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�� 1x1 �Ľ��������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar( SqlConnection connection, string sqlText, SqlParameter[] commandParameters )
        {
            return ExecuteScalar( connection, CommandType.Text, sqlText, commandParameters );
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ִ��һ�� SqlCommand �������һ�� 1x1 �Ľ���������������κβ�������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar( SqlTransaction transaction, string sqlText )
        {
            return ExecuteScalar( transaction, CommandType.Text, sqlText );
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�� 1x1 �Ľ��������
        /// [2006-04-06 Bpusoft Created]
        /// </summary>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="sqlText">T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar( SqlTransaction transaction, string sqlText, SqlParameter[] commandParameters )
        {
            return ExecuteScalar( transaction, CommandType.Text, sqlText, commandParameters );
        }

        #endregion Bpusoft Created

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ִ��һ�� SqlCommand ���
        /// ������һ�� 1x1 �Ľ����������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteScalar(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ���
        /// ������һ�� 1x1 �Ľ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar( string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters )
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            // ��������һ�� SqlConnection������������ͷŸ�������ռ�õ�������Դ��
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // �������ط������� connection string �����ַ����滻Ϊһ�� connection ����ʵ��
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// �����������ַ�����ָ�������ݿ�ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�� 1x1 �Ľ��������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int orderCount = (int)ExecuteScalar(connString, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // ���ṩ��ֵ���ڲ�����˳��������Щ����
                AssignParameterValues(commandParameters, parameterValues);

                // ����һ�����ط������÷������Խ���һ�� SqlParameter[] ��������
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ִ��һ�� SqlCommand �������һ�� 1x1 �Ľ���������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteScalar(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�� 1x1 �Ľ��������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );

            // ����һ�������Ա�ִ��
            SqlCommand cmd = new SqlCommand();

            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );

            // ִ�� command ������ؽ����
            object retval = cmd.ExecuteScalar();

            // ������ command �������Ƴ� SqlParameters �������Ա� commandParameters ���Ա��ٴ�ʹ��
            cmd.Parameters.Clear();

            if( mustCloseConnection )
                connection.Close();

            return retval;
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�� 1x1 �Ľ��������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalarByCmd(SqlConnection connection, CommandType commandType, string commandText,SqlCommand cmd, params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // ����һ�������Ա�ִ��
            //SqlCommand cmd = new SqlCommand();

            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // ִ�� command ������ؽ����
            object retval = cmd.ExecuteScalar();

            // ������ command �������Ƴ� SqlParameters �������Ա� commandParameters ���Ա��ٴ�ʹ��
            cmd.Parameters.Clear();

            if (mustCloseConnection)
                connection.Close();

            return retval;
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�� 1x1 �Ľ��������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // ���ṩ��ֵ���ڲ�����˳��������Щ����
                AssignParameterValues(commandParameters, parameterValues);

                // ����һ�����ط������÷������Խ���һ�� SqlParameter[] ��������
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ִ��һ�� SqlCommand �������һ�� 1x1 �Ľ���������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteScalar(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�� 1x1 �Ľ��������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

            // ����һ�������Ա�ִ��
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );

            // ִ�� command ������ؽ����
            object retval = cmd.ExecuteScalar();

            // ������ command �������Ƴ� SqlParameters �������Ա� commandParameters ���Ա��ٴ�ʹ��
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�� 1x1 �Ľ��������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ӳ���������ȡ���������ҵ����ǲ����뻺�棩�ô洢���̵Ĳ���
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // ���ṩ��ֵ���ڲ�����˳��������Щ����
                AssignParameterValues(commandParameters, parameterValues);

                // ����һ�����ط������÷������Խ���һ�� SqlParameter[] ��������
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }


        #endregion ִ�е�ֵ��ѯ ExecuteScalar

        #region ִ�� XML ֻ����ѯ ExecuteXmlReader
        /// <summary>
        /// ����ָ���� SqlConnection ����ִ��һ�� SqlCommand �������һ������������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">ʹ�� "FOR XML AUTO" �Ĵ洢�������� �� T-SQL ����</param>
        /// <returns>һ�� XmlReader�����а����� command ��������Ľ����</returns>
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteXmlReader(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�����������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">ʹ�� "FOR XML AUTO" �Ĵ洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ�� XmlReader�����а����� command ��������Ľ����</returns>
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );

            bool mustCloseConnection = false;
            // ����һ�������Ա�ִ��
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection );

                // ���� DataAdapter �� DataSet
                XmlReader retval = cmd.ExecuteXmlReader();

                // ������ command �������Ƴ� SqlParameters �������Ա� commandParameters ���Ա��ٴ�ʹ��
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
        /// ����ָ���� SqlConnection ����ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// XmlReader r = ExecuteXmlReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="spName">ʹ�� "FOR XML AUTO" �Ĵ洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        /// <returns>һ�� XmlReader�����а����� command ��������Ľ����</returns>
        public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // ���ṩ��ֵ���ڲ�����˳��������Щ����
                AssignParameterValues(commandParameters, parameterValues);

                // ����һ�����ط������÷������Խ���һ�� SqlParameter[] ��������
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ִ��һ�� SqlCommand �������һ������������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">ʹ�� "FOR XML AUTO" �Ĵ洢�������� �� T-SQL ����</param>
        /// <returns>һ�� XmlReader�����а����� command ��������Ľ����</returns>
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // ͨ��Ϊ SqlParameter[] �����ṩ null ���������ط���
            return ExecuteXmlReader(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�����������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">ʹ�� "FOR XML AUTO" �Ĵ洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <returns>һ�� XmlReader�����а����� command ��������Ľ����</returns>
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );

            // ����һ�������Ա�ִ��
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );

            // ���� DataAdapter �� DataSet
            XmlReader retval = cmd.ExecuteXmlReader();

            // ������ command �������Ƴ� SqlParameters �������Ա� commandParameters ���Ա��ٴ�ʹ��
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// XmlReader r = ExecuteXmlReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        /// <returns>һ�� dataset ���ݼ������а����� command ��������Ľ������</returns>
        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // ���ṩ��ֵ���ڲ�����˳��������Щ����
                AssignParameterValues(commandParameters, parameterValues);

                // ����һ�����ط������÷������Խ���һ�� SqlParameter[] ��������
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ִ�� XML ֻ����ѯ ExecuteXmlReader

        #region ������ݼ� FillDataset
        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ִ��һ�� SqlCommand ���
        /// ������һ�������������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="dataSet">һ�� dataset ���ݼ������н������� command ��������Ľ����</param>
        /// <param name="tableNames">�����齫���ڴ�����ӳ�䣬�Ա�ͨ���û�����ı��������ܾ���ʵ�ʵı��������� DataTable</param>
        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );

            // ��������һ�� SqlConnection������������ͷŸ�������ռ�õ�������Դ��
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // �������ط������� connection string �����ַ����滻Ϊһ�� connection ����ʵ��
                FillDataset(connection, commandType, commandText, dataSet, tableNames);
            }
        }

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand ����
        /// ������������κν������Ҳ�������κΣ���
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        /// <param name="dataSet">һ�� dataset ���ݼ������н������� command ��������Ľ����</param>
        /// <param name="tableNames">�����齫���ڴ�����ӳ�䣬�Ա�ͨ���û�����ı��������ܾ���ʵ�ʵı��������� DataTable</param>
        public static void FillDataset(string connectionString, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            // ��������һ�� SqlConnection������������ͷŸ�������ռ�õ�������Դ��
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // �������ط������� connection string �����ַ����滻Ϊһ�� connection ����ʵ��
                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
        }

        /// <summary>
        /// �����������ַ�����ָ�������ݿ�ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, 24);
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataSet">һ�� dataset ���ݼ������н������� command ��������Ľ����</param>
        /// <param name="tableNames">�����齫���ڴ�����ӳ�䣬�Ա�ͨ���û�����ı��������ܾ���ʵ�ʵı��������� DataTable</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        public static void FillDataset(string connectionString, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            // ��������һ�� SqlConnection������������ͷŸ�������ռ�õ�������Դ��
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // �������ط������� connection string �����ַ����滻Ϊһ�� connection ����ʵ��
                FillDataset (connection, spName, dataSet, tableNames, parameterValues);
            }
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ִ��һ�� SqlCommand �������һ������������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="dataSet">һ�� dataset ���ݼ������н������� command ��������Ľ����</param>
        /// <param name="tableNames">�����齫���ڴ�����ӳ�䣬�Ա�ͨ���û�����ı��������ܾ���ʵ�ʵı��������� DataTable</param>
        public static void FillDataset(SqlConnection connection, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�����������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="dataSet">һ�� dataset ���ݼ������н������� command ��������Ľ����</param>
        /// <param name="tableNames">�����齫���ڴ�����ӳ�䣬�Ա�ͨ���û�����ı��������ܾ���ʵ�ʵı��������� DataTable</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        public static void FillDataset(SqlConnection connection, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// FillDataset(conn, "GetOrders", ds, new string[] {"orders"}, 24, 36);
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataSet">һ�� dataset ���ݼ������н������� command ��������Ľ����</param>
        /// <param name="tableNames">�����齫���ڴ�����ӳ�䣬�Ա�ͨ���û�����ı��������ܾ���ʵ�ʵı��������� DataTable</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        public static void FillDataset(SqlConnection connection, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if ( connection == null ) throw new ArgumentNullException( "connection" );
            if (dataSet == null ) throw new ArgumentNullException( "dataSet" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // ���ṩ��ֵ���ڲ�����˳��������Щ����
                AssignParameterValues(commandParameters, parameterValues);

                // ����һ�����ط������÷������Խ���һ�� SqlParameter[] ��������
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ִ��һ�� SqlCommand �������һ������������������κβ�������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="dataSet">һ�� dataset ���ݼ������н������� command ��������Ľ����</param>
        /// <param name="tableNames">�����齫���ڴ�����ӳ�䣬�Ա�ͨ���û�����ı��������ܾ���ʵ�ʵı��������� DataTable</param>
        public static void FillDataset(SqlTransaction transaction, CommandType commandType,
            string commandText,
            DataSet dataSet, string[] tableNames)
        {
            FillDataset (transaction, commandType, commandText, dataSet, tableNames, null);
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�����������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="dataSet">һ�� dataset ���ݼ������н������� command ��������Ľ����</param>
        /// <param name="tableNames">�����齫���ڴ�����ӳ�䣬�Ա�ͨ���û�����ı��������ܾ���ʵ�ʵı��������� DataTable</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        public static void FillDataset(SqlTransaction transaction, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ���ṩ�Ĳ���ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// ע���÷������ܷ��� output ���������Ǵ洢���̵ķ���ֵ������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// FillDataset(trans, "GetOrders", ds, new string[]{"orders"}, 24, 36);
        /// </remarks>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataSet">һ�� dataset ���ݼ������н������� command ��������Ľ����</param>
        /// <param name="tableNames">�����齫���ڴ�����ӳ�䣬�Ա�ͨ���û�����ı��������ܾ���ʵ�ʵı��������� DataTable</param>
        /// <param name="parameterValues">һ���������飬���е�ֵ������Ϊ����ֵ�����洢���̡�</param>
        public static void FillDataset(SqlTransaction transaction, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // ���ṩ��ֵ���ڲ�����˳��������Щ����
                AssignParameterValues(commandParameters, parameterValues);

                // ����һ�����ط������÷������Խ���һ�� SqlParameter[] ��������
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else
            {
                // �������ǿ��Խ����øô洢���̣��������κβ���
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        /// <summary>
        /// ˽�еĸ����������÷�������ָ���� SqlTransaction ����� SqlConnection ����ʵ��
        /// ʹ���ṩ�Ĳ���ִ��һ�� SqlCommand �������һ�����������
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ��</param>
        /// <param name="transaction">һ����Ч�� SqlTransaction ����</param>
        /// <param name="commandType">�������ͣ��洢���� CommandType.StoredProcedure���ı� CommandType.Text �ȣ�</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="dataSet">һ�� dataset ���ݼ������н������� command ��������Ľ����</param>
        /// <param name="tableNames">�����齫���ڴ�����ӳ�䣬�Ա�ͨ���û�����ı��������ܾ���ʵ�ʵı��������� DataTable</param>
        /// <param name="commandParameters">һ��������������ִ�е� SqlParameter[] ����</param>
        private static void FillDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params SqlParameter[] commandParameters)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( dataSet == null ) throw new ArgumentNullException( "dataSet" );

            // ����һ�������Ա�ִ��
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection );

            // ���� DataAdapter �� DataSet
            using( SqlDataAdapter dataAdapter = new SqlDataAdapter(command) )
            {
                // ������û�ָ���ı�ӳ��
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

                // ʹ����ȱʡֵ������ DataTable ���ݱ���� DataSet ���ݼ�
                dataAdapter.Fill(dataSet);

                // ������ command �������Ƴ� SqlParameters �������Ա� commandParameters ���Ա��ٴ�ʹ��
                command.Parameters.Clear();
            }

            if( mustCloseConnection )
                connection.Close();
        }

        #endregion ������ݼ� FillDataset

        #region �������ݼ� UpdateDataset

        /// <summary>
        /// ���������ݼ���ÿ�β��롢���¡�ɾ����ִ�и��Ե� command ���
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order");
        /// </remarks>
        /// <param name="insertCommand">һ����Ч������������Դ�в����¼�¼�� transact-SQL �����ߴ洢����</param>
        /// <param name="deleteCommand">һ����Ч�����ڴ�����Դ��ɾ����¼�� transact-SQL �����ߴ洢����</param>
        /// <param name="updateCommand">һ����Ч������������Դ�и��¼�¼�� transact-SQL �����ߴ洢����</param>
        /// <param name="dataSet">���ڸ�������Դ�� DataSet ���ݼ�</param>
        /// <param name="tableName">���ڱ�ӳ���Դ�������</param>
        public static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
        {
            if( insertCommand == null ) throw new ArgumentNullException( "insertCommand" );
            if( deleteCommand == null ) throw new ArgumentNullException( "deleteCommand" );
            if( updateCommand == null ) throw new ArgumentNullException( "updateCommand" );
            if( tableName == null || tableName.Length == 0 ) throw new ArgumentNullException( "tableName" );

            // ����һ�� SqlDataAdapter������������ͷ���ռ�õ���Դ
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
            {
                // ���� SqlDataAdapter ����
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;

                // ��������Դ���ı�� DataSet ���ݼ�
                dataAdapter.Update (dataSet, tableName);

                // �ύ���� DataSet ���ݼ�ȫ���ĸı�
                dataSet.AcceptChanges();
            }
        }

        #endregion �������ݼ� UpdateDataset

        #region ���� Command ���� CreateCommand

        /// <summary>
        /// ͨ�������ṩ�洢�����Լ���ѡ�������� SqlCommand �Ĵ�����
        /// </summary>
        /// <remarks>
        /// ʾ����
        /// SqlCommand command = CreateCommand(conn, "AddCustomer", "CustomerID", "CustomerName");
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ�� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="sourceColumns">һ���ַ������飬������ô洢���̵�����Դ�е�����</param>
        /// <returns>һ����Ч�� SqlCommand ����</returns>
        public static SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ����һ�� SqlCommand ʵ��
            SqlCommand cmd = new SqlCommand( spName, connection );
            cmd.CommandType = CommandType.StoredProcedure;

            // ��������յ��˲���ֵ����ô������Ҫָ�����ǵ�ȥ��
            if ((sourceColumns != null) && (sourceColumns.Length > 0))
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // ���ڲ�����˳�򣬽��ṩ�� SourceColumn ����Դ�и�����Щ����
                for (int index=0; index < sourceColumns.Length; index++)
                    commandParameters[index].SourceColumn = sourceColumns[index];

                // ���ҵ��Ĳ����󶨵� SqlCommand ������
                AttachParameters (cmd, commandParameters);
            }

            return cmd;
        }

        #endregion ���� Command ���� CreateCommand

        #region ִ��ǿ���Ͳ������޹���� ExecuteNonQueryTypedParams

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� �������ؽ��������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQueryTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʵ��ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� �������ؽ��������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ�� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQueryTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� �������ؽ��������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="transaction">һ����Ч�� SqlTransaction ���� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ��������������������Ӱ���������</returns>
        public static int ExecuteNonQueryTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ִ��ǿ���Ͳ������޹���� ExecuteNonQueryTypedParams

        #region ִ��ǿ���Ͳ��������ݼ� ExecuteDatasetTypedParams

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ�� dataset ���ݼ������а����� command ��������Ľ������</returns>
        public static DataSet ExecuteDatasetTypedParams(string connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if ( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʵ��ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ�� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ�� dataset ���ݼ������а����� command ��������Ľ������</returns>
        public static DataSet ExecuteDatasetTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="transaction">һ����Ч�� SqlTransaction ���� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ�� dataset ���ݼ������а����� command ��������Ľ������</returns>
        public static DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ִ��ǿ���Ͳ��������ݼ� ExecuteDatasetTypedParams

        #region ִ��ǿ���Ͳ�����ֻ����ѯ ExecuteReaderTypedParams

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ�� SqlDataReader�������� command ��������Ľ����</returns>
        public static SqlDataReader ExecuteReaderTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if ( dataRow != null && dataRow.ItemArray.Length > 0 )
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʵ��ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ�� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ�� SqlDataReader�������� command ��������Ľ����</returns>
        public static SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="transaction">һ����Ч�� SqlTransaction ���� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ�� SqlDataReader�������� command ��������Ľ����</returns>
        public static SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if( dataRow != null && dataRow.ItemArray.Length > 0 )
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion ִ��ǿ���Ͳ�����ֻ����ѯ ExecuteReaderTypedParams

        #region ִ��ǿ���Ͳ����ĵ�ֵ��ѯ ExecuteScalarTypedParams

        /// <summary>
        /// ���������ַ�����ָ�������ݿ�ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� ������һ�� 1x1 �Ľ��������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalarTypedParams(String connectionString, String spName, DataRow dataRow)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlConnection ����ʵ��ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� ������һ�� 1x1 �Ľ��������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ�� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalarTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� ������һ�� 1x1 �Ľ��������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="transaction">һ����Ч�� SqlTransaction ���� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ���������а����� command ��������� 1x1 �������ֵ</returns>
        public static object ExecuteScalarTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
            }
        }
        
        #endregion ִ��ǿ���Ͳ����ĵ�ֵ��ѯ ExecuteScalarTypedParams

        #region ִ��ǿ���Ͳ����� XML ֻ����ѯ ExecuteXmlReaderTypedParams

        /// <summary>
        /// ����ָ���� SqlConnection ����ʵ��ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ�� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ�� XmlReader�����а����� command ��������Ľ����</returns>
        public static XmlReader ExecuteXmlReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// ����ָ���� SqlTransaction ����ʹ�� dataRow ���е�ֵ��Ϊ�ô洢���̵Ĳ���ֵ
        /// ͨ�� SqlCommand ִ��һ���洢���� ������һ�����������
        /// ����ÿ���洢���̵�һ�α�����ʱ���÷����������ݿ���Ϊ�洢���̲��Ҳ����������ڲ�����˳��ֵ��
        /// </summary>
        /// <param name="transaction">һ����Ч�� SqlTransaction ���� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="dataRow">����ʢ�Ŵ洢���̲���ֵ�� DataRow ������</param>
        /// <returns>һ�� XmlReader�����а����� command ��������Ľ����</returns>
        public static XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
        {
            if( transaction == null ) throw new ArgumentNullException( "transaction" );
            if( transaction != null && transaction.Connection == null ) throw new ArgumentException( "The transaction was rollbacked or commited, please provide an open transaction.", "transaction" );
            if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            // ���������ֵ����ô�ô洢���̱��뱻��ʼ��
            if( dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // �Ӳ���������ȡ���ô洢���̵Ĳ������飨�����ҵ����ǲ��ŵ������У�
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                // ���ò���ֵ
                AssignParameterValues(commandParameters, dataRow);

                return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
            }
        }
        
        #endregion ִ��ǿ���Ͳ����� XML ֻ����ѯ ExecuteXmlReaderTypedParams

    }

    /// <summary>
    ///	SqlHelperParameterCache �ṩ��һЩ������
    ///	����ƽ��������ʱ�洢���̲����ľ�̬��������Ҵ洢���̲�����������
    /// </summary>
    public sealed class SqlHelperParameterCache
    {
        #region ˽�з����������Լ����캯�� private methods, variables, and constructors

        /// <summary>
        /// ���ڸ�����ṩһЩ��̬�������������Ĭ�Ϲ��캯��Ϊ private��
        /// ����ֹʹ�� "new SqlHelperParameterCache()" ������ʵ����
        /// </summary>
        private SqlHelperParameterCache() {}

        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// ������ʱΪһ���洢���̽���ƥ��ĵ� SqlParameter[] �������顣
        /// </summary>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ�� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="includeReturnValueParameter">�Ƿ�������ǵ� return value parameter ����ֵ����</param>
        /// <returns>���ҵ��� SqlParameter[] ��������</returns>
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
                // �Ƴ�����ֵ����
                cmd.Parameters.RemoveAt(0);
            }

            SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];

            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // ʹ�� DBNull.Value ��ʼ������
            foreach (SqlParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }

        /// <summary>
        /// ��ȿ�������� SqlParameter[] ���顣
        /// </summary>
        /// <param name="originalParameters">ԭʼ�Ĳ�������</param>
        /// <returns>��ȿ����Ĳ�������</returns>
        private static SqlParameter[] CloneParameters( SqlParameter[] originalParameters )
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        #endregion ˽�з����������Լ����캯�� private methods, variables, and constructors

        #region ���溯�� caching functions

        /// <summary>
        /// �� SqlParameter[] ����������ӵ������С�
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <param name="commandParameters">һ����������� SqlParameter[] �������顣</param>
        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            if( connectionString == null || connectionString.Length == 0 ) throw new ArgumentNullException( "connectionString" );
            if( commandText == null || commandText.Length == 0 ) throw new ArgumentNullException( "commandText" );

            string hashKey = connectionString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }

        /// <summary>
        /// �ӻ�����ȡ�� SqlParameter[] �������顣
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="commandText">�洢�������� �� T-SQL ����</param>
        /// <returns>һ�� SqlParameter[] ��������</returns>
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

        #endregion ���溯�� caching functions

        #region �������Һ��� Parameter Discovery Functions

        /// <summary>
        /// Ϊ�ô洢����ȡ��һ��ƥ��Ĳ������顣
        /// </summary>
        /// <remarks>
        /// �÷����������ݿ��в�ѯ��Щ��Ϣ�����洢���������Ա�����ʹ�á�
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <returns>һ�� SqlParameter[] ��������</returns>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        /// <summary>
        /// Ϊ�ô洢����ȡ��һ��ƥ��Ĳ������顣
        /// </summary>
        /// <remarks>
        /// �÷����������ݿ��в�ѯ��Щ��Ϣ�����洢���������Ա�����ʹ�á�
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ڽ��� SqlConnection �������ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="includeReturnValueParameter">һ������ֵ��ָʾ����ֵ�����Ƿ�Ӧ�ð����ڽ����</param>
        /// <returns>һ�� SqlParameter[] ��������</returns>
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
        /// Ϊ�ô洢����ȡ��һ��ƥ��Ĳ������顣
        /// </summary>
        /// <remarks>
        /// �÷����������ݿ��в�ѯ��Щ��Ϣ�����洢���������Ա�����ʹ�á�
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ�� object</param>
        /// <param name="spName">�洢������</param>
        /// <returns>һ�� SqlParameter[] ��������</returns>
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
        {
            return GetSpParameterSet(connection, spName, false);
        }

        /// <summary>
        /// Ϊ�ô洢����ȡ��һ��ƥ��Ĳ������顣
        /// </summary>
        /// <remarks>
        /// �÷����������ݿ��в�ѯ��Щ��Ϣ�����洢���������Ա�����ʹ�á�
        /// </remarks>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ�� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="includeReturnValueParameter">һ������ֵ��ָʾ����ֵ�����Ƿ�Ӧ�ð����ڽ����</param>
        /// <returns>һ�� SqlParameter[] ��������</returns>
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if( connection == null ) throw new ArgumentNullException( "connection" );
            using (SqlConnection clonedConnection = (SqlConnection)((ICloneable)connection).Clone())
            {
                return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// Ϊ�ô洢����ȡ��һ��ƥ��Ĳ������顣
        /// </summary>
        /// <param name="connection">һ����Ч�� SqlConnection ����ʵ�� object</param>
        /// <param name="spName">�洢������</param>
        /// <param name="includeReturnValueParameter">һ������ֵ��ָʾ����ֵ�����Ƿ�Ӧ�ð����ڽ����</param>
        /// <returns>һ�� SqlParameter[] ��������</returns>
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

        #endregion �������Һ��� Parameter Discovery Functions
    }
}
