using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropProps : Props
{
    public Collectibles collectibles;
    public GameObject colPrefab;
    [Space]
    [Range(0, 100)]
    public float dropChance;
    public Vector3 offsetDrop = new Vector3(0,-1.5f,0);

    public override void ApplyDamage(int value)
    {
        base.ApplyDamage(value);
        SpawnCollectibles();
    }

    private void SpawnCollectibles()
    {
        float rng = UnityEngine.Random.Range(1,100);
        if(rng <= dropChance){
            Vector2 pos = transform.position + offsetDrop;
            GameObject collect = Instantiate(colPrefab, pos, Quaternion.identity);
            CollectiblesHolder holder = collect.GetComponent<CollectiblesHolder>();
            holder.Initialize(collectibles);
        }
    }
}
