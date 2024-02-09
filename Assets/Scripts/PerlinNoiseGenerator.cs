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
        GameObject cube = Instantiate(Resources.Load("CubePrefab") as GameObject);
        cube.transform.position = position;
        cube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
    }
}
