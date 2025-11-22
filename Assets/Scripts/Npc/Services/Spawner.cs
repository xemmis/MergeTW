using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private bool _isOccupied = false;

    public bool IsOccupied()
    {
        return _isOccupied;
    }
    public void SetStatus(bool condition)
    {
        _isOccupied = condition;
    }
}
