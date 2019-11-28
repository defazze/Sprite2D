using Unity.Mathematics;

public struct RowData
{
    public RowData(float2 bounds, float offsetY, int framesCount)
    {
        this.bounds = bounds;
        this.offsetY = offsetY;
        this.framesCount = framesCount;
    }

    public float2 bounds;
    public float offsetY;
    public int framesCount;
}