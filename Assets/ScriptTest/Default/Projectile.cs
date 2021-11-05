using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject destroyAnimPrefab; 
    Charachter charachter;
    HitType hitType;
    float speed;
    float impact;
    bool shake;

    public void Initialize(Charachter charachter, HitType hitType,float speed, float impact,bool shake, Quaternion rotation){
        this.charachter = charachter;
        this.hitType = hitType;
        this.speed = speed;
        this.impact = impact;
        this.shake = shake;
        transform.rotation = rotation;
    }

    private void Update() {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch ((int)hitType)
        {
            case 0: //destroyAll
                DestroyEnemy(other);
                DestroyProps(other);
                break;
            case 1: //destroy Only Enemy
                DestroyEnemy(other);
                break;
            case 2: //pierceAll
                PierceEnemy(other);
                PierceProps(other);
                break;
            case 3: //pierce Only Enemy
                PierceEnemy(other);
                break;
        }
    }

    void DestroyEnemy(Collider2D other){
        if(other.CompareTag("Enemy")){
            Mob targetAtk =  other.GetComponent<Mob>();
            if(targetAtk != null) {
                if(shake) TimeManager.Instance.StartImpactMotion();
                targetAtk.ApplyDamage(charachter);

                Rigidbody2D otherRb = targetAtk.rb;
                if(otherRb != null){
                    Debug.Log("Yes there is RB in "+ other.name);
                    Vector3 dir = (transform.position - other.transform.position).normalized;
                    otherRb.AddForce(-dir * impact, ForceMode2D.Impulse);
                }

                OnProjectileDestroy(this.gameObject);
            }
        }
    }

    void PierceEnemy(Collider2D other){
        if(other.CompareTag("Enemy")){
            Mob targetAtk =  other.GetComponent<Mob>();
            if(targetAtk != null) {
                if(shake) TimeManager.Instance.StartImpactMotion();
                targetAtk.ApplyDamage(charachter);

                Rigidbody2D otherRb = targetAtk.rb; //other.GetComponent<Rigidbody2D>();
                if(otherRb != null){
                    Vector3 dir = (transform.position - other.transform.position).normalized;
                   otherRb.AddForce(-dir * impact, ForceMode2D.Impulse);
                }

                Instantiate(destroyAnimPrefab, other.transform.position, Quaternion.identity);
                if(shake) CameraManager.Instance.Shake(0.25f,1.5f);
            }
        }
    }

    void DestroyProps(Collider2D other){
        if(other.CompareTag("Props")){
            other.GetComponent<Props>().ApplyDamage(Mathf.RoundToInt(charachter.statATK.GetValue()));
            OnProjectileDestroy(this.gameObject);
        }
    }

    void PierceProps(Collider2D other){
        if(other.CompareTag("Props")){
            other.GetComponent<Props>().ApplyDamage(Mathf.RoundToInt(charachter.statATK.GetValue()));
            Instantiate(destroyAnimPrefab, other.transform.position, Quaternion.identity);
        }
    }

    private void OnProjectileDestroy(GameObject obj) {
        if(shake) CameraManager.Instance.Shake(0.25f,1.5f);
        Instantiate(destroyAnimPrefab, obj.transform.position, Quaternion.identity);
        Destroy(obj);
    }
}
