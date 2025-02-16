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
    GameObject wallVertical, wallHorizontal;
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
        terrainParent = new GameObject("TerrainParent");
        for (int i = 0; i < listOfRooms.Count; i++) {
            var room = listOfRooms[i];
            CreateMesh(room.BottomLeftAreaCorner, room.TopRightAreaCorner);
        }
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner) {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[] {
            topLeftV, topRightV, bottomLeftV, bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++) {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[] {
            0, 2, 1,
            1, 2, 3
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.normals = new Vector3[] { -Vector3.up, -Vector3.up, -Vector3.up, -Vector3.up };
        
        GameObject dungeonFloor = new GameObject("Mesh"+bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = new Vector3(0, 0, 0);
        dungeonFloor.transform.localScale = new Vector3(1, 1, 1);
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.transform.SetParent(terrainParent.transform);
    }
}
