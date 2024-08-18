using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TerrainHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            DestroyTerrain();
        }
    }

    void DestroyTerrain()
    {
        Destroy(gameObject);
    }
}
