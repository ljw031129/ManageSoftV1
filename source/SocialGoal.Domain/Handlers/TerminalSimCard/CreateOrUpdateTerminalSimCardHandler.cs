using SocialGoal.CommandProcessor.Command;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Repository;
using SocialGoal.Domain.Commands;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Domain.Handlers
{
    public class CreateOrUpdateTerminalSimCardHandler : ICommandHandler<CreateOrUpdateTerminalSimCardCommand>
    {
        private readonly ITerminalSimCardRepository _terminalSimCardRepository;
        private readonly IUnitOfWork unitOfWork;
        public CreateOrUpdateTerminalSimCardHandler(ITerminalSimCardRepository terminalSimCardRepository, IUnitOfWork unitOfWork)
        {
            this._terminalSimCardRepository = terminalSimCardRepository;
            this.unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdateTerminalSimCardCommand command)
        {
            var terminalSimCard = new TerminalSimCard
            {
                TerminalSimCardId = command.TerminalSimCardId,
                TerminalSimCardNum = command.TerminalSimCardNum,
                TerminalSimCardSerialNum = command.TerminalSimCardSerialNum,
                TerminalSimCardState = command.TerminalSimCardState,
                TerminalSimCardUpdateTime = command.TerminalSimCardUpdateTime,
                TerminalSimCardCreateTime = command.TerminalSimCardCreateTime
            };
            switch (command.oper)
            {
                case "add":
                    terminalSimCard.TerminalSimCardId = Guid.NewGuid().ToString();
                    terminalSimCard.TerminalSimCardUpdateTime = DateTime.Now;
                    terminalSimCard.TerminalSimCardCreateTime = DateTime.Now;                   
                    _terminalSimCardRepository.Add(terminalSimCard);
                    break;
                case "edit":
                    terminalSimCard.TerminalSimCardUpdateTime = DateTime.Now;
                    _terminalSimCardRepository.Update(terminalSimCard);
                    break;

                case "del":                  
                    _terminalSimCardRepository.Delete(_terminalSimCardRepository.GetById(command.TerminalSimCardId));
                    break;
            }            
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
