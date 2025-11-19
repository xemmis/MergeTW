using UnityEngine;

public interface IReturnToPool<T> where T : Component
{
    void ReturnToPool(T component);
}