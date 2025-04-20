using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static UnityAction<Vector2> onSwiped;
    public static UnityAction<Vector2, float> onZoomed;
    private bool _enabled;


    private void Update()
    {
        if (!_enabled) return;
        
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            HandleTouchInput();
        }
        else if (Mouse.current != null)
        {
            HandleMouseInput();
        }
    }


    void OnEnable()
    {
        _enabled = true;
    }

    void OnDisable()
    {
        _enabled = false;
    }

    private void HandleTouchInput()
    {
        int activeTouches = 0;
        foreach (var touch in Touchscreen.current.touches)
        {
            if (touch.press.isPressed)
                activeTouches++;
        }

        if (activeTouches == 1)
        {
            var touch = Touchscreen.current.touches[0];

            if (touch.press.isPressed)
            {
                Vector2 deltta = touch.delta.ReadValue();
                if(deltta != Vector2.zero) onSwiped?.Invoke(deltta);
            }
        }
        else if (activeTouches >= 2)
        {
            var t0 = Touchscreen.current.touches[0];
            var t1 = Touchscreen.current.touches[1];
            Vector2 d0 = t0.delta.ReadValue();
            Vector2 d1 = t1.delta.ReadValue();

            if(d0 != Vector2.zero && d1 != Vector2.zero)
            {
                Vector2 p0 = t0.position.ReadValue();
                Vector2 p1 = t1.position.ReadValue();

                float angle = d0 == Vector2.zero || d1 == Vector2.zero ? 180f : Vector2.Angle(d0, d1);

                // Zoom
                if(angle > 30f)
                {
                    Vector2 midPoint = (p0 + p1) / 2f;
                    Vector2 lpO = p0 - d0;
                    Vector2 lp1 = p1 - d1;
                    float lastMagnitude = Vector2.Distance(lpO, lp1);
                    float curMagnitude = Vector2.Distance(p0, p1);
                    float delta = curMagnitude - lastMagnitude;
                    onZoomed?.Invoke(midPoint, delta);
                }
                // Swipe
                else
                {
                    Vector2 delta = d0 + d1;
                    onSwiped?.Invoke(delta);
                }
            }
        }
    }

    private void HandleMouseInput()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();
            onSwiped?.Invoke(delta);
        }

        float zoomDelta = Mouse.current.scroll.ReadValue().y;
        if (zoomDelta != 0)
        {
            Vector2 centrePos = Mouse.current.position.ReadValue();
            onZoomed?.Invoke(centrePos, zoomDelta);
        }
    }
}