using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollider : MonoBehaviour
{
    public Player player;
    public Animator anim;

    void Start(){
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy") 
        {
           Enemy enemy = other.gameObject.GetComponent<Enemy>();
           other.gameObject.tag = "PostAttackEnemy";
           enemy.Attack();
           player.TakeDamage(enemy.damage);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag == "PostAttackEnemy") 
        {
           Enemy enemy = other.gameObject.GetComponent<Enemy>();
           enemy.isAttacking = false;
        }
    }
}
