using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace AutoSingleton
{
    /// <summary>
    /// Non-generic access class for singleton instances.
    /// </summary>
    public static class Singleton
    {
        /// <summary>
        /// Return a new array of all singleton instances that match the given <paramref name="predicate"/>.
        /// </summary>
        public static Object[] Find(Predicate<Object> predicate)
        {
            Assert.IsPlaying(nameof(Find));

            List<Object> retList = new List<Object>();

            foreach (Object singleton in SingletonContainer.Collection.Values)
                if (predicate(singleton))
                    retList.Add(singleton);

            return retList.ToArray();
        }
    }
}
