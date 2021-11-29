using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    #region Singleton
	private static Party _instance;
	public static Party Instance { get { return _instance; } }

	private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

       Init();
    }
	#endregion

    public delegate void PartyOnActorChanged();
    public PartyOnActorChanged partyOnActorChanged;

    [Header("Party Config")]
    public float actorsDistance = 1;
    public float leaderDistance = 0f;
    public Path path = new Path(1);
    public List<Actor> actors = new List<Actor>();
    public bool isPartyDefeated = false;

    private Vector2 lastVel;
    private Actor actorLeader;
    private int currentIndex = 0;

    public void Init()
    {
        isPartyDefeated = false;
        actors.RemoveAll(actor => actor == null);

        Actor[] actorInParty = gameObject.GetComponentsInChildren<Actor>();
        if(actorInParty.Length != 0){
            path.Add(transform.position);
            foreach (Actor actor in actorInParty)
            {
                // actor.transform.position = spawnPoint.position;
                AddActor(actor);
            }
            actorLeader = GetLeader();
        }
        Start();
    }

    public void Refresh(){
        if(actors.Count > 0){
            Actor currentLeader = GetLeader();
            actors.RemoveAt(0);
            actors.Add(currentLeader);
            
            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].Init(i);
                actors[i].MoveOnPath(path, 0f);
            }

            actorLeader = GetLeader();
            EnableLeaderHitBox();
        }

        if(partyOnActorChanged != null) partyOnActorChanged.Invoke();
    }

    private void Start() {
        EnableLeaderHitBox();
        transform.position = GetLeader().transform.position;
        if(partyOnActorChanged != null) partyOnActorChanged.Invoke();
    }

    public Actor GetActiveActor(){
        return actors[currentIndex];
    }

    public Actor GetLeader(){
        return actors[0];
    }

    public void SetLeader(){
        actorLeader = GetLeader();
    }

    void AddActor(Actor actor)
    {
        // Initialize a minion and give it an index (0,1,2) which is used as offset later on
        actor.Init(actors.Count);
        actors.Add(actor);
        actor.MoveOnPath(path, 0f);

        // Resize the capacity of the path if there are more minions in the snake than the path
        if (path.Capacity <= actors.Count) path.Resize();
    }

    void EnableLeaderHitBox(){
        for (int i = 0; i < actors.Count; i++)
        {
            if(actors[i] != null){
                if(actors[i].transform.gameObject.activeSelf){
                    if(i == 0){
                        // actors[i].gameObject.GetComponent<CircleCollider2D>().enabled = true;
                        // actors[i].gameObject.GetComponent<CircleCollider2D>().radius = 0.25f;
                        // actors[i].gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
                        actors[i].GetComponentInChildren<TargetIndicator>().ShowIndicator();
                        actors[i].directionFace.gameObject.SetActive(true);
                        actors[i].gameObject.GetComponent<Rigidbody2D>().velocity = lastVel;
                        // actors[i].gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    }else{
                        // actors[i].gameObject.GetComponent<CircleCollider2D>().enabled = false;
                        // actors[i].gameObject.GetComponent<CircleCollider2D>().radius = 0.25f;
                        // actors[i].gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
                        actors[i].GetComponentInChildren<TargetIndicator>().HideIndicator();
                        actors[i].directionFace.gameObject.SetActive(false);
                        if(actors[i].gameObject.activeSelf) actors[i].StartRegen();
                        // actors[i].gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    }
                }
            }
        }
    }

    public void SwapLeader(){
        //check if actor count is alive > 1
        //not only check count
        // if(GetLeader().transform == null || !GetLeader().transform.gameObject.activeSelf || !GetLeader().isAlive) return;
        if(GameManager.Instance.isGamePaused && !GetLeader().gameObject.activeSelf) return;
        if(GameManager.Instance.isGamePaused) return;
        
        int aliveMemberCount = 0;
        foreach (Actor actor in actors)
        {
            if(actor.isAlive) aliveMemberCount++;
        }

        if(aliveMemberCount == 1 && GetLeader().isAlive) return;

        if(SkillUI.Instance.CheckAbilityIsAllReady()){
            if(GetLeader().isAlive) lastVel = GetLeader().GetComponentInParent<Rigidbody2D>().velocity;

            // actors.RemoveAll(actor => actor.isAlive == false);
            Actor currentLeader = GetLeader();
            actors.RemoveAt(0);
            actors.Add(currentLeader);

            // List<Actor> actorlist = actors;
            // actorlist.Sort((a1,a2)=>a1.isAlive.CompareTo(a2.isAlive));
            // actors = actorlist;

            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].Init(i);
                actors[i].MoveOnPath(path, 0f);
                if(i == 0)
                actors[i].GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
            }
            if (path.Capacity <= actors.Count) path.Resize();

            actorLeader = GetLeader();
            EnableLeaderHitBox();
            if(partyOnActorChanged != null) partyOnActorChanged.Invoke();
        }

    }

    public void SetActor(int index){
        currentIndex = index;
        if(partyOnActorChanged != null) partyOnActorChanged.Invoke();
    }

    public void NextActor(){
        SoundManager.Instance.Play("ButtonClick");
        if(currentIndex < (actors.Count - 1))
        {
            currentIndex ++;
        }   

        if(partyOnActorChanged != null) partyOnActorChanged.Invoke();
    }

    public void PrevActor(){
        SoundManager.Instance.Play("ButtonClick");
        if(currentIndex > 0)
        {
            currentIndex --;
        }   

        if(partyOnActorChanged != null) partyOnActorChanged.Invoke();
    }

    public void ActorGainExp(int value){
        GetActiveActor().GainExp(value);
        if(partyOnActorChanged != null) partyOnActorChanged.Invoke();
    }

    private void Update() {
        if(!isPartyDefeated){
            CheckMemberAlive();
        }
    }

    private void FixedUpdate()
    {
        if(actors.Count != 0){
            MoveLeader();
            MoveFollower();
        }
    }

    private void CheckMemberAlive(){
        List<Actor> partyMemberList = new List<Actor>();
        partyMemberList.AddRange(actors.ToArray());
        isPartyDefeated = !partyMemberList.Exists(x => x.isAlive == true);
        if(!isPartyDefeated){
            // if(GetLeader() == null && GetLeader().transform == null && !GetLeader().isAlive){
            if(!GetLeader().isAlive){
                SwapLeader();
            }
        }else{
            GameOver();
        }
    }

    private void GameOver(){
        Invoke("Restart",3f);
        Debug.Log("Game Over!");
    }

    private void Restart(){
        GameManager.Instance.GameOver();
    }

    private void MoveLeader()
    {
        if(actorLeader != null){
            // Measure the distance between the leader and the 'head' of that path
            Vector2 headToLeader = ((Vector2)actorLeader.transform.position) - path.Head();

            // Cache the precise distance so we can reuse it when we offset each minion
            leaderDistance = headToLeader.magnitude;

            // When the distance between the leader and the 'head' of the path hits the threshold, spawn a new point in the path
            if (leaderDistance >= actorsDistance)
            {
                // In case leader overshot, let's make sure all points are spaced exactly with 'RADIUS'
                float leaderOvershoot = leaderDistance - actorsDistance;
                Vector2 pushDir = headToLeader.normalized * leaderOvershoot;

                path.Add(((Vector2)actorLeader.transform.position) - pushDir);

                // Update head distance as there is a new point we have to measure from now
                leaderDistance = (((Vector2)actorLeader.transform.position) - path.Head()).sqrMagnitude;
            }
        }
    }

    private void MoveFollower()
    {
        float headDistUnit = leaderDistance / actorsDistance;

        for (int i = 1; i < actors.Count; i++)
        {
            Actor actor = actors[i];

            if(actor != null){
                // Move minion on the path
                actor.MoveOnPath(path, headDistUnit);

                if(actor.transform != null && actors.ToArray()[i - 1].transform != null){
                    // Extra push to avoid minions stepping on each other
                    Vector2 prevToNext = actors.ToArray()[i - 1].transform.position - actor.transform.position;

                    float distance = prevToNext.magnitude;
                    if (distance < actorsDistance)
                    {
                        float intersection = actorsDistance - distance;
                        actor.Push(-prevToNext.normalized * actorsDistance * intersection);
                    }
                }
            }
        }
    }

    public void SwapLeaderButton(){
        if(!GameManager.Instance.isGamePaused || !SkillUI.Instance.CheckAbilityIsAllReady()){
            SoundManager.Instance.Play("ButtonCancel");
        }else{
            SoundManager.Instance.Play("ButtonClick");
        }
        SwapLeader();
    }

    private void OnDrawGizmos() {
        if(actors.Count > 0){
            if( path != null && path.Points.Length > 1){
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(GetLeader().transform.position, 0.35f);

                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(path.Head(), 0.35f);

                foreach (Vector2 point in path.Points)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(point, 0.25f);
                }

                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(path.Tail(), 0.35f);
            }
        }
    }
}
