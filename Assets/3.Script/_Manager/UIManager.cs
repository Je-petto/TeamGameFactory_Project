using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private List<Sprite> abilitiesSprites = new List<Sprite>();
    [SerializeField] private Image abilityUI;
    [SerializeField] private Text scoreUI;
    [SerializeField] private GameObject gameOverUI;

    [Header("InGame")]
    [SerializeField] private Image bloodScreen;
    [SerializeField] private AnimationCurve curve_animation;
    [SerializeField] private PlayerBehaviour player;



    [ReadOnly] public int maxHealth;
    [ReadOnly] public int health;
    void Start()
    {
        maxHealth = player.data[GameManager.Instance.selectPlayer].maxHealth;
        health = maxHealth;
        abilityUI.sprite = abilitiesSprites[GameManager.Instance.selectPlayer];
    }

    void Update()
    {
        Option();
        SetHealth();
        SetScore();
        Update_AbilityUI();
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
    }
    private IEnumerator OnBloodScreen_co()
    {
        float persent = 0f;
        while (persent < 1)
        {
            persent += Time.deltaTime;
            Color c = bloodScreen.color;
            c.a = Mathf.Lerp(1, 0, curve_animation.Evaluate(persent));
            bloodScreen.color = c;
            yield return null;
        }
    }

    public void Option()
    {
        if (Input.GetKeyDown(KEYSTOP) && !gameOverUI.activeSelf)
        {
            GameManager.Instance.isPause = !GameManager.Instance.isPause;
            Time.timeScale = GameManager.Instance.isPause ? 0f : 1f;
            /*
            if (GameManager.isPause)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
            */
            optionUI.SetActive(GameManager.Instance.isPause);

            if (GameManager.Instance.isPause)
                // UI가 켜질 때 버튼 효과음 리스너 등록
                SoundMG.Instance.ButtonSoundsCall(optionUI);
        }
    }
    private void SetScore()
    {
        scoreUI.text = $"Score : {(int)GameManager.Instance.totalScore}";
    }

    public void Update_AbilityUI()
    {
        abilityUI.enabled = player.canUse;
    }
}