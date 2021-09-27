using System;
using System.Linq;
using System.Reflection;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Handlers;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Shared.Nodes;
using Zoo.Rpc.Shared.Utils;

namespace Zoo.Rpc.Shared.Extensions
{
    public static class InvokerExtensions
    {
        /// <summary>
        /// Get the order of a handler.
        /// </summary>
        /// <param name="handlerType"></param>
        /// <returns></returns>
        private static int GetOrder(Type handlerType)
        {
            return handlerType.GetCustomAttribute<OrderAttribute>()?.Value ?? 0;
        }
        
        /// <summary>
        /// Add handlers logic into a given invoker.
        /// </summary>
        /// <param name="invoker"></param>
        /// <returns></returns>
        public static IRpcInvoker WithHandlers(this IRpcInvoker invoker)
        {
            foreach (var handlerType in TypeUtils.ImplementationsOf<IRpcHandler>().OrderBy(GetOrder))
            {
                invoker = new HandledRpcInvoker((IRpcHandler) Activator.CreateInstance(handlerType), invoker);
            }

            return invoker;
        }
    }
}