using System;
using UnityEngine;

public interface IDamagable
{
    float CurrentHealth { get; }
    float MaxHealth { get; }

    void TakeDamage(float damageAmount);

    event Action<float> OnTakeDamage;
    event Action<GameObject> OnDeath;

}
