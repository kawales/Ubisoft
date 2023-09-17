using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{
    // Start is called before the first frame update
    [SerializeField] Animator anim;
    Collider2D col = null;
    [SerializeField] public bool player1;
    private float direction;
    protected Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    protected bool inCombat;
    protected Health health;
    protected Health enemyHealth;
    [Header("Layers")]
    [SerializeField] LayerMask player1Layer;
    [SerializeField] LayerMask player2Layer;

    [Header("cooldown")]

    [SerializeField] private float attackCooldown;
    private float coolDownTimer = Mathf.Infinity;


    private void Start()
    {
        col = null;
    }

    private void Awake()
    {
        if (player1)
        {
            this.gameObject.layer = 6;
            direction = 1.0f;
        }
        else
        {
            this.gameObject.layer = 7;
            direction = -1.0f;
        }
        rb = this.GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        col = null;
        //anim = GetComponent<Animator>();

        startMonster();
        health = this.GetComponent<Health>();
    }

    Monster enemy = null;

    private void Update()
    {
        coolDownTimer += Time.deltaTime;
        if (EnemyInSight())
        {
            if (coolDownTimer >= attackCooldown)
            {
                coolDownTimer = 0.0f;
                anim.SetTrigger("attack");
            }
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = new Vector2(speed * direction, 0);
            enemy = null;
            coolDownTimer = Mathf.Infinity;
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Monster")
        {
            enemy = collision.GetComponent<Monster>();
            if (collision.gameObject.layer != this.gameObject.layer)
            {
                rb.velocity = new Vector2(0, 0);
                col = collision;
            }
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

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Monster")
        {
            rb.velocity = new Vector2(direction * speed, 0);
            col = null;
            inCombat = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center ,
                            new Vector3(boxCollider.bounds.size.x , boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public bool EnemyInSight()
    {
        LayerMask enemyLayer;
        if (player1)
            enemyLayer = player2Layer;
        else
            enemyLayer = player1Layer;

        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center
                                            , new Vector3(boxCollider.bounds.size.x , boxCollider.bounds.size.y, boxCollider.bounds.size.z)
                                            , 0, Vector2.left, 0, enemyLayer);


        if (hit.collider != null)
        {
            enemyHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null && hit.collider.tag == "Monster";
    }

    public override void dealDamage()
    {
        if (col != null)
        {
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

            if (col.tag == "Monster")
            {
                col.GetComponent<Health>().TakeDamage(damage, this);
            }
        }
    }
    public override void startMonster()
    {
        { // dobije brzinu i ostalo tek kad se zavrsi animacija
            rb.velocity = new Vector2(direction * speed, 0);
        }
    }
    public void disableCollider()
    {
        //Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), col, true);
        this.GetComponent<BoxCollider2D>().enabled = false;
        rb.velocity = new Vector2(0, 0);
    }


    public void AttackEnemy()
    {
        dealDamage();
    }
}
