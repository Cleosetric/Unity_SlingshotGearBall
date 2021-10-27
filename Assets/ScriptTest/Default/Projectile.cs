using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject destroyAnimPrefab; 
    Actor actor;
    HitType hitType;
    float speed;
    float impact;

    public void Initialize(Actor actor, HitType hitType,float speed, float impact, Quaternion rotation){
        this.actor = actor;
        this.hitType = hitType;
        this.speed = speed;
        this.impact = impact;
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
                TimeManager.Instance.StartImpactMotion();
                targetAtk.ApplyDamage(actor);

                Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();
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
                TimeManager.Instance.StartImpactMotion();
                targetAtk.ApplyDamage(actor);

                Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();
                if(otherRb != null){
                    Vector3 dir = (transform.position - other.transform.position).normalized;
                   otherRb.AddForce(-dir * impact, ForceMode2D.Impulse);
                }

                Instantiate(destroyAnimPrefab, other.transform.position, Quaternion.identity);
                CameraManager.Instance.Shake(0.25f,0.05f);
            }
        }
    }

    void DestroyProps(Collider2D other){
        if(other.CompareTag("Props")){
            other.GetComponent<Box>().ApplyDamage(Mathf.RoundToInt(actor.statATK.GetValue()));
            OnProjectileDestroy(this.gameObject);
        }
    }

    void PierceProps(Collider2D other){
        if(other.CompareTag("Props")){
            other.GetComponent<Box>().ApplyDamage(Mathf.RoundToInt(actor.statATK.GetValue()));
            Instantiate(destroyAnimPrefab, other.transform.position, Quaternion.identity);
        }
    }

    private void OnProjectileDestroy(GameObject obj) {
        CameraManager.Instance.Shake(0.25f,0.05f);
        Instantiate(destroyAnimPrefab, obj.transform.position, Quaternion.identity);
        Destroy(obj);
    }
}
