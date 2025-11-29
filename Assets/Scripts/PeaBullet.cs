using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaBullet : MonoBehaviour
{
    private float speed;
    private float damage;

    [SerializeField] private GameObject peaBulletHitPrefab;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right);
        if (transform.position.x > Camera.main.orthographicSize * Camera.main.aspect + 1f) // When not visible anymore
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie"))
        {
            Zombie zombie = collision.GetComponent<Zombie>();
            OnHitZombie(zombie);
        }
    }

    protected virtual void OnHitZombie(Zombie zombie)
    {
        if (zombie != null)
        {
            zombie.OnHitByPea(damage);
        }
        Instantiate(peaBulletHitPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
