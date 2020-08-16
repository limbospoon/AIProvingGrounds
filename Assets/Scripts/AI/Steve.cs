using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steve : MonoBehaviour
{
    public SteveStats steveStats;

    [Header("Stats Settings")]
    public float hungerDecayAmount;
    public float thirstDecayAmount;
    public float hungerDecayRate;
    public float thirstDecayRate;

    private float health;
    private float hunger;
    private float thirst;

    private float currentHunger;
    private float currentThirst;
    private SteveAI steveAI;
    private bool turnOnAI = false;


    private void Awake()
    {
        health = steveStats.health;
        hunger = steveStats.hunger;
        thirst = steveStats.thirst;

        currentHunger = hunger;
        currentThirst = thirst;
        steveAI = GetComponent<SteveAI>();
    }

    private void Start()
    {
        StartCoroutine(UpdateStat(currentHunger, hungerDecayAmount, hungerDecayRate, (i) => { currentHunger = i; }));
        StartCoroutine(UpdateStat(currentThirst, thirstDecayAmount, thirstDecayRate, (i) => { currentThirst = i; }));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            turnOnAI = !turnOnAI;

        if (turnOnAI)
            steveAI.UpdateAI();
    }

    IEnumerator UpdateStat(float stat, float decay, float rate, Action<float> callback)
    {
        while(true)
        {
            stat -= decay;
            callback(stat);
            yield return new WaitForSeconds(rate);
        }
    }
}
