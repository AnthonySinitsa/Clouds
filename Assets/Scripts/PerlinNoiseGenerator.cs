using UnityEngine;
using System.Collections;

public class PerlinNoiseGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // Drag your cube prefab here in the Inspector
    public int resolution; // Set the resolution here
    public float perlinScale; // Adjust the scale of the Perlin noise
    public float minCubeSize; // min size for cubes to spawn
    public float waveSpeed;
    public float offset;
    public float scaleAnimationSpeed = 2.0f; // Adjust the speed of the scaling animation

    void Update()
    {
        // Delete existing cubes before regenerating the platform
        ClearPlatform();

        // Regenerate the platform with updated Perlin noise values
        GeneratePlatform();
    }

    void ClearPlatform()
    {
        // Destroy all child cubes
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
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
                float xPos = startX + x * cubeSize + 0.5f;
                float zPos = startZ + z * cubeSize + 0.5f;

                // Use Perlin noise with time-dependent offset and speed to determine the height variation
                float timeDependentOffset = Time.time * waveSpeed + offset;
                float scaleMultiplier =
                    Mathf.PerlinNoise(x * perlinScale + timeDependentOffset, z * perlinScale + timeDependentOffset) * 2.0f;

                if (cubeSize * scaleMultiplier > minCubeSize)
                {
                    // Instantiate a cube prefab
                    GameObject cube = Instantiate(cubePrefab, new Vector3(xPos, 0, zPos), Quaternion.identity);

                    // Calculate the animation scale based on the scaleMultiplier
                    float animationScale = Mathf.Lerp(0.5f, scaleMultiplier, scaleAnimationSpeed * Time.deltaTime);

                    // Set the initial scale to 0.5
                    cube.transform.localScale = new Vector3(animationScale, animationScale, animationScale);

                    // Make the cube a child of the prefab cube
                    cube.transform.parent = transform;

                    // Start scaling animation
                    StartCoroutine(ScaleAnimation(cube.transform, animationScale, scaleMultiplier));
                }
            }
        }
    }

    // Coroutine for scaling animation
    IEnumerator ScaleAnimation(Transform transform, float startScale, float endScale)
    {
        float elapsedTime = 0f;
        float duration = 1.0f / scaleAnimationSpeed;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(new Vector3(startScale, startScale, startScale),
                                                new Vector3(endScale, endScale, endScale),
                                                elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set the final scale to endScale
        transform.localScale = new Vector3(endScale, endScale, endScale);
    }
}
