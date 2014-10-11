using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocialGoal.CommandProcessor;
using SocialGoal.Core.Common;

namespace SocialGoal.CommandProcessor.Command
{
    public interface IValidationHandler<in TCommand> where TCommand : ICommand
    {
        IEnumerable<ValidationResult>  Validate(TCommand command);
    }
}
