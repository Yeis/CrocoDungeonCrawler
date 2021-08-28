using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollider : MonoBehaviour
{
    public Animator anim;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy") 
        {
           Enemy enemy = other.gameObject.GetComponent<Enemy>();
           enemy.isAttacking = true;
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
