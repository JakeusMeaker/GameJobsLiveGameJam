﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scenario", menuName = "CreateNewScenario", order = 1)]
public class ScenarioSO : ScriptableObject
{
    public string scenarioText;

    public string passText;
    public string failText;

    public string characterCriticalFail;
    public string partyCriticalFail;

    public E_Trait traitToPass;
}
