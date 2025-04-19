using System;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "NPCStates", menuName = "ScriptableObjects/NPCStates")]
    public class NPCStates : ScriptableObject
    {
        [SerializeField] private StateAndCoefficient[] _states;

        public NPCState GetRandomState()
        {
            float total = 0f;
            foreach (var s in _states)
                total += s.coefficient;

            float rand = UnityEngine.Random.Range(0f, total);
            float cumulative = 0f;

            foreach (var s in _states)
            {
                cumulative += s.coefficient;
                if (rand <= cumulative)
                    return s.state;
            }

            return _states[^1].state;
        }

        [Serializable]
        private class StateAndCoefficient
        {
            public NPCState state;
            public float coefficient;
        }
    }
}