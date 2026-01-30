using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event Action OnLeft;
    public event Action OnRight;
    public event Action OnConfirm;
    public event Action OnChange;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnLeft?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnRight?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnConfirm?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnChange?.Invoke();
        }
    }
}
