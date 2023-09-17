using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Monster
{
    // Start is called before the first frame update
    [SerializeField] Animator animator;
    Collider2D col;
    [SerializeField] public int player; // -1 -> desni player; 1-> levi player
    protected Rigidbody2D rb;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    //[SerializeField] public HealthBarBehaviour healthbar;
    protected bool inCombat;
    protected Health health;
    private float maxhealth;
    protected Health enemyHealth;

    void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        health = this.GetComponent<Health>();
        if (player == 1) {
            this.gameObject.layer = 6;
        }
        else {
            this.gameObject.layer = 7;
        }
        startMonster();
        health = this.GetComponent<Health>();
        //maxhealth = health.currentHealth;
        //healthbar.SetHealth(maxhealth, maxhealth);
        
        //
    }
    
    protected void OnTriggerEnter2D(Collider2D collision) {

        
        if (collision.tag == "Monster") {
            if (collision.gameObject.layer != this.gameObject.layer) {
                col = collision;
                //Debug.Log("HEALTH MAGEA:" + col.gameObject.GetComponent<Health>().currentHealth + "       HEALTH MOG" + health.currentHealth);
                rb.velocity = new Vector2(0, 0);
                animator.SetBool("Attacking", true);
            }
        }
        if(collision.tag == "Tower") {
            col = collision;
            TowerScript tower = collision.gameObject.GetComponent<TowerScript>();
            if (collision.gameObject.layer != this.gameObject.layer) {
                
                tower.takeDamage();
            }
            else {

                tower.OpenAndClose(1.5f);
            }
        }
        if(collision.tag == "DisableMask" && collision.gameObject.layer != this.gameObject.layer) {
            disableObject();
        }
        

    }

    protected void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Monster" && collision.gameObject.layer != this.gameObject.layer) {
            rb.velocity = new Vector2(player * speed, 0);
            //Debug.Log("HEALTH MAGEA:" + col.gameObject.GetComponent<Health>().currentHealth + "       HEALTH MOG" + health.currentHealth);
            //Debug.Log("IZASAO");
            animator.SetBool("Attacking", false);
            col = null;
            inCombat = false;
        }
    }
  
    public override void dealDamage() {
        if (col != null) {
            Monster enemy = col.gameObject.GetComponent<Monster>();
            float multiplicator=0;
            if (enemy.element == element) {
                multiplicator = 1;
            }
            else if (enemy.element == Element.WATER && element == Element.FIRE) {
                multiplicator = 0.0f;
            }
            else if (enemy.element == Element.GRASS && element == Element.WATER) {
                multiplicator = 0.0f;
            }
            else if (enemy.element == Element.FIRE && element == Element.GRASS) {
                multiplicator = 0.0f;
            }
            else if (enemy.element == Element.FIRE && element == Element.WATER) {
                multiplicator = 100f;
            }
            else if (enemy.element == Element.WATER && element == Element.GRASS) {
                multiplicator = 100f;
            }
            else if (enemy.element == Element.GRASS && element == Element.FIRE) {
                multiplicator = 100f;
            }//ubicu se
            enemyHealth = col.gameObject.GetComponent<Health>();
            enemyHealth.TakeDamage(damage * multiplicator,this);
        }
    }
    public override void startMonster() {
        { // dobije brzinu i ostalo tek kad se zavrsi animacija
            rb.velocity = new Vector2(player * speed, 0);
        }
    }
    public void disableCollider() {
        //Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), col, true);
        this.GetComponent<BoxCollider2D>().enabled = false;
        rb.velocity = new Vector2(0, 0);
    }
    public void adjustHealthbar(float curHealth) {

    }
}
