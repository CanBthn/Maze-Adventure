using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyHealth = 100f;

    public void DeductHealth(float deductHealth)
    {
        enemyHealth -= deductHealth;

        if (enemyHealth <= 0)
        {
            enemyDead();
        }
    }
    void enemyDead()
    {
        Destroy(gameObject);
    }
}