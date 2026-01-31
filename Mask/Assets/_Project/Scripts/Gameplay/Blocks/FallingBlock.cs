namespace _Project.Scripts.Gameplay.Input.Blocks
{
    using UnityEngine;

    public class FallingBlock : Block
    {
        public void Fall(float distance)
        {
            transform.position += Vector3.down * distance;
        }


        public void HandleMissmatchedUserBlock()
        {
            _view.ShowMissplay();
        }
    }
}