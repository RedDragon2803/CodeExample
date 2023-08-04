using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "NewRandomEvent", menuName = "Random Event")]
public class Event : ScriptableObject
{
    public LocalizedString description;
    public LocalizedString endDescription1;
    public LocalizedString endDescription2;
    public LocalizedString endDescription3;
    public LocalizedString choice1;
    public LocalizedString choice2;
    public LocalizedString choice3;
}
