using UnityEngine;
using UnityEngine.Events;

public interface ISelectable
{
    Vector3 StartDragPosition { get; }
    bool IsSelectable { get; set; }
    void OnDragStart();
    void OnDragUpdate(Vector3 worldPosition);
    void OnDragEnd(Vector3 worldPosition);

    UnityEvent OnDrag { get; }
    UnityEvent<Vector3> OnDragFinish { get; }
}
