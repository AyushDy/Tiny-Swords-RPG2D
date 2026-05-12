using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    private static Dictionary<Type,Dictionary<string, List<Action<object>>>> listeners = new();

    public static void Subscribe<T>(string targetId, Action<object> callback)
    {
        Type eventType = typeof(T);

        if (!listeners.ContainsKey(eventType))
        {
            listeners[eventType] = new Dictionary<string, List<Action<object>>>();
        }

        if (!listeners[eventType].ContainsKey(targetId))
        {
            listeners[eventType][targetId] = new List<Action<object>>();
        }

        listeners[eventType][targetId].Add(callback);
    }


    public static void Unsubscribe<T>(string targetId, Action<object> callback)
    {
        Type eventType = typeof(T);

        if (!listeners.ContainsKey(eventType))
        {
            return;
        }

        if(!listeners[eventType].ContainsKey(targetId))
        {
            return;
        }

        listeners[eventType][targetId].Remove(callback);
    }


    public static void Publish<T>(string targetId, T eventData)
    {
        Type eventType = typeof(T);

        if (!listeners.ContainsKey(eventType))
        {
            return;
        }

        if (!listeners[eventType].ContainsKey(targetId))
        {
            return;
        }

        List<Action<object>> targetListeners = listeners[eventType][targetId];

        for(int i = 0; i < targetListeners.Count; i++)
        {
            targetListeners[i]?.Invoke(eventData);
        }
    }
}
