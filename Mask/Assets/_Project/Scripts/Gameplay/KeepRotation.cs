namespace _Project.Scripts.Gameplay.Input
{
    using UnityEngine;

    public class KeepRotation : MonoBehaviour
    {
        void LateUpdate()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}