﻿using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Repository;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Service
{

    public interface IReceiveDataLastService
    {
        void Delete(ReceiveDataLast pBit);
        void Save();
        List<TerminalDataViewModel> GetTerminalDataByTerminalNum(string terminalNum);
        ReceiveDataLast GetReceiveDataLastByTerminalNum(string num);
    }
    public class ReceiveDataLastService : IReceiveDataLastService
    {
        private readonly IReceiveDataLastRepository _receiveDataLastRepository;
        private readonly IReceiveDataDisplayRepository _receiveDataDisplayRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ReceiveDataLastService(IReceiveDataLastRepository receiveDataLastRepository, IUnitOfWork unitOfWork, IReceiveDataDisplayRepository receiveDataDisplayRepository)
        {
            this._receiveDataDisplayRepository = receiveDataDisplayRepository;
            this._receiveDataLastRepository = receiveDataLastRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Delete(PmDataBit pBit)
        {
            //  _pmDataBitsRepository.Delete(_pmDataBitsRepository.GetById(pBit.PmDataBitId));
            Save();
        }


        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Delete(ReceiveDataLast pBit)
        {
            throw new NotImplementedException();
        }

        public List<TerminalDataViewModel> GetTerminalDataByTerminalNum(string terminalNum)
        {
            string pmId = "1";
            ReceiveDataLast rl = _receiveDataLastRepository.GetReceiveDataLastByTerminalNum(terminalNum);
            List<TerminalDataViewModel> tdvList = new List<TerminalDataViewModel>();

            Type type = rl.GetType(); //获取类型
            //PropertyInfo propertyInfo = type.GetProperty("Property1"); //获取指定名称的属性
            //string value_Old = (string)type.GetProperty("Property1").GetValue(rl, null); //获取属性值


            IEnumerable<ReceiveDataDisplay> rd = _receiveDataDisplayRepository.GetDataByPmFInterpreterId(pmId);

            foreach (ReceiveDataDisplay itemRd in rd.ToList())
            {
                TerminalDataViewModel tdv = new TerminalDataViewModel();
                tdv.DictionaryKey = itemRd.DictionaryValue;
                tdv.DictionaryValue = type.GetProperty(itemRd.DictionaryKey).GetValue(rl, null) != null ? type.GetProperty(itemRd.DictionaryKey).GetValue(rl, null).ToString() : "";
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
            return tdvList;
        }


        public ReceiveDataLast GetReceiveDataLastByTerminalNum(string num)
        {
            return _receiveDataLastRepository.GetReceiveDataLastByTerminalNum(num);
        }
    }
}
