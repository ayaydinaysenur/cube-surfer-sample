using System;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private float firstTouchPos, lastTouchPos, distance = 0;
    private float maxSwipeAmount = 4;//yolun genisligine bagli, yarisi kadar
    private float totalSwipeAmount;
    private float horizontalMovementAmount = 0.05f;
    public static event Action<float> OnSwipe;

    void Update()
    {
#if UNITY_EDITOR
        CheckSwipeOnPc();
#else 
        CheckSwipeOnMobile();
#endif
    }

    private void CheckSwipeOnMobile()
    {
        //tek bir touch icin olmali
        foreach (Touch touch in Input.touches)
        {
            if (firstTouchPos == 0)
            {
                firstTouchPos = touch.position.x;
                lastTouchPos = touch.position.x;
            }
            else
            {
                lastTouchPos = touch.position.x;
                distance = lastTouchPos - firstTouchPos;
                firstTouchPos = lastTouchPos;
            }
        }
        if (distance > 0)
        {
            print("left");
            CheckForHorizontalMovement(-horizontalMovementAmount);
        }
        else if (distance < 0)
        {
            print("right");
            CheckForHorizontalMovement(horizontalMovementAmount);
        }
    }

    private void CheckSwipeOnPc()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            print("left");
            CheckForHorizontalMovement(-horizontalMovementAmount);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            print("right");
            CheckForHorizontalMovement(horizontalMovementAmount);

        }
    }

    private void CheckForHorizontalMovement(float movementAmount)
    {
        if ((totalSwipeAmount + movementAmount) < maxSwipeAmount &&
            (totalSwipeAmount + movementAmount) > -maxSwipeAmount)
        {
            totalSwipeAmount += movementAmount;
            OnSwipe?.Invoke(movementAmount);
        }
    }
}
