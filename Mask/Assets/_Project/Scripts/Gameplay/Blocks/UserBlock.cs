namespace _Project.Scripts.Gameplay.Input.Blocks
{
    using UnityEngine;

    public class UserBlock : Block
    {
        [SerializeField] private float _moveSpeed = 4f;
        
        private void FixedUpdate()
        {
            transform.Translate(Vector3.up * (_moveSpeed * Time.deltaTime));
            
            var hit = Physics2D.Raycast(transform.position, Vector3.up, 20f, LayerMask.GetMask("Default"));
            if (hit.collider && hit.transform.TryGetComponent(out Block otherBlock)
                             && hit.distance < Mathf.Abs(NextBlockPosition.localPosition.y))
            {
                 Debug.Log("Block hit");
                 transform.position = otherBlock.NextBlockPosition.position;
                 _moveSpeed = 0f;
            }
        }
    }
}