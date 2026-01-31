namespace _Project.Scripts.Gameplay.Input
{
    using DG.Tweening;
    using UnityEngine;

    public class BlockDestroyParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _particles;

        public void Setup(Color color)
        {
            foreach (var particle in _particles)
            {
                var mainModule = particle.main;
                mainModule.startColor = color;
            }

            DOVirtual.DelayedCall(5, () => Destroy(gameObject));
        }
    }
}