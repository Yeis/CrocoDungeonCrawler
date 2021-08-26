using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapNode
    {
       public GameObject node;
       public MapNode leftRoom;
       public MapNode rightRoom;
       public MapNode topRoom;
       public MapNode bottomRoom;
       public bool visited;
       public bool characterVisited;
       public bool isNodeVisible;
       public bool isCharacterInRoom;
       public bool isBossInRoom;
       public Vector3 position;    


       public MapNode(GameObject node) {
           this.node = node;
           this.position = node.transform.position;
           this.visited = false;
           this.characterVisited = false;
           this.isNodeVisible = true;
           this.isCharacterInRoom = false;
           this.isBossInRoom = false;
       }

       public void VisitRoom() {
           this.characterVisited = true;
           this.node.GetComponent<SpriteRenderer>().color = Color.green;
       }

       public void UnvisitRoom(){
           this.characterVisited = false;
           this.node.GetComponent<SpriteRenderer>().color = Color.white;
       }
    }
}

