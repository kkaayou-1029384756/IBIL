using UnityEngine;

public class GetEnemyAttack : EnemyHP
{
    int damage = 5;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyDamage(damage, gameObject);
            Destroy(collision.gameObject);
        }
    }
}
