using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // reference to NPC prefabs
    [SerializeField] private GameObject[] NPC_prefabs;
    // reference to NPC parent class
    [SerializeField] Transform NPCParent;

    // list of all npcs outside the room.
    [SerializeField] private List<NPC_Movement> npcMovements = new List<NPC_Movement>();

    // get the waiting point location
    private Vector3 waitingPointPos;
    // waiting distance, randomly assigned between these two numbers
    private float waitingDistance = 3;

    // consider putting them as serialize field if needed.
    private int maxWaitingNPC = 1;
    private float intervalMin = 10;
    private float intervalMax = 20;
    private float spawnIntervalTimer = 0;


    private void Start()
    {
        waitingPointPos = transform.GetChild(0).position;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnControl();
        RemoveNPCList();
        WaitingDistanceControl();
    }

    private void SpawnControl()
    {
        // stop spawing if there're enough NPC waiting in line already
        if(npcMovements.Count >= maxWaitingNPC)
        {
            return;
        }
        
        // when it's able to spawn, only spawn when timer is over
        if(spawnIntervalTimer >= 0)
        {
            spawnIntervalTimer -= Time.deltaTime;
            return;
        }

        // now that timer is over, spawn a single npc and reset the timer
        SpawnSingleNPC();
        spawnIntervalTimer = Random.Range(intervalMin, intervalMax);
        
    }

    private void SpawnSingleNPC()
    {
        // first choose one from the NPC_prefabs
        GameObject npc = Instantiate(NPC_prefabs[Random.Range(0, NPC_prefabs.Length)], 
            transform.position, Quaternion.identity, NPCParent);
        // then randomly select its type and set attributes (may add some weight to npc 1 and 2)
        NPC_Logic npcLogic = npc.GetComponent<NPC_Logic>();
        switch (Random.Range(0, 3))
        {
            // normal NPC
            case 0:
                npcLogic.SetNPCType(NPC_Logic.NPC_Type.normal);
                npcLogic.SetLife(4);
                npcLogic.SetInfectionPhase(1);
                break;

            // infected NPC
            case 1:
                npcLogic.SetNPCType(NPC_Logic.NPC_Type.infected);
                npcLogic.SetLife(2);
                npcLogic.SetInfectionPhase(1);
                break;

            // dying NPC
            case 2:
                npcLogic.SetNPCType(NPC_Logic.NPC_Type.dying);
                npcLogic.SetLife(1);
                npcLogic.SetInfectionPhase(1);
                break;
        }

        // Add this NPC movement to the list
        npcMovements.Add(npc.GetComponent<NPC_Movement>());
    }

    // remove the NPCs that are already in room.
    private void RemoveNPCList()
    {
        foreach(NPC_Movement m in npcMovements)
        {
            if (m.isInRoom || m == null)
            {
                npcMovements.Remove(m);
                break;
            }
        }
    }

    // make the npc wait in line outside the room
    private void WaitingDistanceControl()
    {

        for(int i = 0; i < npcMovements.Count; ++i)
        {
            if(i == 0)
            {
                // the first one needs special treatment
                if (npcMovements[i].transform.position.x < waitingPointPos.x)
                {
                    npcMovements[i].isWaitingInLine = false;
                }
                else
                {
                    npcMovements[i].isWaitingInLine = true;
                }
                continue;
            }


            // if the previous NPC is not in room,
            // apply the distance from stop point times the number of previous npcs(based on index number)
            if (!npcMovements[i - 1].isInRoom)
            {
                float distance = waitingDistance * i;
                if(npcMovements[i].transform.position.x < (waitingPointPos.x - distance))
                {
                    npcMovements[i].isWaitingInLine = false;
                }
                else
                {
                    npcMovements[i].isWaitingInLine = true;
                }
            }

        }
    }
}
