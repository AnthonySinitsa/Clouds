using UnityEngine;
using System.Collections;

public class CloudNoiseGenerator : MonoBehaviour{

    public int resolution = 10;
    public float scale = 0.1f;
    public float cubeSizeMultiplier = 2f;

    void Start(){
       GenerateCloud(); 
    }

    void GenerateCloud(){
        for(int x = 0; x < resolution; x++){
            for(int z = 0; z < resolution; z++){
                float xCoord = transform.position.x + x * scale;
                float zCoord = transform.position.z + z * scale;

                float perlinValue = Mathf.PerlinNoise(xCoord, zCoord);

                float cubeSize = perlinValue * cubeSizeMultiplier;

                cubeSize = Mathf.Max(cubeSize, 0.1f);

                Vector3 cubePosition = new Vector3(x * cubeSize, 0f, z * cubeSize);

                InstantiateCube(cubePosition, transform, cubeSize);
            }
        }
    }

    void InstantiateCube(Vector3 position, Transform parent, float cubeSize){
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