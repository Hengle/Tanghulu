/// Uncomment the line below to check the script in build.
// #undef UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AutoSingleton
{
    /// <summary>
    /// Generic access class for singleton instances.
    /// </summary>
    /// <typeparam name="T"> Singleton type/parent type/interface. </typeparam>
    public static class Singleton<T> where T : class
    {
        static T _instance;

        static IReadOnlyList<T> _instances;

        /// <summary> Singleton instance of type <typeparamref name="T"/> or the one derived of <typeparamref name="T"/> selected using <see cref="SelectInstance()"/>. </summary>
        public static T Instance =>
#if UNITY_EDITOR
             EditorGetInstance();
#else
            _instance;
#endif

        /// <summary> All singleton instances derived from <typeparamref name="T"/>. </summary>
        public static IReadOnlyList<T> Instances =>
#if UNITY_EDITOR
             EditorGetInstances();
#else
            _instances;
#endif

        /// <summary> True if <see cref="Instance"/> has a value, false if it would throw en exception. </summary>
        public static bool HasInstance => (_instance != null);

        /// <summary>
        /// Return a new array of all singleton instances that match the given <paramref name="predicate"/>.
        /// </summary>
        public static T[] Find(Predicate<T> predicate)
        {
            Assert.IsPlaying(nameof(Find));

            List<T> retList = new List<T>();

            foreach (T singleton in Instances)
                if (predicate(singleton))
                    retList.Add(singleton);

            return retList.ToArray();
        }

        /// <summary>
        /// Set <see cref="Instance"/> to the unique singleton in <see cref="Instances"/> that match the given <paramref name="predicate"/>.
        /// </summary>
        /// <returns> Returns wether we found an instance to select. </returns>
        public static bool SelectInstance(Predicate<T> predicate)
        {
            Assert.IsPlaying(nameof(SelectInstance));

            bool duplicateMatch = false;
            T selectedInstance = null;

            foreach (T singleton in Instances)
                if (predicate(singleton))
                {
                    if (selectedInstance != null)
                        duplicateMatch = true;
                    else
                        selectedInstance = singleton;
                }

            if (duplicateMatch == true)
                return false;

            _instance = selectedInstance;
            return true;
        }

        /// <summary>
        /// Set <see cref="Instance"/> to the unique singleton in <see cref="Instances"/> that have the highest given <paramref name="priority"/>.
        /// </summary>
        /// <returns> <inheritdoc cref="SelectInstance(Predicate{T})" path="/returns"/> </returns>
        public static bool SelectInstance(Func<T, int> priority) => SelectInstanceFromPriority(priority, int.MinValue, (a, b) => a > b);
        /// <inheritdoc cref="SelectInstance(Func{T, int})"/>
        public static bool SelectInstance(Func<T, float> priority) => SelectInstanceFromPriority(priority, float.MinValue, (a, b) => a > b);

        /// <summary>
        /// Set <see cref="Instance"/> to the given <paramref name="instance"/>.
        /// </summary>
        /// <returns> <inheritdoc cref="SelectInstance(Predicate{T})" path="/returns"/> </returns>
        public static bool SelectInstance(T instance)
        {
            Assert.IsPlaying(nameof(SelectInstance));

            foreach (T singleton in Instances)
                if (singleton == instance)
                {
                    _instance = singleton;
                    return true;
                }

            return false;
        }

        /// <summary>
        /// Set <see cref="Instance"/> to the singleton of type <typeparamref name="SubT"/>.
        /// </summary>
        /// <returns> <inheritdoc cref="SelectInstance(Predicate{T})" path="/returns"/> </returns>
        public static bool SelectInstance<SubT>() where SubT : T
        {
            Assert.IsPlaying(nameof(SelectInstance));

            foreach (T singleton in Instances)
                if (singleton.GetType() == typeof(SubT))
                {
                    _instance = singleton;
                    return true;
                }

            return false;
        }

        /// <summary>
        /// Set <see cref="Instance"/> to the unique instance derived from <typeparamref name="T"/>.
        /// </summary>
        /// <returns> <inheritdoc cref="SelectInstance(Predicate{T})" path="/returns"/> </returns>
        public static bool SelectInstance()
        {
            Assert.IsPlaying(nameof(SelectInstance));

            if (Instances.Count == 1)
            {
                _instance = Instances[0];
                return true;
            }

            return false;
        }

        static Singleton()
        {
            if (SingletonContainer.Collection.TryGetValue(typeof(T), out Object obj))
                _instance = obj as T;

            List<T> instancesList = new List<T>();
            foreach (Object o in SingletonContainer.Collection.Values)
                if (o is T instance)
                    instancesList.Add(instance);
            _instances = instancesList;
        }

        [HideInCallstack]
        static bool SelectInstanceFromPriority<TValue>(Func<T, TValue> priority, TValue minValue, Func<TValue, TValue, bool> isSuperior) where TValue : struct
        {
            Assert.IsPlaying(nameof(SelectInstance));

            TValue highestPriority = minValue;
            bool duplicateHighest = false;
            T selectedInstance = null;

            foreach (T instance in Instances)
            {
                TValue instancePriority = priority(instance);

                if (isSuperior(instancePriority, highestPriority))
                {
                    highestPriority = instancePriority;
                    duplicateHighest = false;
                    selectedInstance = instance;
                }
                else if (Equals(instancePriority, highestPriority))
                    duplicateHighest = true;
            }

            if (duplicateHighest)
                return false;

            _instance = selectedInstance;
            return true;
        }

#if UNITY_EDITOR
        [HideInCallstack]
        static T EditorGetInstance()
        {
            Assert.IsPlaying(nameof(Instance));

            if (_instance != null)
                return _instance;

            if (Instances.Count >= 1)
                throw new InvalidOperationException($"No singleton of type '{typeof(T).Name}' was selected.");
            else
                throw new InvalidOperationException($"No singleton of type '{typeof(T).Name}' exists.");
        }

        [HideInCallstack]
        static IReadOnlyList<T> EditorGetInstances()
        {
            Assert.IsPlaying(nameof(Instances));

            return _instances;
        }
#endif
    }
}
