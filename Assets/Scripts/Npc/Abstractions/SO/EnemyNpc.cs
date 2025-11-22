using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Npc", menuName = "Nps Menu/New Enemy Npc")]
public class EnemyNpc : ScriptableObject
{
    public int Level;
    public int Health;
    public int Damage;
    public SkeletonBehaviorLogic Prefab;
}