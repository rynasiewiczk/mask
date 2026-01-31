using UnityEngine;

public class PreparingPanel : MonoBehaviour
{
    [SerializeField] private Animation _animation;
    [SerializeField] private string _hideAnimation;
    [SerializeField] private string _showAnimation;
    

    public void Hide()
    {
        _animation.Play(_hideAnimation);
    }

    public void Show()
    {
        _animation.Play(_showAnimation);
    }
}