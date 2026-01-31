namespace _Project.Scripts.Ui
{
    using DG.Tweening;
    using UnityEngine;

    public class TutorialSwitcher : MonoBehaviour
    {
        [SerializeField] private RectTransform _rootPos;
        [SerializeField] private RectTransform _startPos;
        [SerializeField] private RectTransform _endPos;
        
        private bool _isShown = true;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _rootPos.DOLocalMove(_isShown ? _endPos.localPosition : _startPos.localPosition, .5f);
                _isShown = !_isShown;
            }
        }
    }
}