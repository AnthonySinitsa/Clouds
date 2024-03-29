using UnityEngine;
using System.Collections;

public class CubePlatformGenerator : MonoBehaviour{
    public GameObject cubePrefab; // Drag your cube prefab here in the Inspector
    public int resolution = 64; // Set the resolution here
    public float perlinScale = 0.15f; // Adjust the scale of the Perlin noise
    public float minCubeSize = 0.8f; // min size for cubes to spawn
    public float waveSpeed = 0.01f;
    public float offset = 2.0f;
    public float scaleChangeSpeed = 0.15f;

    void Update(){
        ClearPlatform();
        GeneratePlatform();
    }

    void ClearPlatform(){
        foreach(Transform child in transform){
            Destroy(child.gameObject);
        }
    }

    void GeneratePlatform(){
        float cubeSize = 1.0f; // Size of each cube
        float platformSize = resolution * cubeSize; // Total size of the platform

        // Calculate the starting point for the platform
        float startX = -platformSize / 2;
        float startZ = -platformSize / 2;

        for (int x = 0; x < resolution; x++){
            for (int z = 0; z < resolution; z++){
                // Calculate the position for each cube
                float xPos = startX + x * cubeSize + 0.5f;
                float zPos = startZ + z * cubeSize + 0.5f;
                float yPos = Mathf.PerlinNoise(xPos, zPos) * 2.0f;

                // use perlin noise with time-dependent offset and speed to determine the height variation
                float timeDependentOffset = Time.time * waveSpeed + offset;
                float scaleMultiplier = 
                    Mathf.PerlinNoise(x * perlinScale + timeDependentOffset, z * perlinScale + timeDependentOffset) * 2.0f;

                if(cubeSize * scaleMultiplier > minCubeSize){
                    // Instantiate a cube prefab with adjusted scale
                    GameObject cube = 
                        Instantiate(cubePrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity);


                    // this var for better readibility for the transformation local var
                    float perlinCubeSize = cubeSize * scaleMultiplier;

                    // float dynamicScale = Mathf.Sin(Time.time * scaleChangeSpeed) * minCubeSize + perlinCubeSize;
                    float dynamicScale = Mathf.Lerp(0.5f, perlinCubeSize, Mathf.Sin(Time.time * scaleChangeSpeed) * minCubeSize + perlinCubeSize);

                    cube.transform.localScale = 
                        new Vector3(perlinCubeSize * dynamicScale, perlinCubeSize * dynamicScale, perlinCubeSize * dynamicScale);

                    // Make the cube a child of the prefab cube
                    cube.transform.parent = transform;
                }
            }
        }
    }
}