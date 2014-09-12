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
    public interface IReDataDisplayFormatService
    {       
        void Delete(ReDataDisplayFormat pBit);
        void Save();
    }
    public class ReDataDisplayFormatService : IReDataDisplayFormatService
    {
        private readonly IReDataDisplayFormatRepository _reDataDisplayFormatRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ReDataDisplayFormatService(IReDataDisplayFormatRepository reDataDisplayFormatRepository, IUnitOfWork unitOfWork)
        {
            this._reDataDisplayFormatRepository = reDataDisplayFormatRepository;
            this._unitOfWork = unitOfWork;
        }

        public void DeleteByDisPlayId(string disPlayId)
        {
            _reDataDisplayFormatRepository.DeleteByDisplayId(disPlayId);
        }
        public void Delete(ReDataDisplayFormat rf)
        {
            _reDataDisplayFormatRepository.Delete(_reDataDisplayFormatRepository.GetById(rf.ReDataDisplayFormatId));
            Save();
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
