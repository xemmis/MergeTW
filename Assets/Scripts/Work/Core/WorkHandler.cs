using System.Collections;
using UnityEngine;

public class WorkHandler : MonoBehaviour, IInteractable
{
    [SerializeField] private Spawner _positionToWork = null;
    [SerializeField] private SkeletonBehaviorLogic _worker = null;
    [SerializeField] private SkeletonConfigurator _skeletonConfigurator = null;
    [SerializeField] private WorkType _workType = WorkType.Other;
    private Coroutine _workRoutine = null;
    private int _earnTick = 5;
    private int _earnAmount = 5;

    public int Priority { get; private set; } = 50;

    public void Init(SkeletonConfigurator skeletonConfigurator)
    {
        _skeletonConfigurator = skeletonConfigurator;
    }

    public void HandleWork(SkeletonBehaviorLogic newWorker)
    {
        _worker = newWorker;
        _worker.GetSelectable().IsSelectable = false;
        _earnAmount = newWorker.GetNpc().GetEarnAmount();
        _earnTick = newWorker.GetNpc().GetEarnTick();

        _skeletonConfigurator.Configure(newWorker, newWorker.GetNpcData(), _positionToWork);
        _worker.ChangeState(new WorkState(_workType));

        _workRoutine = StartCoroutine(EarnTick());
    }

    private void SwapWorkerLogic(SkeletonBehaviorLogic newWorker)
    {
        _worker.GetSelectable().IsSelectable = true;
        _skeletonConfigurator.Configure(_worker, _worker.GetNpcData(), newWorker.GetNpc().GetSpawner());
        StopCoroutine(_workRoutine);
    }

    private IEnumerator EarnTick()
    {
        yield return new WaitForSeconds(_earnTick);
        Wallet.EarnMoney(_earnAmount);

        if (_worker != null)
        {
            _workRoutine = StartCoroutine(EarnTick());
        }
    }

    public bool TryInteract(SkeletonBehaviorLogic skeleton)
    {
        if (skeleton == null)
        {
            return false;
        }

        if (_worker != null)
        {
            SwapWorkerLogic(skeleton);
            HandleWork(skeleton);
            return true;
        }

        Spawner oldSpawner = skeleton.GetNpc().GetSpawner();
        oldSpawner.SetStatus(false);
        HandleWork(skeleton);

        return true;
    }
}
