using UnityEngine;

public class Spawner : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isOccupied = false;

    public int Priority { get; private set; } = 90;

    public bool IsOccupied()
    {
        return _isOccupied;
    }

    public void SetStatus(bool condition)
    {
        _isOccupied = condition;
    }

    public bool TryInteract(SkeletonBehaviorLogic skeleton)
    {
        if (_isOccupied)
        {
            return false;
        }

        Spawner oldSpawner = skeleton.GetNpc().GetSpawner();
        oldSpawner.SetStatus(false);

        skeleton.GetNpc().SetSpawner(this);
        skeleton.ChangeState(new IdleState());
        skeleton.transform.position = transform.position;
        SetStatus(true);
        return true;
    }
}
