using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public float healthPoints = 100;
    public string gameScene;
    public GameObject hpMask;
    public Sprite lockSprite;
    public AudioController audioController;
    public GameObject lockedEnemy;
    private Animator animator;
    private bool isAlive;


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
        if ((enemies.Length > 0 && lockedEnemy == null) ||(lockedEnemy != null && lockedEnemy.tag != "Enemy")) {
            if(lockedEnemy != null) lockedEnemy.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
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
        animator.SetTrigger("TakingDamage");
        audioController.PlayCrocoAttackClip();
        hpMask.transform.localPosition = new Vector3(Mathf.Lerp(0.1284f,0.66f, healthPoints/ 100f), hpMask.transform.localPosition.y, hpMask.transform.localPosition.z);
        if (this.healthPoints == 0)
        {
            GameOver();

        }
    }

    public void AddHealth(int healthPoints) {
        this.healthPoints = this.healthPoints + healthPoints;
        hpMask.transform.localPosition = new Vector3(Mathf.Lerp(0.1284f,0.66f, healthPoints/ 100), hpMask.transform.localPosition.y, hpMask.transform.localPosition.z);
    }
    
    public void GameOver()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void Attack(GemColor attackColor) {
        audioController.PlayPlayerAttackClip();
        switch (attackColor) {
            case GemColor.blue:
                animator.SetTrigger("AttackBlue");
                break;
            case GemColor.orange:
                animator.SetTrigger("AttackOrange");
                break;
            case GemColor.green:
                animator.SetTrigger("AttackGreen");
                break;
            case GemColor.pink:
                animator.SetTrigger("AttackPink");
                break;
        }
        if (lockedEnemy != null) {
            lockedEnemy.GetComponent<Enemy>().KillCrocodile();
            lockedEnemy = null;
        };
    }
}
