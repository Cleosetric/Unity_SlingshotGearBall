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

    Vector2 moveDir = Vector2.zero;

    private Actor actorLeader;
    private int currentIndex = 0;
    // private int oldLeaderIndex = 0;

    void Init()
    {
        Actor[] actorInParty = gameObject.GetComponentsInChildren<Actor>();
        if(actorInParty.Length != 0){
            path.Add(transform.position);
            foreach (Actor actor in actorInParty)
            {
                // actor.transform.position = spawnPoint.position;
                AddActor(actor);
            }
            actorLeader = actors[0];
            EnableLeaderHitBox();
        }
    }

    public Actor GetActiveActor(){
        return actors[currentIndex];
    }

    public Actor GetLeader(){
        return actors[0];
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
            if(i == 0){
                // actors[i].gameObject.GetComponent<CircleCollider2D>().enabled = true;
                actors[i].gameObject.GetComponent<CircleCollider2D>().radius = 0.2f;
                actors[i].gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
                // actors[i].gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            }else{
                // actors[i].gameObject.GetComponent<CircleCollider2D>().enabled = false;
                actors[i].gameObject.GetComponent<CircleCollider2D>().radius = 0.1f;
                actors[i].gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
                // actors[i].gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
    }

    public void SwapLeader(){
        // actors[0].transform.position = actors[actors.Count-1].transform.position;
        // actors[0].transform.position = actors[1].transform.position;

        IListExtension.Swap(actors,0,actors.Count-1);
        IListExtension.Swap(actors,0,1);
        
        for (int i = 0; i < actors.Count; i++)
        {
            if(actors[i].isAlive) actors[i].Init(i);
        }

        actorLeader = actors[0];
        EnableLeaderHitBox();

        if(partyOnActorChanged != null) partyOnActorChanged.Invoke();
    }

    public void NextActor(){
        if(currentIndex < (actors.Count - 1))
        {
            currentIndex ++;
        }   

        if(partyOnActorChanged != null) partyOnActorChanged.Invoke();
    }

    public void PrevActor(){
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

    void FixedUpdate()
    {
        if(actors.Count != 0){
            MoveLeader();
            MoveFollower();
        }
    }

    void CheckMemberAlive(){
        isPartyDefeated = !actors.Exists(x => x.isAlive == true);
        if(!isPartyDefeated){
            if(!GetLeader().isAlive){
                SwapLeader();
            }
        }else{
            Invoke("Restart",1f);
            Debug.Log("Game Over!");
        }
    }

    void Restart(){
        EnemyManager.Instance.RestartGame();
    }

    private void MoveLeader()
    {
        // Move the first minion (leader) towards the 'dir'
        // if(Input.GetKey(KeyCode.RightArrow)){
        //     moveDir = Vector2.right;
        // }else if(Input.GetKey(KeyCode.LeftArrow)){
        //     moveDir = Vector2.left;
        // }else if(Input.GetKey(KeyCode.DownArrow)){
        //     moveDir = Vector2.down;
        // }else if(Input.GetKey(KeyCode.UpArrow)){
        //     moveDir = Vector2.up;
        // }
        // actorLeader.transform.position += ((Vector3)moveDir) * 5f * Time.deltaTime;

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

    void MoveFollower()
    {
        float headDistUnit = leaderDistance / actorsDistance;

        for (int i = 1; i < actors.Count; i++)
        {
            Actor actor = actors[i];

            if(actor.isAlive){
                // Move minion on the path
                actor.MoveOnPath(path, headDistUnit);

                // Extra push to avoid minions stepping on each other
                Vector2 prevToNext = actors[i - 1].transform.position - actor.transform.position;

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