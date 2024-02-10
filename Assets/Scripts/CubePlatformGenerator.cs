using UnityEngine;

public class CubePlatformGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int resolution = 10;

    void Start()
    {
        GenerateCubePlatform();
    }

    [ContextMenu("Generate Platform")]
    void GenerateCubePlatform()
    {
        // Clear existing cubes
        ClearPlatform();

        // Calculate half of the platform size
        float halfSize = resolution / 2f;

        // Spawn cubes based on the resolution
        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                Vector3 spawnPosition = new Vector3(x - halfSize, 0, z - halfSize);
                Instantiate(cubePrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }

    void ClearPlatform()
    {
        // Destroy all child objects (cubes)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Called when the resolution value changes in the editor
    void OnValidate()
    {
        // Ensure resolution is at least 1
        resolution = Mathf.Max(1, resolution);

        // Regenerate platform when resolution changes in the editor
        GenerateCubePlatform();
    }
}
