using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Unit attributes
    public string unitName;
    public float HP;
    public float DMG;
    public List<UnitTrait> traits;

    // Default values for specific traits
    public static float tankDefaultHP = 200f;
    public static float missionaryDefaultDMG = 50f;

    // Variables to store initial stats for resetting or reference
    private float initialHP;
    private float initialDMG;

    private void Start()
    {
        ApplyDefaultStats();  // Set default stats based on traits
        StoreInitialStats();  // Save initial stats for reference
        StartCoroutine(PrintStatsPeriodically());  // Start periodic stat logging
    }

    // Assign default stats based on unit traits
    public void ApplyDefaultStats()
    {
        if (HasTrait(UnitTrait.Tank))
        {
            HP = tankDefaultHP;
        }

        if (HasTrait(UnitTrait.Missionary))
        {
            DMG = missionaryDefaultDMG;
        }
    }

    // Store the initial stats for potential resets
    public void StoreInitialStats()
    {
        initialHP = HP;
        initialDMG = DMG;
    }

    // Reset unit stats to their initial values
    public void ResetStats()
    {
        HP = initialHP;
        DMG = initialDMG;
    }

    // Log unit stats to the console
    private void PrintStats()
    {
        Debug.Log($"{unitName} - HP: {HP}, DMG: {DMG}");
    }

    // Coroutine to print unit stats periodically
    private IEnumerator PrintStatsPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            PrintStats();
        }
    }

    // Check if the unit has a specific trait
    public bool HasTrait(UnitTrait trait)
    {
        return traits.Contains(trait);
    }
}

// Enum defining different unit traits
public enum UnitTrait
{
    Tank,
    Noble,
    Missionary,
    Archer
}