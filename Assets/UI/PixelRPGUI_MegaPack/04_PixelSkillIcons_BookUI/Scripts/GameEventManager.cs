using System;
using System.Collections.Generic;
using UnityEngine;

namespace Indigolay
{
  

    /// <summary>
    /// Static event manager for global game events
    /// </summary>
    public static class GameEventManager
    {
        private static Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();
        private static Dictionary<string, Delegate> eventDictionaryWithParam = new Dictionary<string, Delegate>();

        /// <summary>
        /// Add a listener to an event (no parameters)
        /// </summary>
        public static void AddListener(string eventName, Action listener)
        {
            if (listener == null)
            {
                Debug.LogWarning($"Trying to add null listener to event: {eventName}");
                return;
            }

            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] += listener;
            }
            else
            {
                eventDictionary[eventName] = listener;
            }
        }

        /// <summary>
        /// Add a listener to an event (with 1 parameter)
        /// </summary>
        public static void AddListener<T>(string eventName, Action<T> listener)
        {
            if (listener == null)
            {
                Debug.LogWarning($"Trying to add null listener to event: {eventName}");
                return;
            }

            if (eventDictionaryWithParam.ContainsKey(eventName))
            {
                eventDictionaryWithParam[eventName] = Delegate.Combine(eventDictionaryWithParam[eventName], listener);
            }
            else
            {
                eventDictionaryWithParam[eventName] = listener;
            }
        }

        /// <summary>
        /// Remove a listener from an event (no parameters)
        /// </summary>
        public static void RemoveListener(string eventName, Action listener)
        {
            if (listener == null) return;

            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] -= listener;

                // Remove the key if no listeners remain
                if (eventDictionary[eventName] == null)
                {
                    eventDictionary.Remove(eventName);
                }
            }
        }

        /// <summary>
        /// Remove a listener from an event (with 1 parameter)
        /// </summary>
        public static void RemoveListener<T>(string eventName, Action<T> listener)
        {
            if (listener == null) return;

            if (eventDictionaryWithParam.ContainsKey(eventName))
            {
                eventDictionaryWithParam[eventName] = Delegate.Remove(eventDictionaryWithParam[eventName], listener);

                // Remove the key if no listeners remain
                if (eventDictionaryWithParam[eventName] == null)
                {
                    eventDictionaryWithParam.Remove(eventName);
                }
            }
        }

        /// <summary>
        /// Trigger an event (no parameters)
        /// </summary>
        public static void TriggerEvent(string eventName)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                try
                {
                    eventDictionary[eventName]?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error invoking event '{eventName}': {e.Message}\n{e.StackTrace}");
                }
            }
        }

        /// <summary>
        /// Trigger an event (with 1 parameter)
        /// </summary>
        public static void TriggerEvent<T>(string eventName, T parameter)
        {
            if (eventDictionaryWithParam.ContainsKey(eventName))
            {
                try
                {
                    var action = eventDictionaryWithParam[eventName] as Action<T>;
                    action?.Invoke(parameter);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error invoking event '{eventName}': {e.Message}\n{e.StackTrace}");
                }
            }
        }

        /// <summary>
        /// Clear all events (useful for scene transitions or cleanup)
        /// </summary>
        public static void ClearAllEvents()
        {
            eventDictionary.Clear();
            eventDictionaryWithParam.Clear();
        }

        /// <summary>
        /// Check if an event has any listeners
        /// </summary>
        public static bool HasListeners(string eventName)
        {
            return eventDictionary.ContainsKey(eventName) || eventDictionaryWithParam.ContainsKey(eventName);
        }

        /// <summary>
        /// Get the number of listeners for an event
        /// </summary>
        public static int GetListenerCount(string eventName)
        {
            int count = 0;

            if (eventDictionary.ContainsKey(eventName) && eventDictionary[eventName] != null)
            {
                count += eventDictionary[eventName].GetInvocationList().Length;
            }

            if (eventDictionaryWithParam.ContainsKey(eventName) && eventDictionaryWithParam[eventName] != null)
            {
                count += eventDictionaryWithParam[eventName].GetInvocationList().Length;
            }

            return count;
        }
    }
}