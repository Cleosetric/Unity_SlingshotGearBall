using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectibles", menuName = "Collectibles/Random Collectible", order = 5)]
public class CollectibleRandom : Collectibles
{
    public List<Collectibles> collectibles = new List<Collectibles>();

    public override void ApplyEffect(Transform parent)
    {
        int rng = Random.Range(1, collectibles.Count);
        collectibles[rng].ApplyEffect(parent);
    }

}
