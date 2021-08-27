using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class RoomDifficultyManager : MonoBehaviour
{
    
    private int roomCounter = 0;
    public RoomType difficultyLevel = RoomType.tutorial;
    public List<RoomType> difficultyArray = new List<RoomType>();
    public GameObject mapGameLogic;
    private MapGameLogic mapController;

    void Awake() {
        mapController = mapGameLogic.GetComponent<MapGameLogic>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateRoomCounterAndDifficulty() {
        if(!mapController.characterPositionNode.characterVisited && !mapController.characterPositionNode.isBossInRoom){
            roomCounter++;
            difficultyLevel = difficultyArray[roomCounter-1];
        }
    }


    public List<RoomType> generateDifficultyArray() {
        //rooms without root or bossroom AND after first 2 rooms
        int normalRooms = mapController.currentAmountOfRooms - 4;

        difficultyArray.Add(RoomType.easyRoom);
        difficultyArray.Add(RoomType.mediumRoom);

        float eventRoomQuantity = Mathf.Floor(normalRooms/3); 
        int randy = 0;
        int numberOfCurrentEvents = 0; 

        for (int i = 0; i < normalRooms; i++)
        {
            randy = Random.Range(0, 2);
            if(randy == 1 && numberOfCurrentEvents < eventRoomQuantity) {
                difficultyArray.Add(RoomType.eventRoom);
                numberOfCurrentEvents++;
            } else {
                difficultyArray.Add(RoomType.mediumRoom);
            }
        }

        return difficultyArray;
    }

}

public enum RoomType {
    tutorial = 0, easyRoom = 1, mediumRoom = 2, bossRoom = 3, eventRoom = 4
}