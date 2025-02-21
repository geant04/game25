using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    private int maxIterations, roomLengthMin, roomWidthMin;

    public RoomGenerator(int maxIterations, int roomLengthMin, int roomWidthMin)
    {
        this.maxIterations = maxIterations;
        this.roomLengthMin = roomLengthMin;
        this.roomWidthMin = roomWidthMin;
    }

    public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, Vector3Int dungeonCenter) {
        List<RoomNode> listToReturn = new List<RoomNode>();
        foreach (var space in roomSpaces) {
            Vector2Int newBottomLeftPoint = StructureHelper.GenerateBottomLeftCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomBottomCornerModifier, roomOffset);

            Vector2Int newTopRightPoint = StructureHelper.GenerateTopRightCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, roomTopCornerModifier, roomOffset);

            space.BottomLeftAreaCorner = newBottomLeftPoint;
            space.TopRightAreaCorner = newTopRightPoint;
            space.BottomRightAreaCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
            space.TopLeftAreaCorner = new Vector2Int(newBottomLeftPoint.x, newTopRightPoint.y);

            GenerateSpawnZones(space, dungeonCenter);

            listToReturn.Add((RoomNode)space);
        }

        return listToReturn;
    }

    public void GenerateSpawnZones(Node space, Vector3Int dungeonCenter) {
        Vector3 bottomLeftV = new Vector3(space.BottomLeftAreaCorner.x, 0, space.BottomLeftAreaCorner.y);
        Vector3 topRightV = new Vector3(space.TopRightAreaCorner.x, 0, space.TopRightAreaCorner.y);
        Vector3 avgPos = (bottomLeftV + topRightV) / 2;

        for (int i = space.BottomLeftAreaCorner.x + 1; i < space.TopRightAreaCorner.x - 1; i += 10) {
            for (int j = space.BottomLeftAreaCorner.y + 1; j < space.TopRightAreaCorner.y - 1; j += 10) {
                Vector3 pos = new Vector3(i, 1, j);
                if (Vector3.Distance(pos, avgPos) < 5) {
                    continue;
                }
                SpawnZone zone = new SpawnZone(pos - dungeonCenter);
                space.AddSpawnZone(zone);
            }
        }
    }
}