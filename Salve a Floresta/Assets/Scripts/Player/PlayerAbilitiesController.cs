using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesController : MonoBehaviour
{
    public const string curupiraSkill = "curupiraSkill";
    public const string iaraSkill = "iaraSkill";
    public const string saciSkill = "saciSkill";

    public void ReleaseSkill(string key)
    {
        PlayerPrefs.SetInt(key, 1);
    }

    public void DisableAllAbilities()
    {
        PlayerPrefs.SetInt(curupiraSkill, 0);
        PlayerPrefs.SetInt(iaraSkill, 0);
        PlayerPrefs.SetInt(saciSkill, 0);
    }
}
