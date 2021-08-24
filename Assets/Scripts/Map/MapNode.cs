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


       public MapNode(GameObject node) {
           this.node = node;
           this.visited = false;
           this.isNodeVisible = true;
       }
    }
}

