using ProtocolsManage.Common;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Repository;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Service
{
    public interface IProtocolManageService
    {
        IEnumerable<PmFInterpreter> GetPmFInterpreter();
        PropertyInfo[] GetPropertyInfoArray();
        //使用SQLHelper
        PmFInterpreter GetPmFInterpreterById(string pmId);

        ProtocolTestViewModel TestProtocol(string pmId, string sendData);
    }
    public class ProtocolManageService : IProtocolManageService
    {
        //配置文件中连接数据库字符串
        private static string connetString = ConfigurationManager.ConnectionStrings["SocialGoalEntities"].ConnectionString;

        private readonly IReceiveDataDisplayRepository _receiveDataDisplayRepository;
        private readonly IProtocolManageRepository _protocolManageRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProtocolManageService(IProtocolManageRepository protocolManageRepository, IUnitOfWork unitOfWork, IReceiveDataDisplayRepository receiveDataDisplayRepository)
        {
            this._receiveDataDisplayRepository = receiveDataDisplayRepository;
            this._protocolManageRepository = protocolManageRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<PmFInterpreter> GetPmFInterpreter()
        {
            var pmFInterpreter = _protocolManageRepository.GetAll();
            return pmFInterpreter;
        }

        public PropertyInfo[] GetPropertyInfoArray()
        {
            PropertyInfo[] props = null;
            try
            {
                Type type = typeof(ReceiveDataLast);
                object obj = Activator.CreateInstance(type);
                props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            }
            catch (Exception ex)
            { }
            return props;
        }

        //协议测试程序
        public ProtocolTestViewModel TestProtocol(string pmId, string sendData)
        {
            ProtocolTestViewModel pmr = new ProtocolTestViewModel();
            Dictionary<string, string> dicDatas = new Dictionary<string, string>();
            Dictionary<string, string> dicD = new Dictionary<string, string>();

            PmFInterpreter fInterpretersModel = _protocolManageRepository.GetById(pmId);
            IEnumerable<ReceiveDataDisplay> rd = _receiveDataDisplayRepository.GetDataByPmFInterpreterId(pmId);
            //数据解析过程
            byte[] requestInfo = Comm.StrToToHexByte(sendData);
            //FInterpretersModel fInterpretersModel = session.AppServer.fInterpretersModel;
            //分隔字符串
            Dictionary<string, byte[]> dicSplitDatas = PmFInterpreterUtil.SplitData(fInterpretersModel, requestInfo);


            foreach (var item in dicSplitDatas)
            {

                List<PmDataBody> dataBodyList = fInterpretersModel.PmDataBodys.Where(t => t.InfoTypeNumber == item.Key).ToList();

                foreach (PmDataBody itemDataBodyModel in dataBodyList)
                {
                    switch (itemDataBodyModel.DataType)
                    {
                        //byte
                        case 1:
                            PmFInterpreterUtil.AnalysisByte(itemDataBodyModel, dicDatas, item.Value);
                            break;
                        case 2:
                            PmFInterpreterUtil.AnalysisBit(itemDataBodyModel, dicDatas, item.Value);
                            break;
                        case 3:
                            PmFInterpreterUtil.AnalysisBitState(itemDataBodyModel, dicDatas, item.Value);
                            break;

                        default:
                            break;
                    }
                }
            }
            //解析过程结束
            List<KeyValue> kvList = new List<KeyValue>();
            foreach (var item in dicDatas)
            {
                KeyValue ky = new KeyValue();
                ky.Key = item.Key;
                ky.Value = item.Value;
                kvList.Add(ky);
            }
            pmr.PmFInterpreterResult = kvList;

            //显示协议
            List<TerminalDataViewModel> tdvList = new List<TerminalDataViewModel>();
            foreach (ReceiveDataDisplay itemRd in rd.ToList())
            {
                TerminalDataViewModel tdv = new TerminalDataViewModel();
                tdv.DictionaryKey = itemRd.DictionaryValue;
                tdv.DictionaryValue = dicDatas[itemRd.DictionaryKey];
                tdv.ShowPostion = itemRd.ShowPostion;
                tdv.ShowIcon = itemRd.ShowIcon;
                tdv.ShowUnit = itemRd.ShowUnit;

                if (itemRd.ReDataDisplayFormats.Count() > 0)
                {
                    foreach (ReDataDisplayFormat itemRdf in itemRd.ReDataDisplayFormats)
                    {
                        if (itemRd.FormatType == 1)//状态
                        {
                            if (itemRdf.FormatExpression == tdv.DictionaryValue)
                            {
                                tdv.DictionaryValue = itemRdf.FormatValue;
                                tdv.ShowColor = itemRdf.FormatColor;
                                break;
                            }
                        }
                        if (itemRd.FormatType == 2)//数值范围"30-50"
                        {
                            string[] sp = itemRdf.FormatExpression.Split('-');
                            double leftE = 0;
                            double.TryParse(sp[0].ToString(), out leftE);
                            double rightE = 0;
                            double.TryParse(sp[1].ToString(), out rightE);
                            double currentE = 0;
                            double.TryParse(tdv.DictionaryValue.ToString(), out currentE);
                            if (currentE > leftE && currentE <= rightE)
                            {
                                //tdv.DictionaryValue = itemRdf.FormatValue;
                                tdv.ShowColor = itemRdf.FormatColor;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    tdv.ShowColor = "#468847";
                }
                //itemRd.DictionaryKey
                tdvList.Add(tdv);
            }

            pmr.ReceiveDataDisplayResult = tdvList;
            return pmr;
        }
        /// <summary>
        /// 使用sqlHelperc查找数据
        /// </summary>
        /// <param name="pmid"></param>
        /// <returns></returns>
        public PmFInterpreter GetPmFInterpreterById(string pmid)
        {
            var strPmFInterpreter = "select * from  PmFInterpreters where PmFInterpreterId='" + pmid + "'";
           
            DataTable rePmFInterpreter = GetDateTable(strPmFInterpreter);           


            DateSetTransform<PmFInterpreter> dsTf = new DateSetTransform<PmFInterpreter>();
            List<PmFInterpreter> pmF = dsTf.FillModel(rePmFInterpreter);
           
            return null;
        }
        public DataTable GetDateTable(string sqlStr)
        {
            using (SqlConnection conn = new SqlConnection(connetString))
            {
                conn.Open();
                try
                {
                    var returnSql = SqlHelper.ExecuteDataTable(conn, sqlStr);

                    return returnSql;
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
