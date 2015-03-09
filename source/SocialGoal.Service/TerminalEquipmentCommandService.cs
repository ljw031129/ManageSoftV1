using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Repository;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Utilities;

namespace SocialGoal.Service
{
    public interface ITerminalEquipmentCommandService
    {
        Task CreateAsync(TerminalEquipmentCommand terminalEquipmentCommand);
        void Save();

        Task<IEnumerable<TerminalEquipmentCommand>> GetTerminalEquipmentCommands(string IMEI, int currentPage, int numPerPage, out int count);

        Task<IEnumerable<TerminalEquipmentCommandCurrent>> GetTerminalEquipmentCurrentCommands( List<String> ls,int currentPage, int numPerPage, out int count);
    }
    public class TerminalEquipmentCommandService : ITerminalEquipmentCommandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITerminalEquipmentCommandRepository _terminalEquipmentCommandRepository;
        private readonly ITerminalEquipmentCommandCurrentRepository _terminalEquipmentCommandCurrentRepository;
        public TerminalEquipmentCommandService(ITerminalEquipmentCommandRepository terminalEquipmentCommandRepository, ITerminalEquipmentCommandCurrentRepository terminalEquipmentCommandCurrentRepository, IUnitOfWork unitOfWork)
        {
            this._terminalEquipmentCommandCurrentRepository = terminalEquipmentCommandCurrentRepository;
            this._terminalEquipmentCommandRepository = terminalEquipmentCommandRepository;

            this._unitOfWork = unitOfWork;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        /// <summary>
        /// u-s  s-c c-s c-s 
        /// </summary>
        /// <param name="terminalEquipmentCommand"></param>
        /// <returns></returns>
        public Task CreateAsync(TerminalEquipmentCommand terminalEquipmentCommand)
        {
            string guid = Guid.NewGuid().ToString();
            terminalEquipmentCommand.TerminalEquipmentCommandId = guid;
            terminalEquipmentCommand.OperateTime = DateUtils.ConvertDateTimeIntInt(DateTime.Now);
            terminalEquipmentCommand.CommandFromTo = "u-s";
            //命令设置   命令等待发送  命令已发送  命令响应成功
            terminalEquipmentCommand.Dtype = "1";
            _terminalEquipmentCommandRepository.Add(terminalEquipmentCommand);

            //写入当前等待发送指令表---保证当前等待发送表中只有一条记录
            TerminalEquipmentCommandCurrent tcl = _terminalEquipmentCommandCurrentRepository.GetMany(m => m.IMEI == terminalEquipmentCommand.IMEI).FirstOrDefault();
            if (tcl != null)
            {
                tcl.CommandFromTo = terminalEquipmentCommand.CommandFromTo;
                tcl.IMEI = terminalEquipmentCommand.IMEI;
                tcl.OperateDataHex = terminalEquipmentCommand.OperateDataHex;
                tcl.OperateStatue = terminalEquipmentCommand.OperateStatue;
                tcl.OperateTime = terminalEquipmentCommand.OperateTime;
                tcl.UserId = terminalEquipmentCommand.UserId;
                tcl.CommandJsonData = terminalEquipmentCommand.CommandJsonData;
                tcl.Dtype = terminalEquipmentCommand.Dtype;
                _terminalEquipmentCommandCurrentRepository.Update(tcl);
            }
            else
            {
                TerminalEquipmentCommandCurrent tc = new TerminalEquipmentCommandCurrent();
                tc.TerminalEquipmentCommandCurrentId = Guid.NewGuid().ToString();
                tc.CommandFromTo = terminalEquipmentCommand.CommandFromTo;
                tc.IMEI = terminalEquipmentCommand.IMEI;
                tc.OperateDataHex = terminalEquipmentCommand.OperateDataHex;
                tc.OperateStatue = terminalEquipmentCommand.OperateStatue;
                tc.OperateTime = terminalEquipmentCommand.OperateTime;
                tc.UserId = terminalEquipmentCommand.UserId;
                tc.CommandJsonData = terminalEquipmentCommand.CommandJsonData;
                tc.Dtype = terminalEquipmentCommand.Dtype;
                _terminalEquipmentCommandCurrentRepository.Add(tc);
            }
            Save();
            return Task.FromResult(true);
        }


        public Task<IEnumerable<TerminalEquipmentCommand>> GetTerminalEquipmentCommands(string IMEI, int currentPage, int numPerPage, out int count)
        {
            IEnumerable<TerminalEquipmentCommand> re = _terminalEquipmentCommandRepository.GetTerminalEquipmentCommands(IMEI, currentPage, numPerPage, out count);
            return Task.FromResult(re);
        }


        public Task<IEnumerable<TerminalEquipmentCommandCurrent>> GetTerminalEquipmentCurrentCommands( List<String> ls,int currentPage, int numPerPage, out int count)
        {
            IEnumerable<TerminalEquipmentCommandCurrent> re = _terminalEquipmentCommandCurrentRepository.GetTerminalEquipmenttCurrentCommands(ls,currentPage, numPerPage, out count);
            return Task.FromResult(re);
        }
    }
}
