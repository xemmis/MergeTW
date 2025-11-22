using System.Collections.Generic;

public class SkeletonFabric : BaseNpcFabric<SkeletonBehaviorLogic>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void Init(INpsConfigurator<SkeletonBehaviorLogic> npsConfigurator)
    {
        _configurator = npsConfigurator;
    }

    public override int GetNpcLevel(SkeletonBehaviorLogic npc)
    {
        return npc.GetNpcData().Level;
    }
}
