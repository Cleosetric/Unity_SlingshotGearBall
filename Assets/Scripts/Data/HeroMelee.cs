using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroBase", menuName = "Hero/Melee", order = 0)]
public class HeroMelee : HeroBase
{
    [Space]
    [Header("Skills Prefab")]
    public GameObject baseAttack;
    public GameObject baseSkill;
    public GameObject baseUltimate;

    public override void Attack(GameObject obj){
        Vector3 pos = obj.transform.position;
        GameObject attackPrefab = Instantiate(baseAttack, pos, Quaternion.identity);
        attackPrefab.transform.SetParent(obj.transform);
        attackPrefab.GetComponent<Sword1>().InitializeSkill(attack, range);
    }
    
    public override void Skill(GameObject obj){
        obj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Vector3 pos = obj.transform.position;
        GameObject attackPrefab = Instantiate(baseSkill, pos, Quaternion.identity);
        attackPrefab.transform.SetParent(obj.transform);
        attackPrefab.GetComponent<Sword2>().InitializeSkill(attack, 0.45f);
    }
    
    public override void Ultimate(GameObject obj){
        Vector3 pos = obj.transform.position;
        GameObject attackPrefab = Instantiate(baseUltimate, pos, Quaternion.identity);
        attackPrefab.transform.SetParent(obj.transform);
        attackPrefab.GetComponent<Sword3>().InitializeSkill(attack, range, 5f);
    }
    
}
