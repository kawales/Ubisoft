using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageEnemy : Monster
{
    
    [SerializeField] private bool player1; 
    private float direction;
    protected Rigidbody2D body;
    [SerializeField] private float range;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Animator anim;
    private float coolDownTimer = Mathf.Infinity;

    [Header("Layers")]
    [SerializeField] LayerMask player1Layer;
    [SerializeField] LayerMask player2Layer;


    [Header("Collider Params")]
    [SerializeField] private float colliderDistance;
    private BoxCollider2D boxCollider;

    [SerializeField] protected float speed;
    [SerializeField] private int damage;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip fireballSound;

    private Health enemyHealth;

    private Collider2D collider;

    private void Start()
    {
        collider = null;
    }

    private void Awake()
    {
        if (player1)
        {
            direction = 1;
            gameObject.layer = 6;
        }
        else
        {
            direction = -1;
            gameObject.layer = 7;
        }

        Debug.Log(gameObject.name + " " + gameObject.layer);

        collider = null;

        body = GetComponent<Rigidbody2D>();
        body.velocity = new Vector2(speed * direction, 0);
        boxCollider = GetComponent<BoxCollider2D>();

        transform.localScale = new Vector3(transform.localScale.x * direction, transform.localScale.y, transform.localScale.z);

    }

    public int GetDamage() { return damage; }

    // Update is called once per frame
    void Update()
    {
        coolDownTimer += Time.deltaTime;
        if (EnemyInSight())
        {
            if (coolDownTimer >= attackCooldown)
            {
                coolDownTimer = 0.0f;
                anim.SetTrigger("attack");
            }
            body.velocity = Vector2.zero;
        }
        else
        {
            body.velocity = new Vector2(speed * direction, 0);
            enemy = null;
            coolDownTimer = Mathf.Infinity;
        }

    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (EnemyInSight())
        {
            body.velocity = new Vector2(0, 0);
            collider = collision;
        }
        if (collision.tag == "Tower")
        {
            TowerScript tower = collision.gameObject.GetComponent<TowerScript>();
            if (collision.gameObject.layer != this.gameObject.layer)
            {

                tower.takeDamage();
            }
            else
            {
                tower.OpenAndClose(1.5f);
            }
        }
        if (collision.tag == "DisableMask" && collision.gameObject.layer != this.gameObject.layer)
        {
            disableObject();
        }
    }
    Monster enemy;
    protected void OnTriggerExit2D(Collider2D collision)
    {
        body.velocity = new Vector2(direction * speed, 0);
        collider = null;
    }

    public void disableObject()
    {
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * transform.localScale.x * range * colliderDistance,
                            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public bool EnemyInSight()
    {
        LayerMask enemyLayer;
        if (player1)
            enemyLayer = player2Layer;
        else
            enemyLayer = player1Layer;

        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * transform.localScale.x * range * colliderDistance
                                            , new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
                                            , 0, Vector2.left, 0, enemyLayer);


        if (hit.collider != null)
        {
            enemyHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null && hit.collider.tag == "Monster";
    }

    private void DamageEnemy()
    {
        if (EnemyInSight())
        {
            enemyHealth.TakeDamage(damage, this);
        }
    }

    private void RangedAttack()
    {
        SoundManager.instance.PlaySound(fireballSound);
        coolDownTimer = 0.0f;

        fireballs[findFireball()].transform.position = firePoint.position;
        fireballs[findFireball()].GetComponent<Projectile>().ActivateProjectile();
        //Shoot
    }

    private int findFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                Debug.Log(i);
                return i;
            }
        }

        return 0;
    }


    public override void startMonster()
    {
        throw new System.NotImplementedException();
    }

    public override void dealDamage()
    {
        if (collider != null)
        {
            Monster enemy = collider.gameObject.GetComponent<Monster>();
            int multiplicator = 0;
            if (enemy.element == element)
            {
                multiplicator = 1;
            }
            else if (enemy.element == Element.WATER && element == Element.FIRE)
            {
                multiplicator = 0;
            }
            else if (enemy.element == Element.GRASS && element == Element.WATER)
            {
                multiplicator = 0;
            }
            else if (enemy.element == Element.FIRE && element == Element.GRASS)
            {
                multiplicator = 0;
            }
            else if (enemy.element == Element.FIRE && element == Element.WATER)
            {
                multiplicator = 100;
            }
            else if (enemy.element == Element.WATER && element == Element.GRASS)
            {
                multiplicator = 100;
            }
            else if (enemy.element == Element.GRASS && element == Element.FIRE)
            {
                multiplicator = 100;
            }//ubicu se
            damage *= multiplicator;
        }
    }
}
