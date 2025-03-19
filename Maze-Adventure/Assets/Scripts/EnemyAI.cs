using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float distance;
    public Transform player;
    Vector3 posit; // Position
    public float damageAmount = 20f;
    private PlayerHealth playerHealth;
    private Animator animator;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        
        if (playerObject != null)
        {
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing!");
        }
    }
    void Update()
    {
        if (player != null)
        {
            distance = Vector3.Distance(transform.position, player.position);
            posit = new Vector3(player.position.x, transform.position.y, player.position.z);

            if (distance < 50f)
            {
                animator.SetBool("Run", true);
                animator.SetBool("Idle", false);
                transform.LookAt(posit);
                float speed = 15f; //Emeny speed
                transform.position = Vector3.MoveTowards(transform.position, posit, speed * Time.deltaTime);
            
            }else{
                animator.SetBool("Run", false);
                animator.SetBool("Idle", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.GetDamage(damageAmount);
            }
        }
    }
}