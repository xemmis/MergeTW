using UnityEngine;

public interface ISkeletonSelectHandler
{
    bool IsSelectable { get; }
    void OnDragStart();
    void OnDragUpdate(Vector3 worldPosition);
    void OnDragEnd(Vector3 worldPosition);
}