using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth;
    [SerializeField] float viewRadius;
    [SerializeField] Vector3 sphereOffset;
    [SerializeField] LayerMask playerMask;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public event Action<float> OnTakeDamage;
    public event Action<GameObject> OnDeath;

    private bool isDead = false;

    private Transform player;
    private Animator animator;

    private Collider[] hitResults = new Collider[1];

    #region Unity Callback Function
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        player = CheckForPlayer();
        if (player != null)
        {
            transform.LookAt(player);
        }
    }

    #endregion

    #region Check Functions

    private Transform CheckForPlayer()
    {
        Transform playerTransform = null;
        float closestDistanceSqr = Mathf.Infinity;

        int hitCount = Physics.OverlapSphereNonAlloc(transform.position + sphereOffset, viewRadius, hitResults, playerMask);

        for (int i = 0; i < hitCount; i++)
        {
            Transform target = hitResults[i].transform;
            float distToTarget = Vector3.Distance(transform.position, target.position);

            if (distToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = distToTarget;
                playerTransform = target;
            }
        }

        return playerTransform;
    }

    #endregion

    #region Enemy Damage

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        animator.SetBool("hit", true);
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnTakeDamage?.Invoke(amount);

        if (currentHealth <= 0)
        {
            Die();
            animator.SetBool("hit", false);
        }
    }

    private void Die()
    {
        isDead = true;
        OnDeath?.Invoke(this.gameObject);
        //gameObject.SetActive(false);
        animator.SetBool("die", true);
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + sphereOffset, viewRadius);
    }

    #endregion

}
