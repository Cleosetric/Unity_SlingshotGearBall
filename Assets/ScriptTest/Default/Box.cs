using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box : Props
{
    [Range(1, 10)]
    [SerializeField] private int coinLoot = 3;

    protected override void OnDestroyedProps(){
        base.OnDestroyedProps();
        SpawnPoolCoin(coinLoot);
    }
    
    void SpawnPoolCoin(int total)
    {
        for (int i = 0; i < total; i++)
        {
            GameObject coin = ObjectPooling.Instance.GetPooledObject("Coin");
            if(coin != null){
                coin.GetComponent<Coin>().CoinSpawn(transform.position);
            }
        }
    }

}
