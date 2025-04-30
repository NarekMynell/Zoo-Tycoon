using UnityEngine;
using UnityEngine.Events;

namespace States
{
    public class NPCCharacterIdle : NPCIdle
    {


        public NPCCharacterIdle(Animator animator, string clipName = default) : base(animator, clipName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
    }
}