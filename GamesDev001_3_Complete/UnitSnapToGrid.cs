using UnityEngine;

public class UnitSnapToGrid : MonoBehaviour
{
    private HexGrid hexGrid;
    private bool isPlaced = false; // Tracks if the unit has been placed

    // Initialize the unit with a reference to the hex grid
    public void Initialize(HexGrid grid)
    {
        hexGrid = grid;
    }

    private void Update()
    {
        if (isPlaced) return; // Stop updating once the unit is placed

        if (Input.GetMouseButtonDown(0)) // Left-click to place the unit
        {
            Vector3 closestHexCenter = hexGrid.GetClosestHexCenter(transform.position);
            transform.position = closestHexCenter; // Snap to grid
            isPlaced = true; // Lock position to prevent further movement

            Debug.Log("Unit placed at: " + transform.position);
        }
    }
}