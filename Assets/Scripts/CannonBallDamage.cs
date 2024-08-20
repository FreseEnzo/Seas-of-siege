using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float damageAmount = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Encontre o componente EnemyBoat no barco
            EnemyBoat enemyBoat = collision.gameObject.GetComponent<EnemyBoat>();
            if (enemyBoat != null)
            {
                // Aplique dano ao barco
                enemyBoat.TakeDamage(damageAmount);
                // Opcionalmente, destrua a bola após o impacto
                Destroy(gameObject);
            } 
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
			Destroy(gameObject);
			}
    }
}
