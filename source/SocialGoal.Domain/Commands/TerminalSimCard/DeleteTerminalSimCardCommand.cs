using SocialGoal.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Domain.Commands
{
    public class DeleteTerminalSimCardCommand : ICommand
    {
        public string TerminalSimCardId { get; set; }
    }
}
