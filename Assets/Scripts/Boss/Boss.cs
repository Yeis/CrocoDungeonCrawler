using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour {

    public int healthPoints = 3;
    private Animator animator;
    private bool isOnWeakness = false;
    public string gameScene = "VictoryScreen";
    private float attackPosDifference = 0.17f;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void Dying() {
        animator.SetTrigger("Dead");
    }

    public void Dead() {
        Victory();
        Destroy(gameObject);
    }

    public void TakeDamage() {
        healthPoints--;
        if(healthPoints <= 0) {
            Dying();
        } else {
            animator.SetTrigger("Damage");  
        }
    }

    public void Chuckle() {
        animator.SetTrigger("Chuckle");
    }

    public void Attack() {
        animator.SetTrigger("Attack");
    }

    public void Point() {
        animator.SetTrigger("Point");
    }

    public void Weakness() {
        isOnWeakness = !isOnWeakness;
        animator.SetBool("Weakness", isOnWeakness);
    }

    public void Victory(){
        SceneManager.LoadScene(gameScene);
    }

    public void Idle() {
        // transform.position = new Vector3(transform.position.x + attackPosDifference, transform.position.y, transform.position.z);
    }
}
