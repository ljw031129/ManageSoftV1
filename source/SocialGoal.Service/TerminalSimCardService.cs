using SocialGoal.Core.Common;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Repository;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Service.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Service
{
    public interface ITerminalSimCardService
    {
        Task<IEnumerable<TerminalSimCard>> GeTerminalSimCard(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count);
        void Save();

        Task<IEnumerable<TerminalSimCard>> GetAll();

        Task CreateAsync(TerminalSimCard terminalSimCard);

        Task<bool> DeleteAsync(string p);

        Task UpdateAsync(TerminalSimCard terminalSimCard);

        IEnumerable<Core.Common.ValidationResult> Validate(TerminalSimCardViewModel terminalSimCard);

        Task<IEnumerable<TerminalSimCard>> GetAllByTerminalEquipment();
    }
    public class TerminalSimCardService : ITerminalSimCardService
    {
        private readonly ITerminalSimCardRepository _terminalSimCardRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TerminalSimCardService(ITerminalSimCardRepository terminalSimCardRepository, IUnitOfWork unitOfWork)
        {
            this._terminalSimCardRepository = terminalSimCardRepository;
            this._unitOfWork = unitOfWork;
        }
        public Task<IEnumerable<TerminalSimCard>> GeTerminalSimCard(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count)
        {
            IEnumerable<TerminalSimCard> terminalSimCard = _terminalSimCardRepository.GetPageJqGrid<TerminalSimCard>(jqGridSetting, out count);
            return Task.FromResult(terminalSimCard);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public Task<IEnumerable<TerminalSimCard>> GetAll()
        {
            return Task.FromResult(_terminalSimCardRepository.GetAll());
        }


        public Task CreateAsync(TerminalSimCard terminalSimCard)
        {
            _terminalSimCardRepository.Add(terminalSimCard);
            Save();
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(string id)
        {
            var orgEnterprise = _terminalSimCardRepository.GetById(id);
            _terminalSimCardRepository.Delete(orgEnterprise);           
            Save();
            return Task.FromResult(true);
        }

        public Task UpdateAsync(TerminalSimCard terminalSimCard)
        {
            _terminalSimCardRepository.Update(terminalSimCard);
            Save();
            return Task.FromResult(true);
        }


        public IEnumerable<Core.Common.ValidationResult> Validate(TerminalSimCardViewModel terminalSimCard)
        {
            TerminalSimCard isTerminalSimCardExists = null;
            if (terminalSimCard.oper == "add")
                isTerminalSimCardExists = _terminalSimCardRepository.Get(c => c.TerminalSimCardNum == terminalSimCard.TerminalSimCardNum);
            else
                isTerminalSimCardExists = _terminalSimCardRepository.Get(c => c.TerminalSimCardNum == terminalSimCard.TerminalSimCardNum && c.TerminalSimCardId != terminalSimCard.TerminalSimCardId);
            if (isTerminalSimCardExists != null)
            {
                yield return new ValidationResult("Name", "SIM卡已存在");
            }
        }


        public Task<IEnumerable<TerminalSimCard>> GetAllByTerminalEquipment()
        {
            IEnumerable<TerminalSimCard> result = _terminalSimCardRepository.GetMany(t => t.TerminalSimCardState == "1");
            return Task.FromResult(result);
        }
    }
}
