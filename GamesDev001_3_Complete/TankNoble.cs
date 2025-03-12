using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankNoble : Unit
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
        unitName = "Tank + Noble";
        traits = new List<UnitTrait> { UnitTrait.Tank, UnitTrait.Noble };

        ApplyDefaultStats();  // Apply default stats based on traits
        StoreInitialStats();  // Store initial values for resetting if needed
    }
}