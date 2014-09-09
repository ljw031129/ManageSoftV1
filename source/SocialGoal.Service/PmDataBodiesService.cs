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

    }
    public class PmDataBodiesService : IPmDataBodiesService
    {
        private readonly IPmDataBodiesRepository _pmDataBodiesRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PmDataBodiesService(IPmDataBodiesRepository pmDataBodiesRepository, IUnitOfWork unitOfWork)
        {
            this._pmDataBodiesRepository = pmDataBodiesRepository;
            this._unitOfWork = unitOfWork;
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
    }
}
