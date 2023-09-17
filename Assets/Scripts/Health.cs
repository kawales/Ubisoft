using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private GameObject waterDeath;
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    private HealthBarBehaviour healthBar;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    [Header("Sound")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip dieSound;

    private void Awake()
    {
        waterDeath = Resources.Load("WaterDeath") as GameObject;
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();

        foreach (Behaviour item in components)
        {
            item.enabled = true;
        }
        healthBar = this.gameObject.GetComponentInChildren<HealthBarBehaviour>();
        healthBar.SetHealth(startingHealth, startingHealth);
    }

    public void TakeDamage(float _damage, Monster monster)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        this.gameObject.GetComponentInChildren<HealthBarBehaviour>().SetHealth(currentHealth, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead)
            {
                foreach (Behaviour item in components)
                {
                    item.enabled = false;
                }
                if(monster.element == Monster.Element.WATER) {
                    Instantiate(waterDeath).transform.position = this.transform.position;
                }
                else if (monster.element == Monster.Element.FIRE) {
                    
                }
                else {

                }
                anim.SetTrigger("die");
                dead = true;
                SoundManager.instance.PlaySound(dieSound);
            }
        }

    }
}
