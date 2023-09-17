using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{


    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator anim;
    private float damage;
    private BoxCollider2D coll;

    private bool hit;

    private void Start()
    {
        Physics.IgnoreLayerCollision(8, 8);
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0.0f;
        gameObject.SetActive(true);
        coll.enabled = true;
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime >= resetTime)
            Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;

        damage = gameObject.GetComponentInParent<MageEnemy>().GetDamage();

        Monster enemy = collision.GetComponent<Monster>();
        Monster.Element element = GetComponentInParent<Monster>().element;

        int multiplicator = 0;
        if (enemy.element == element)
        {
            multiplicator = 1;
        }
        else if (enemy.element == Monster.Element.WATER && element == Monster.Element.FIRE)
        {
            multiplicator = 0;
        }
        else if (enemy.element == Monster.Element.GRASS && element == Monster.Element.WATER)
        {
            multiplicator = 0;
        }
        else if (enemy.element == Monster.Element.FIRE && element == Monster.Element.GRASS)
        {
            multiplicator = 0;
        }
        else if (enemy.element == Monster.Element.FIRE && element == Monster.Element.WATER)
        {
            multiplicator = 100;
        }
        else if (enemy.element == Monster.Element.WATER && element == Monster.Element.GRASS)
        {
            multiplicator = 100;
        }
        else if (enemy.element == Monster.Element.GRASS && element == Monster.Element.FIRE)
        {
            multiplicator = 100;
        }

        damage *= multiplicator;

        if (collision.tag == "Monster" && collision.gameObject.layer != GetComponentInParent<>)
        {
            collision.GetComponent<Health>().TakeDamage(damage, GetComponentInParent<Monster>());
        }
        coll.enabled = false;

        if (anim != null)
            anim.SetTrigger("explode");

        if (collision.tag == "Tower")
        {
            Deactivate();
        }
        if (hit)
        {
            Deactivate();
        }
        anim.SetTrigger("hit");

        gameObject.SetActive(false);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
        coll.enabled = false;
    }
}
