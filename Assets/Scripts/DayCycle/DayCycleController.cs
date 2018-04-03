﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(DayCycleView))]
public class DayCycleController : TurnChangeListener {
    private bool changeLightIntensity;
    [SyncVar]
    public int cycleLengthIndex;
    [SyncVar]
    public int cycleIndex;
    public GameObject[] cycles;
    public Light worldLight;
    public float lightChangeSpeed;

    void Start () {
        changeLightIntensity = true;
	}

    [Server]
    public override void NextTurn()
    {
        IncrementCycleIndex();
        Rpc_NextCycle();
        Debug.Log("Received turn");
    }

    [Server]
    private void IncrementCycleIndex()
    {
        if(++cycleLengthIndex == LocalDataManager.GetPlayersGameObjects().Length)
        {
            cycleLengthIndex = 0;

            if (++cycleIndex > cycles.Length - 1)
            {
                cycleIndex = 0;
            }
        }
    }

    [ClientRpc]
    private void Rpc_NextCycle()
    {
        GameObject cycle = cycles[cycleIndex];
        changeLightIntensity = true;
        PlayCycleSound(cycle);
        if (cycleIndex == 0)
            DisplayNewDayPopOut();
    }

    private void PlayCycleSound(GameObject cycle)
    {
        AudioClip cycleBeginClip = cycle.GetComponent<DayCycle>().cycleBeginSound;

        if (cycleBeginClip != null)
        {
            GetComponent<AudioSource>().clip = cycleBeginClip;
            GetComponent<AudioSource>().Play();
        }
    }

    private void DisplayNewDayPopOut()
    {
        GetComponent<DayCycleView>().DisplayNewDayPopOut();
    }

    void Update () {
        if(cycleIndex != -1)
        {
            if (HasCycleChanged())
            {
                ChangeLightIntensityInTime();
            }
        }
	}

    private bool HasCycleChanged()
    {
        return changeLightIntensity;
    }

    private void ChangeLightIntensityInTime()
    {
        float lightIntensity = cycles[cycleIndex].GetComponent<DayCycle>().lightIntensity;
        if (worldLight.intensity < lightIntensity)
        {
            worldLight.intensity += lightChangeSpeed * Time.deltaTime;

            if (worldLight.intensity >= lightIntensity)
                changeLightIntensity = false;
        }
        else
        {
            worldLight.intensity -= lightChangeSpeed * Time.deltaTime;

            if (worldLight.intensity <= lightIntensity)
                changeLightIntensity = false;
        }
    }
}
