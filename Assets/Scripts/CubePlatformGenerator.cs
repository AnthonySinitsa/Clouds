using UnityEngine;

public class CubePlatformGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // Drag your cube prefab here in the Inspector
    public int resolution = 25; // Set the resolution here

    void Start()
    {
        GeneratePlatform();
    }

    void GeneratePlatform()
    {
        float cubeSize = 1.0f; // Size of each cube
        float platformSize = resolution * cubeSize; // Total size of the platform

        // Calculate the starting point for the platform
        float startX = -platformSize / 2;
        float startZ = -platformSize / 2;

        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                // Calculate the position for each cube
                float xPos = startX + x * cubeSize;
                float zPos = startZ + z * cubeSize;

                // Instantiate a cube prefab at the calculated position
                GameObject cube = Instantiate(cubePrefab, new Vector3(xPos, 0, zPos), Quaternion.identity);

                // Make the cube a child of the prefab cube
                cube.transform.parent = transform;
            }
        }
    }
}
