using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CustomInspector;

public class UIManager : MonoBehaviour
{
    [Header("KeyCode")]
    private KeyCode KEYSTOP = KeyCode.Escape; 

    [Header("UI")]
    [SerializeField] private GameObject optionUI;
    [SerializeField] private Image healthUI;
    [SerializeField] private Image maxHealthUI;

    [SerializeField] private Image bloodScreen;
    [SerializeField] private AnimationCurve curve_animation;
    [SerializeField] private PlayerBehaviour player;
    [SerializeField] private CameraShake cameraShake;

    [ReadOnly] public int maxHealth;
    [ReadOnly] public int health;
    void Start()
    {
        maxHealth = player.data[GameManager.selectPlayer].maxHealth;
    }

    void Update()
    {
        Option();
        SetHealth();
    }
    void SetHealth()
    {
        health = player.health;
        
        float xScale = (maxHealth > 0) ? (float)health / maxHealth : 0f;
        healthUI.transform.localScale = new Vector3(xScale, 1f, 1f);
    }
    public void DamageUI()
    {
        StopCoroutine("OnBloodScreen_co");
        StartCoroutine(OnBloodScreen_co());
        if (cameraShake != null)
        {
            // Shake 메소드 호출!
            // 흔들림 시간(duration)과 강도(magnitude)를 인자로 넘겨줘.
            // 이 값들은 원하는 효과에 따라 조절해봐.
            cameraShake.Shake(0.5f, 100f); // 예: 0.3초 동안 강도 0.2f로 흔들기
        }
    }
    private IEnumerator OnBloodScreen_co()
    {
        float persent = 0f;
        while(persent < 1)
        {
            persent += Time.deltaTime;
            Color c = bloodScreen.color;
            c.a = Mathf.Lerp(1, 0, curve_animation.Evaluate(persent));
            bloodScreen.color = c;
            yield return null;
        }
    }
    bool optionUISwitch = false;
    public void Option()
    {
        if (Input.GetKeyDown(KEYSTOP)) 
        {
            optionUISwitch = !optionUISwitch;
            Time.timeScale = optionUISwitch ? 0f : 1f;
            optionUI.SetActive(optionUISwitch);
        }
    }
}