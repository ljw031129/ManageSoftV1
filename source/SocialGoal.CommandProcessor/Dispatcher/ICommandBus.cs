using SocialGoal.CommandProcessor.Command;
using SocialGoal.Core.Common;
using System.Collections.Generic;
namespace SocialGoal.CommandProcessor.Dispatcher
{
    public interface ICommandBus
    {
        ICommandResult Submit<TCommand>(TCommand command) where TCommand: ICommand;
        IEnumerable<ValidationResult> Validate<TCommand>(TCommand command) where TCommand : ICommand;
    }
}

