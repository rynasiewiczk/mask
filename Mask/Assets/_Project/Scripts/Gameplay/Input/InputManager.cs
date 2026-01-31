using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event Action OnLeft;
    public event Action OnRight;
    public event Action OnConfirm;
    public event Action OnChange;

    public event Action<int> OnNumber;
    
    public float threshold = 0.5f;
    private float _prevX;

    private void Update()
    {
        
        float x = Input.GetAxisRaw("Horizontal"); // Raw is snappier for d-pad

        // Left: crossing from >= -threshold to < -threshold
        if ((_prevX >= -threshold && x < -threshold) || Input.GetKeyDown(KeyCode.LeftArrow))
            OnLeft?.Invoke();

        // Right: crossing from <= threshold to > threshold
        if ((_prevX <= threshold && x > threshold) || Input.GetKeyDown(KeyCode.RightArrow))
            OnRight?.Invoke();

        _prevX = x;
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnNumber?.Invoke(1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnNumber?.Invoke(2);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnNumber?.Invoke(3);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnNumber?.Invoke(4);
        }
        
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.UpArrow) )
        {
            OnConfirm?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            OnChange?.Invoke();
        }
    }
}
