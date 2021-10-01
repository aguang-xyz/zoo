using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Models;

namespace Zoo.Protocol.AspNetCore.Middlewares
{
    internal sealed class RpcMiddleware
    {
        private static readonly Dictionary<Type, IRpcInvoker> Invokers = new();

        public static void RegisterInvoker(Type serviceType, IRpcInvoker invoker)
        {
            lock (Invokers)
            {
                Invokers[serviceType] = invoker;
            }
        }

        public static void UnregisterService(Type serviceType)
        {
            lock (Invokers)
            {
                Invokers.Remove(serviceType);
            }
        }

        private readonly RequestDelegate _next;

        public RpcMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("X-Zoo-Rpc"))
            {
                return _next(context);
            }
            
            return Task.Run(() =>
            {
                var payload = new StreamReader(context.Request.Body).ReadToEndAsync().Result;
                var invocation = new RpcInvocation(JObject.Parse(payload));
                var invoker = Invokers[invocation!.ServiceType]!;

                context.Response.WriteAsync(JsonSerializer.Serialize(invoker.Invoke(invocation)));
            });
        }
    }
}