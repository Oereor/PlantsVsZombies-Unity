using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaBullet : MonoBehaviour
{
    [SerializeField] private float speed;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
        if (transform.position.x > Camera.main.orthographicSize * Camera.main.aspect + 1f) // When not visible anymore
        {
            Destroy(gameObject);
        }
    }
}
