using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map {
    public class MapNode {
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
            this.node.GetComponent<SpriteRenderer>().color = new Color(96.0f / 255f, 44.0f / 255f, 44.0f / 255f, 1.0f);
        }

        public void UnvisitRoom() {
            this.characterVisited = false;
            this.node.GetComponent<SpriteRenderer>().color = new Color(192.0f / 255f, 148.0f / 255f, 115.0f / 255f, 1.0f);
        }

        // DEBUG
        public void setInBossRoom() {
            this.isBossInRoom = true;
        }
    }
}

