using SocialGoal.CommandProcessor.Command;
using SocialGoal.Core.Common;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Repository;
using SocialGoal.Domain.Commands;
using SocialGoal.Domain.Properties;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Domain.Handlers
{
    public class CanTerminalSimCard : IValidationHandler<CreateOrUpdateTerminalSimCardCommand>
    {
        private readonly ITerminalSimCardRepository _terminalSimCardRepository;
        public CanTerminalSimCard(ITerminalSimCardRepository terminalSimCardRepository, IUnitOfWork unitOfWork)
        {
            this._terminalSimCardRepository = terminalSimCardRepository;
        }
        public IEnumerable<ValidationResult> Validate(CreateOrUpdateTerminalSimCardCommand command)
        {
            TerminalSimCard isTerminalSimCardExists = null;
            if (command.oper == "add")
                isTerminalSimCardExists = _terminalSimCardRepository.Get(c => c.TerminalSimCardNum == command.TerminalSimCardNum);
            else
                isTerminalSimCardExists = _terminalSimCardRepository.Get(c => c.TerminalSimCardNum == command.TerminalSimCardNum && c.TerminalSimCardId != command.TerminalSimCardId);
            if (isTerminalSimCardExists != null)
            {
                yield return new ValidationResult("Name", Resources.TerminalSimCardNumExists);
            }
        }
    }
}
