# Clouds
 
## Install

1. Clone repo to machine

1. Create cube prefab

1. Attach CubePlatformGenerator and Cloud texture to the cube prefab

1. Attach CubePrefab to the Cube Prefab in the script

1. Mess around with some values

## _______________________________________________________________________________

Animation speed increased for more satisfaction

![homer](cloudgif1.gif)
![homer](cloudgif2.gif)
![homer](cloudgif3.gif)

## How it's done

This is my take on voxilized clouds, it utilizes perlin noise to get that natural effect. 

1. Platform Initialization:

    - Provided a cube prefab set a various parameters like resolution, Perlin noise scale and minimum cube size

1. Update Method:

    - Update method continuously calls methods

    - First it clears existing cubes to prepare for regeneration

    - Then it generates a new platform

1. Cube Generation:

    - Script uses nested loops to iterate through positions on platform

    - For each position, it calculates the cube's position (xPos, yPox) and determins its height (yPos) using perlin noise

    - Heigh is influenced by a time-dependent Perlin noise function, resulting in a dynamic and changing landscape

1. Conditional Cube Instantiation:

    - It checks if calculated cube size is above a minimum threshold

    - If true, it instantiates a cube at calculated position, adjusting its scale based on Perlin noise

    - Scale also dynamically changes over time, creating a visually interesting effect

1. Dynamic Scaling:

    - Cubes undergo dynamic scaling, smoothly transitioning between a minimum size and the Perlin-based size over time

    - This dynamic scaling adds variation and motion to the platform

1. Time-Dependent Transformations:

    - The script utilizes 'Time.time' to introduce time-dependent changes in Perlin noise and dynamic scaling

    - As a result, the platform continually evolves, making it visually appealing and dynamic

In summary, the CubePlatformGenerator script utilizes Perlin noise and dynamic scaling to generate a dynamic platform of cubes in Unity. The script is flexible, allowing users to tweak parameter in the Inspector to achieve different visula effects and styles for their game or simulation. 