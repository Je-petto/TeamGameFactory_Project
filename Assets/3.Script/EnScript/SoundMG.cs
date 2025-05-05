using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundMG : MonoBehaviour
{
    public static SoundMG Instance = null;

    // 씬 인덱스에 맞는 BGM 클립 (인덱스 0은 씬 0번의 BGM)
    public List<AudioClip> scenesClip = new List<AudioClip>();

    public AudioSource BGMaudio, SFXaudio;
    [SerializeField] private AudioClip ButtonClip;
    
    public void Awake()
    { 
        if( Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadScene)
    {
        PlayScencBgm();
    }

    public void PlayScencBgm()
    {
        int Scene_i = SceneManager.GetActiveScene().buildIndex;

        if( Scene_i < scenesClip.Count && scenesClip[Scene_i] != null)
        {
            BGMaudio.Stop();
            BGMaudio.clip = scenesClip[Scene_i];
            BGMaudio.loop =true;
            BGMaudio.Play();
        }
    }
}
