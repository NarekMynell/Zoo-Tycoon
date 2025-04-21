using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "BusinessData", menuName = "ScriptableObjects/BusinessData")]
    public class BusinessData : ScriptableObject
    {
        [SerializeField] private float[] _levelsIncomes;
        public float LevelsCount => _levelsIncomes.Length;

        public float GetLevelIncome(int level)
        {
            if(level > LevelsCount)
            {
                Debug.LogError("Index out of range exception");
                return 0;
            }
            return _levelsIncomes[level - 1];
        }
    }
}