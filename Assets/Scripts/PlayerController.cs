using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    private static PlayerController instance;
    public static PlayerController Instance { get => instance; }

    [Header("Base player settings")]
    [SerializeField, Range(0f, 10f)] private float speed;
    [SerializeField, Range(0, 10)] private int maxHP;
    [SerializeField, Range(0, 10)] private int attackDamageBase;
    [SerializeField, Range(0f, 2f)] private float attackRange;
    [SerializeField] private Animator damageFX;

    [Header("SFX")]
    [SerializeField] private AudioClip damageSFX;
    [SerializeField] private AudioClip attackSFX;

    [Header("Gizmos")]
    [SerializeField] private Color gizmoColor = new Color(0, 0, 0, 1);
    private AudioSource audioSource;

    private int hp;
    public int HP
    {
        get => hp;
        set
        {
            if (value < hp)
            {
                hp = (int) (value / InventoryController.Instance.Defence);

                if (hp <= 0)
                {
                    OnDeath();
                }
                else
                {
                    HPController.Instance.HP = value;
                    audioSource.PlayOneShot(damageSFX);

                    animator.SetTrigger("damage");
                    damageFX.SetTrigger("damage");
                }
            }
            else
            {
                //Healing here
                hp = (value < maxHP) ? value : maxHP;
                HPController.Instance.HP = hp;
            }
        }
    }

    private Vector2 speedVector;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void OnDisable()
    {
        animator.SetTrigger("death");
    }

    public void OnDeath()
    {
        HPController.Instance.HP = 0;

        damageFX.SetTrigger("damage");
        audioSource.PlayOneShot(damageSFX);
        this.enabled = false;
    }

    public void TriggerSceneChangeAnimation()
    {
        damageFX.SetTrigger("death");
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (!instance)
            instance = this;

        hp = maxHP;
    }

    private void Update()
    {
        speedVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed * InventoryController.Instance.Speed;

        transform.Translate(speedVector * Time.deltaTime);
        animator.SetBool("walking", speedVector.magnitude > 0);
        if (speedVector.magnitude > 0 && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if (speedVector.x != 0) spriteRenderer.flipX = speedVector.x < 0f;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Attach logic here
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        
            foreach (Collider2D collider in colliders)
            {
                IDamagable damagable = collider.GetComponent<IDamagable>();
                if (damagable != null && damagable != (IDamagable) this)
                    damagable.HP -= (int) (attackDamageBase * InventoryController.Instance.Damage);

                animator.SetTrigger("attack");
            }
            audioSource.PlayOneShot(attackSFX);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}
