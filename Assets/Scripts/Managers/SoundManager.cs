using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _bgSource;

    void Start()
    {
        OnMusicActiveChanged(GameData.music);
        OnSoundActiveChanged(GameData.sound);
    }

    void OnEnable()
    {
        SettingsView.OnMusicActivityChanged += OnMusicActiveChanged;
        SettingsView.OnSoundActivityChanged += OnSoundActiveChanged;
    }

    void OnDisable()
    {
        SettingsView.OnMusicActivityChanged -= OnMusicActiveChanged;
        SettingsView.OnSoundActivityChanged -= OnSoundActiveChanged;
    }

    private void OnMusicActiveChanged(bool isOn)
    {
        _bgSource.mute = !isOn;
    }

    private void OnSoundActiveChanged(bool isOn)
    {

    }
}
