using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSound;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private GameObject muteSound;
    [SerializeField] private int soundRead;
    [SerializeField] private float soundSliderRead;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private bool activeSoundPanel;
    // Start is called before the first frame update
    void Start()
    {
        activeSoundPanel = false;
        soundPanel.SetActive(activeSoundPanel);

        soundSlider.maxValue = 10;

        soundSliderRead = PlayerPrefs.GetFloat("SoundSliderRead");
        soundRead = PlayerPrefs.GetInt("SoundPlayerPref");
        ReadSoundPlayerPref();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            activeSoundPanel = !activeSoundPanel;
            soundPanel.SetActive(activeSoundPanel);
        }
    }
    private void ReadSoundPlayerPref()
    {
        // 0 play, 1 mute
        bool read = soundRead == 1;
        muteSound.SetActive(read);
        musicSound.mute = read;
        muteSound.SetActive(read);
        musicSound.mute = read;
        //slider
        soundSlider.value = soundSliderRead;
        musicSound.volume = soundSliderRead;
    }
    private void MusicVolumn(float volume)
    {
        musicSound.volume = volume;
        PlayerPrefs.SetFloat("SoundSliderRead", volume);
    }
    public void ControlVolume()
    {
        MusicVolumn(soundSlider.value);
    }
    public void MuteSound()
    {
        soundRead = soundRead < 1 ? 1 : 0;
        bool read = soundRead == 1;
        muteSound.SetActive(read);
        musicSound.mute = read;
        PlayerPrefs.SetInt("SoundPlayerPref", soundRead);
    }
}
