using UnityEngine;

public class SelectState : INpcState
{
    private Animator _animator = null;
    public void Enter(NpcBehaviorLogic controller)
    {
        _animator = controller.GetAnimator();

        _animator.SetBool("Fly", true);
    }

    public void Update(NpcBehaviorLogic controller) { }

    public void Exit(NpcBehaviorLogic controller)
    {
        _animator.SetBool("Fly", false);

        _animator = null;
    }
}
