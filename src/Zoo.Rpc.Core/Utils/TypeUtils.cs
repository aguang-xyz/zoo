using System;
using System.Collections.Generic;
using System.Linq;

namespace Zoo.Rpc.Core.Utils
{
    /// <summary>
    /// Type utils.
    /// </summary>
    public static class TypeUtils
    {
        /// <summary>
        /// Get type by fullname.
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static Type TypeOf(string fullname)
        {
            return AssemblyUtils
                .GetAll()
                .SelectMany(assembly => assembly.GetTypes())
                .First(type => type.FullName == fullname);
        }

        /// <summary>
        /// Get implementation types of a given interface.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> ImplementationsOf<TInterface>()
        {
            var interfaceType = typeof(TInterface);
            
            if (!interfaceType.IsInterface)
            {
                throw new InvalidOperationException($"{nameof(interfaceType)} should be an interface type");
            }

            return AssemblyUtils
                .GetAll()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && interfaceType.IsAssignableFrom(type))
                .GroupBy(x => x.FullName)
                .Select(g => g.First());
        }
    }
}