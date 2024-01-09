using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

public class SwipeHandler : IInitializable, IDisposable
{
    private enum State
    {
        none = 0,
        swiping = 1
    }

    public enum Direction
    {
        none = 0,
        left = 1,
        right = 2,
        up = 3,
        down = 4
    }

    public static event Action<Direction> OnSwipe;

    private PlayerControls playerControls;
    [Inject] private SwipeSettings swipeSettings;

    private Vector2 CurrentTouchPosition => playerControls.Touch.TouchPosition.ReadValue<Vector2>();

    private Vector2? touchStartPosition = null;
    private Direction lastSwipeDirection = Direction.none;
    private State state = State.none;

    public void Initialize()
    {
        playerControls = new PlayerControls();
        playerControls.Enable();

        playerControls.Touch.TouchContact.performed += TouchContact_performed;
        playerControls.Touch.TouchContact.canceled += TouchContact_canceled;

        playerControls.Touch.TouchPosition.performed += TouchPosition_performed;
    }

    public void Dispose()
    {
        playerControls.Disable();

        playerControls.Touch.TouchContact.performed -= TouchContact_performed;
        playerControls.Touch.TouchContact.canceled -= TouchContact_canceled;

        playerControls.Touch.TouchPosition.performed -= TouchPosition_performed;
    }

    private void TouchPosition_performed(InputAction.CallbackContext context)
    {
        if (state == State.none) return;

        if (touchStartPosition == null)
        {
            touchStartPosition = CurrentTouchPosition;
        }
        CheckSwipe();
    }

    private void TouchContact_performed(InputAction.CallbackContext context)
    {
        state = State.swiping;
    }

    private void TouchContact_canceled(InputAction.CallbackContext context)
    {
        state = State.none;
        touchStartPosition = null;
        lastSwipeDirection = Direction.none;
    }

    private void CheckSwipe()
    {
        if (touchStartPosition == null || state == State.none) return;

        Vector2 delta = CurrentTouchPosition - (Vector2)touchStartPosition;

        if (delta.magnitude >= swipeSettings.lengthOfSwipePX)
        {
            Direction direction = delta.GetDirection();
            if (lastSwipeDirection != direction)
            {
                lastSwipeDirection = direction;
                touchStartPosition = CurrentTouchPosition;
                OnSwipe?.Invoke(direction);
            }
        }
    }
}
