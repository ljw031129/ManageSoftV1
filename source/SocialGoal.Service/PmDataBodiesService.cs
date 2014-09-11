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
    public interface IPmDataBodiesService
    {
        IEnumerable<PmDataBody> GePmDataBody();
        IEnumerable<PmDataBody> GePmDataBody(string pmId);

        PmFInterpreter GetPmFInterpreterById(string pmId);
        void SavePmDataBody();
        Task<bool> UpdatePmDataBodyAsync(PmDataBody pmdataBody);

        void Delete(PmDataBody pDataBody);
    }
    public class PmDataBodiesService : IPmDataBodiesService
    {
        private readonly IPmDataBitRepository _pmDataBitRepository;
        private readonly IPmDataBodiesRepository _pmDataBodiesRepository;
        private readonly IPmDataBytesRepository _pmDataBytesRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PmDataBodiesService(IPmDataBodiesRepository pmDataBodiesRepository, IUnitOfWork unitOfWork, IPmDataBitRepository pmDataBitRepository, IPmDataBytesRepository pmDataBytesRepository)
        {
            this._pmDataBodiesRepository = pmDataBodiesRepository;
            this._unitOfWork = unitOfWork;
            this._pmDataBitRepository = pmDataBitRepository;
            this._pmDataBytesRepository = pmDataBytesRepository;
        }

        public IEnumerable<PmDataBody> GePmDataBody()
        {
            var pmDataBody = _pmDataBodiesRepository.GetAll();
            return pmDataBody;
        }


        public IEnumerable<PmDataBody> GePmDataBody(string pmId)
        {
            var pmDataBody = _pmDataBodiesRepository.GePmDataBody(pmId);
            return pmDataBody;
        }


        public Task<bool> UpdatePmDataBodyAsync(PmDataBody pmdataBody)
        {
            //bit类型
            if (pmdataBody.DataType == 2)
            {
                pmdataBody.PmDataByte = null;
                _pmDataBitRepository.DeleteByte(pmdataBody.PmDataBodyId);

            }
            //byte类型
            if (pmdataBody.DataType == 1)
            {
                _pmDataBitRepository.DeleteBit(pmdataBody.PmDataBodyId);

            }


            if (pmdataBody.PmDataByte != null)
            {
                if (_pmDataBytesRepository.CanUpdate(pmdataBody.PmDataByte.PmDataByteId))
                {
                    _pmDataBytesRepository.Update(pmdataBody.PmDataByte);
                }
                else
                {

                    _pmDataBytesRepository.Add(pmdataBody.PmDataByte);
                }
            }

            if (pmdataBody.PmDataBits != null && pmdataBody.PmDataBits.Count > 0)
            {
                foreach (var item in pmdataBody.PmDataBits)
                {
                    if (!_pmDataBitRepository.CanUpData(item.PmDataBitId))
                    {
                        _pmDataBitRepository.Update(item);
                    }
                    else
                    {
                        item.PmDataBodyId = pmdataBody.PmDataBodyId;
                        _pmDataBitRepository.Add(item);
                    }

                }
            }

            if (_pmDataBodiesRepository.CanUpdate(pmdataBody.PmDataBodyId))
            {
                _pmDataBodiesRepository.Update(pmdataBody);
            }
            else
            {
                _pmDataBodiesRepository.Add(pmdataBody);
            }
            SavePmDataBody();
            return Task.FromResult(true);
        }


        public void SavePmDataBody()
        {
            _unitOfWork.Commit();
        }


        public PmFInterpreter GetPmFInterpreterById(string pmId)
        {
            var pmDataBody = _pmDataBodiesRepository.GetPmFInterpreterById(pmId);
            return pmDataBody;
        }


        public void Delete(PmDataBody pDataBody)
        {
            _pmDataBodiesRepository.Delete(_pmDataBodiesRepository.GetById(pDataBody.PmDataBodyId));
            if (pDataBody.PmDataBits != null)
            {
                foreach (var item in pDataBody.PmDataBits)
                {
                    _pmDataBitRepository.Delete(_pmDataBitRepository.GetById(item.PmDataBitId));

                }
            }
            _pmDataBodiesRepository.Delete(_pmDataBodiesRepository.GetById(pDataBody.PmDataBodyId));
            SavePmDataBody();
        }
    }
}
