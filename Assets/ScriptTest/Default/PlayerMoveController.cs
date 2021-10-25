using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMoveController : MonoBehaviour
{
    public float slowmotionTime = 3f;
    public Image slowmotionBar;
    public GameObject cancelSprite;
    [Space]
    [SerializeField] private GameObject trajectoryPrefab;
    [SerializeField] private LayerMask layerHit;
    [Space]
    [Range(1, 3)]
    [SerializeField] private int maxControlDrag = 3;
    [Range(1, 3)]
    [SerializeField] private int maxIterations = 3;

    Party party;
    Actor actor;

    private SpriteRenderer sprite;
    private Touch touch;
    private Camera cam;
    private LineRenderer lr;
    private LineRenderer trajectory;

    private Vector2 minPower = new Vector2(-3, -3);
    private Vector2 maxPower = new Vector2(3, 3);
    private Vector2 force;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 currentPoint;
    private Vector3 initialPos;
    private Rigidbody2D actorRb;

    private bool isSlingValid = false;  
    private bool onSling = false;  
    private bool onSlowmo = false;  
    private bool isRegen = false;  
    private float downClickTime;
    private float delayDeltaTime = 0.05f;  
    private float currentSlowTime;
    private int count;

    private void Awake() {
        #if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
        #else
            Debug.unityLogger.logEnabled=false;
        #endif
    }

    // Start is called before the first frame update
    void Start()
    {
        party = Party.Instance;
        actor = party.GetLeader();
        party.partyOnActorChanged += RefreshParty;

        cam = Camera.main;
        sprite = GetComponent<SpriteRenderer>();
        lr = GetComponent<LineRenderer>();
        trajectory = trajectoryPrefab.GetComponent<LineRenderer>();
        actorRb = actor.GetComponentInParent<Rigidbody2D>();

        currentSlowTime = slowmotionTime;
        slowmotionBar.fillAmount = 1;
        initialPos = transform.position;
        cancelSprite.SetActive(false);
        // sprite.enabled = false;
    }

    private void RefreshParty()
    {
        actor = party.GetLeader();
        actorRb = actor.GetComponentInParent<Rigidbody2D>();
    }

    private bool IsMouseOnArea(Vector3 MousePosition){
        if(MousePosition.y > 200 && MousePosition.y < 1200){
            return true;
        }
        return false;
    }

    private bool IsSlingable(){
        if(actor.currentSP >= actor.actionCost) return true;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTouchControl();
        CheckPlayerMovement();
        CountdownSlomotion();
    }

    private void FixedUpdate() {
        if(actor.currentSP > 1f){
            actorRb.velocity = actor.statAGI.GetValue() * (actorRb.velocity.normalized);
        }
        if(actorRb.velocity.magnitude > 0.5f){
            actor.ApplyAction(1f * Time.fixedDeltaTime);
        }else{
            if(actor.gameObject.activeSelf && !isRegen){
                actorRb.velocity = Vector2.zero;
                actor.StartRegen();
                isRegen = true;
            } 
        }
    }

    private void CheckPlayerMovement()
    {
        if(actor != null && actorRb.velocity.magnitude >= 1.5f){
            ShowDustTrail();
        }else{
            HideDustTrail();
        }
    }

    private void CheckTouchControl()
    {
        if (Input.touchCount > 0 && actor.gameObject.activeSelf && actor.isAlive)
        {
            touch = Input.GetTouch(0);
            // if(!EventSystem.current.IsPointerOverGameObject(touch.fingerId)){
                switch (touch.phase) {
                case TouchPhase.Began:
                    if(!EventSystem.current.IsPointerOverGameObject(touch.fingerId)){
                        if(IsMouseOnArea(touch.position)){
                            downClickTime = Time.time;
                            startPoint = cam.ScreenToWorldPoint(touch.position);
                            startPoint.z = -4;
                        }else{
                            downClickTime = Time.time;
                            startPoint = new Vector2( cam.transform.position.x, cam.transform.position.y-3.5f);
                            startPoint.z = -4;
                        }
                        currentSlowTime = slowmotionTime;
                        slowmotionBar.fillAmount = 1;
                        isSlingValid = true;
                        onSlowmo = true;
                        isRegen = false;
                        actor.StopRegen();
                    }else{
                        isSlingValid = false;
                    }
                break;

                case TouchPhase.Moved:
                    if(isSlingValid){
                        if(IsSlingable()){
                            if((Time.time - downClickTime) >= delayDeltaTime){
                                onSling = true;
                                sprite.transform.position = startPoint;
                                sprite.enabled = true;
                                currentPoint = cam.ScreenToWorldPoint(touch.position);
                                currentPoint.z = -4;
                                ShowLine(startPoint,currentPoint);
                                ShowTrajectory(startPoint, currentPoint);
                                TimeManager.Instance.EnterSlowmotion();
                            }else{
                                onSlowmo = false;
                            }
                        }else{
                            onSling = false;
                            onSlowmo = false;
                        }
                    }else{
                        onSlowmo = false;
                    }
                break;

                case TouchPhase.Ended:
                    EndPhase();
                break;
                }
            // }
        }
    }

    void EndPhase(){
        if(isSlingValid){
            if(onSling){
                DoMove();
            }else{
                DoDash();
            }
        }
        onSlowmo = false;
        isSlingValid = false;
        onSling = false;

        slowmotionBar.fillAmount = 1;
        cancelSprite.SetActive(false);
    }

    void CountdownSlomotion(){
        if(onSlowmo){
            currentSlowTime -= 0.5f * Time.unscaledDeltaTime;
            float slowTime = currentSlowTime / slowmotionTime;
            slowmotionBar.fillAmount = slowTime;
            if(slowTime <= 0f){
                TimeManager.Instance.ReleaseSlowmotion();
                EndPhase();
                onSlowmo = false;
            }
        }
    }

    void DoMove(){
        endPoint = cam.ScreenToWorldPoint(touch.position);
        endPoint.z = -4;
        
        force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x,minPower.x,maxPower.x),
        Mathf.Clamp(startPoint.y - endPoint.y,minPower.y,maxPower.y));
        bool isForceWeak = force.magnitude > 0.5f ? false : true;

        if(!isForceWeak){
            actor.ActionMove(force);
        }else{
            actorRb.velocity = Vector2.zero;
        }
        
        TimeManager.Instance.ReleaseSlowmotion();
        HideLine();
    }

    void DoDash(){
        if(EventSystem.current.IsPointerOverGameObject()) return;

        if(actor != null && IsSlingable()){
            actor.ActionDash();
        }
    }

    void ShowLine(Vector3 startPoint, Vector3 endPoint){
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.numCapVertices = 10;

        Vector3 [] points = new Vector3[2];
        points[0] = startPoint;
        points[1] = Vector3.MoveTowards(startPoint, endPoint, maxControlDrag);
        
        lr.SetPositions(points);
        float distance = Vector2.Distance(startPoint, endPoint);
        float fade =  distance / 100 * 50;
        if(distance < 1){
            cancelSprite.SetActive(true);
        }else{
            cancelSprite.SetActive(false);
        }
        lr.startColor = new Color(1.0F, fade + 1 - (fade * 2), 0.0F, 1.0F);
        lr.endColor = new Color(1.0F, fade + 1 - (fade * 2), 0.0F, 0.0F);
    }

     void HideLine(){
        lr.positionCount = 0;
        trajectory.positionCount = 0;
        transform.position = initialPos;
        // sprite.enabled = false;
    }

    void ShowTrajectory(Vector2 startPoint, Vector2 endPoint){
        count = 0;
        Vector3 playerPos = actor.transform.position;
        Vector3 direction = (startPoint - endPoint).normalized;

        trajectory.positionCount = 1;
		trajectory.SetPosition(0, playerPos);
        DrawTrajectory(playerPos, direction);
    }

   private bool DrawTrajectory(Vector2 position, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction, 5f, layerHit);
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
                trajectory.startColor = Color.white;
                trajectory.endColor = new Color(16,207,255, 0);
                trajectory.positionCount = (count + 1);
                trajectory.SetPosition(count, hit.point);
                DrawTrajectory(hit.point + reflectAngle, reflectAngle);
            }
            return true;
        }
        
        if(hit == false){
            trajectory.startColor = Color.white;
            trajectory.endColor = new Color(16,207,255, 0);
            trajectory.positionCount = (count + 2);
            trajectory.SetPosition(count + 1, position + (direction * 5f));
        }
        return false;
    }

    private void HideDustTrail()
    {
        foreach (Actor actor in party.actors)
        {
           actor.parent.GetComponent<ParticleSystem>().Stop();
        }
    }

    private void ShowDustTrail()
    {
        foreach (Actor actor in party.actors)
        {
           actor.parent.GetComponent<ParticleSystem>().Play();
        }
    }
}
