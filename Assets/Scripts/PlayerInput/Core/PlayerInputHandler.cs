using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private IInputDetector _inputDetector;
    [SerializeField] private IObjectSelector _objectSelector;
    [SerializeField] private IDragController _dragController;

    public void Initialize(IInputDetector inputDetector, IObjectSelector objectSelector, IDragController dragController)
    {
        _inputDetector = inputDetector;
        _objectSelector = objectSelector;
        _dragController = dragController;

        InitializeInputDetector();
    }

    private void InitializeInputDetector()
    {
        if (_inputDetector == null)
        {
            throw new System.ArgumentNullException(nameof(InputDetector), "InputDetector is not assigned");
        }

        _inputDetector.OnTouchBegan += HandleTouchBegan;
        _inputDetector.OnTouchMoved += HandleTouchMoved;
        _inputDetector.OnTouchEnded += HandleTouchEnded;
    }

    private void HandleTouchBegan(Vector2 touchPosition)
    {
        ISelectable selectable = _objectSelector.GetSelectableAtPosition(touchPosition);
        if (selectable != null)
        {
            _dragController.StartDrag(selectable, touchPosition);
        }
    }

    private void HandleTouchMoved(Vector2 touchPosition)
    {
        _dragController.UpdateDrag(touchPosition);
    }

    private void HandleTouchEnded(Vector2 touchPosition)
    {
        _dragController.EndDrag(touchPosition);
    }

    private void OnDestroy()
    {
        _inputDetector.OnTouchBegan -= HandleTouchBegan;
        _inputDetector.OnTouchMoved -= HandleTouchMoved;
        _inputDetector.OnTouchEnded -= HandleTouchEnded;
    }
}
