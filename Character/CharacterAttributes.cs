using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterAttributes
{
    [Header("Max Values")]
    [SerializeField] private float _MaxStrength = 1;
    [SerializeField] private float _MaxDexterity = 1;
    [SerializeField] private float _MaxVitality = 1;
    [SerializeField] private float _MaxIntelligence = 1;
    [SerializeField] private float _MaxCharisma = 1;

    [Header("Current Values")]
    [SerializeField, Range(0, 1)] private float _CurrentStrength = 0.1f;
    public float CurrentStrength { get => _CurrentStrength; }
    [SerializeField, Range(0, 1)] private float _CurrentDexterity = 0.1f;
    public float CurrentDexterity { get => _CurrentDexterity; }
    [SerializeField, Range(0, 1)] private float _CurrentVitality = 0.1f;
    public float CurrentVitality { get => _CurrentDexterity; } 
    [SerializeField, Range(0, 1)] private float _CurrentIntelligence = 0.1f;
    public float CurrentIntelligence { get => _CurrentIntelligence; }
    [SerializeField, Range(0, 1)] private float _CurrentCharisma = 0.1f;
    public float CurrentCharisma { get => _CurrentCharisma; }
}