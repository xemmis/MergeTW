using UnityEngine;

[CreateAssetMenu(fileName = "NpsData_", menuName = "Nps/Level")]
public class NpsData : ScriptableObject
{
    public int Level;
    public int Health;
    public int Damage;
    public BaseNpc Prefab; 
}