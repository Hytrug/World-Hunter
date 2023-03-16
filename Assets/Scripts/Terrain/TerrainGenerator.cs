using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

    public int colliderLODIndex;
    public LODInfo[] detailLevels;//para o LOD=0: 4*heightMultiplier | para melhor performance 2*heightMultiplier

    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureSettings;

    public Transform viewer;
    public Material mapMaterial;

    Vector2 viewerPosition;
    Vector2 viewerPositionOld;
    float meshWorldSize;
    int chunkVisibleInViewDst;

    //max range e min range para os objetos
    /*int maxRangeXOldPositivo = 2500;
    int minRangeXOldPositivo = -2500;
    int maxRangeXOldNegative = 2500;
    int minRangeXOldNegative = -2500;
    int maxRangeX = 2500;
    int minRangeX = -2500;
    int maxRangeZOldPositivo = 2500;
    int minRangeZOldPositivo = -2500;
    int maxRangeZOldNegative = 2500;
    int minRangeZOldNegative = -2500;
    int maxRangeZ = 2500;
    int minRangeZ = -2500;
    int limiteXNegative = -2000;
    int limiteXPositivo = 2000;
    int limiteZNegative = -2000;
    int limiteZPositivo = 2000;
    int sairX = 0;
    int sairZ = 0;

    //TreeGenerator script
    public TreeGenerator treeGenerator = new TreeGenerator();*/

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        textureSettings.ApplyToMaterial (mapMaterial);
        textureSettings.UpdateMeshHeights (mapMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);

        float maxViewDst = detailLevels [detailLevels.Length - 1].visibleDstThreshold;
        meshWorldSize = meshSettings.meshWorldSize;
        chunkVisibleInViewDst = Mathf.RoundToInt(maxViewDst / meshWorldSize);

        UpdateVisibleChunks();
    }

    void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

        if (viewerPosition != viewerPositionOld)
        {
            foreach (TerrainChunk chunk in visibleTerrainChunks)
            {
                chunk.UpdateCollisionMesh();
            }
        }

        if((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
        {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks ();
        }

        /*if (limiteXNegative > viewerPosition.x || limiteXPositivo < viewerPosition.x || limiteZNegative > viewerPosition.y || limiteZPositivo < viewerPosition.y)
        {
            if (limiteXNegative > viewerPosition.x)
            {
                minRangeXOldNegative= minRangeXOldNegative -5000;
                maxRangeXOldNegative= maxRangeXOldNegative -5000;
                limiteXNegative= limiteXNegative - 5000;
                minRangeX = minRangeXOldNegative;
                maxRangeX = maxRangeXOldNegative;
            }
            if (limiteXPositivo < viewerPosition.x)
            {
                minRangeXOldPositivo= minRangeXOldPositivo +5000;
                maxRangeXOldPositivo= maxRangeXOldPositivo +5000;
                limiteXPositivo= limiteXPositivo + 5000;
                minRangeX = minRangeXOldPositivo;
                maxRangeX = maxRangeXOldPositivo;
            }
            if (limiteZNegative > viewerPosition.y)
            {
                minRangeZOldNegative= minRangeZOldNegative -5000;
                maxRangeZOldNegative= maxRangeZOldNegative -5000;
                limiteZNegative= limiteZNegative - 5000;
                minRangeZ = minRangeZOldNegative;
                maxRangeZ = maxRangeZOldNegative;
            }
            if (limiteZPositivo < viewerPosition.y)
            {
                minRangeZOldPositivo= minRangeZOldPositivo +5000;
                maxRangeZOldPositivo= maxRangeZOldPositivo +5000;
                limiteZPositivo= limiteZPositivo + 5000;
                minRangeZ = minRangeZOldPositivo;
                maxRangeZ = maxRangeZOldPositivo;
            }

            treeGenerator.UpdateTree(minRangeX, maxRangeX, minRangeZ, maxRangeZ);
        }*/
    }

    void UpdateVisibleChunks()
    {
        HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();
        for (int i = visibleTerrainChunks.Count-1; i >= 0; i--)
        {
            alreadyUpdatedChunkCoords.Add(visibleTerrainChunks[i].coord);
            visibleTerrainChunks[i].UpdateTerrainChunk();
        }

        int currentChunkCoordX = Mathf.RoundToInt( viewerPosition.x / meshWorldSize);
        int currentChunkCoordY = Mathf.RoundToInt( viewerPosition.y / meshWorldSize);

        for (int yOffset = -chunkVisibleInViewDst; yOffset <= chunkVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunkVisibleInViewDst; xOffset <= chunkVisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (!alreadyUpdatedChunkCoords.Contains (viewedChunkCoord))
                {
                    if (terrainChunkDictionary.ContainsKey (viewedChunkCoord))
                    {
                        terrainChunkDictionary [viewedChunkCoord].UpdateTerrainChunk ();
                    }else
                    {
                        TerrainChunk newChunk = new TerrainChunk(viewedChunkCoord, heightMapSettings, meshSettings, detailLevels, colliderLODIndex, transform, viewer, mapMaterial);
                        terrainChunkDictionary.Add(viewedChunkCoord, newChunk);
                        newChunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;
                        newChunk.Load();
                    }
                }
            }

        }
    }

    void OnTerrainChunkVisibilityChanged(TerrainChunk chunk, bool isVisible)
    {
        if (isVisible)
        {
            visibleTerrainChunks.Add (chunk);
        }else
        {
            visibleTerrainChunks.Remove (chunk);
        }
    }
}

[System.Serializable]
public struct LODInfo
{
    [Range(0,MeshSettings.numSupportedLODs-1)]
    public int lod;
    public float visibleDstThreshold;

    public float sqrVisibleDstThreshold
    {
        get 
        {
            return visibleDstThreshold * visibleDstThreshold; 
        }
    }
}
