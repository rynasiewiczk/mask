namespace _Project.Scripts.Gameplay.Input
{
    using DG.Tweening;
    using UnityEngine;

    public class InputRowView : MonoBehaviour
    {
        [SerializeField] private Transform _selection;

        public void SetSelectionVisible(bool visible)
        {
            _selection.gameObject.SetActive(visible);
        }

        public void SetSelectionPos(Vector3 position, bool instant)
        {
            if (instant)
            {
                _selection.position = position;
                return;
            }
            _selection.DOMove(position, .4f);
        }
    }
}