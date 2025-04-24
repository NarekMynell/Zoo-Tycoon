using UnityEngine;
using UnityEngine.UI;

public class SettingsView : MonoBehaviour
{
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Button _testMoneyBtn;


    private void Awake()
    {
        _soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
        _musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        _testMoneyBtn.onClick.AddListener(FillMoney);
    }

    private void OnEnable()
    {
        _soundToggle.isOn = GameData.sound;
        _musicToggle.isOn = GameData.music;

        _testMoneyBtn.gameObject.SetActive(GameData.totalMoney < 999_999_999_000);
    }

    private void OnSoundToggleChanged(bool value)
    {
        GameData.sound = value;
    }

    private void OnMusicToggleChanged(bool value)
    {
        GameData.music = value;
    }

    private void FillMoney()
    {
        GameData.totalMoney = 999_999_999_000;
        _testMoneyBtn.gameObject.SetActive(false);
    }
}