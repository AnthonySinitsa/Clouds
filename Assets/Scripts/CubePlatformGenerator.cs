using UnityEngine;
using System.Collections;

public class CubePlatformGenerator : MonoBehaviour{
    public GameObject cubePrefab; // Drag your cube prefab here in the Inspector
    public int resolution; // Set the resolution here
    public float perlinScale; // Adjust the scale of the Perlin noise
    public float minCubeSize; // min size for cubes to spawn
    public float waveSpeed;
    public float offset;

    private float fadeInTime = 0.5f;
    private float fadeOutTime = 0.5f;

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
                    cube.transform.localScale = 
                        new Vector3(perlinCubeSize, perlinCubeSize, perlinCubeSize);

                    // Make the cube a child of the prefab cube
                    cube.transform.parent = transform;

                    // start coroutine for fading in and out
                    StartCoroutine(FadeInAndOut(cube, fadeInTime, fadeOutTime, scaleMultiplier));
                }
            }
        }
    }

    IEnumerator FadeInAndOut(GameObject cube, float fadeInTime, float fadeOutTime, float finalScaleMultiplier)
    {
        float elapsedTime = 0f;
        float startScale = 0.1f;
        Vector3 initialScale = cube.transform.localScale;

        // Fade in
        while (elapsedTime < fadeInTime && cube != null)
        {
            float t = elapsedTime / fadeInTime;
            cube.transform.localScale = Vector3.Lerp(initialScale, new Vector3(finalScaleMultiplier, finalScaleMultiplier, finalScaleMultiplier), t);
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
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Check if cube is null before setting final scale
            if (cube != null)
            {
                cube.transform.localScale = initialScale;

                // Destroy the cube after fading out
                Destroy(cube);
            }
        }
    }
}
