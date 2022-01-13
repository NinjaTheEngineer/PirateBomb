using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    public Sound[] sounds;

    private static SoundManager instance;
    public static SoundManager Instance //Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
        Invoke("PlayBackgroundMusic", 0.5f);
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1f);
        }

        Load();
    }

    public void ChangeVolume() //Should change game music volume
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load() //Load game music volume
    {
        if(volumeSlider == null)
        {
            volumeSlider = GameObject.FindGameObjectWithTag("VolumeSlider").GetComponent<Slider>();
        }
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        AudioListener.volume = volumeSlider.value;
    }

    private void Save() //Save game music volume
    {
        if (volumeSlider == null)
        {
            volumeSlider = GameObject.FindGameObjectWithTag("VolumeSlider").GetComponent<Slider>();
        }
        PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);

        AudioListener.volume = volumeSlider.value;
    }
    public void Play(string name) //Play sound by name
    {
        //Play sound by its name if it exists
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        Debug.LogWarning("Sound: " + name + " Played!");
        s.source.Play();
    }

    public void PlayButtonClick()
    {
        Play("Button_Click");
    }
    public void PlayBombExplode()
    {
        Play("Bomb_Explode");
    }
    public void PlayDoorOpen()
    {
        Play("Door_Open");
    }
    public void PlayPirateDead()
    {
        Play("Pirate_Dead");
    }
    public void PlayPirateHit()
    {
        Play("Pirate_Hit");
    }
    public void PlayPirateHm()
    {
        Play("Pirate_Hm");
    }
    public void PlayPirateKick()
    {
        Play("Pirate_Kick");
    }
    public void PlayPlayerHit()
    {
        Play("Player_Hit");
    }
    public void PlayPlayerDead()
    {
        Play("Player_Dead");
    }
    public void PlayPlayerJump()
    {
        Play("Player_Jump");
    }
    public void PlayBackgroundMusic()
    {
        //Find sound with 'Background_Music' name and play it, if it is found.
        Sound s = Array.Find(sounds, sound => sound.name == "Background_Music");
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        else
        {
            s.source.loop = true;
            s.source.Play();
            Debug.Log("Sound: " + name + " Playing");
        }
    }

    public void PauseBackgroundMusic()
    {
        //Find sound with 'BackgroundMusic' name and pause it, if it is found.
        Sound s = Array.Find(sounds, sound => sound.name == "Background_Music");
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        else
        {
            s.source.Pause();
        }
    }

    public void PlayCreditsMusic()
    {
        //Find sound with 'Credits_Music' name and play it, if it is found.
        Sound s = Array.Find(sounds, sound => sound.name == "Credits_Music");
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        else
        {
            s.source.loop = true;
            s.source.Play();
        }
        Invoke("PauseBackgroundMusic", 0.51f);
    }
}
