﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Logic : MonoBehaviour
{
    // enum for NPC type.
    public enum NPC_Type { normal, infected, dying, zombie };


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
    // 4. (*) Dorm can have at most x people
    // 5. (*) observation and ICU can have at most 1 people inside
    // 6. normal NPC can turn into infected NPC if stayed in an infected room for x, say 40 seconds
    private float NormalToInfectedTimer = 40.0f;
    // 6.1. NPC1 can be applied with vaccine, then it will never turn into NPC2
    private bool isVaccinated = false;
    // 7. infected NPC can turn into dying NPC if not treated with serum(血清) after 1 minute (implicit timer)
    private float InfectedToDyingTimer = 60f;
    // 8. (*) infected NPC will contaminate a dorm immediately
    // 9. dying NPC has a explicit timer (progress bar) shown on top of head, if not treated with serum, will turn into zombie after 30 seconds
    private float DyingToZombieTimer = 30f;
    // will add some image and UI component here. 这个我可以来做，以前做过。
    // 10. (*) dying NPC will contaminate a dorm immediately
    // 11. zombies cannot be dragged (already done, simply remove the Drag_And_Drop.cs on zombie prefab)
    // 12. zombies can only be killed by special agents.
    // 13. zombies can kill other NPC on contact (can check the collision tag).
    // 14. (*) in observation room, if it has a NPC inside, display their heart rate on UI differently according to type
    //  if type 1, heart rate stays in the range of 70-90, with several outliers.
    //  if type 2, heart rate stays in the range of 70-90, but with a lot outliers.
    //  if type 3, heart rate is constantly 120
    //  if type 4, heart rate is 0.
    // 15. might keep a reference to what room this npc is currently in. 






    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // ---------- some state transition function -------------
    public void Die()
    {
        // play dead animation
        // destry object
    }

    private void NormalToInfected()
    {
        // if the room this NPC stays in is infected, start the timer-=Time.deltaTime
        // when timer <= 0, destroy this NPC and instantiate a new infected NPC at this location
        // also set the life to be the same as the destroyed normal NPC if new NPC's initial life > old NPC life.
        // might also need to set other attributes. 
    }

    private void InfectedToDying()
    {
        // logic is the same as previous
    }

    private void DyingToZombie()
    {
        // add logic here
        infectionPhase = 2;
    }

    public void CureBySerum()
    {
        // only can cure if infection phase is 1, might check this attribute in other places.
        // destroy the current NPC and instantiate a normal NPC. 
        // might carry over the attributes
    }

    public void Vaccinate()
    {
        // might check NPC type. can only give vaccine to normal NPC (npc1)
        // might no need to check type since type is checked elsewhere (e.g. when applying the vaccine)
        isVaccinated = true;
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
}
