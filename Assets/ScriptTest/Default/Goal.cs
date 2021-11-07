using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    CircleCollider2D col;
    bool isCleared = false;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CircleCollider2D>();  
        isCleared = false;  
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Actors")){
            // float moveX = Mathf.Lerp(other.transform.position.x, transform.position.x, 1f);
            // float moveY = Mathf.Lerp(other.transform.position.y, transform.position.y, 1f);
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.transform.position = transform.position;
            if(!isCleared){
                GameManager.Instance.StageCleared();
                isCleared = true;
            }
        }
    }
}
