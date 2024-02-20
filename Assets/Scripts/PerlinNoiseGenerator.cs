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

    private float fadeInTime = 0.5f; // Time to fade in the cubes
    private float fadeOutTime = 0.5f; // Time to fade out the cubes

    void Start()
    {
        GeneratePlatform();
    }

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
                    // Instantiate a cube prefab with adjusted scale
                    GameObject cube =
                        Instantiate(cubePrefab, new Vector3(xPos, 0, zPos), Quaternion.identity);

                    // this var for better readability for the transformation local var
                    float perlinCubeSize = 0.5f; // Starting scale
                    cube.transform.localScale =
                        new Vector3(perlinCubeSize, perlinCubeSize, perlinCubeSize);

                    // Make the cube a child of the prefab cube
                    cube.transform.parent = transform;

                    // Start coroutine for fading in and out
                    StartCoroutine(FadeInAndOut(cube, fadeInTime, fadeOutTime, scaleMultiplier));
                }
            }
        }
    }

    IEnumerator FadeInAndOut(GameObject cube, float fadeInTime, float fadeOutTime, float finalScaleMultiplier)
    {
        float elapsedTime = 0f;
        float startScale = 0.5f;
        Vector3 initialScale = cube.transform.localScale;
        Material cubeMaterial = cube.GetComponent<Renderer>().material;

        // Set initial alpha to 0
        Color initialColor = cubeMaterial.color;
        initialColor.a = 0f;
        cubeMaterial.color = initialColor;

        // Fade in
        while (elapsedTime < fadeInTime && cube != null)
        {
            float t = elapsedTime / fadeInTime;
            cube.transform.localScale = Vector3.Lerp(initialScale, new Vector3(finalScaleMultiplier, finalScaleMultiplier, finalScaleMultiplier), t);

            // Fade in by changing alpha
            initialColor.a = Mathf.Lerp(0f, 1f, t);
            cubeMaterial.color = initialColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Check if cube is null before setting final scale
        if (cube != null)
        {
            cube.transform.localScale = new Vector3(finalScaleMultiplier, finalScaleMultiplier, finalScaleMultiplier);

            // Wait for a while before fading out
            yield return new WaitForSeconds(1.0f); // Adjust as needed

            // Fade out
            elapsedTime = 0f;
            while (elapsedTime < fadeOutTime && cube != null)
            {
                float t = elapsedTime / fadeOutTime;
                cube.transform.localScale = Vector3.Lerp(new Vector3(finalScaleMultiplier, finalScaleMultiplier, finalScaleMultiplier), initialScale, t);

                // Fade out by changing alpha
                initialColor.a = Mathf.Lerp(1f, 0f, t);
                cubeMaterial.color = initialColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Check if cube is null before setting final scale
            if (cube != null)
            {
                cube.transform.localScale = initialScale;

                // Set alpha to 0 before destroying the cube
                initialColor.a = 0f;
                cubeMaterial.color = initialColor;

                // Destroy the cube after fading out
                Destroy(cube);
            }
        }
    }
}
