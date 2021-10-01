using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Core.Utils;

namespace Zoo.Rpc.Core.Models
{
    /// <summary>
    /// RPC invocation.
    /// </summary>
    public class RpcInvocation : IRpcInvocation
    {
        [JsonIgnore]
        public Type ServiceType { get; private set; }

        public string ServiceTypeName
        {
            get => ServiceType.FullName;
            set => ServiceType = TypeUtils.TypeOf(value);
        }

        public string MethodName { get; set; }

        [JsonIgnore]
        public Type[] ParameterTypes { get; private set; }

        public string[] ParameterTypeNames
        {
            get => ParameterTypes.Select(x => x.FullName).ToArray();
            set => ParameterTypes = value.Select(TypeUtils.TypeOf).ToArray();
        }
        
        public object[] Parameters { get; set; }
        
        public IDictionary<string, string> Attachments { get; set; }

        public RpcInvocation()
        {
        }

        public RpcInvocation(IRpcInvocation rpcInvocation)
        {
            ServiceTypeName = rpcInvocation.ServiceType.FullName!;
            MethodName = rpcInvocation.MethodName;
            ParameterTypeNames = rpcInvocation
                .ParameterTypes
                .Select(type => type.FullName!)
                .ToArray();
            Parameters = rpcInvocation.Parameters;
            Attachments = rpcInvocation.Attachments;
        }

        public RpcInvocation(JObject payload)
        {
            ServiceTypeName = payload["serviceTypeName"].ToObject<string>();
            MethodName = payload["methodName"].ToObject<string>();
            ParameterTypeNames = payload["parameterTypeNames"].ToObject<string[]>();

            // Load parameters.
            var parameters = new List<object>();
            for (var i = 0; i < ParameterTypes!.Length; i++)
            {
                parameters.Add(payload["parameters"][i]!.ToObject(ParameterTypes![i]));
            }
            Parameters = parameters.ToArray();
            
            Attachments = payload["attachments"].ToObject<Dictionary<string, string>>();
        }
    }
}