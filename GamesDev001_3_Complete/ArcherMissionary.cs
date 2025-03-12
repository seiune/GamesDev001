using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherMissionary : Unit
{
    // Set default values when the prefab is created in the editor
    private void Reset()
    {
        ApplyDefaults();
    }

    // Ensure default values are applied when the unit is instantiated
    private void OnEnable()
    {
        ApplyDefaults();
    }

    // Apply unit-specific defaults
    private void ApplyDefaults()
    {
        unitName = "Archer + Missionary";
        traits = new List<UnitTrait> { UnitTrait.Archer, UnitTrait.Missionary };

        ApplyDefaultStats();  // Apply default stats based on traits
        StoreInitialStats();  // Store initial values for resetting if needed
    }
}