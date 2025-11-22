using System.Collections.Generic;
using UnityEngine;

public interface INpcFabric<T> where T : Component
{
    T SpawnNpc(Spawner spawner, int level = 1);
    T BuyNpc(Spawner spawner, int level = 1);
    T MergeNpc(Spawner spawner, int level = 1);
    void ReturnNpc(T component);
    List<T> ReturnCurrentNpc();
}
