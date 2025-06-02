using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 15;
    public float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out var target))
        {
            target.TakePhysicalDamage(damage);
            Destroy(gameObject);
        }
    }
}
