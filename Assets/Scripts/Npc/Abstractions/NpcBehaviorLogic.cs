using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Npc))]
public abstract class NpcBehaviorLogic : MonoBehaviour
{
    [SerializeField] private protected NpcData _npcData = null;
    [SerializeField] private protected Npc _currentNpc = null;
    [SerializeField] private protected Animator _animator = null;
    [SerializeField] private protected BoxCollider _boxCollider = null;
    [SerializeField] private protected float _detectionRange = 2;
    [SerializeField] private protected float _attackRange = 1;
    private protected INpcState _currentState = null;

    protected virtual void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        if (_currentNpc == null)
        {
            _currentNpc = GetComponent<Npc>();
        }

        if (_boxCollider == null)
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }

    public void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; 
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void ChangeState(INpcState newState)
    {
        print(newState.GetType());
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState?.Enter(this);
    }

    protected virtual void Update()
    {
        _currentState?.Update(this);
    }

    public Npc GetNpc() => _currentNpc;
    public INpcState GetCurrentState() => _currentState;
    public Animator GetAnimator() => _animator;
    public NpcData GetNpcData() => _npcData;
    public BoxCollider GetCollider() => _boxCollider;
    public float GetDetectionRange() => _detectionRange;
    public float GetAttackRange() => _attackRange;
}