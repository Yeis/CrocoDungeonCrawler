using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public int healthPoints = 3;
    private Animator animator;
    private bool isOnWeakness = false;

    private float attackPosDifference = 0.17f;

    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage() {
        healthPoints--;
        animator.SetTrigger("Damage");
    }

    public void Chuckle() {
        animator.SetTrigger("Chuckle");
    }

    public void Attack() {
        // transform.position = new Vector3(transform.position.x - attackPosDifference, transform.position.y, transform.position.z);
        animator.SetTrigger("Attack");
    }

    public void Point() {
        // transform.position = new Vector3(transform.position.x - attackPosDifference, transform.position.y, transform.position.z);
        animator.SetTrigger("Point");
    }

    public void Weakness() {
        isOnWeakness = !isOnWeakness;
        animator.SetBool("Weakness", isOnWeakness );
    }

    public void Idle() {
        // transform.position = new Vector3(transform.position.x + attackPosDifference, transform.position.y, transform.position.z);
    }
}