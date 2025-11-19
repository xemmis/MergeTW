using System.Collections.Generic;
public class EmptyCellChecker : IEmptyCellChecker
{
    public Spawner Check(List<Spawner> spawners)
    {
        foreach (Spawner spawner in spawners)
        {
            if (!spawner.IsOccupied())
            {
                spawner.SetStatus(true);
                return spawner;
            }
        }

        return null;
    }
}
