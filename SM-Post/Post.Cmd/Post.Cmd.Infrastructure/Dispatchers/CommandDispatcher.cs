using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Commands;
using CQRS.Core.Infrastructure;
namespace Post.Cmd.Infrastructure.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Dictionary<Type, Func<BaseCommand , Task> > handlers =  new();
        public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(BaseCommand command)
        {
            throw new NotImplementedException();
        }
    }
}