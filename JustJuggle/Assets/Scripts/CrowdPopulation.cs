using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdPopulation : MonoBehaviour
{
    public GameObject crowdPersonPrefab; // Reference to your crowd person prefab
    public int personsPerStand = 300; // Number of crowd persons per stand
    public float jumpHeight = 1.0f; // Maximum height of the jump
    public float jumpSpeed = 1.0f; // Speed of the jump animation

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
            position = new Vector3(26.145f, 5.5181f, 3.2013f),
            rotation = Quaternion.Euler(-40f, -84f, -0.529f),
            scale = new Vector3(73.15f, 20.929f, 1.3301f)
        },
        new StandInfo {
            position = new Vector3(-0.385f, 5.4142f, 32.5f),
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
            Vector3 spawnPosition = CalculateRandomSpawnPosition(standPosition, standRotation, standScale);
            GameObject crowdPerson = Instantiate(crowdPersonPrefab, spawnPosition, Quaternion.identity);

            // Assign a random material to the instantiated CrowdPerson
            AssignRandomMaterial(crowdPerson);

            crowdPerson.transform.parent = transform; // Set the crowd person's parent to the stands GameObject

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
        float offsetX = Random.Range(-standScale.x / 2f, standScale.x / 2f);
        float offsetY = Random.Range(-standScale.y / 2f, standScale.y / 2f);
        float offsetZ = Random.Range(-standScale.z / 2f, standScale.z / 2f);

        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);
        Vector3 rotatedOffset = standRotation * offset;

        return standPosition + rotatedOffset;
    }
}
