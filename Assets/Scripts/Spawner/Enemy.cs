using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float speed;
    public GemColor color;
    public bool isMoving;
    // public bool hasAttack;
    public bool isAttacking = false;
    public Sprite newMarker;

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
            StartCoroutine("Attack");
        } else {
            anim.SetBool("isAttacking", false);
        }

        this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = newMarker;
    }

    public void TriggerAttack() {
        //Funcion triggerea attack animation
    }

    private IEnumerator Attack() {
        isMoving = false;
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
        isMoving = true;
    }
    

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
