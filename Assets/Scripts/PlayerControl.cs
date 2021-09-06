using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    #region Singleton
	private static PlayerControl _instance;
	public static PlayerControl Instance { get { return _instance; } }

	private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
	#endregion

    public float powerShoot = 10f;
    public float maxDistance = 10f;

    [Space]
    [Range(1, 3)]
    [SerializeField] private int maxControlDrag = 3;
    [Range(1, 3)]
    [SerializeField] private int maxIterations = 3;
    
    private int count;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private CircleCollider2D col;
    private Player player;
    private Camera cam;
    private LineRenderer lr;
    private LineRenderer trajectory;
    private ParticleSystem dust;

    private Vector2 minPower = new Vector2(-8, -8);
    private Vector2 maxPower = new Vector2(8, 8);
    private Vector2 force;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 currentPoint;

    private bool onSling = false;
    private bool onDrag = false;
    private bool isDoUltimate = false;

    private float downClickTime;
    private float delayDeltaTime = 0.175f;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        player = FindObjectOfType<Player>();
        lr = GetComponent<LineRenderer>();

        rb = player.gameObject.GetComponent<Rigidbody2D>();
        col = player.gameObject.GetComponent<CircleCollider2D>();
        dust = player.gameObject.GetComponentInChildren<ParticleSystem>();
        trajectory = player.gameObject.GetComponent<LineRenderer>();

        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        onSling = false;

        StartCoroutine(InputListener());
        player.RefreshHero();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerControl();
        CheckPlayerMovement();
    }

    private bool IsMouseOnArea(Vector3 MousePosition){
        if(MousePosition.y > 200 && MousePosition.y < 1200){
            return true;
        }
        return false;
    }

    void ShowTrajectory(){
        count = 0;
        Vector3 playerPos = player.transform.position;
        Vector3 direction = startPoint - currentPoint;

        trajectory.positionCount = 1;
		trajectory.SetPosition(0, playerPos);
        DrawTrajectory(playerPos, direction);
    }

   private bool DrawTrajectory(Vector2 position, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction, maxDistance);
        if (hit && count <= maxIterations - 1)
        {
            if(hit.collider.tag == "Enemy"){
                trajectory.startColor = Color.red;
                trajectory.endColor = new Color(255,25,20, 0);
                trajectory.positionCount = (count + 2);
                trajectory.SetPosition(count + 1, position + direction);
            }else{
                count++;
                var reflectAngle = Vector2.Reflect(direction, hit.normal);
                trajectory.startColor = Color.blue;
                trajectory.endColor = new Color(16,207,255, 0);
                trajectory.positionCount = (count + 1);
                trajectory.SetPosition(count, hit.point);
                DrawTrajectory(hit.point + reflectAngle, reflectAngle);
            }
            return true;
        }
        
        if(hit == false){
            trajectory.startColor = Color.blue;
            trajectory.endColor = new Color(16,207,255, 0);
            trajectory.positionCount = (count + 2);
            trajectory.SetPosition(count + 1, position + direction);
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
        count = 0;
        lr.positionCount = 0;
        trajectory.positionCount = 0;
        sprite.enabled = false;
    }

    void CheckPlayerMovement(){
        if(rb.velocity.magnitude >= 1.5f){
            dust.Play();
        }else{
            dust.Stop();
        }
    }

    private IEnumerator InputListener(){
        while(enabled){
            if(Input.GetMouseButtonDown(0)){
                yield return ClickEvent();
            }
            yield return null;
        }
    }

    private IEnumerator ClickEvent(){
        yield return new WaitForEndOfFrame();
        float clickCount = 0f;
        while (clickCount < 0.25f)
        {
            if(Input.GetMouseButtonDown(0)){
                CheckPlayerUltimate();
                yield break;
            }
            clickCount += Time.unscaledDeltaTime;
            yield return null;
        }
        CheckPlayerSkill();
    }

    private void CheckPlayerControl(){
        if(player.IsSlingable() && player.gameObject.activeSelf && !isDoUltimate){
            
            if(Input.GetMouseButtonDown(0)){
                // if(EventSystem.current.IsPointerOverGameObject()) return;
                if(IsMouseOnArea(Input.mousePosition)){
                    downClickTime = Time.time;
                    startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                    startPoint.z = -4;
                }else{
                    downClickTime = Time.time;
                    startPoint = new Vector2( cam.transform.position.x, cam.transform.position.y-3.5f);
                    startPoint.z = -4;
                }
               
            }

            if(Input.GetMouseButton(0)){                
                if((Time.time - downClickTime) >= delayDeltaTime){
                    onSling = true;
                    onDrag = true;
                    sprite.transform.position = startPoint;
                    sprite.enabled = true;
                    TimeManager.Instance.EnterSlowmotion();
                    currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                    currentPoint.z = -4;
                    ShowLine(startPoint, currentPoint);
                    ShowTrajectory();
                }else{
                    onDrag = false;
                    onSling = false;

                }
            }

            if(Input.GetMouseButtonUp(0) && onSling){
                if((Time.time - downClickTime) >= delayDeltaTime){
                    rb.velocity = Vector2.zero;
                    endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                    endPoint.z = -4;
                    force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x,minPower.x,maxPower.x),
                    Mathf.Clamp(startPoint.y - endPoint.y,minPower.y,maxPower.y));
                    rb.AddForce(force * powerShoot, ForceMode2D.Impulse);
                    HideLine();
                    player.OnSlingshot();
                    TimeManager.Instance.ReleaseSlowmotion();
                    onSling = false;
                    onDrag = false;
                }
            }

        }
    }

    private void CheckPlayerUltimate(){
        if(EventSystem.current.IsPointerOverGameObject()) return;

        if(player != null && IsMouseOnArea(Input.mousePosition)){
            player.DoUltimate();
            isDoUltimate = true;
        }
    }

    private void CheckPlayerSkill(){
        if(EventSystem.current.IsPointerOverGameObject()) return;

        if(player != null && !onDrag && IsMouseOnArea(Input.mousePosition)){
            player.DoSkill();
        }
    }

    public void SetOffUltimate(){
        isDoUltimate = false;
    }

}
