using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float powerShoot = 10f;
    public int maxControlDrag = 3;
    [Space]
    [Range(1, 5)]
    [SerializeField] private int _maxIterations = 3;
    [SerializeField] private float _maxDistance = 10f;
    public int _count;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private CircleCollider2D col;
    private PlayerBall player;
    private Camera cam;
    private LineRenderer lr;
    private LineRenderer trajectory;

    private Vector2 minPower = new Vector2(-8, -8);
    private Vector2 maxPower = new Vector2(8, 8);
    private Vector2 force;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 currentPoint;
    private TimeManager timeMgr;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    
        timeMgr = FindObjectOfType<TimeManager>();
        player = FindObjectOfType<PlayerBall>();
        lr = GetComponent<LineRenderer>();
        rb = player.gameObject.GetComponent<Rigidbody2D>();
        col = player.gameObject.GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        trajectory = player.gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = -4;
            sprite.transform.position = startPoint;
            sprite.enabled = true;
            timeMgr.EnterSlowmotion();
        }

        if(Input.GetMouseButton(0)){
            currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = -4;
            ShowLine(startPoint, currentPoint);
            ShowTrajectory();
        }

        if(Input.GetMouseButtonUp(0)){
            rb.velocity = Vector2.zero;
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = -4;
            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x,minPower.x,maxPower.x),
            Mathf.Clamp(startPoint.y - endPoint.y,minPower.y,maxPower.y));
            rb.AddForce(force * powerShoot, ForceMode2D.Impulse);
            HideLine();
            timeMgr.ReleaseSlowmotion();
        }
    }

    void ShowTrajectory(){
        _count = 0;
        Vector3 playerPos = player.transform.position;
        Vector3 direction = startPoint - currentPoint;
        
        trajectory.positionCount = 1;
		trajectory.SetPosition(0, playerPos);
        DrawTrajectory(playerPos, direction);
    }

   private bool DrawTrajectory(Vector2 position, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction, _maxDistance);
        if (hit && _count <= _maxIterations - 1)
        {
            if(hit.collider.tag == "Enemy"){
                trajectory.startColor = Color.red;
                trajectory.endColor = new Color(255,25,20, 0);
                trajectory.positionCount = (_count + 2);
                trajectory.SetPosition(_count + 1, position + direction);
            }else{
                _count++;
                var reflectAngle = Vector2.Reflect(direction, hit.normal);
                trajectory.startColor = Color.blue;
                trajectory.endColor = new Color(16,207,255, 0);
                trajectory.positionCount = (_count + 1);
                trajectory.SetPosition(_count, hit.point);
                DrawTrajectory(hit.point + reflectAngle, reflectAngle);
            }
            return true;
        }
        
        if(hit == false){
            trajectory.startColor = Color.blue;
            trajectory.endColor = new Color(16,207,255, 0);
            trajectory.positionCount = (_count + 2);
            trajectory.SetPosition(_count + 1, position + direction);
        }
        return false;
    }

    void ShowLine(Vector3 startPoint, Vector3 endPoint){
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.numCapVertices = 10;

        Vector3 [] points = new Vector3[2];
        points[0] = startPoint;
        points[1] = Vector3.MoveTowards(startPoint, endPoint, maxControlDrag);
        
        lr.SetPositions(points);
        float fade = Vector2.Distance(startPoint, endPoint) / 100 * 50;
        lr.startColor = new Color(1.0F, fade + 1 - (fade * 2), 0.0F, 1.0F);
        lr.endColor = new Color(1.0F, fade + 1 - (fade * 2), 0.0F, 0.0F);
    }

    void HideLine(){
        _count = 0;
        lr.positionCount = 0;
        trajectory.positionCount = 0;
        sprite.enabled = false;
    }

}
