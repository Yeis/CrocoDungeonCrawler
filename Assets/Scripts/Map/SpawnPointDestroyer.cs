using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class SpawnPointDestroyer : MonoBehaviour
    {
       	void OnTriggerEnter2D(Collider2D other){
		Destroy(other.gameObject);

	}
    }
}

