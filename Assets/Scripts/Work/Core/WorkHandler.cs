using System.Collections;
using UnityEngine;

public class WorkHandler : MonoBehaviour
{
    [SerializeField] private Spawner _positionToWork = null;
    [SerializeField] private SkeletonBehaviorLogic _worker = null;
    [SerializeField] private SkeletonConfigurator _skeletonConfigurator = null;
    [SerializeField] private WorkType _workType = WorkType.Other;
    private int _earnTick = 5;
    private int _earnAmount = 5;

    public void Init(SkeletonConfigurator skeletonConfigurator)
    {
        _skeletonConfigurator = skeletonConfigurator;
    }

    public void HandleWork(SkeletonBehaviorLogic newWorker)
    {
        if (_worker != null)
        {
            _worker.GetSelectable().IsSelectable = true;
            _skeletonConfigurator.Configure(_worker, _worker.GetNpcData(), newWorker.GetNpc().GetSpawner());
        }
        else
        {
            Spawner spawner = newWorker.GetNpc().GetSpawner();
            spawner.SetStatus(false);
        }

        _worker = newWorker;
        _worker.GetSelectable().IsSelectable = false;
        _earnAmount = newWorker.GetNpc().GetEarnAmount();
        _earnTick = newWorker.GetNpc().GetEarnTick();

        _skeletonConfigurator.Configure(newWorker, newWorker.GetNpcData(), _positionToWork);
        _worker.ChangeState(new WorkState(_workType));
        StartCoroutine(EarnTick());
    }

    private IEnumerator EarnTick()
    {
        yield return new WaitForSeconds(_earnTick);
        Wallet.EarnMoney(_earnAmount);

        if (_worker != null)
        {
            StartCoroutine(EarnTick());
        }
    }
}
