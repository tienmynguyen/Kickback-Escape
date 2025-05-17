
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    public TMP_Text slot1;
    public TMP_Text slot2;
    public TMP_Text slot3;

    // Start is called before the first frame update
    private void Start()
    {
        LoadVolume();
        MusicManager.Instance.PlayMusic("Main Menu");
        LoadAndDisplaySlots();
    }

    // Update is called once per frame
    public void NewGame()
    {
        SceneManager.LoadScene("CutSceneIntro");
        // MusicManager.Instance.PlayMusic("Game");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
    public void UpdateSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float SFXVolume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);

    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void LoadAndDisplaySlots()
    {
        for (int i = 1; i <= 3; i++)
        {
            GameData data = SaveManager.Instance.LoadGameData(i);
            string display = "";

            if (data != null)
            {
                display = $"Level: {data.level}";
            }
            else
            {
                display = "Empty";
            }

            switch (i)
            {
                case 1: slot1.text = display; break;
                case 2: slot2.text = display; break;
                case 3: slot3.text = display; break;
            }
        }
    }

}
