using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

public class AreaFloorBaker : MonoBehaviour
{
    //serialize faz a variavel privada aparecer no inspetor
    [SerializeField]
    //local com o componente navmeshsurface que é quem cria a navmesh
    private NavMeshSurface surface;
    [SerializeField]
    private PlayerMovement player;
    [SerializeField]
    //velocidade de update
    private float updateRate = 0.1f;
    [SerializeField]
    //quantos m o player tem de se mexer para o navmesh atualizar
    private float movementThreshold = 3;
    [SerializeField]
    //tamanho da navmesh
    private Vector3 navMeshSize = new Vector3(100, 650, 100);

    //localizacao antiga do player
    private Vector3 worldAnchor;
    //dados da navmesh
    private NavMeshData NavMeshData;
    //lista de objetos na navmesh
    private List<NavMeshBuildSource> Sources = new List<NavMeshBuildSource>();

    void Start()
    {
        NavMeshData = new NavMeshData();
        NavMesh.AddNavMeshData(NavMeshData);
        BuildNavMesh(false);
        StartCoroutine(CheckPlayerMovement());
    }

    private IEnumerator CheckPlayerMovement()
    {
        //variavel com - esperar o tempo do updaterate
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        while(true)
        {
            //se a distancia entre a localização anterior do player e a atual for maior que o threshold
            if(Vector3.Distance(worldAnchor, player.transform.position) > movementThreshold)
            {
                //fazer a navmesh
                BuildNavMesh(true);
                //defenir a posição antiga outra vez
                worldAnchor = player.transform.position;
            }
            //esperar o tempo do wait
            yield return wait;
        }
    }
    //fazer a mesh
    private void BuildNavMesh(bool Async)
    {
        //limites da navmesh
        Bounds navMeshBounds = new Bounds(player.transform.position, navMeshSize);
        //inicar os markups, varioavel para determinar como os objetos na mesh são tratados
        List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();
        //determina como o objeto dentro da navmesh se comporta
        List<NavMeshModifier> modifiers;
        //se os objetos dentro do surface forem iguais aos seu filhos
        if (surface.collectObjects == CollectObjects.Children)
        {
            //então os filhos vão passar a fazer parte da surface
            modifiers = new List<NavMeshModifier>(surface.GetComponentsInChildren<NavMeshModifier>());
        }
        else
        {
            //se não os modifiers continuam iguais
            modifiers = NavMeshModifier.activeModifiers;
        }
        //numero de mudanças no terreno ou modifiers
        for (int i = 0; i < modifiers.Count; i++)
        {
            // Verifica se o modificador afeta o tipo de agente definido pela superfície
            if (((surface.layerMask & (1 << modifiers[i].gameObject.layer)) == 1) && modifiers[i].AffectsAgentType(surface.agentTypeID))
            {
                // Adiciona uma nova marcação à lista de marcações
                markups.Add(new NavMeshBuildMarkup()
                {
                    root = modifiers[i].transform,
                    overrideArea = modifiers[i].overrideArea,
                    area = modifiers[i].area,
                    ignoreFromBuild = modifiers[i].ignoreFromBuild
                });
            }
        }

        // Coleta as fontes de dados para a construção do NavMesh
        if (surface.collectObjects == CollectObjects.Children)
        {
            // Coleta as geometrias marcadas com a camada definida pela superfície
            NavMeshBuilder.CollectSources(surface.transform, surface.layerMask, surface.useGeometry, surface.defaultArea, markups, Sources);
        }
        else
        {
            // Coleta as geometrias dentro da região definida pela superfície
            NavMeshBuilder.CollectSources(navMeshBounds, surface.layerMask, surface.useGeometry, surface.defaultArea, markups, Sources);
        }

        //Sources.RemoveAll(source => source.component != null && source.component.gameObject.GetComponent<NavMeshAgent>() != null);

        // Atualiza o NavMesh com base nas fontes de dados coletadas
        if (Async)
        {
            // Atualiza o NavMesh de forma assíncrona
            NavMeshBuilder.UpdateNavMeshDataAsync(NavMeshData, surface.GetBuildSettings(), Sources, new Bounds(player.transform.position, navMeshSize));
        }
        else
        {
            // Atualiza o NavMesh de forma síncrona
            NavMeshBuilder.UpdateNavMeshData(NavMeshData, surface.GetBuildSettings(), Sources, new Bounds(player.transform.position, navMeshSize));
        }
    }
}
