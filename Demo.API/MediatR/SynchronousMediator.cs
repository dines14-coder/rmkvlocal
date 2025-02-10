using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.MediatR
{
    public class SynchronousMediator : Mediator
    {
        public SynchronousMediator(ServiceFactory serviceFactory) : base(serviceFactory)
        {
        }

        protected override async Task PublishCore(IEnumerable<Func<Task>> allHandlers)
        {
            foreach (var handler in allHandlers)
            {
                await handler.Invoke();
            }
        }
    }
}
