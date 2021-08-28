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

    // Start is called before the first frame update
    void Start() {
        isMoving = true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (isMoving) {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - speed, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        if(isAttacking){
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - slide, gameObject.transform.position.y, gameObject.transform.position.z);
            anim.SetBool("isAttacking", true);
            StartCoroutine("Attack");
        } else {
            anim.SetBool("isAttacking", false);
        }

    }

    public void TriggerAttack() {
        //Funcion triggerea attack animation
    }

    private IEnumerator Attack() {
        isMoving = false;
        yield return new WaitForSeconds(1.0f);
        isMoving = true;
        isAttacking = false;
    }
    

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
