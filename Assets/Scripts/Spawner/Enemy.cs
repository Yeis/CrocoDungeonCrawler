using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float speed;
    public GemColor color;
    public bool isMoving;
    public bool hasAttack;
    public bool isAttacking = false;

    public Animator anim;

    // Start is called before the first frame update
    void Start() {
        isMoving = true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (isMoving) {
            float frameSpeed = speed * Time.deltaTime;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - speed, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        if(isAttacking){
            anim.SetBool("isAttacking", true);
        } else {
            anim.SetBool("isAttacking", false);
        }
    }

    public void TriggerAttack() {
        StartCoroutine("Attack");
        //Funcion triggerea attack animation
        //animator.SetBool("Attack", true);
    }

    private IEnumerator Attack() {
        isMoving = false;
        hasAttack = true;
        yield return new WaitForSeconds(1.0f);
        isMoving = true;
    }
    

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
