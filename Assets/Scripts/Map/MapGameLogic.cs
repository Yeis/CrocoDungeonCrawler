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
        public Sprite characterSprite;
        
        private RoomTemplates templates;
        private int currentAmountOfRooms;
        private int random;
        private Queue<MapNode> queue;

        //This list is only used to calculate distance between rooms to fill with right piece
        private List<MapNode> rooms;
        public MapNode characterPositionNode;
        private MapNode bossPositionNode;
        // Start is called before the first frame update
        void Start()
        {
            templates = GameObject.FindGameObjectWithTag("RoomsTemplates").GetComponent<RoomTemplates>();
            rootNode = new MapNode(Instantiate(templates.TRBL, transform.position, Quaternion.identity));
            currentAmountOfRooms = 1; 
            queue = new Queue<MapNode>();
            rooms = new List<MapNode>();

            GenerateMap();
            SetupPlayerInMap();
            SetupBossInMap();
        }

        //Player in map functions
        private void SetupPlayerInMap() {
            characterPositionNode = GetRandomRoom();
            //Make it Appear 
            characterPositionNode.node.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = characterSprite;
            characterPositionNode.isCharacterInRoom = true;
        }
        public void MovePlayerLeft() {
            if(characterPositionNode.leftRoom != null) {
                characterPositionNode.node.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                characterPositionNode.isCharacterInRoom = false;
                characterPositionNode.VisitRoom();
                //Update properties in new Node
                characterPositionNode = characterPositionNode.leftRoom;
                characterPositionNode.node.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = characterSprite;
                characterPositionNode.isCharacterInRoom = true;
            }
        }

        public void MovePlayerRight() {
            if(characterPositionNode.rightRoom != null) {
                characterPositionNode.node.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                characterPositionNode.isCharacterInRoom = false;
                characterPositionNode.VisitRoom();
                //Update properties in new Node
                characterPositionNode = characterPositionNode.rightRoom;
                characterPositionNode.node.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = characterSprite;
                characterPositionNode.isCharacterInRoom = true;
            }
        }
        public void MovePlayerTop() {
            if(characterPositionNode.topRoom != null) {
                characterPositionNode.node.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                characterPositionNode.isCharacterInRoom = false;
                characterPositionNode.VisitRoom();
                //Update properties in new Node
                characterPositionNode = characterPositionNode.topRoom;
                characterPositionNode.node.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = characterSprite;
                characterPositionNode.isCharacterInRoom = true;
            }
        }
        public void MovePlayerBottom() {
            if(characterPositionNode.bottomRoom != null) {
                characterPositionNode.node.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
                characterPositionNode.isCharacterInRoom = false;
                characterPositionNode.VisitRoom();
                //Update properties in new Node
                characterPositionNode = characterPositionNode.bottomRoom;
                characterPositionNode.node.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = characterSprite;
                characterPositionNode.isCharacterInRoom = true;
            }
        }

        //Boss room function
        public void SetupBossInMap() {
            //Keep looking until we find an empty room for the boss
            int adjacentRoomCounter;
            do {
                adjacentRoomCounter = 0;
                bossPositionNode = GetRandomRoom();
                if(bossPositionNode.leftRoom != null) adjacentRoomCounter++;
                if(bossPositionNode.topRoom != null) adjacentRoomCounter++;
                if(bossPositionNode.bottomRoom != null) adjacentRoomCounter++;
                if(bossPositionNode.rightRoom != null) adjacentRoomCounter++;

            } while(bossPositionNode.isCharacterInRoom || adjacentRoomCounter > 1);
       
            bossPositionNode.isBossInRoom = true;
            bossPositionNode.node.GetComponent<SpriteRenderer>().color = Color.blue;
        }

        //Map Functions
        private void GenerateMap() {
            //Generate Open Rooms
            queue.Enqueue(rootNode);
            rooms.Add(rootNode);
            while (queue.Count > 0 && currentAmountOfRooms < minAmountOfRooms) {
                MapNode mapNode = queue.Dequeue();
                Room currentRoom =  mapNode.node.GetComponent<Room>();
                if(currentRoom.hasTopDoor && mapNode.topRoom == null) {
                    MapNode blockingRoom =  getBlockingRoom(new Vector3(mapNode.node.transform.position.x, mapNode.node.transform.position.y + 0.5f, mapNode.node.transform.position.z));
                    //Regular logic there is no blockingRoom
                    if(blockingRoom == null) {
                        random = Random.Range(0, templates.bottomRooms.Length);
                        GameObject topRoom = Instantiate(templates.bottomRooms[random], new Vector3(mapNode.node.transform.position.x, mapNode.node.transform.position.y + 0.5f, mapNode.node.transform.position.z), Quaternion.identity);
                        MapNode topMapNode = new MapNode(topRoom);
                        topMapNode.bottomRoom = mapNode;
                        mapNode.topRoom = topMapNode;
                        currentAmountOfRooms++;
                        queue.Enqueue(topMapNode);
                        rooms.Add(topMapNode);
                    } 
                    //There is an already blockingRoom that we need to update
                    else {
                        currentRoom.hasTopDoor = false;
                        if(mapNode.leftRoom != null) {
                            GameObject leftRoom = Instantiate(templates.L, mapNode.node.transform.position, Quaternion.identity);
                            Destroy(mapNode.node);
                            mapNode.node = leftRoom;
                        } else if(mapNode.rightRoom != null) {
                            GameObject rightRoom = Instantiate(templates.R, mapNode.node.transform.position, Quaternion.identity);
                            Destroy(mapNode.node);
                            mapNode.node = rightRoom;
                        } else if(mapNode.topRoom != null) {
                            GameObject topRoom = Instantiate(templates.T, mapNode.node.transform.position, Quaternion.identity);
                            Destroy(mapNode.node);
                            mapNode.node = topRoom;
                        }
                        mapNode.node.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
                if(currentRoom.hasBottomDoor && mapNode.bottomRoom == null) {
                    MapNode blockingRoom =  getBlockingRoom(new Vector3(mapNode.node.transform.position.x, mapNode.node.transform.position.y - 0.5f, mapNode.node.transform.position.z));
                    if(blockingRoom == null) {
                        random = Random.Range(0, templates.topRooms.Length);
                        GameObject bottomRoom = Instantiate(templates.topRooms[random], new Vector3(mapNode.node.transform.position.x, mapNode.node.transform.position.y - 0.5f, mapNode.node.transform.position.z), Quaternion.identity);
                        MapNode bottomMapNode = new MapNode(bottomRoom);
                        bottomMapNode.topRoom = mapNode;
                        mapNode.bottomRoom = bottomMapNode;
                        currentAmountOfRooms++;
                        queue.Enqueue(bottomMapNode);
                        rooms.Add(bottomMapNode);                   
                    }
                    //There is an already blockingRoom that we need to update
                    else {
                        currentRoom.hasBottomDoor = false;
                        if(mapNode.leftRoom != null) {
                            GameObject leftRoom = Instantiate(templates.L, mapNode.node.transform.position, Quaternion.identity);
                            Destroy(mapNode.node);
                            mapNode.node = leftRoom;
                        } else if(mapNode.rightRoom != null) {
                            GameObject rightRoom = Instantiate(templates.R, mapNode.node.transform.position, Quaternion.identity);
                            Destroy(mapNode.node);
                            mapNode.node = rightRoom;
                        } else if(mapNode.bottomRoom != null) {
                            GameObject bottomRoom = Instantiate(templates.B, mapNode.node.transform.position, Quaternion.identity);
                            Destroy(mapNode.node);
                            mapNode.node = bottomRoom;
                        }
                        mapNode.node.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                } 
                if(currentRoom.hasRightDoor && mapNode.rightRoom == null) {
                    MapNode blockingRoom = getBlockingRoom(new Vector3(mapNode.node.transform.position.x + 0.5f, mapNode.node.transform.position.y, mapNode.node.transform.position.z));
                    if(blockingRoom == null) {
                        random = Random.Range(0, templates.leftRooms.Length);
                        GameObject rightRoom = Instantiate(templates.leftRooms[random], new Vector3(mapNode.node.transform.position.x + 0.5f, mapNode.node.transform.position.y, mapNode.node.transform.position.z), Quaternion.identity);
                        MapNode rightMapNode =  new MapNode(rightRoom);
                        rightMapNode.leftRoom = mapNode;
                        mapNode.rightRoom = rightMapNode; 
                        currentAmountOfRooms++;
                        queue.Enqueue(rightMapNode);
                        rooms.Add(rightMapNode);
                    }
                    //There is an already blockingRoom that we need to update
                    else {
                        currentRoom.hasRightDoor = false;
                        if(mapNode.topRoom != null) {
                            GameObject topRoom = Instantiate(templates.T, mapNode.node.transform.position, Quaternion.identity);
                            Destroy(mapNode.node);
                            mapNode.node = topRoom;
                        } else if(mapNode.rightRoom != null) {
                            GameObject rightRoom = Instantiate(templates.R, mapNode.node.transform.position, Quaternion.identity);
                            Destroy(mapNode.node);
                            mapNode.node = rightRoom;
                        } else if(mapNode.bottomRoom != null) {
                            GameObject bottomRoom = Instantiate(templates.B, mapNode.node.transform.position, Quaternion.identity);
                            Destroy(mapNode.node);
                            mapNode.node = bottomRoom;
                        }
                        mapNode.node.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                } 
                if(currentRoom.hasLeftDoor && mapNode.leftRoom == null) {
                    MapNode blockingRoom = getBlockingRoom(new Vector3(mapNode.node.transform.position.x - 0.5f, mapNode.node.transform.position.y, mapNode.node.transform.position.z));
                    if(blockingRoom == null) {
                        random = Random.Range(0, templates.rightRooms.Length);
                        GameObject leftRoom = Instantiate(templates.rightRooms[random], new Vector3(mapNode.node.transform.position.x - 0.5f, mapNode.node.transform.position.y, mapNode.node.transform.position.z), Quaternion.identity);
                        MapNode leftMapNode = new MapNode(leftRoom);
                        leftMapNode.rightRoom = mapNode;
                        mapNode.leftRoom = leftMapNode;
                        currentAmountOfRooms++;
                        queue.Enqueue(leftMapNode);
                        rooms.Add(leftMapNode);
                    }
                    //There is an already blockingRoom that we need to update
                    else {
                        currentRoom.hasLeftDoor = false;
                        if(mapNode.topRoom != null) {
                            GameObject topRoom = Instantiate(templates.T, mapNode.node.transform.position, Quaternion.identity);
                            Destroy(mapNode.node);
                            mapNode.node = topRoom;
                        } else if(mapNode.leftRoom != null) {
                            GameObject leftRoom = Instantiate(templates.L, mapNode.node.transform.position, Quaternion.identity);
                            Destroy(mapNode.node);
                            mapNode.node = leftRoom;
                        } else if(mapNode.bottomRoom != null) {
                            GameObject bottomRoom = Instantiate(templates.B, mapNode.node.transform.position, Quaternion.identity);
                            Destroy(mapNode.node);
                            mapNode.node = bottomRoom;
                        }
                        mapNode.node.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                } 
            }

            //After creating simple map we need to cover all the holes 
            FillMapPieces();
            print("Amount of Rooms: " + rooms.Count);
        }

        private MapNode getBlockingRoom(Vector3 atPosition) {
            foreach (MapNode room in rooms)
            {
                if(Vector3.Distance(room.position, atPosition) < 0.1) return room;
            }
            return null;
        }
        //Find spaces where rooms are missing and fill them with closed paths
        private void FillMapPieces() {
            Stack<MapNode> stack = new Stack<MapNode>();
            stack.Push(rootNode);

            while(stack.Count != 0) {
                MapNode currentNode = stack.Pop();
                Room currentRoom = currentNode.node.GetComponent<Room>();
                if(currentRoom.hasTopDoor) {
                    if(currentNode.topRoom != null) {
                        if(!currentNode.topRoom.visited) stack.Push(currentNode.topRoom);
                    } 
                    //Create room at top of current
                    else {
                        GameObject topRoom = Instantiate(templates.B, new Vector3(currentNode.node.transform.position.x, currentNode.node.transform.position.y + 0.5f, currentNode.node.transform.position.z), Quaternion.identity);
                        MapNode topMapNode = new MapNode(topRoom);
                        currentNode.topRoom = topMapNode;
                        topMapNode.bottomRoom = currentNode;
                        rooms.Add(topMapNode);
                    }
                }
                if(currentRoom.hasBottomDoor) {
                    if(currentNode.bottomRoom != null) {
                        if(!currentNode.bottomRoom.visited) stack.Push(currentNode.bottomRoom);
                    } 
                    //Create room at top of current
                    else {
                        GameObject bottomRoom = Instantiate(templates.T, new Vector3(currentNode.node.transform.position.x, currentNode.node.transform.position.y - 0.5f, currentNode.node.transform.position.z), Quaternion.identity);
                        MapNode bottomMapNode = new MapNode(bottomRoom);
                        currentNode.bottomRoom = bottomMapNode;
                        bottomMapNode.topRoom = currentNode;
                        rooms.Add(bottomMapNode);
                    }
                }
                 if(currentRoom.hasLeftDoor) {
                    if(currentNode.leftRoom != null ) {
                        if(!currentNode.leftRoom.visited) stack.Push(currentNode.leftRoom);
                    } 
                    //Create room at top of current
                    else {
                        GameObject leftRoom = Instantiate(templates.R, new Vector3(currentNode.node.transform.position.x - 0.5f, currentNode.node.transform.position.y, currentNode.node.transform.position.z), Quaternion.identity);
                        MapNode leftMapNode = new MapNode(leftRoom);
                        currentNode.leftRoom = leftMapNode;
                        leftMapNode.rightRoom = currentNode;
                        rooms.Add(leftMapNode);
                    }
                }
                 if(currentRoom.hasRightDoor) {
                    if(currentNode.rightRoom != null) {
                        if(!currentNode.rightRoom.visited) stack.Push(currentNode.rightRoom);
                    } 
                    //Create room at top of current
                    else {
                        GameObject rightRoom = Instantiate(templates.L, new Vector3(currentNode.node.transform.position.x + 0.5f, currentNode.node.transform.position.y, currentNode.node.transform.position.z), Quaternion.identity);
                        MapNode rightMapNode = new MapNode(rightRoom);
                        currentNode.rightRoom = rightMapNode;
                        rightMapNode.leftRoom = currentNode;
                        rooms.Add(rightMapNode); 
                    }
                }
                currentNode.visited = true;
            }

        }

        //Public function
        public MapNode GetRandomRoom() {
            random = Random.Range(0, rooms.Count);
            return rooms[random];
        }
    }
}


