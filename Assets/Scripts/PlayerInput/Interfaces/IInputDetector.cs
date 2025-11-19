using UnityEngine;

public interface IInputDetector
{
    event System.Action<Vector2> OnTouchBegan;
    event System.Action<Vector2> OnTouchMoved;
    event System.Action<Vector2> OnTouchEnded;
}
