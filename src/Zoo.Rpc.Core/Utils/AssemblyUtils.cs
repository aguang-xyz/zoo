using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Zoo.Rpc.Core.Utils
{
    /// <summary>
    /// Assembly utils.
    /// </summary>
    public class AssemblyUtils
    {
        /// <summary>
        /// Get assemblies by search pattern.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static IEnumerable<Assembly> Get(string searchPattern)
        {
            return Directory
                .GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, searchPattern)
                .Select(Assembly.LoadFile);
        }

        /// <summary>
        /// Get all assemblies.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAll()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Concat(Get("Zoo.*.dll"))
                .GroupBy(assembly => assembly.FullName)
                .Select(group => group.First());
        }
    }
}