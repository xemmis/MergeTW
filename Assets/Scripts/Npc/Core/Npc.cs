using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] private Spawner _spawner = null;
    private int _earnAmount;
    private int _earnTick;

    public void Configure(NpcData data, Spawner spawner)
    {
        SetSpawner(spawner);
        _earnAmount = data.EarnPerTick;
        _earnTick = data.EarnTick;
        transform.position = _spawner.transform.position;
    }

    public void SetSpawner(Spawner spawner)
    {
        if (spawner != null)
        {
            _spawner = spawner;
            _spawner.SetStatus(true);
        }
    }

    public Spawner GetSpawner()
    {
        return _spawner;
    }

    public int GetEarnAmount()
    {
        return _earnAmount;
    }

    public int GetEarnTick()
    {
        return _earnTick;
    }
}
