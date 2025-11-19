using UnityEngine;

[CreateAssetMenu(fileName = "SkeletonData_", menuName = "Skeletons/Level")]
public class SkeletonData : ScriptableObject
{
    public int Level;
    public int Health;
    public int Damage;
    public Skeleton Prefab; 
}