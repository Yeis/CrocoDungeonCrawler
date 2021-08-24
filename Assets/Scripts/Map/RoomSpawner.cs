using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Map
{
    public class RoomSpawner : MonoBehaviour
    {
        public int openingDirection;
        // 1 -> need bottom door
        // 2 -> need top door
        // 3 -> need left door
        // 4 -> need right door

        private int random;
        private RoomTemplates templates;
        private MapGameLogic mapGameLogic;
        private bool spawned = false;


        // Start is called before the first frame update
        void Start()
        {
            templates = GameObject.FindGameObjectWithTag("RoomsTemplates").GetComponent<RoomTemplates>();
            mapGameLogic = GameObject.FindGameObjectWithTag("MapGameLogic").GetComponent<MapGameLogic>();
            Invoke("Spawn", 0.1f);
        }

        // Update is called once per frame
        void Spawn()
        {
            // if(!spawned) {
            //     // random = Random.Range(0, 1);
            //     if(openingDirection == 1) {
            //         random = Random.Range(0, templates.bottomRooms.Length);
            //         Instantiate(templates.bottomRooms[random], transform.position, Quaternion.identity);
            //     } else if(openingDirection == 2) {
            //         random = Random.Range(0, templates.topRooms.Length);
            //         Instantiate(templates.topRooms[random], transform.position, Quaternion.identity);
            //     } else if(openingDirection == 3) {
            //         random = Random.Range(0, templates.leftRooms.Length);
            //         Instantiate(templates.leftRooms[random], transform.position, Quaternion.identity);
            //     } else if(openingDirection == 4) {
            //         random = Random.Range(0, templates.rightRooms.Length);
            //         Instantiate(templates.rightRooms[random], transform.position, Quaternion.identity);
            //     } 
            //     spawned = true;
            // }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("SpawnPoint")) {
                if(other.GetComponent<RoomSpawner>().spawned == false && spawned == false) {
                    //Spawn walls blocking
                    // Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                    //Coloring for debugging
                    gameObject.transform.parent.GetComponent<SpriteRenderer>().color = Color.red;
                    other.gameObject.transform.parent.GetComponent<SpriteRenderer>().color = Color.red;
                    print(gameObject.transform.parent.name  +": This openingDirection: " + this.openingDirection);
                    print(other.gameObject.transform.parent.name + ": Collider openingDirection: " + other.GetComponent<RoomSpawner>().openingDirection);
                    Destroy(gameObject);               

                }
                spawned = true;

              
            } 
        }
    }
}


