namespace LazySloth.Utilities.Animator
{
    using System;
    using UnityEngine;

    public class StateMachineTrigger : StateMachineBehaviour
    {
        public event Action OnStateEnterEvt;
        public event Action OnStateExitEvt;

        [SerializeField] private string _tag = default;

        public string Tag => _tag;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            OnStateEnterEvt?.Invoke();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            OnStateExitEvt?.Invoke();
        }
    }
}