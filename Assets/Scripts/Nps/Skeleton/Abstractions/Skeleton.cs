using UnityEngine;

public abstract class Skeleton : MonoBehaviour, ISkeletonSelectHandler
{
    [SerializeField] private SkeletonData _skeletonData = null;
    [SerializeField] private Spawner _currentSpawner = null;
    [SerializeField] private bool _isSelectable = true;
    private Vector3 _startDragPosition = new();
    public bool IsSelectable => _isSelectable && gameObject.activeInHierarchy;

    public void OnDragStart()
    {
        _startDragPosition = transform.position;
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

            if (collider.gameObject.TryGetComponent<Skeleton>(out var skeleton))
            {
                if (skeleton != this)
                {
                    SpawnEventManager.OnSkeletonMerge.Invoke(this, skeleton, skeleton.GetSpawner());
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

    public SkeletonData GetData()
    {
        if (_skeletonData == null)
        {
            throw new System.ArgumentNullException(nameof(SkeletonData), "SkeletonData is null on GetData");
        }

        return _skeletonData;
    }

    public void SetData(SkeletonData skeletonData)
    {
        if (_currentSpawner == null)
        {
            throw new System.ArgumentNullException(nameof(SkeletonData), "recieved SkeletonData is null on SetData");
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
        if (spawner == null)
        {
            throw new System.ArgumentNullException(nameof(Spawner), "Spawner is null on SetSpawner");
        }

        _currentSpawner = spawner;
    }
}
