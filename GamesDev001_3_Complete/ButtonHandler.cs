using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonHandler : MonoBehaviour
{
    // References to the HexGrid and UI buttons
    [SerializeField] private HexGrid hexGrid;
    [SerializeField] private Button tankNobleButton;
    [SerializeField] private Button tankMissionaryButton;
    [SerializeField] private Button archerMissionaryButton;

    private GameObject previewUnitInstance = null;

    // Prefabs for different unit types
    [SerializeField] private GameObject tankNoblePrefab;
    [SerializeField] private GameObject tankMissionaryPrefab;
    [SerializeField] private GameObject archerMissionaryPrefab;

    private void Start()
    {
        // Assign button click events
        tankNobleButton.onClick.AddListener(() => OnSpawnButtonClicked("TankNoble"));
        tankMissionaryButton.onClick.AddListener(() => OnSpawnButtonClicked("TankMissionary"));
        archerMissionaryButton.onClick.AddListener(() => OnSpawnButtonClicked("ArcherMissionary"));
    }

    // Handle unit spawn button click
    private void OnSpawnButtonClicked(string unitType)
    {
        CreatePreviewUnit(unitType);
    }

    // Create a preview of the unit before placement
    private void CreatePreviewUnit(string unitType)
    {
        if (previewUnitInstance != null)
        {
            Destroy(previewUnitInstance);
        }

        // Select the appropriate unit prefab
        GameObject selectedPrefab = null;
        switch (unitType)
        {
            case "TankNoble":
                selectedPrefab = tankNoblePrefab;
                break;
            case "TankMissionary":
                selectedPrefab = tankMissionaryPrefab;
                break;
            case "ArcherMissionary":
                selectedPrefab = archerMissionaryPrefab;
                break;
        }

        // Instantiate the unit preview if a valid prefab exists
        if (selectedPrefab != null)
        {
            previewUnitInstance = Instantiate(selectedPrefab);
            Unit previewUnitScript = previewUnitInstance.GetComponent<Unit>();

            // Set unit attributes based on type
            switch (unitType)
            {
                case "TankNoble":
                    previewUnitScript.unitName = "Tank + Noble";
                    previewUnitScript.HP = 200f;
                    previewUnitScript.DMG = 50f;
                    previewUnitScript.traits = new List<UnitTrait> { UnitTrait.Tank, UnitTrait.Noble };
                    break;
                case "TankMissionary":
                    previewUnitScript.unitName = "Tank + Missionary";
                    previewUnitScript.HP = 250f;
                    previewUnitScript.DMG = 40f;
                    previewUnitScript.traits = new List<UnitTrait> { UnitTrait.Tank, UnitTrait.Missionary };
                    break;
                case "ArcherMissionary":
                    previewUnitScript.unitName = "Archer + Missionary";
                    previewUnitScript.HP = 100f;
                    previewUnitScript.DMG = 70f;
                    previewUnitScript.traits = new List<UnitTrait> { UnitTrait.Archer, UnitTrait.Missionary };
                    break;
            }

            StartCoroutine(WaitForMouseClickAndPlace(unitType));
        }
    }

    // Wait for the player to click and place the unit
    private IEnumerator WaitForMouseClickAndPlace(string unitType)
    {
        while (!Input.GetMouseButtonDown(0))
        {
            // Move the preview unit with the mouse
            Vector3 position = GetMousePositionOnGrid();
            Vector3 snappedPosition = hexGrid.GetClosestHexCenter(position);
            previewUnitInstance.transform.position = snappedPosition;

            yield return null;
        }

        // Finalize placement and spawn the unit in the grid
        Vector3 finalPosition = previewUnitInstance.transform.position;
        hexGrid.SpawnUnit(finalPosition, unitType);

        Destroy(previewUnitInstance);
    }

    // Get the mouse position in world space
    private Vector3 GetMousePositionOnGrid()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}