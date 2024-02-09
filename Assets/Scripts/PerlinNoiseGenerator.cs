using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour{

    public int resolution = 10;
    public float scale = 1f;
    public float cubeSize = 1f;

    void Start(){
       GenerateTerrain(); 
    }

    void GenerateTerrain(){
        for(int x = 0; x < resolution; x++){
            for(int z = 0; z < resolution; z++){
                float xCoord = transform.position.x + x * scale;
                float zCoord = transform.position.z + z * scale;

                float y = Mathf.PerlinNoise(xCoord, zCoord) * cubeSize;

                Vector3 cubePosition = new Vector3(x * cubeSize, y, z * cubeSize);

                InstantiateCube(cubePosition);
            }
        }
    }

    void InstantiateCube(Vector3 position){
        GameObject cubePrefab = Resources.Load<GameObject>("CubePrefab");

        // Check if the prefab is found
        if (cubePrefab != null){
            // Instantiate the cube prefab at the calculated position
            GameObject cube = Instantiate(cubePrefab);
            cube.transform.position = position;
            cube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
        }
        else{
            Debug.LogError("CubePrefab not found in Resources folder.");
        }
    }
}