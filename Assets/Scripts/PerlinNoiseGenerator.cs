using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour{
    public GameObject cubePrefab; // Drag your cube prefab here in the Inspector
    public int resolution; // Set the resolution here
    public float perlinScale; // Adjust the scale of the Perlin noise
    public float minCubeSize; // min size for cubes to spawn
    public float scaleChangeSpeed = 0.5f; // Speed at which the cubes change size
    public float maxScaleChange = 0.15f; // Maximum additional scale change

    void Update(){
        ClearPlatform();
        GeneratePlatform();
    }

    void ClearPlatform(){
        foreach (Transform child in transform){
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

                // Use Perlin noise to determine the height variation
                float scaleMultiplier =
                    Mathf.PerlinNoise(x * perlinScale, z * perlinScale) * 2.0f;

                if (cubeSize * scaleMultiplier > minCubeSize){
                    // Instantiate a cube prefab with adjusted scale
                    GameObject cube =
                        Instantiate(cubePrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity);

                    // this var for better readability for the transformation local var
                    float perlinCubeSize = cubeSize * scaleMultiplier;

                    // Check if the cube's size is within the specified range
                    if (perlinCubeSize >= minCubeSize && perlinCubeSize <= (minCubeSize + maxScaleChange)){
                        // Use a sine function to smoothly change the scale over time
                        float dynamicScale = Mathf.Sin(Time.time * scaleChangeSpeed) * 0.25f + 1.0f;
                        cube.transform.localScale =
                            new Vector3(perlinCubeSize * dynamicScale, perlinCubeSize * dynamicScale, perlinCubeSize * dynamicScale);
                    }

                    // Make the cube a child of the prefab cube
                    cube.transform.parent = transform;
                }
            }
        }
    }
}