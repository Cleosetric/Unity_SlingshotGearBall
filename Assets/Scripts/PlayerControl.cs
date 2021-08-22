using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float powerShoot = 10f;
    public float maxLineLength = 3f;
    private Rigidbody2D rb;
    private PlayerBall player;
    private Camera cam;
    private LineRenderer lr;


    public Vector2 minPower;
    public Vector2 maxPower;
    private Vector2 force;
    private Vector3 startPoint;
    private Vector3 endPoint;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        player = FindObjectOfType<PlayerBall>();
        lr = GetComponent<LineRenderer>();
        rb = player.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = -4;
        }

        if(Input.GetMouseButton(0)){
            
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 clampedPoint = new Vector3(Mathf.Clamp(startPoint.x + currentPoint.x,minPower.x,maxPower.x),
            Mathf.Clamp(startPoint.y + currentPoint.y,minPower.y,maxPower.y),-4);
            currentPoint.z = -4;
            ShowLine(startPoint, currentPoint);
        }

        if(Input.GetMouseButtonUp(0)){
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = -4;
            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x,minPower.x,maxPower.x),
            Mathf.Clamp(startPoint.y - endPoint.y,minPower.y,maxPower.y));
            rb.AddForce(force * powerShoot, ForceMode2D.Impulse);
            HideLine();
        }
    }

    void ShowLine(Vector3 startPoint, Vector3 endPoint){
        lr.positionCount = 2;
        Vector3 [] points = new Vector3[2];
        points[0] = startPoint;
        points[1] = Vector3.MoveTowards(startPoint, endPoint, maxLineLength);
        
        lr.SetPositions(points);

        float fade = Vector2.Distance(startPoint, endPoint) / 100 * 15;
 
        lr.startColor = new Color(1.0F, fade + 1 - (fade * 2), 0.0F, 1.0F);
        lr.endColor = new Color(1.0F, fade + 1 - (fade * 2), 0.0F, 0.0F);
    }

    void HideLine(){
        lr.positionCount = 0;
    }

}
