using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMoveController : MonoBehaviour
{
    [Space]
    [Range(1, 3)]
    [SerializeField] private int maxControlDrag = 3;
    // [Range(1, 3)]
    // [SerializeField] private int maxIterations = 3;

    Party party;
    Actor actor;

    private SpriteRenderer sprite;
    private Touch touch;
    private Camera cam;
    private LineRenderer lr;

    private Vector2 minPower = new Vector2(-3, -3);
    private Vector2 maxPower = new Vector2(3, 3);
    private Vector2 force;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 currentPoint;
    private Vector3 initialPos;

    private bool onSling = false;  
    private float downClickTime;
    private float delayDeltaTime = 0.175f;  

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
        initialPos = transform.position;
        // sprite.enabled = false;
    }

    private void RefreshParty()
    {
        actor = party.GetLeader();
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
    }

    private void CheckPlayerMovement()
    {
        if(actor.GetComponent<Rigidbody2D>().velocity.magnitude >= 1.5f){
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
            if(!EventSystem.current.IsPointerOverGameObject(touch.fingerId)){
                switch (touch.phase) {
                case TouchPhase.Began:
                    if(IsMouseOnArea(touch.position)){
                        downClickTime = Time.time;
                        startPoint = cam.ScreenToWorldPoint(touch.position);
                        startPoint.z = -4;
                    }else{
                        downClickTime = Time.time;
                        startPoint = new Vector2( cam.transform.position.x, cam.transform.position.y-3.5f);
                        startPoint.z = -4;
                    }
                    break;

                case TouchPhase.Moved:
                    if(IsSlingable()){
                        if((Time.time - downClickTime) >= delayDeltaTime){
                            onSling = true;
                            sprite.transform.position = startPoint;
                            sprite.enabled = true;
                            TimeManager.Instance.EnterSlowmotion();
                            currentPoint = cam.ScreenToWorldPoint(touch.position);
                            currentPoint.z = -4;
                            ShowLine(startPoint, currentPoint);
                        }
                    }else{
                        onSling = false;
                    }
                    break;

                case TouchPhase.Ended:
                    if(onSling){
                        DoMove();
                    }else{
                        DoDash();
                    }
                    break;
                }
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
            actor.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        
        TimeManager.Instance.ReleaseSlowmotion();
        HideLine();
        onSling = false;
    }

    void DoDash(){
        if(actor != null){
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
        float fade = Vector2.Distance(startPoint, endPoint) / 100 * 50;
        lr.startColor = new Color(1.0F, fade + 1 - (fade * 2), 0.0F, 1.0F);
        lr.endColor = new Color(1.0F, fade + 1 - (fade * 2), 0.0F, 0.0F);
    }

     void HideLine(){
        lr.positionCount = 0;
        transform.position = initialPos;
        // sprite.enabled = false;
    }

    private void HideDustTrail()
    {
        foreach (Actor actor in party.actors)
        {
           actor.GetComponentInChildren<ParticleSystem>().Stop();
        }
    }

    private void ShowDustTrail()
    {
        foreach (Actor actor in party.actors)
        {
           actor.GetComponentInChildren<ParticleSystem>().Play();
        }
    }
}
