using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int healthPoints = 100;
    
    private GameObject lockedEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LockEnemy();
        lockedEnemy.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void LockEnemy() {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if(enemies.Length > 0 && lockedEnemy == null) lockedEnemy = enemies[0];

            foreach (GameObject enemy in enemies)
            {
                //distance Current < current locked Enemy
                if(Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, lockedEnemy.transform.position)) {
                    lockedEnemy.GetComponent<SpriteRenderer>().color = Color.white;
                    lockedEnemy = enemy;
    
                }
            }
    }

    public void TakeDamage(int damage) {
        this.healthPoints = this.healthPoints - damage;
    }

    public void Attack() {}
    
}
