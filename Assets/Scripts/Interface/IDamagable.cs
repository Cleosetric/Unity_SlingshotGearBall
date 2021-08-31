using UnityEngine;

public interface IDamagable
{
    int currentHealth { get; }

    void ApplyDamage(int damage);
}
