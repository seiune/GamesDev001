using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private int hexSize = 5;
    [SerializeField] private HexOrientation orientation = HexOrientation.FlatTop;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 centerPosition = HexMetrics.Center(hexSize, x, z, orientation) + transform.position;
                DrawHex(centerPosition);
            }
        }
    }

    private void DrawHex(Vector3 center)
    {
        GameObject hexOutline = new GameObject("HexOutline");
        hexOutline.transform.position = center;

        LineRenderer lineRenderer = hexOutline.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 7;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.3f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        Vector3[] corners = HexMetrics.Corners(hexSize, orientation);
        for (int i = 0; i < 6; i++)
        {
            lineRenderer.SetPosition(i, center + corners[i]);
        }
        lineRenderer.SetPosition(6, center + corners[0]);
    }
}

public enum HexOrientation
{
    FlatTop,
    PointyTop
}