using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsPlayer;
    public float enemyHealth;
    
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    
    public float attackRange;
    public bool playerInAttackRange;
    
    public PlayerHealth pHealth;

    public void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        pHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInAttackRange)
        {
            Chasing();
        }
        if (playerInAttackRange)
        {
            Attack();
        }
    }

    public void Chasing()
    {
        Vector3 look;
        look = new Vector3(player.position.x, transform.position.y, player.position.z);
        agent.SetDestination(player.position);
        transform.LookAt(look);
        
    }

    public void Attack()
    {
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            pHealth.TakeDamage(10);
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

        }
    }

    public void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void EnemyTakeDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

}
