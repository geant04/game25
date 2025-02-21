using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DungeonGenerator
{
    List<RoomNode> allNodesCollection = new List<RoomNode>();
    private int dungeonWidth, dungeonLength;

    // Start is called before the first frame update
    public DungeonGenerator(int width, int length)
    {
        dungeonWidth = width;
        dungeonLength = length;
    }

    // Update is called once per frame
    public List<Node> CalculateDungeon(int maxIterations, int roomWidthMin, int roomLengthMin, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidth)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
        allNodesCollection = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLengthMin);
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeaves(bsp.RootNode);

        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomLengthMin, roomWidthMin);
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces, roomBottomCornerModifier, roomTopCornerModifier, roomOffset);

        CorridorGenerator corridorGenerator = new CorridorGenerator();
        var corridorList = corridorGenerator.CreateCorridor(allNodesCollection, corridorWidth);

        foreach(var room in roomList)
        {
            Vector3 bottomLeftV = new Vector3(room.BottomLeftAreaCorner.x, 0, room.BottomLeftAreaCorner.y);
            Vector3 topRightV = new Vector3(room.TopRightAreaCorner.x, 0, room.TopRightAreaCorner.y);

            Vector3 offset = new Vector3(-dungeonWidth / 2, 1.0f, -dungeonLength / 2);
            Vector3 avgPos = (bottomLeftV + topRightV) / 2;

            SpawnZone zone = new SpawnZone(avgPos + offset);
            room.AddSpawnZone(zone);
        }
        
        return new List<Node>(roomList).Concat(corridorList).ToList();
    }
}
