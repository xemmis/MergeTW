public class EnemyBehaviorLogic : NpcBehaviorLogic
{
    protected override void Awake()
    {
        base.Awake();
        
        ChangeState(new IdleState());
    }

    
}