using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Player Input Components")]
    [SerializeField] private DragController _dragController;
    [SerializeField] private InputDetector _inputDetector;
    [SerializeField] private ObjectSelector _objectSelector;
    [Header("Player Input Core")]
    [SerializeField] private PlayerInputHandler _playerInputHandler;

    private void Awake()
    {
        InitializePlayerInput();
    }

    private void InitializePlayerInput()
    {
        _playerInputHandler.Initialize(_inputDetector, _objectSelector, _dragController);
    }
}