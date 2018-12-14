using System;
using System.Collections.Generic;
using System.Linq;

namespace MEF.Launcher.Contract.IoC
{
    public static class SimpleIoC
    {
        /// <summary>
        /// The get instance
        /// </summary>
        public static Func<Type, string, object> GetInstance;

        /// <summary>
        /// The get all instances
        /// </summary>
        public static Func<Type, IEnumerable<object>> GetAllInstances;

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T Get<T>(string key = null)
        {
            return (T) GetInstance(typeof(T), key);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAll<T>()
        {
            return GetAllInstances(typeof(T)).Select(obj => (T) obj);
        }
    }
}
