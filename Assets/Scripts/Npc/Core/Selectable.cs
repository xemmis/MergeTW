using UnityEngine;
using UnityEngine.Events;

public class Selectable : MonoBehaviour, ISelectable
{
    public Vector3 StartDragPosition { get; private set; } = Vector3.zero;
    public bool IsSelectable { get; set; } = true;

    public UnityEvent OnDrag { get; } = new UnityEvent();
    public UnityEvent<Vector3> OnDragFinish { get; } = new UnityEvent<Vector3>();

    public void OnDragStart()
    {
        StartDragPosition = transform.position;
        OnDrag?.Invoke();
    }

    public void OnDragUpdate(Vector3 worldPosition)
    {
        transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);

        // Можно добавить визуальные эффекты во время перетаскивания
        // Например, изменение прозрачности или размера
    }

    public void OnDragEnd(Vector3 worldPosition)
    {
        transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
        OnDragFinish?.Invoke(worldPosition);

        // Проверяем возможные взаимодействия (слияние, зона продажи и т.д.)
        // CheckInteractionsAtPosition(worldPosition);
    }

    public void SetSelectable(bool condition)
    {
        IsSelectable = condition;
    }
}

