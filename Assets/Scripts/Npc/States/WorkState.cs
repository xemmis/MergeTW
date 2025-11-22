using UnityEngine;

public class WorkState : INpcState
{
    public WorkState(WorkType workType)
    {
        _type = workType;
    }
    private WorkType _type;
    private Animator _animator = null;
    public void Enter(NpcBehaviorLogic controller)
    {
        _animator = controller.GetAnimator();
        ChangeState(true);
    }

    private void ChangeState(bool condition)
    {
        if (_type == WorkType.Rock)
        {
            _animator.SetBool("RockWork", condition);
        }
        else
        {
            _animator.SetBool("OtherWork", condition);
        }
    }

    public void Exit(NpcBehaviorLogic controller)
    {
        ChangeState(false);
        _animator = null;
    }

    public void Update(NpcBehaviorLogic controller) { }
}
