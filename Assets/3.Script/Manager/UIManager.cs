using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CustomInspector;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image healthUI;
    [SerializeField] private Image maxHealthUI;

    [SerializeField] private Image bloodScreen;
    [SerializeField] private AnimationCurve curve_animation;
    [SerializeField] private PlayerBehaviour player;

    [ReadOnly] public int maxHealth;
    [ReadOnly] public int health;
    void Start()
    {
        maxHealth = player.data.maxHealth;
    }

    void Update()
    {
        SetHealth();
    }
    void SetHealth()
    {
        health = player.health;
        
        float xScale = (float)health / maxHealth;
        healthUI.transform.localScale = new Vector3(xScale, 1f, 1f);
    }
    public void DamageUI()
    {
        StopCoroutine("OnBloodScreen_co");
        StartCoroutine("OnBloodScreen_co");
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
}