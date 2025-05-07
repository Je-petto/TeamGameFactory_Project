using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // 싱글톤 인스턴스.
    public static SoundManager Instance = null;


    // 씬 인덱스에 맞는 BGM 클립을 저장하는 리스트.
    // 인덱스 0은 씬 0번의 BGM, 인덱스 1은 씬 1번의 BGM.
    public List<AudioClip> scenesClip = new List<AudioClip>();

    // BGM(배경음악)을 재생하는 AudioSource
    public AudioSource BGMaudio;
    // SFX(효과음)을 재생하는 AudioSource
    public AudioSource SFXaudio;
    // 버튼 클릭 효과음 클립
    [SerializeField] public AudioClip ButtonClip;
    // 버튼 효과음 클립의 Resources 폴더 내 경로 (Inspector에서 설정 가능)
    [SerializeField] public string buttonClipPath = "ButtonSound"; // 기본 경로 설정 (Resources 폴더 내)

    // Awake() 함수는 스크립트 인스턴스가 생성될 때 호출됩니다.
    public void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            // 첫 번째 인스턴스인 경우
            Instance = this;
            // 씬이 바뀌어도 파괴되지 않도록 설정합니다.
            DontDestroyOnLoad(gameObject);

            // AudioSource 컴포넌트들을 찾아 할당
            var auDio = GetComponents<AudioSource>();
            if (auDio.Length >= 2)
            {
                BGMaudio = auDio[0];  // 첫 번째 AudioSource를 BGM으로
                SFXaudio = auDio[1];  // 두 번째 AudioSource를 SFX로
            }

            // 버튼 효과음 클립을 Resources에서 동적으로 로드합니다.
            if (ButtonClip == null) // 이미 할당된 클립이 없는 경우
            {
                ButtonClip = Resources.Load<AudioClip>(buttonClipPath); // Resources에서 로드
                if (ButtonClip == null) // 로드 실패 시
                {
                    Debug.LogWarning("ButtonClip 로드 실패! 경로 확인 필요: " + buttonClipPath); // 경고 메시지 출력
                }
            }
            // 씬 로드 완료 시 OnSceneLoaded 함수를 호출하도록 이벤트를 등록합니다.
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        else
        {
            // 이미 인스턴스가 존재하는 경우, 중복 생성을 막고 이 객체를 파괴합니다.
            Destroy(gameObject);
        }
    }


    // 씬이 로드될 때마다 호출되는 함수입니다.
    public void OnSceneLoaded(Scene scene, LoadSceneMode loadScene)
    {
        PlaySceneBgm(); // 씬에 맞는 BGM을 재생합니다.

        // 버튼 클릭 시 효과음을 재생하도록 버튼에 리스너를 추가합니다. (ButtonClip이 null인 경우에만)
        if (ButtonClip == null)
        {
            // 현재 씬에서 Button 컴포넌트를 찾습니다.
            Button button = FindObjectOfType<Button>(); //씬에 있는 button 오브젝트 찾기
            if (button != null && SoundManager.Instance != null)
            {
                // 버튼 클릭 시 OnButtonSound 함수를 호출하도록 리스너를 추가합니다.
                button.onClick.AddListener(SoundManager.Instance.OnButtonSound); //button 클릭하면 OnButtonSound 함수 실행
            }
        }
    }

    // 현재 씬에 맞는 BGM을 재생하는 함수입니다.
    public void PlaySceneBgm()
    {
        // 현재 활성화된 씬의 인덱스를 가져옵니다.
        int Scene_i = SceneManager.GetActiveScene().buildIndex;

        // 씬 인덱스가 scenesClip 리스트의 범위 내에 있고, 해당 씬의 BGM 클립이 null이 아닌 경우
        if (Scene_i < scenesClip.Count && scenesClip[Scene_i] != null)
        {
            BGMaudio.Stop();                         // 기존에 재생 중인 BGM을 정지합니다.
            BGMaudio.clip = scenesClip[Scene_i];      // 해당 씬의 BGM 클립으로 설정합니다.
            BGMaudio.loop = true;                     // BGM을 반복 재생하도록 설정합니다.
            BGMaudio.Play();                         // BGM을 재생합니다.
        }
    }

    // 버튼 클릭 효과음을 재생하는 함수입니다.
    public void OnButtonSound()
    {
        if (ButtonClip != null)
            SFXaudio.PlayOneShot(ButtonClip, 3f); // SFXaudio에서 ButtonClip을 3f 볼륨으로 재생
        else
        {
            Debug.LogWarning("ButtonClip이 null입니다!"); // ButtonClip이 null인 경우 경고 메시지를 출력합니다.
        }
    }

    //원래 모든 씬 버튼에 효과음을 내고 싶었는데 다른 씬에서 버튼을 눌러도 소리가 안남.
}