using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamagable
{
    [SerializeField, Range(0, 10)] private int attackDamageBase;
    [SerializeField, Range(0, 10)] private int maxHP;
    [SerializeField, Range(0f, 10f)] private float corpseLifetime;
    [SerializeField, Range(0f, 10f)] private float speed;
    [Header("AI Logic")]
    [SerializeField, Range(0f, 10f)] private float triggerRadius;
    [SerializeField, Range(0f, 10f)] private float attackRadius;
    [SerializeField, Range(0f, 10f)] private float attackDelay;
    [Header("SFX")]
    [SerializeField] private AudioClip swordSFX;
    [SerializeField] private AudioClip damageSFX;
    [SerializeField] private AudioClip deathSFX;
    [Header("Gizmos")]
    [SerializeField] private Color gizmoColorTrigger = new Color(1, 0, 0, 1);
    [SerializeField] private Color gizmoColorAttack = new Color(0.5f, 0.5f, 0, 1);

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private LayerMask mask;
    private float timePassed = 0f;

    private int hp;
    public int HP
    {
        get => hp;
        set {
            hp = value;

            if (hp <= 0)
            {
                OnDeath();
            }
            else
            {
                animator.SetTrigger("damage");
                audioSource.PlayOneShot(damageSFX);
            }
        }
    }

    private void OnDisable()
    {
        audioSource.PlayOneShot(deathSFX);
        animator.SetTrigger("death");
        GetComponent<Collider2D>().enabled = false;
        Destroy(this.gameObject, corpseLifetime);
    }

    public void OnDeath()
    {
        Instantiate(ListCollectables.Instance.getCollectable().gameObject, transform.position, Quaternion.identity);
        this.enabled = false;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        mask = LayerMask.GetMask("AI");
        hp = maxHP;
    }

    public void DamagePlayer()
    {
        float distance = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        if (distance <= attackRadius)
        {
            PlayerController.Instance.HP -= attackDamageBase;
        }
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        float distance = Vector2.Distance(PlayerController.Instance.transform.position, transform.position);
        if (distance < triggerRadius && timePassed >= attackDelay)
        {
            spriteRenderer.flipX = (PlayerController.Instance.transform.position.x < transform.position.x);

            if (distance > attackRadius)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, PlayerController.Instance.transform.position - transform.position, distance, mask);

                if (hit.collider.tag == "Player")
                {
                    transform.position = Vector2.Lerp(transform.position, PlayerController.Instance.transform.position, speed * Time.deltaTime);

                    animator.SetBool("walking", true);
                    if (!audioSource.isPlaying)
                        audioSource.Play();
                }
            }
            else
            {
                if (timePassed >= attackDelay)
                {
                    animator.SetTrigger("attack");
                    audioSource.PlayOneShot(swordSFX);
                    timePassed = 0f;
                }
                animator.SetBool("walking", false);
            }
        }
        else
            animator.SetBool("walking", false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColorTrigger;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
        Gizmos.color = gizmoColorAttack;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
#endif
}
