namespace _Project.Scripts.Gameplay.Input
{
    using UnityEngine;

    public class BlockDestroyParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private ParticleSystem _sparksParticles;

        public void Setup(Color color)
        {
            var mainModule = _particleSystem.main;
            mainModule.startColor = color;
            
            var sparkMain = _sparksParticles.main;
            sparkMain.startColor = color;
        }
    }
}