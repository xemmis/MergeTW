using UnityEngine;

public class InputDetector : MonoBehaviour, IInputDetector
{
    public event System.Action<Vector2> OnTouchBegan;
    public event System.Action<Vector2> OnTouchMoved;
    public event System.Action<Vector2> OnTouchEnded;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchBegan?.Invoke(touchPosition);
                    break;
                case TouchPhase.Moved:
                    OnTouchMoved?.Invoke(touchPosition);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    OnTouchEnded?.Invoke(touchPosition);
                    break;
            }
        }
    }
}
