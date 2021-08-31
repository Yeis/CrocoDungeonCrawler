    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float speed;
    public GemColor color;
    public bool isMoving;
    public bool isAttacking = false;
    public int damage = 10;
    public float slide = 0.0037f;
    public Animator anim;
    private AudioController audioController;

    public bool hasAttack = false;
    // Start is called before the first frame update
    void Start() {
        isMoving = true;
        anim = GetComponent<Animator>();
        audioController = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update() {
        if (isMoving) {
            
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - speed * Time.deltaTime , gameObject.transform.position.y, gameObject.transform.position.z);
        }

        if(isAttacking){
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - slide * Time.deltaTime, gameObject.transform.position.y, gameObject.transform.position.z);
        } 
    }

    public void KillCrocodile(){
        anim.SetTrigger("Dead");
        gameObject.tag = "DyingEnemy";
        isMoving = false;
    }

    public void DestroyCrocodile(){
        Destroy(gameObject);
    }

    public void Attack() {
        isMoving = false;
        anim.SetTrigger("Attack");
        isAttacking = true;

    }

    public void FinishAttack()
    {
        isMoving = true;
        isAttacking = false;
    }
    
    void OnBecameInvisible() {  
        Destroy(gameObject);
    }
}
