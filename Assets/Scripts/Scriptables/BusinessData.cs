using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "BusinessData", menuName = "ScriptableObjects/BusinessData")]
    public class BusinessData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private LevelData[] _levels;
        [SerializeField] private int _defaultLevel = 0;
        [SerializeField] private bool _autoSendMoney = true;

        public string Name => _name;
        public int LevelsCount => _levels.Length;
        public int DefaultLevel => _defaultLevel;
        public bool AutoSendMoney => _autoSendMoney;

        public LevelData GetLevelData(int level)
        {
            if(level > LevelsCount)
            {
                Debug.LogError("Index out of range exception");
                return null;
            }
            return _levels[level - 1];
        }
    }
}