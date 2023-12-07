using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class CrowdPopulation : MonoBehaviour
{
    public GameObject crowdPersonPrefab; // Reference to your crowd person prefab
    public int personsPerStand = 0; // Number of crowd persons per stand
    public float jumpHeight = 1.0f; // Maximum height of the jump
    public float jumpSpeed = 1.0f; // Speed of the jump animation
    public TextMeshProUGUI uitScore;

    public Material[] materials;

    struct StandInfo
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

    StandInfo[] stands = {
        new StandInfo {
            position = new Vector3(-26f, 4.8851f, 2.7999f),
            rotation = Quaternion.Euler(-40f, 84f, -0.529f),
            scale = new Vector3(73.15f, 19.411f, 1.2391f)
        },
        new StandInfo {
            position = new Vector3(27f, 5.5181f, 3.2013f),
            rotation = Quaternion.Euler(-40f, -84f, -0.529f),
            scale = new Vector3(73.15f, 20.929f, 1.3301f)
        },
        new StandInfo {
            position = new Vector3(-0.385f, 5.4142f, 32f),
            rotation = Quaternion.Euler(-40f, -180f, -0.529f),
            scale = new Vector3(73.328f, 20.423f, 1f)
        }
    };

    void Start()
    {
        PopulateStands();
    }

    void PopulateStands()
    {
        foreach (StandInfo stand in stands)
        {
            SpawnCrowdInStand(stand.position, stand.rotation, stand.scale);
        }
    }

    void SpawnCrowdInStand(Vector3 standPosition, Quaternion standRotation, Vector3 standScale)
    {
        for (int i = 0; i < personsPerStand; i++)
        {
            // Calculate random spawn position within the current stand
            Vector3 spawnPosition = CalculateRandomSpawnPosition(standPosition, standRotation, standScale);

            // Instantiate a crowd person at the calculated spawn position
            GameObject crowdPerson = Instantiate(crowdPersonPrefab, spawnPosition, Quaternion.identity);

            // Assign a random material to the instantiated CrowdPerson
            AssignRandomMaterial(crowdPerson);

            // Apply the jumping effect to the crowd person
            JumpingEffect(crowdPerson);
        }
    }

    void AssignRandomMaterial(GameObject crowdPerson)
    {
        if (materials != null && materials.Length > 0)
        {
            Renderer[] renderers = crowdPerson.GetComponentsInChildren<Renderer>();

            if (renderers != null)
            {
                foreach (Renderer renderer in renderers)
                {
                    Material randomMaterial = materials[Random.Range(0, materials.Length)];
                    renderer.material = randomMaterial;
                }
            }
        }
    }

    void JumpingEffect(GameObject crowdPerson)
    {
        float randomStartTime = Random.Range(0f, 2f); // Random start time for jump offset
        float randomStartHeight = Random.Range(0f, 2f); // Random start height for jump offset

        Renderer[] renderers = crowdPerson.GetComponentsInChildren<Renderer>();

        if (renderers != null)
        {
            foreach (Renderer renderer in renderers)
            {
                Material randomMaterial = materials[Random.Range(0, materials.Length)];
                renderer.material = randomMaterial;
            }
        }

        // Store the initial position of the crowd person
        Vector3 initialPosition = crowdPerson.transform.position;

        // Apply the bouncing effect using the Update method
        crowdPerson.GetComponent<CrowdPersonBounce>().InitializeBounce(jumpHeight, jumpSpeed, randomStartTime, randomStartHeight, initialPosition);
    }

    Vector3 CalculateRandomSpawnPosition(Vector3 standPosition, Quaternion standRotation, Vector3 standScale)
    {
        // Generate random values within the stand scale
        float offsetX = Random.Range(-standScale.x / 2f, standScale.x / 2f);
        float offsetY = Random.Range(-standScale.y / 2f, standScale.y / 2f);
        float offsetZ = Random.Range(-standScale.z / 2f, standScale.z / 2f);

        // Apply the random offsets to a vector
        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);

        // Rotate the offset according to the stand's rotation
        Vector3 rotatedOffset = standRotation * offset;

        // Calculate the spawn position by adding the rotated offset to the stand's position
        return standPosition + rotatedOffset;
    }
}
