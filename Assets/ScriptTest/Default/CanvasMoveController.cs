using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasMoveController : MonoBehaviour
{
    public Image slowmotionBar;
    public Image cancelSprite;
    public RectTransform directionTransform;
    public LayerMask layerHit;
    public LineRenderer trajectory;
    // public GameObject touchEffect;

    [Space]
    public float dashTime = 0.25f;
    public float dashMultiplier = 4f;
    public float stopZone = 100f;
    public float slowmotionTime = 3f;
    [Range(1, 3)]
    public int maxIterations = 3;

    private Party party;
    private Actor actor;
    private Touch touch;
    private Camera cam;
    private Rigidbody2D actorRb;
    private RectTransform controlTransform;

    private Vector2 minPower = new Vector2(-3, -3);
    private Vector2 maxPower = new Vector2(3, 3);
    private Vector2 force;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 currentPoint;
    private Vector3 initialPos;
    
    private bool isSlingValid = false;  
    private bool onSling = false;  
    private bool onSlowmo = false;  
    private bool isRegen = false;  
    private float downClickTime;
    private float delayDeltaTime = 0.05f;  
    private float currentSlowTime;

    private int count;

    // Start is called before the first frame update
    void Start()
    {
        party = Party.Instance;
        actor = party.GetLeader();
        party.partyOnActorChanged += RefreshParty;

        cam = Camera.main;
        actorRb = actor.GetComponentInParent<Rigidbody2D>();
        controlTransform = GetComponent<RectTransform>();

        initialPos = new Vector3(0, -370, 0);
        controlTransform.localPosition = initialPos;
        directionTransform.localRotation = Quaternion.Euler(0, 0, 0);
        directionTransform.gameObject.SetActive(false);

        currentSlowTime = slowmotionTime;
        slowmotionBar.fillAmount = 1;

        cancelSprite.enabled = false;
    }

    private void RefreshParty()
    {
        actor = party.GetLeader();
        if(actor != null)
        actorRb = actor.parent.GetComponent<Rigidbody2D>();
    }

    private bool IsMouseOnArea(Vector3 MousePosition){
        if(MousePosition.y > 200 && MousePosition.y < 1200){
            return true;
        }
        return false;
    }

    private bool IsSlingable(){
        // if(actor.currentSP >= actor.actionCost && isSlingValid) return true;
        if(isSlingValid) return true;
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
        if(actor != null && actor.parent.gameObject.activeSelf && !GameManager.Instance.isGamePaused){
            actorRb.velocity = actor.statAGI.GetValue() * (actorRb.velocity.normalized);

            if(actorRb.velocity.magnitude < 0.5f){
                if(actor.parent != null && !isRegen){
                    actorRb.velocity = Vector2.zero;
                    actor.StartRegen();
                    isRegen = true;
                }
            }
        }
        
        // if(actor != null && actor.parent.gameObject.activeSelf && !GameManager.Instance.isGamePaused){
        //     if(actor.currentSP > 1f){
        //         actorRb.velocity = actor.statAGI.GetValue() * (actorRb.velocity.normalized);
        //     }
        //     if(actorRb.velocity.magnitude > 0.5f){
        //         actor.ApplyAction(1f * Time.fixedDeltaTime);
        //     }else{
        //         if(actor.parent != null && !isRegen){
        //             actorRb.velocity = Vector2.zero;
        //             actor.StartRegen();
        //             isRegen = true;
        //         } 
        //     }
        // }
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
        // if(!EventSystem.current.IsPointerOverGameObject(touch.fingerId)){
        //     if (Input.touchCount > 0)
        //     {
        //         touch = Input.GetTouch(0);
        //         Instantiate(touchEffect, touch.position, Quaternion.identity);
        //     }
        // }
        
        if(actor != null && actor.parent.gameObject.activeSelf && !GameManager.Instance.isGamePaused){
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                switch (touch.phase) {
                    case TouchPhase.Began:
                        if(!EventSystem.current.IsPointerOverGameObject(touch.fingerId)){
                            downClickTime = Time.time;
                            startPoint = touch.position;
                            startPoint.z = -4;
                            
                            currentSlowTime = slowmotionTime;
                            slowmotionBar.fillAmount = 1;

                            isSlingValid = true;
                            isRegen = false;

                            actor.StopRegen();
                        }else{
                            isSlingValid = false;
                        }
                    break;

                    case TouchPhase.Moved:
                        if(IsSlingable()){
                            if((Time.time - downClickTime) >= delayDeltaTime){
                                onSlowmo = true;
                                onSling = true;

                                controlTransform.position = startPoint;
                                currentPoint = touch.position;
                                currentPoint.z = -4;

                                float distance = Vector2.Distance(startPoint, currentPoint);
                                if(distance < stopZone){
                                    cancelSprite.enabled = true;
                                    directionTransform.gameObject.SetActive(false);
                                }else{
                                    cancelSprite.enabled = false;
                                    directionTransform.gameObject.SetActive(true);
                                }
                                
                                Vector2 direction = (currentPoint - startPoint).normalized;
                                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                                
                                directionTransform.rotation = Quaternion.Euler(0, 0, angle + -90);
                                
                                count = 0;
                                trajectory.positionCount = 1;
                                trajectory.SetPosition(0, actor.parent.position);
                                DrawTrajectory(actor.parent.position, -direction);

                                TimeManager.Instance.EnterSlowmotion();
                            }
                        }else{
                            onSling = false;
                            onSlowmo = false;
                        }
                    break;

                    case TouchPhase.Ended:
                        EndPhase();
                    break;
                }
            }
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
        cancelSprite.enabled = false;
        controlTransform.localPosition = initialPos;
        directionTransform.localRotation = Quaternion.Euler(0, 0, 0);
        directionTransform.gameObject.SetActive(false);
        trajectory.positionCount = 0;
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

    private bool DrawTrajectory(Vector2 position, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction, 5f, layerHit);
        if (hit && count <= maxIterations - 1)
        {
            if(hit.collider.tag == "Enemy"){
                trajectory.startColor = Color.red;
                trajectory.endColor = new Color32(255,255, 255, 0);
                trajectory.positionCount = (count + 2);
                trajectory.SetPosition(count + 1, position + direction);
            }else{
                count++;
                var reflectAngle = Vector2.Reflect(direction, hit.normal);
                trajectory.startColor = Color.white;
                trajectory.endColor = new Color32(255,255, 255, 0);
                trajectory.positionCount = (count + 1);
                trajectory.SetPosition(count, hit.point);
                DrawTrajectory(hit.point + reflectAngle, reflectAngle);
            }
            return true;
        }else{
            trajectory.startColor = Color.red;
            trajectory.endColor = new Color(255,25,20, 0);
        }
        
        if(hit == false){
            trajectory.startColor = Color.white;
            trajectory.endColor = new Color32(255,255, 255, 0);
            trajectory.positionCount = (count + 2);
            trajectory.SetPosition(count + 1, position + (direction * 5f));
        }
        return false;
    }

    void DoMove(){
        // endPoint = cam.ScreenToWorldPoint(touch.position);
        endPoint = touch.position;
        endPoint.z = -4;
        
        // force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x,minPower.x,maxPower.x),
        // Mathf.Clamp(startPoint.y - endPoint.y,minPower.y,maxPower.y));
        force = (currentPoint - startPoint).normalized;

        float distance = Vector2.Distance(startPoint, currentPoint);
        if(distance > stopZone){
            actor.ActionMove(-force);
        }else{
            actorRb.velocity = Vector2.zero;
        }
        
        TimeManager.Instance.ReleaseSlowmotion();
        // HideLine();
    }

    void DoDash(){
        if(EventSystem.current.IsPointerOverGameObject()) return;
        if(onSling) return;

        if(actor != null){
            if(actorRb.velocity.magnitude > 0.5f)
            actor.ActionDash(dashTime, dashMultiplier);
        }
    }

    private void HideDustTrail()
    {
        foreach (Actor actor in party.actors)
        {
            if(actor != null)
            actor.parent.GetComponent<ParticleSystem>().Stop();
        }
    }

    private void ShowDustTrail()
    {
        foreach (Actor actor in party.actors)
        {
            if(actor != null)
            actor.parent.GetComponent<ParticleSystem>().Play();
        }
    }
}
