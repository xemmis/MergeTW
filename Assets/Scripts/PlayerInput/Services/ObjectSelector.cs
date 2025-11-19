using UnityEngine;

public class ObjectSelector : MonoBehaviour, IObjectSelector
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _selectableLayerMask;

    private RaycastHit[] _raycastHits = new RaycastHit[3];

    public ISelectable GetSelectableAtPosition(Vector2 screenPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        int hitCount = Physics.RaycastNonAlloc(ray, _raycastHits, Mathf.Infinity, _selectableLayerMask);

        for (int i = 0; i < hitCount; i++)
        {
            ISelectable selectable = _raycastHits[i].collider.GetComponent<ISelectable>();
            if (selectable != null && selectable.IsSelectable)
            {
                return selectable;
            }
        }
        return null;
    }
}
