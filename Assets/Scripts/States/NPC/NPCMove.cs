using UnityEngine;

namespace States
{
    public class NPCMove : NPCState
    {
        public NPCMove(Animator animator, string clipName) : 
            base(animator, string.IsNullOrEmpty(clipName) ? "Move" : clipName) {}
    }
}