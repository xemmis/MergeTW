using UnityEngine;

public interface IObjectSelector
{
    ISelectable GetSelectableAtPosition(Vector2 screenPosition);
}
