using UnityEngine;
using System.Collections;

public class CloudNoiseGenerator : MonoBehaviour{

    public int resolution = 10;
    public float scale = 0.1f;
    public float cubeSize = 0.1f;

    void Start(){
       GenerateTerrain(); 
    }

    void GenerateTerrain(){

        float halfResolution = (resolution - 1) * cubeSize / 2f;

        for(int x = 0; x < resolution; x++){
            for(int z = 0; z < resolution; z++){
                float xCoord = transform.position.x + x * scale;
                float zCoord = transform.position.z + z * scale;

                float y = Mathf.PerlinNoise(xCoord, zCoord) * cubeSize * 10f;

                Vector3 cubePosition = new Vector3((x - halfResolution) * cubeSize, y, (z - halfResolution) * cubeSize);

                InstantiateCube(cubePosition, transform);
            }
        }
    }

    void InstantiateCube(Vector3 position, Transform parent){
        GameObject cubePrefab = Resources.Load<GameObject>("CubePrefab");

        // Check if the prefab is found
        if (cubePrefab != null){
            // Instantiate the cube prefab at the calculated position
            GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity, parent);
            cube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
        }
        else{
            Debug.LogError("CubePrefab not found in Resources folder.");
        }
    }
}