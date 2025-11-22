using UnityEngine;

[CreateAssetMenu(fileName = "Npc", menuName = "Npc Menu/New Npc")]
public class NpcData : ScriptableObject
{
    public int Cost;
    public int Level;
    public int EarnPerTick;
    public int EarnTick;
    public NpcBehaviorLogic Prefab;
    public bool UseCostOnSpawn;
}
