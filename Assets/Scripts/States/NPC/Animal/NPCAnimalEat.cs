using UnityEngine;
using UnityEngine.Events;

namespace States
{
    public class NPCAnimalEat : NPCEat
    {
        private UnityAction _finishCallback;
        private float _remainingTime;

        
        public NPCAnimalEat(Animator animator, UnityAction finishCallback, string clipName = default) : base(animator, clipName)
        {
            _finishCallback = finishCallback;
        }

        public override void Enter()
        {
            base.Enter();
            _remainingTime = Random.Range(3f, 6f);
        }

        public override void Update()
        {
            _remainingTime -= Time.deltaTime;
            if (_remainingTime <= 0)
            {
                _finishCallback?.Invoke();
            }
        }
    }
}