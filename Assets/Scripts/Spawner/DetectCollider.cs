using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollider : MonoBehaviour
{
    public Player player;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy") 
        {
           Enemy enemy = other.gameObject.GetComponent<Enemy>();
           enemy.isAttacking = true;
           other.gameObject.tag = "PostAttackEnemy";
          player.TakeDamage(enemy.damage);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy") 
        {
           Enemy enemy = other.gameObject.GetComponent<Enemy>();
           enemy.isAttacking = false;
        }
    }
}
