using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct AtlasData : ISharedComponentData, IEquatable<AtlasData>
{
    public NativeHashMap<int, RowData> rows;
    public Material material;
    public float2 bounds;

    public bool Equals(AtlasData other)
    {
        return bounds.x == other.bounds.x && bounds.y == other.bounds.y && rows.GetHashCode() == other.rows.GetHashCode();
    }

    public override int GetHashCode()
    {
        return rows.GetHashCode() / (int)bounds.y * (int)bounds.x;
    }
}