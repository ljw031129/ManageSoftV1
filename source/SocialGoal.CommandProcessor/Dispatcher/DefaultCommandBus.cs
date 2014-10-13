using System.Collections.Generic;
using System.Web.Mvc;
using SocialGoal.CommandProcessor.Command;
using SocialGoal.Core.Common;
using Autofac;

namespace SocialGoal.CommandProcessor.Dispatcher
{
    public class DefaultCommandBus : ICommandBus
    {
        private readonly IComponentContext _context;
        public DefaultCommandBus(IComponentContext context)
        {
            this._context = context;
        }
        public ICommandResult Submit<TCommand>(TCommand command) where TCommand: ICommand
        {     
            var handler = _context.Resolve<ICommandHandler<TCommand>>();  
          //  var handler = DependencyResolver.Current.GetService<ICommandHandler<TCommand>>();
            if (!((handler != null) && handler is ICommandHandler<TCommand>))
            {
                throw new CommandHandlerNotFoundException(typeof(TCommand));
            }  
            return handler.Execute(command);
 
        }
        public IEnumerable<ValidationResult> Validate<TCommand>(TCommand command) where TCommand : ICommand
        {
           // var handler = DependencyResolver.Current.GetService<IValidationHandler<TCommand>>();
            var handler = _context.Resolve<IValidationHandler<TCommand>>(); 
            if (!((handler != null) && handler is IValidationHandler<TCommand>))
            {
                throw new ValidationHandlerNotFoundException(typeof(TCommand));
            }  
            return handler.Validate(command);
        }
    }
}

