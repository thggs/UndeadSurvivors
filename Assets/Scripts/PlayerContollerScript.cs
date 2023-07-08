using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerContollerScript : MonoBehaviour
{
    [SerializeField]
    private GameStats gameStats;
    private bool takingDamage = false;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip pickupSound;
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    private Vector3 input;

    public bool hasWhip = false;
    public bool hasBible = false;
    public bool hasHolyWater = false;
    public bool hasThrowingKnife = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = pickupSound;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        gameStats.player.PlayerHealth = gameStats.player.PlayerMaxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f).normalized;

        if (!takingDamage)
        {
            sprite.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }
        if (gameStats.player.PlayerHealth > gameStats.player.PlayerMaxHealth)
        {
            gameStats.player.PlayerHealth = gameStats.player.PlayerMaxHealth;
        }

        gameObject.transform.GetChild(0).gameObject.SetActive(hasWhip);
        gameObject.transform.GetChild(1).gameObject.SetActive(hasBible);
        gameObject.transform.GetChild(2).gameObject.SetActive(hasHolyWater);
        gameObject.transform.GetChild(3).gameObject.SetActive(hasThrowingKnife);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + input * Time.deltaTime * gameStats.player.PlayerSpeed);

        // Character animations and sprite flipping
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            animator.SetTrigger("player_walk");
            if (Input.GetAxisRaw("Horizontal") > 0)
                transform.eulerAngles = new Vector3(0, 0, 0);
            else if (Input.GetAxisRaw("Horizontal") < 0)
                transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            animator.SetTrigger("player_idle");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            gameStats.player.PlayerHealth -= collision.GetComponent<EnemyControllerScript>().damage;

            if (gameStats.player.PlayerHealth <= 0)
            {
                Die();
            }
            else
            {
                takingDamage = true;
                sprite.color = new Vector4(1.0f, 1.0f, 1.0f, 0.5f);
            }
        }
        if(collision.tag == "Projectile")
        {
            gameStats.player.PlayerHealth -= gameStats.boss.BossProjectileDamage;
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            takingDamage = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ExitDoor")
        {
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
        if (collision.tag == "EnterDoor")
        {
            SceneManager.LoadScene("HouseScene", LoadSceneMode.Single);
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
