using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    // Calculate the outer radius of a hexagon based on size
    public static float OuterRadius(float hexSize)
    {
        return hexSize;
    }

    // Calculate the inner radius (distance from center to midpoint of an edge)
    public static float InnerRadius(float hexSize)
    {
        return hexSize * 0.866025404f; // sqrt(3)/2, used for flat-top hexagons
    }

    // Get all six corner positions of a hexagon
    public static Vector3[] Corners(float hexSize, HexOrientation orientation)
    {
        Vector3[] corners = new Vector3[6];
        for (int i = 0; i < 6; i++)
        {
            corners[i] = Corner(hexSize, orientation, i);
        }
        return corners;
    }

    // Calculate the position of a single corner of the hexagon
    public static Vector3 Corner(float hexSize, HexOrientation orientation, int index)
    {
        float angle = 60f * index; // Each corner is 60 degrees apart
        if (orientation == HexOrientation.PointyTop)
        {
            angle += 30f; // Adjust angle for pointy-top hexagons
        }

        return new Vector3(
            hexSize * Mathf.Cos(angle * Mathf.Deg2Rad),
            0f,
            hexSize * Mathf.Sin(angle * Mathf.Deg2Rad)
        );
    }

    // Get the center position of a hexagon in the grid based on coordinates
    public static Vector3 Center(float hexSize, int x, int z, HexOrientation orientation)
    {
        Vector3 centerPosition;

        if (orientation == HexOrientation.PointyTop)
        {
            centerPosition.x = (x + z * 0.5f - z / 2) * (InnerRadius(hexSize) * 2f);
            centerPosition.y = 0f;
            centerPosition.z = z * (OuterRadius(hexSize) * 1.5f);
        }
        else
        {
            centerPosition.x = x * (OuterRadius(hexSize) * 1.5f);
            centerPosition.y = 0f;
            centerPosition.z = (z + x * 0.5f - x / 2) * (InnerRadius(hexSize) * 2f);
        }

        return centerPosition;
    }
}