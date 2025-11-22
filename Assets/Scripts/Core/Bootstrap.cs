using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Player Input Components")]
    [SerializeField] private DragController _dragController;
    [SerializeField] private InputDetector _inputDetector;
    [SerializeField] private ObjectSelector _objectSelector;
    [Header("Player Input Core")]
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [Header("Wallet")]
    [SerializeField] private int _startMoney;

    [Header("Npc Spawners")]
    [SerializeField] private SkeletonFabric _skeletonFabric = null;
    [SerializeField] private SkeletonConfigurator _skeletonConfigurator = null;

    [Header("Work Handler Core")]
    [SerializeField] private List<WorkHandler> _workHandlers = new();

    private void Awake()
    {
        InitializeConfigurators();
        InitializePlayerInput();
        InitializeFabrics();
        InitializeWorkHandlers();
    }

    private void Start()
    {
        Wallet.SetMoney(_startMoney);
    }

    private void InitializeWorkHandlers()
    {
        foreach (WorkHandler workHandler in _workHandlers)
        {
            workHandler.Init(_skeletonConfigurator);
        }
    }

    private void InitializeConfigurators()
    {
        _skeletonConfigurator = new SkeletonConfigurator();
    }

    private void InitializeFabrics()
    {
        SpawnManager.RegisterFactory(_skeletonFabric);
        _skeletonFabric.Init(_skeletonConfigurator);
    }

    private void InitializePlayerInput()
    {
        _playerInputHandler.Initialize(_inputDetector, _objectSelector, _dragController);
    }
}