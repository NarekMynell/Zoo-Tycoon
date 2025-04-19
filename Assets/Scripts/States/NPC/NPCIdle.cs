using UnityEngine;

namespace States
{
    public class NPCIdle : NPCState
    {
        public NPCIdle(Animator animator, string clipName) : 
            base(animator, string.IsNullOrEmpty(clipName) ? "Idle" : clipName) {}
    }
}