using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    [SerializeField] public int dungeonWidth;
    public int dungeonLength;
    public int roomWidthMin;
    public int roomLengthMin;
    public int maxIterations;
    public int corridorWidth;

    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f, 1.0f)]
    public float roomTopCornerModifier;
    [Range(0.0f, 2.0f)]
    public int roomOffset;
    public GameObject wallVertical, wallHorizontal;
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;

    public Material material;

    private GameObject terrainParent;

    // Start is called before the first frame update
    void Start()
    {
        CreateDungeons();
    }

    private void CreateDungeons() {
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateDungeon(maxIterations, roomWidthMin, roomLengthMin, roomBottomCornerModifier, roomTopCornerModifier, roomOffset, corridorWidth);
        
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();

        wallParent.transform.position = new Vector3(-dungeonWidth / 2, 0, -dungeonLength / 2);
        
        terrainParent = new GameObject("TerrainParent");

        for (int i = 0; i < listOfRooms.Count; i++) {
            var room = listOfRooms[i];
            CreateMesh(room.BottomLeftAreaCorner, room.TopRightAreaCorner);
        }
        CreateWalls(wallParent);
    }

    private void CreateWalls(GameObject wallParent) {
        foreach(var wallPosition in possibleWallHorizontalPosition) {
            CreateWall(wallParent, wallPosition, wallHorizontal);
        }
        foreach(var wallPosition in possibleWallVerticalPosition) {
            CreateWall(wallParent, wallPosition, wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wall) {
        wallPosition += new Vector3Int(-dungeonWidth / 2, 0, -dungeonLength / 2);
        Instantiate(wall, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner) {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        // Vector3[] vertices = new Vector3[] {
        //     topLeftV, topRightV, bottomLeftV, bottomRightV
        // };

        // Vector2[] uvs = new Vector2[vertices.Length];
        // for (int i = 0; i < uvs.Length; i++) {
        //     uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        // }

        // int[] triangles = new int[] {
        //     1, 2, 0,
        //     3, 2, 1
        // };

        // Mesh mesh = new Mesh();
        // mesh.vertices = vertices;
        // mesh.uv = uvs;
        // mesh.triangles = triangles;
        // // mesh.normals = new Vector3[] { -Vector3.up, -Vector3.up, -Vector3.up, -Vector3.up };
        
        // GameObject dungeonFloor = new GameObject("Mesh"+bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        // dungeonFloor.transform.position = new Vector3(-dungeonWidth / 2, 0, -dungeonLength / 2);
        // dungeonFloor.transform.localScale = new Vector3(1, 1, 1);
        // dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        // dungeonFloor.GetComponent<MeshRenderer>().material = material;
        // dungeonFloor.transform.SetParent(terrainParent.transform);

        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point)){
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }
}
