using UnityEngine;

public class SkeletonBehaviorLogic : NpcBehaviorLogic
{
    private MergeHandler _mergeHandler = null;
    private ISelectable _selectable = null;

    protected override void Awake()
    {
        base.Awake();

        ChangeState(new IdleState());
    }

    protected override void Initialize()
    {
        base.Initialize();

        _mergeHandler = new MergeHandler();

        _selectable = GetComponent<ISelectable>();
        _selectable?.OnDrag.AddListener(HandleDrag);
        _selectable?.OnDragFinish.AddListener(CheckInteractionsAtPosition);
    }

    private void OnDestroy()
    {
        _selectable?.OnDrag.RemoveListener(HandleDrag);
        _selectable?.OnDragFinish.RemoveListener(CheckInteractionsAtPosition);
    }

    private void HandleDrag()
    {
        ChangeState(new SelectState());
        return;
    }

    private void CheckInteractionsAtPosition(Vector3 position)
    {
        Collider[] nearbyColliders = new Collider[10];
        int colliderCount = Physics.OverlapSphereNonAlloc(position, .35f, nearbyColliders);

        for (int i = 0; i < colliderCount; i++)
        {
            Collider collider = nearbyColliders[i];

            if (collider.gameObject.TryGetComponent<SkeletonBehaviorLogic>(out var npc))
            {
                if (npc != this)
                {
                    ChangeState(new IdleState());
                    _mergeHandler.HandleMerge(this, npc);
                }
            }

            if (collider.TryGetComponent<WorkHandler>(out var workHandler))
            {
                workHandler.HandleWork(this);
                return;
            }

            if (collider.TryGetComponent<Spawner>(out var spawner) && !spawner.IsOccupied())
            {
                Spawner currentSpawner = _currentNpc.GetSpawner();

                if (currentSpawner != null)
                {
                    currentSpawner.SetStatus(false);
                }

                transform.position = spawner.transform.position;
                _currentNpc.SetSpawner(spawner);
                ChangeState(new IdleState());
                return;
            }

            if (collider.TryGetComponent<ITrashCan>(out var trashCan))
            {
                trashCan.AbortSkeleton(this);
                return;
            }
        }

        transform.position = _selectable.StartDragPosition;
        ChangeState(new IdleState());
    }

    public ISelectable GetSelectable() => _selectable;
}
