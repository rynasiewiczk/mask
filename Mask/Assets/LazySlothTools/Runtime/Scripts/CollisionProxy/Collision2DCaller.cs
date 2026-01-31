namespace LazySloth.Pong.Gameplay
{
    using UnityEngine;

    [RequireComponent(typeof(Collider2D))]
    public class Collision2DCaller : MonoBehaviour
    {
        [SerializeField] private CollisionTag _collisionTag = CollisionTag.Default;
        [SerializeField] private Collision2DProxy _proxy;

        private void OnTriggerEnter2D(Collider2D other)
        {
            _proxy.TriggerEnter2D(new Trigger2DInfo(other, _collisionTag));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _proxy.TriggerExit2D(new Trigger2DInfo(other, _collisionTag));
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            _proxy.TriggerStay2D(new Trigger2DInfo(other, _collisionTag));
        }
    }
}