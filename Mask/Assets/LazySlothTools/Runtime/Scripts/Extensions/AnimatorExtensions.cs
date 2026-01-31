namespace LazySloth.Utilities.Animator
{
    using System.Linq;
    using UnityEngine;

    public static class AnimatorExtensions
    {
        public static StateMachineTrigger GetStateTrigger(this Animator animator, string tag)
        {
            return animator.GetBehaviours<StateMachineTrigger>().SingleOrDefault(x => x.Tag == tag);
        }
    }
}