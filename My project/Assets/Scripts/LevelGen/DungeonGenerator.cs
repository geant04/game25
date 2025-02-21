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

        Vector3Int dungeonCenter = new Vector3Int(dungeonWidth / 2, 0, dungeonLength / 2);
        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomLengthMin, roomWidthMin);
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces, roomBottomCornerModifier, roomTopCornerModifier, roomOffset, dungeonCenter);

        CorridorGenerator corridorGenerator = new CorridorGenerator();
        var corridorList = corridorGenerator.CreateCorridor(allNodesCollection, corridorWidth);

        return new List<Node>(roomList).Concat(corridorList).ToList();
    }
}
