using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scenario", menuName = "CreateNewScenario", order = 1)]
public class ScenarioSO : ScriptableObject
{
    public Sprite backgroundImage;
    public Sprite foregroundImage;

    public bool isRestRoom;

    [TextArea]
    public string scenarioText;

    [TextArea]
    public string passTraitText;
    [TextArea]
    public string secondary1PassText;
    [TextArea]
    public string secondary2PassText;
    
    [TextArea]
    public string luckyPassText;

    [TextArea]
    public string failText;

    [TextArea]
    public string characterCriticalFail;
    [TextArea]
    public string partyCriticalFail;

    public E_Trait traitToPass; //80% Success Rate
    public E_Trait[] secondaryTraits = new E_Trait[2]; //50% Success Rate
    
}
