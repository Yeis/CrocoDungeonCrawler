using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int healthPoints = 100;
    public Sprite lockSprite;
    public GameObject lockedEnemy;
    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        LockEnemy();
    }

    private void LockEnemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0 && lockedEnemy == null) {
            lockedEnemy = enemies[0];
            lockedEnemy.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = lockSprite;
        }

        foreach (GameObject enemy in enemies) {
            //distance Current < current locked Enemy
            if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, lockedEnemy.transform.position)) {
                lockedEnemy.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                lockedEnemy = enemy;
                lockedEnemy.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = lockSprite;
            }
        }
    }

    public void TakeDamage(int damage) {
        this.healthPoints = this.healthPoints - damage;
    }

    public void Attack() {
        animator.SetTrigger("Attack");
        if (lockedEnemy != null) {
            Destroy(lockedEnemy);
            lockedEnemy = null;
        };
    }
}
