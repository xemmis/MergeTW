using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class BaseNpc : MonoBehaviour, ISelectable
{
    [SerializeField] private NpsData _skeletonData = null;
    [SerializeField] private Spawner _currentSpawner = null;
    [SerializeField] private bool _isSelectable = true;
    [SerializeField] private Collider _boxCollider = null;
    private Vector3 _startDragPosition = new();
    public bool IsSelectable => _isSelectable && gameObject.activeInHierarchy;

    public void OnDragStart()
    {
        _startDragPosition = transform.position;
        print("start");
    }

    public void OnDragUpdate(Vector3 worldPosition)
    {
        transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);

        // Можно добавить визуальные эффекты во время перетаскивания
        // Например, изменение прозрачности или размера
    }

    public void OnDragEnd(Vector3 worldPosition)
    {
        transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);

        // Проверяем возможные взаимодействия (слияние, зона продажи и т.д.)
        CheckInteractionsAtPosition(worldPosition);
    }

    private void CheckInteractionsAtPosition(Vector3 position)
    {
        Collider[] nearbyColliders = new Collider[10];
        int colliderCount = Physics.OverlapSphereNonAlloc(position, 1f, nearbyColliders);

        for (int i = 0; i < colliderCount; i++)
        {
            Collider collider = nearbyColliders[i];

            if (collider.gameObject.TryGetComponent<BaseNpc>(out var skeleton))
            {
                if (skeleton != this)
                {
                    SpawnEventManager.SpawnEventInstance.OnSkeletonMerge.Invoke(this, skeleton);
                    transform.position = _startDragPosition;
                    return;
                }
            }

            if (collider.TryGetComponent<Spawner>(out var spawner) && !spawner.IsOccupied())
            {
                if (_currentSpawner != null)
                {
                    _currentSpawner.SetStatus(false);
                }

                transform.position = spawner.transform.position;
                _currentSpawner = spawner;
                _currentSpawner.SetStatus(true);
                return;
            }
        }

        transform.position = _startDragPosition;
    }

    public NpsData GetData()
    {
        if (_skeletonData == null)
        {
            throw new System.ArgumentNullException(nameof(NpsData), "SkeletonData is null on GetData");
        }

        return _skeletonData;
    }

    public void SetData(NpsData skeletonData)
    {
        if (_currentSpawner == null)
        {
            throw new System.ArgumentNullException(nameof(NpsData), "recieved SkeletonData is null on SetData");
        }

        _skeletonData = skeletonData;
    }

    public Spawner GetSpawner()
    {
        if (_currentSpawner == null)
        {
            throw new System.ArgumentNullException(nameof(Spawner), "recieved Spawner is null on GetSpawner");
        }

        return _currentSpawner;
    }

    public void SetSpawner(Spawner spawner)
    {
        _currentSpawner = spawner;
    }
}
