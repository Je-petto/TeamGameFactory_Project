using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundMG : MonoBehaviour
{
    public static SoundMG Instance = null;


    // 씬 인덱스에 맞는 BGM 클립 (인덱스 0은 씬 0번의 BGM)
    public List<AudioClip> scenesClip = new List<AudioClip>();

    public AudioSource BGMaudio, SFXaudio;
    [SerializeField] public AudioClip ButtonClip;
    [SerializeField] public string buttonClipPath = "ButtonSound";

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            var auDio = GetComponents<AudioSource>();
            if (auDio.Length >= 2)
            {
                BGMaudio = auDio[0];
                SFXaudio = auDio[1];

            }
            // 여기서 버튼 효과음 클립을 Resources에서 동적으로 로드
            if (ButtonClip == null)
            {
                ButtonClip = Resources.Load<AudioClip>(buttonClipPath);
                if (ButtonClip == null)
                {
                    Debug.LogWarning("ButtonClip 로드 실패! 경로 확인 필요: " + buttonClipPath);
                }
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        else
        {
            Destroy(gameObject);
        }
    }


    public void OnSceneLoaded(Scene scene, LoadSceneMode loadScene)
    {
        PlaySceneBgm();
        if (ButtonClip == null)
        {
            Button button = FindObjectOfType<Button>(); // 버튼을 찾습니다 (더 구체적인 버튼을 찾을 수도 있음)
            if (button != null && SoundMG.Instance != null)
            {
                button.onClick.AddListener(SoundMG.Instance.OnButtonSound);
            }
        }
    }

    public void PlaySceneBgm()
    {
        int Scene_i = SceneManager.GetActiveScene().buildIndex;

        if (Scene_i < scenesClip.Count && scenesClip[Scene_i] != null)
        {
            BGMaudio.Stop();
            BGMaudio.clip = scenesClip[Scene_i];
            BGMaudio.loop = true;
            BGMaudio.Play();
        }
    }

    public void OnButtonSound()
    {
        //SFXaudio.clip = ButtonClip;
        if (ButtonClip != null)
            SFXaudio.PlayOneShot(ButtonClip, 3f);
        else
        {
            Debug.LogWarning("ButtonClip이 null입니다!");
        }
    }
}