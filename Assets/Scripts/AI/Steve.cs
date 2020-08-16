using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steve : MonoBehaviour
{
    public SteveStats steveStats;

    public float hungerDecayAmount;
    public float thirstDecayAmount;
    public float hungerDecayRate;
    public float thirstDecayRate;

    private float health;
    private float hunger;
    private float thirst;

    public float currentHunger;
    public float currentThirst;

    private void Awake()
    {
        health = steveStats.health;
        hunger = steveStats.hunger;
        thirst = steveStats.thirst;

        currentHunger = hunger;
        currentThirst = thirst;
    }

    private void Start()
    {
        StartCoroutine(UpdateStat(currentHunger, hungerDecayAmount, hungerDecayRate, (i) => { currentHunger = i; }));
        StartCoroutine(UpdateStat(currentThirst, thirstDecayAmount, thirstDecayRate, (i) => { currentThirst = i; }));
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
