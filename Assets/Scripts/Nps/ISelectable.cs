using UnityEngine;

public interface ISelectable
{
    bool IsSelectable { get; }
    void OnDragStart();
    void OnDragUpdate(Vector3 worldPosition);
    void OnDragEnd(Vector3 worldPosition);
}