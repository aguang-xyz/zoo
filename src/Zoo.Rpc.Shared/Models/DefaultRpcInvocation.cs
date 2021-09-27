using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Shared.Utils;

namespace Zoo.Rpc.Shared.Models
{
    /// <summary>
    /// Default RPC invocation.
    /// </summary>
    public class DefaultRpcInvocation : IRpcInvocation
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
            get
            {
                return ParameterTypes.Select(x => x.FullName).ToArray();
            }
            
            set => ParameterTypes = value.Select(TypeUtils.TypeOf).ToArray();
        }
        
        public object[] Parameters { get; set; }
        
        public IDictionary<string, string> Attachments { get; set; }

        public DefaultRpcInvocation()
        {
        }

        public DefaultRpcInvocation(IRpcInvocation invocation)
        {
            ServiceTypeName = invocation.ServiceType.FullName!;
            MethodName = invocation.MethodName;
            ParameterTypeNames = invocation
                .ParameterTypes
                .Select(type => type.FullName!)
                .ToArray();
            Parameters = invocation.Parameters;
            Attachments = invocation.Attachments;
        }

        public DefaultRpcInvocation(JObject payload)
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