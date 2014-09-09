using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Repository;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Service
{
    public interface IProtocolManageService
    {
        IEnumerable<PmFInterpreter> GetPmFInterpreter();

    }
    public class ProtocolManageService : IProtocolManageService
    {
        private readonly IProtocolManageRepository _protocolManageRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProtocolManageService(IProtocolManageRepository protocolManageRepository, IUnitOfWork unitOfWork)
        {
            this._protocolManageRepository = protocolManageRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<PmFInterpreter> GetPmFInterpreter()
        {
            var pmFInterpreter = _protocolManageRepository.GetAll();
            return pmFInterpreter;
        }
    }
}
