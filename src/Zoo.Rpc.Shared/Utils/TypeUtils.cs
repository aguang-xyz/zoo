using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Zoo.Rpc.Shared.Utils
{
    /// <summary>
    /// Type utils.
    /// </summary>
    public static class TypeUtils
    {
        private static IEnumerable<Assembly> GetAssemblies()
        {
            // Load assemblies of current app domain.
            var assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
            
            // Load additional assemblies of Zoo related packages.
            foreach (var dll in Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Zoo.*.dll"))
            {
                assemblies.Add(Assembly.LoadFile(dll));
            }

            return assemblies.Distinct();
        }
        /// <summary>
        /// Get type by fullname.
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static Type TypeOf(string fullname)
        {
            return GetAssemblies()
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

            return GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && interfaceType.IsAssignableFrom(type))
                .GroupBy(x => x.FullName)
                .Select(g => g.First())
                .ToArray();
        }
    }
}