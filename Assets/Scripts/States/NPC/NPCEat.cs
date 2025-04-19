using UnityEngine;

namespace States
{
    public class NPCEat : NPCState
    {
        public NPCEat(Animator animator, string clipName) : 
            base(animator, string.IsNullOrEmpty(clipName) ? "Eat" : clipName) {}
    }
}