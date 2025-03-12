using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    // Grid settings
    [field: SerializeField] public HexOrientation Orientation { get; private set; }
    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public int Height { get; private set; }
    [field: SerializeField] public int HexSize { get; private set; }

    // Unit prefabs
    [Header("Unit Prefabs")]
    public GameObject tankNoblePrefab;
    public GameObject tankMissionaryPrefab;
    public GameObject archerMissionaryPrefab;

    // Tracking units and bonuses
    private List<Unit> allUnits = new List<Unit>();
    private HashSet<string> uniqueTankUnits = new HashSet<string>(); // Tracks unique Tank units
    private HashSet<string> uniqueMissionaryUnits = new HashSet<string>(); // Tracks unique Missionary units
    private bool tankBonusApplied = false;
    private bool missionaryBonusApplied = false;

    private void Start()
    {
        GenerateGrid();
    }

    // Generate hex grid based on defined width and height
    private void GenerateGrid()
    {
        for (int z = 0; z < Height; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector3 centrePosition = HexMetrics.Center(HexSize, x, z, Orientation) + transform.position;
                DrawHex(centrePosition);
            }
        }
    }

    // Draws the outline of a hex at a specified center position
    private void DrawHex(Vector3 center)
    {
        GameObject hexOutline = new GameObject("HexOutline");
        hexOutline.transform.position = center;

        LineRenderer lineRenderer = hexOutline.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 7; // Six corners plus the looped start point
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.3f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        Vector3[] corners = HexMetrics.Corners(HexSize, Orientation);
        for (int i = 0; i < 6; i++)
        {
            lineRenderer.SetPosition(i, center + corners[i]);
        }
        lineRenderer.SetPosition(6, center + corners[0]); // Close the hex shape
    }

    // Finds the closest hex center to a given position
    public Vector3 GetClosestHexCenter(Vector3 position)
    {
        float closestDistance = float.MaxValue;
        Vector3 closestCenter = Vector3.zero;

        for (int z = 0; z < Height; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector3 hexCenter = HexMetrics.Center(HexSize, x, z, Orientation) + transform.position;
                float distance = Vector3.Distance(position, hexCenter);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCenter = hexCenter;
                }
            }
        }
        return closestCenter;
    }

    // Spawns a unit of the specified type at the given position
    public void SpawnUnit(Vector3 position, string unitType)
    {
        GameObject unitToSpawn = null;

        switch (unitType)
        {
            case "TankNoble":
                unitToSpawn = tankNoblePrefab;
                break;
            case "TankMissionary":
                unitToSpawn = tankMissionaryPrefab;
                break;
            case "ArcherMissionary":
                unitToSpawn = archerMissionaryPrefab;
                break;
        }

        if (unitToSpawn != null)
        {
            GameObject unitObject = Instantiate(unitToSpawn, position, Quaternion.identity);
            Unit unit = unitObject.GetComponent<Unit>();
            allUnits.Add(unit);

            // Check if the unit has a tank or missionary trait and track it
            if (unit.HasTrait(UnitTrait.Tank))
            {
                uniqueTankUnits.Add(unit.unitName);
                CheckAndApplyTankBonus();
            }

            if (unit.HasTrait(UnitTrait.Missionary))
            {
                uniqueMissionaryUnits.Add(unit.unitName);
                CheckAndApplyMissionaryBonus();
            }
        }
    }

    // Applies tank bonus if at least 2 unique tank units are present
    private void CheckAndApplyTankBonus()
    {
        if (uniqueTankUnits.Count >= 2 && !tankBonusApplied)
        {
            ApplyTankBonus();
            tankBonusApplied = true;
        }
    }

    // Applies missionary bonus if at least 2 unique missionary units are present
    private void CheckAndApplyMissionaryBonus()
    {
        if (uniqueMissionaryUnits.Count >= 2 && !missionaryBonusApplied)
        {
            ApplyMissionaryBonus();
            missionaryBonusApplied = true;
        }
    }

    // Increases HP for all tank units and updates default tank HP
    private void ApplyTankBonus()
    {
        foreach (var unit in allUnits)
        {
            if (unit.HasTrait(UnitTrait.Tank))
            {
                unit.HP = Unit.tankDefaultHP + 100f;
            }
        }
        Unit.tankDefaultHP += 100f;
    }

    // Increases damage for all missionary units and updates default missionary DMG
    private void ApplyMissionaryBonus()
    {
        foreach (var unit in allUnits)
        {
            if (unit.HasTrait(UnitTrait.Missionary))
            {
                unit.DMG = Unit.missionaryDefaultDMG + 50f;
            }
        }
        Unit.missionaryDefaultDMG += 50f;
    }

    // Removes a unit from the grid and updates trait tracking
    public void RemoveUnit(Unit unit)
    {
        if (unit.HasTrait(UnitTrait.Tank))
        {
            uniqueTankUnits.Remove(unit.unitName);
            if (uniqueTankUnits.Count < 2)
            {
                tankBonusApplied = false;
            }
        }

        if (unit.HasTrait(UnitTrait.Missionary))
        {
            uniqueMissionaryUnits.Remove(unit.unitName);
            if (uniqueMissionaryUnits.Count < 2)
            {
                missionaryBonusApplied = false;
            }
        }

        allUnits.Remove(unit);
        Destroy(unit.gameObject);
    }
}

// Defines possible hex orientations
public enum HexOrientation
{
    FlatTop,
    PointyTop
}