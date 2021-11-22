﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Logic : MonoBehaviour
{
    // enum for NPC type.
    public enum NPC_Type { normal, infected, dying, zombie };

    //give a heartRate for all NPCS
    public int[] heartRate = new int[5];

    // The following fields are set in the editor, and should be modified through getter and setter
    // infectionPhase can be 1 or 2
    // 1 - NPC1, 2, 3 are all at phase 1. Phase 1 can be cured.
    // 2 - NPC4 (zombie) Phase 2 cannot be cured. Can only be killed.
    [SerializeField] private int infectionPhase;
    [SerializeField] private NPC_Type type;


    /* Logics to be implemented, while implementing, may introduce new variables and functions 
        Some logics might be better to implement in room, noted with (*)
     */
    // 1. All NPC(except from zombie) has an integer indicating their HP, normal NPC will have more lives than the others, vice versa.
    // will -1 for those who didn't get food
    [SerializeField] private int life;
    // 2. food is allocated automatically by the game.
    // 3. All NPC (except from zombie) will die if their HP <= 0
    // 6. normal NPC can turn into infected NPC if stayed in an infected room for x, say 40 seconds
    //private float NormalToInfectedTimer = 40.0f;
    //private float NormalToInfectedTime = 40.0f;
    private float NormalToInfectedTimer = 10.0f;
    private float NormalToInfectedTime = 10.0f;
    public bool infectedByRoom = false;
    // 6.1. NPC1 can be applied with vaccine, then it will never turn into NPC2
    public bool isVaccinated = false;


    // 7. infected NPC can turn into dying NPC if not treated with serum(血清) after 1 minute (implicit timer)
    private float InfectedToDyingTimer = 40f;
    // 8. (*) infected NPC will contaminate a dorm immediately
    // 9. dying NPC has a explicit timer (progress bar) shown on top of head, if not treated with serum, will turn into zombie after 30 seconds
    private float DyingToZombieTimer = 30f;
    private float DyingToZombieTime = 30f;
    private Progress_bar zombieProgress;


    // the NPC_Movement reference of this game object
    private NPC_Movement npcMovement;



    // Start is called before the first frame update
    void Start()
    {
        if(transform.Find("Timer_UI_NPC") != null)
        {
            zombieProgress = transform.Find("Timer_UI_NPC").gameObject.GetComponent<Progress_bar>();
        }

        GenerateHeartRate();

        npcMovement = gameObject.GetComponent<NPC_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        NormalToInfected();
        InfectedToDying();
        DyingToZombie();
        TimerUIDisplay();
        CheckLife();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // other npc touch zombie, they die
        if(this.type != NPC_Type.zombie && collision.gameObject.tag.Equals("Zombie"))
        {
            Die();            
        }
    }


    // ---------- some state transition function -------------
    public void Die()
    {
        // disable collider and stuff and disable rigid body
        // play dead animation
        // destry object
        npcMovement.FreezePosAndDisableCol();
        npcMovement.isDying = true;
        Destroy(gameObject, 2);
    }


    private void CheckLife()
    {
        if(life <= 0)
        {
            Die();
        }
    }


    private void NormalToInfected()
    {
        if (infectedByRoom && type == NPC_Type.normal)
        {
            // timer start count down
            if(NormalToInfectedTimer >= 0)
            {
                NormalToInfectedTimer -= Time.deltaTime;
                return;
            }

            // when time's up, set type to infected.
            type = NPC_Type.infected;
        }
        else
        {
            NormalToInfectedTimer = NormalToInfectedTime;
            infectedByRoom = false;
        }
    }

    private void InfectedToDying()
    {
        // logic is the same as previous
        if (type == NPC_Type.infected)
        {
            // start the timer
            if(InfectedToDyingTimer >= 0)
            {
                InfectedToDyingTimer -= Time.deltaTime;
                return;
            }

            // when time's up, turn into dying type:
            type = NPC_Type.dying;
        }
    }

    private void DyingToZombie()
    {
        if(type == NPC_Type.dying)
        {
            if(DyingToZombieTimer >= 0)
            {
                DyingToZombieTimer -= Time.deltaTime;
                // also set the max and current for progress bar
                zombieProgress.current = DyingToZombieTime - DyingToZombieTimer;
                return;
            }

            // set animation transition
            npcMovement.FreezePosAndDisableCol();
            npcMovement.isBecomingZombie = true;
        }
    }

    public void CureBySerum()
    {
        // only can cure if infection phase is 1, might check this attribute in other places.
        // destroy the current NPC and instantiate a normal NPC. 
        // set type to normal
        // reset timers
        // reset life
    }

    public void Vaccinate()
    {
        // might check NPC type. can only give vaccine to normal NPC (npc1)
        // might no need to check type since type is checked elsewhere (e.g. when applying the vaccine)
        isVaccinated = true;
    }

    // ------------ Other helpers -----------
    private void TimerUIDisplay()
    {
        if(zombieProgress == null)
        {
            return;
        }

        if(type== NPC_Type.dying)
        {
            zombieProgress.gameObject.SetActive(true);
        }
        else
        {
            zombieProgress.gameObject.SetActive(false);
        }
    }


    // ----------- Getters and Setters --------------
    public void SetInfectionPhase(int i) 
    {
        infectionPhase = i;
    }

    public int GetInfectinPhase()
    {
        return infectionPhase;
    }

    public void SetNPCType(NPC_Type t)
    {
        type = t;
    }

    public NPC_Type GetNPCType()
    {
        return type;
    }

    public void DeductLife()
    {
        life--;
    }

    public int GetLife()
    {
        return life;
    }

    public void SetLife(int l)
    {
        life = l;
    }

    private void GenerateHeartRate()
    {



        int[] heartRateNormal = {Random.Range(70, 111), Random.Range(70, 111), Random.Range(70, 111),
                                Random.Range(70, 111), Random.Range(70, 111)};
        int[] heartRateInfected = {Random.Range(80, 121), Random.Range(80, 121), Random.Range(80, 121),
                                Random.Range(80, 121), Random.Range(111, 121)};
        int[] heartRateDying = {Random.Range(121, 181), Random.Range(121, 181), Random.Range(121, 181),
                                Random.Range(121, 181), Random.Range(121, 181)};
        int[] heartRateZombie = {Random.Range(200, 301), Random.Range(200, 301), Random.Range(200, 301),
                                 Random.Range(200, 301), Random.Range(200, 301)};


        switch (GetNPCType())
        {
            case NPC_Type.normal:

                heartRate = heartRateNormal;
                break;
            case NPC_Type.infected:

                heartRate = heartRateInfected;
                break;
            case NPC_Type.dying:

                heartRate = heartRateDying;
                break;
            case NPC_Type.zombie:

                heartRate = heartRateZombie;
                break;
        }


    }
}
