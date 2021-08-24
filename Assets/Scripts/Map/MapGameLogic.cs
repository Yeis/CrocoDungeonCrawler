using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

namespace Map 
{
    public class MapGameLogic : MonoBehaviour
    {
        public int minAmountOfRooms = 10;
        public MapNode rootNode;
        
        private RoomTemplates templates;
        private int currentAmountOfRooms;
        private int random;
        private Queue<MapNode> queue;

        // Start is called before the first frame update
        void Start()
        {
            templates = GameObject.FindGameObjectWithTag("RoomsTemplates").GetComponent<RoomTemplates>();
            rootNode = new MapNode(Instantiate(templates.tRBL, transform.position, Quaternion.identity));
            currentAmountOfRooms = 1; 
            queue = new Queue<MapNode>();

            GenerateMap();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void GenerateMap() {
            //Generate Open Rooms
      
            queue.Enqueue(rootNode);
            while (queue.Count > 0 && currentAmountOfRooms < minAmountOfRooms) {
                MapNode mapNode = queue.Dequeue();
                Room currentRoom =  mapNode.node.GetComponent<Room>();
                if(currentRoom.hasTopDoor) {
                    random = Random.Range(0, templates.bottomRooms.Length);
                    GameObject topRoom = Instantiate(templates.bottomRooms[random], new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                    MapNode topMapNode = new MapNode(topRoom);
                    topMapNode.bottomRoom = mapNode;
                    mapNode.topRoom = topMapNode;
                    currentAmountOfRooms++;
                }
                if(currentRoom.hasBottomDoor) {
                    random = Random.Range(0, templates.topRooms.Length);
                    GameObject bottomRoom = Instantiate(templates.topRooms[random], new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
                    MapNode bottomMapNode = new MapNode(bottomRoom);
                    bottomMapNode.topRoom = mapNode;
                    mapNode.bottomRoom = bottomMapNode;
                    currentAmountOfRooms++;
                } 
                if(currentRoom.hasRightDoor) {
                    random = Random.Range(0, templates.leftRooms.Length);
                    GameObject rightRoom = Instantiate(templates.leftRooms[random], new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Quaternion.identity);
                    MapNode rightMapNode =  new MapNode(rightRoom);
                    rightMapNode.leftRoom = mapNode;
                    mapNode.rightRoom = rightMapNode; 
                    currentAmountOfRooms++;
                } 
                if(currentRoom.hasLeftDoor) {
                    random = Random.Range(0, templates.rightRooms.Length);
                    GameObject leftRoom = Instantiate(templates.rightRooms[random], new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z), Quaternion.identity);
                    MapNode leftMapNode = new MapNode(leftRoom);
                    leftMapNode.rightRoom = mapNode;
                    mapNode.leftRoom = leftMapNode;
                    currentAmountOfRooms++;
                } 


            }
            
        }
    }
}


