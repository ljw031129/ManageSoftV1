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

    public interface IPmDataBitsService
    {
        void Delete(PmDataBit pBit);
        void Save();
    }
    public class PmDataBitsService : IPmDataBitsService
    {
        private readonly IPmDataBitRepository _pmDataBitsRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PmDataBitsService(IPmDataBitRepository pmDataBitsRepository, IUnitOfWork unitOfWork)
        {
            this._pmDataBitsRepository = pmDataBitsRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Delete(PmDataBit pBit)
        {
            _pmDataBitsRepository.Delete(_pmDataBitsRepository.GetById(pBit.PmDataBitId));
            Save();
        }


        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
