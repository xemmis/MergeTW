using UnityEngine;

public class DragController : MonoBehaviour, IDragController
{
    [SerializeField] private Camera _mainCamera;
    private ISelectable _draggedObject;
    private bool _isDragging = false;
    private Plane _dragPlane;
    private Vector3 _dragOffset;
    private float _originalZPosition;

    #region IDragController
    public void StartDrag(ISelectable selectable, Vector2 screenPosition)
    {
        _draggedObject = selectable;
        _isDragging = true;

        var transform = ((MonoBehaviour)_draggedObject).transform;
        _originalZPosition = transform.position.z;
        _dragPlane = new Plane(Vector3.forward, new Vector3(0, 0, _originalZPosition));

        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        if (_dragPlane.Raycast(ray, out float enter))
        {
            Vector3 worldPoint = ray.GetPoint(enter);
            _dragOffset = transform.position - worldPoint;
        }

        _draggedObject.OnDragStart();
    }

    public void UpdateDrag(Vector2 screenPosition)
    {
        if (!_isDragging || _draggedObject == null) return;

        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        if (_dragPlane.Raycast(ray, out float enter))
        {
            Vector3 worldPosition = ray.GetPoint(enter) + _dragOffset;
            worldPosition.z = _originalZPosition;
            _draggedObject.OnDragUpdate(worldPosition);
        }
    }

    public void EndDrag(Vector2 screenPosition)
    {
        if (!_isDragging || _draggedObject == null) return;

        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        if (_dragPlane.Raycast(ray, out float enter))
        {
            Vector3 worldPosition = ray.GetPoint(enter) + _dragOffset;
            worldPosition.z = _originalZPosition;
            _draggedObject.OnDragEnd(worldPosition);
        }

        _isDragging = false;
        _draggedObject = null;
    }
    #endregion
}
