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
       public bool isNodeVisible;
       public Vector3 position;    


       public MapNode(GameObject node) {
           this.node = node;
           this.position = node.transform.position;
           this.visited = false;
           this.isNodeVisible = true;
       }
    }
}

