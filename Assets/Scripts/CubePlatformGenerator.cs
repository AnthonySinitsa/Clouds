using UnityEngine;

public class CubePlatformGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // Drag your cube prefab here in the Inspector
    [Range(1, 50)] public int resolution = 25; // Set the resolution here
    public float perlinScale = 0.1f; // Adjust the scale of the Perlin noise

    void Start()
    {
        GeneratePlatform();
    }

    void GeneratePlatform()
    {
        float targetPlatformSize = 20.0f; // Desired platform size
        float normalizedResolution = Mathf.Clamp01(resolution / targetPlatformSize); // Normalize resolution to [0, 1]

        // Calculate the cube size based on the normalized resolution
        float cubeSize = targetPlatformSize / resolution;

        // Calculate the starting point for the platform
        float startX = -targetPlatformSize / 2;
        float startZ = -targetPlatformSize / 2;

        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                // Calculate the position for each cube
                float xPos = startX + x * cubeSize;
                float zPos = startZ + z * cubeSize;

                // Use Perlin noise to determine the height variation
                float yOffset = Mathf.PerlinNoise(x * perlinScale, z * perlinScale) * 2.0f;

                // Instantiate a cube prefab with adjusted position and scale
                GameObject cube = Instantiate(cubePrefab, new Vector3(xPos, yOffset, zPos), Quaternion.identity);
                cube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize); // Set the Y scale to cubeSize

                // Make the cube a child of the prefab cube
                cube.transform.parent = transform;
            }
        }
    }
}
