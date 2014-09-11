using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Repository;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
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
    }
}
