using UnityEngine;

namespace States
{
    public class NPCState : State
    {
        protected Animator _animator;
        protected string _clipName;

        public NPCState(Animator animator, string clipName)
        {
            _animator = animator;
            _clipName = string.IsNullOrEmpty(clipName) ? "Idle" : clipName;
        }

        public override void Enter()
        {
            _animator.SetBool(_clipName, true);
        }

        public override void Exit()
        {
            _animator.SetBool(_clipName, false);
        }
    }
}