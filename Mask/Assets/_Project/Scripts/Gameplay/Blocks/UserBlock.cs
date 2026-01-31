namespace _Project.Scripts.Gameplay.Input.Blocks
{
    using Spawning;
    using UnityEngine;

    public class UserBlock : Block
    {
        [SerializeField] private float _moveSpeed = 4f;

        private void FixedUpdate()
        {
            if (!LevelManager.Instance.IsPlaying)
            {
                return;
            }

            transform.Translate(Vector3.up * (_moveSpeed * Time.deltaTime));

            var hit = Physics2D.Raycast(transform.position, Vector3.up, 20f, LayerMask.GetMask("Default"));
            if (hit.collider && hit.transform.TryGetComponent(out FallingBlock otherBlock)
                             && hit.distance < Mathf.Abs(NextBlockPosition.localPosition.y))
            {
                Debug.Log("Block hit");

                if (otherBlock.BlockType == BlockType)
                {
                    ClearHittedBlock(otherBlock);
                }
                else
                {
                    var newFallingBlock = BlockFactory.Instance.CreateFallingBlock();
                    newFallingBlock.SetType(BlockType);
                    newFallingBlock.transform.position = otherBlock.NextBlockPosition.position;
                    FallingBlocksModel.Instance.AddBlock(newFallingBlock);
                }
                
                Destroy(gameObject);
            }
        }

        private void ClearHittedBlock(FallingBlock otherBlock)
        {
            FallingBlocksModel.Instance.DestroyBlock(otherBlock);
        }
    }
}