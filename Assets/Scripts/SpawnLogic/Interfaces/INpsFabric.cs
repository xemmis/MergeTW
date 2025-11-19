using UnityEngine;

public interface INpsFabric
{
    GameObject CreateNpc(Spawner positionToSpawn, int level = 1);
    bool CanCreateLevel(int level);
}