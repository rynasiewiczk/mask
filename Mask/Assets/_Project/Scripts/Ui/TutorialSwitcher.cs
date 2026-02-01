namespace _Project.Scripts.Ui
{
    using System;
    using DG.Tweening;
    using UnityEngine;

    public class TutorialSwitcher : MonoBehaviour
    {
        [SerializeField] private RectTransform _rootPos;
        [SerializeField] private RectTransform _startPos;
        [SerializeField] private RectTransform _endPos;
        [SerializeField] private GameObject _hiddenObject;
        
        private bool _isShown = false;

        private void Awake()
        {
            _hiddenObject.SetActive(true);
            _rootPos.localPosition = _endPos.localPosition;
        }

        private void Start()
        {
            LevelManager.Instance.State.Subscribe(Refresh, true);
        }

        private void Refresh(LevelState state)
        {
            if (state == LevelState.Preparing)
            {
                if(!_isShown) { Toggle(); }
            }
            else if (state == LevelState.Playing)
            {
                if(_isShown) { Toggle(); }
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Toggle();
            }
        }

        private void Toggle()
        {
            _rootPos.DOLocalMove(_isShown ? _endPos.localPosition : _startPos.localPosition, .5f);
            _isShown = !_isShown;
            _hiddenObject.SetActive(!_isShown);
        }
    }
}