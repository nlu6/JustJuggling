using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdPersonBounce : MonoBehaviour
{
    float jumpHeight;
    float jumpSpeed;
    float randomStartTime;
    float randomStartHeight;
    Vector3 initialPosition;

    public void InitializeBounce(float height, float speed, float startTime, float startHeight, Vector3 initialPos)
    {
        jumpHeight = height;
        jumpSpeed = speed;
        randomStartTime = startTime;
        randomStartHeight = startHeight;
        initialPosition = initialPos;
    }

    void Update()
    {
        float offset = Mathf.Sin((Time.time + randomStartTime) * jumpSpeed) * jumpHeight + randomStartHeight;
        transform.position = initialPosition + new Vector3(0f, offset, 0f);
    }
}
