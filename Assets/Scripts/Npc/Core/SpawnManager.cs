using System;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnManager
{
    private static Dictionary<Type, object> _factories = new();

    public static void RegisterFactory<T>(INpcFabric<T> factory) where T : Component
    {
        _factories[typeof(T)] = factory;
    }

    public static T BuyNpc<T>(Spawner spawner, int level = 1) where T : Component
    {
        if (TryGetFactory<T>(out INpcFabric<T> factory))
        {
            if (spawner != null)
            {
                return factory.BuyNpc(spawner, level);
            }
            else
            {
                Debug.LogWarning("No free spawners available!");
                return null;
            }
        }

        Debug.LogError($"Factory for type {typeof(T)} not registered!");
        return null;
    }

    public static T MergeNpc<T>(Spawner spawner, int level = 1) where T : Component
    {
        if (TryGetFactory<T>(out INpcFabric<T> factory))
        {
            return factory.MergeNpc(spawner, level);
        }

        return null;
    }
    
    public static T SpawnNpc<T>(Spawner spawner, int level = 1) where T : Component
    {
        if (TryGetFactory<T>(out INpcFabric<T> factory))
        {   
            return factory.SpawnNpc(spawner, level);
        }

        return null;
    }

    public static void ReturnNpc<T>(T npc) where T : Component
    {
        if (TryGetFactory<T>(out INpcFabric<T> factory))
        {
            factory?.ReturnNpc(npc);
        }
        else
        {
            Debug.LogError($"Factory for type {typeof(T)} not registered! Destroying NPC.");
            UnityEngine.Object.Destroy(npc.gameObject);
        }
    }

    public static bool TryGetFactory<T>(out INpcFabric<T> factory) where T : Component
    {
        if (_factories.TryGetValue(typeof(T), out object factoryObj))
        {
            factory = (INpcFabric<T>)factoryObj;
            return true;
        }

        factory = null;
        return false;
    }

    public static void ClearAll()
    {
        _factories.Clear();
    }
}
