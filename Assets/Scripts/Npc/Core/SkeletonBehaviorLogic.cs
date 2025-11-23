using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehaviorLogic : NpcBehaviorLogic, IInteractable
{
    private MergeHandler _mergeHandler = null;
    private ISelectable _selectable = null;
    [SerializeField] private float _interactionRadius = 0.35f;

    private List<IInteractable> _interactables = new List<IInteractable>();

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
        _selectable?.OnDragFinish.AddListener(CheckInteractions);
    }

    private void OnDestroy()
    {
        _selectable?.OnDrag.RemoveListener(HandleDrag);
        _selectable?.OnDragFinish.RemoveListener(CheckInteractions);
    }

    private void HandleDrag()
    {
        ChangeState(new SelectState());
        return;
    }

    public void CheckInteractions(Vector3 position)
    {
        FindInteractables(position);

        foreach (var interactable in _interactables)
        {
            if (interactable.TryInteract(this))
                return;
        }

        ReturnToStartPosition();
    }

    private void FindInteractables(Vector3 position)
    {
        _interactables.Clear();
        Collider[] colliders = new Collider[5];
        int count = Physics.OverlapSphereNonAlloc(position, _interactionRadius, colliders);

        for (int i = 0; i < count; i++)
        {
            if (colliders[i].TryGetComponent<IInteractable>(out var interactable))
            {
                _interactables.Add(interactable);
            }
        }

        _interactables.Sort((a, b) => b.Priority.CompareTo(a.Priority));
    }

    private void ReturnToStartPosition()
    {
        transform.position = _selectable.StartDragPosition;
        ChangeState(new IdleState());
    }

    #region  IInteractable

    public int Priority { get; private set; } = 40;

    public ISelectable GetSelectable() => _selectable;

    public bool TryInteract(SkeletonBehaviorLogic skeleton)
    {
        if (skeleton.gameObject == gameObject)
        {
            return false;
        }

        ChangeState(new IdleState());
        _mergeHandler.HandleMerge(skeleton, this);

        return true;
    }
    #endregion
}


public interface IInteractable
{
    int Priority { get; }
    bool TryInteract(SkeletonBehaviorLogic skeleton);
}