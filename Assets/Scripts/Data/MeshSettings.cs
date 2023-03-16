using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MeshSettings : UpdatableData
{
    public const int numSupportedLODs = 5;
    public const int numSupportedChunkSizes = 9;
    public const int numSupportedFlatshadedChunkSizes = 3;
    public static readonly int[] supportedChunkSizes = {48, 72, 96, 120, 144, 168, 192, 216, 240};

    //tamanho da mesh (heightMultiplier a /10) better one = 50
    public float meshScale = 2.5f;
    public bool useFlatShading;

    //range são barras para aumentar e diminuir
    [Range(0,numSupportedChunkSizes-1)]
    public int chunkSizeIndex;
    [Range(0,numSupportedFlatshadedChunkSizes-1)]
    public int flatshadedChunkSizeIndex;

    //numero de vertices por linha da mesh renderizada com a LOD=0. Inclui os 2 vertices extra que são escluidos da mesh final, mas são usado para calcular normais.
    public int numVertsPerLine
    {
        get
        {
            return supportedChunkSizes[(useFlatShading)?flatshadedChunkSizeIndex:chunkSizeIndex] + 5;
        }
    }

    //tamanho do mundo
    public float meshWorldSize
    {
        get
        {
            return (numVertsPerLine - 3) * meshScale;
        }
    }
}
