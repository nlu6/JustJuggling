using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdBlocker : MonoBehaviour
{
    public Material[] materials; // Array to hold different materials
    public float minX = -6f; // Minimum x-coordinate
    public float maxX = 6f; // Maximum x-coordinate
    public float moveSpeed = 3f; // Speed of movement
    public float bounceHeight = 10f; // Height of the bounce
    public float bounceSpeed = 3f; // Speed of the bounce

    private Renderer rend;
    private Vector3 startPosition;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startPosition = transform.position;
        StartCoroutine(MoveAndChangeMaterial());
    }

    IEnumerator MoveAndChangeMaterial()
    {
        while (true)
        {
            float targetX = transform.position.x < (maxX + minX) / 2f ? maxX : minX; // Determine the target x-coordinate

            float initialY = transform.position.y;
            float timeElapsed = 0f;

            while (Mathf.Abs(transform.position.x - targetX) > 0.1f)
            {
                timeElapsed += Time.deltaTime;
                float bounceOffset = Mathf.Sin(timeElapsed * bounceSpeed) * bounceHeight; // Calculate bounce offset
                float step = moveSpeed * Time.deltaTime;
                Vector3 targetPos = new Vector3(targetX, startPosition.y + bounceOffset, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
                yield return null;
            }

            // Change material when reaching target x-coordinate
            rend.material = materials[Random.Range(0, materials.Length)];

            float delayTime = Random.Range(30f, 40f); // Random delay time between 30 and 40 seconds
            yield return new WaitForSeconds(delayTime);
        }
    }
}
