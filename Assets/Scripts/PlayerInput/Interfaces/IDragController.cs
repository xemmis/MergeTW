using UnityEngine;

public interface IDragController
{
    void StartDrag(ISelectable selectable, Vector2 screenPosition);
    void UpdateDrag(Vector2 screenPosition);
    void EndDrag(Vector2 screenPosition);
}