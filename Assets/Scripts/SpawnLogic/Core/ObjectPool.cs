using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private Queue<T> _pool = new();
    private T _prefab = null;

    public ObjectPool(T prefab, int initialSize)
    {
        _prefab = prefab;
        for (int i = 0; i < initialSize; i++)
        {
            T obj = UnityEngine.Object.Instantiate(prefab);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }

    public T Get()
    {
        if (_pool.Count == 0)
        {
            return UnityEngine.Object.Instantiate(_prefab); //spawn new Object if pool is empty
        }
        return _pool.Dequeue();
    }
}