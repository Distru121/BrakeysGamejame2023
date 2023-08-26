using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    public Transform playerTransform;

    public float levelWidth = 32;
    public int startGenOffset = -2; //until this y*5 it will not generate
    public int generationSegments = 8; //generation segments present in each layer (each one can have a structure)
    public float randomPrefabYGenOffset = 2f; //a small offset in the y when it generates the assets
    public float randomPrefabXGenOffset = 2f; //a small offset in the y when it generates the assets

    public int maxWorldHeight = 10; //still calculated in multiples of 5.
    public int numberOfCheckpoints = 3;
    private bool canGenerateCheckpoint = false;

    public GameObject layerPrefab;
    public GameObject endOfLevel;

    public GameObject[] sideDangerPrefabs; //the prefabs for the lateral spikes and harmful objects
    public GameObject[] sidePrefabs; //the prefabs that generate only at the sides (they are all large)
    public GameObject[] sidePrefabsR;
    public GameObject[] middlePrefabs_S; //the prefabs that generate only at the middle, small size
    public GameObject[] middlePrefabs_M; //the prefabs that generate only at the middle, medium size
    public GameObject checkPointIsland; //the checkpoints island. they get generated after a certain distance.
    //the size values of the variuous prefabs. Smaller prefabs will have smaller values, and larger will have larger ones. They will scale percentage-wise
    /*private float largePrefabSizeValue = 3;
    private float mediumPrefabSizeValue = 2;
    private float smallPrefabSizeValue = 1;*/

    private int hasGeneratedUntil = 4; //the generation height it already generated. Each level equals to 5 tiles in the y, downwards. Starts at 20 because the player starts satarts at 0 0 so it generates a bit higher.

    public float startingObjectSpawnChance = 0.2f;
    private float objectSpawnChance = 0.2f; //generation chance in each segment of spawning
    private float genXOffset = 0f; //default x offset position for the assets

    // Update is called once per frame
    void Update()
    {
        //procedurally spawns the layer dependant on player position
        //increases the spawn chance of objects a little, with a cap;
        if(objectSpawnChance < 0.5f)
            objectSpawnChance = ((1/(float)maxWorldHeight)*(-(float)hasGeneratedUntil)) + startingObjectSpawnChance;

        if(playerTransform.position.y - 25 < hasGeneratedUntil*5 && -hasGeneratedUntil < maxWorldHeight)
        {
            Instantiate(layerPrefab, new Vector3(0, hasGeneratedUntil*5, 0), Quaternion.identity); //generates the base layer prefab (bg and sides)

            if(-hasGeneratedUntil % (int)Mathf.Round((float)maxWorldHeight/(float)numberOfCheckpoints) == 0 && -hasGeneratedUntil != 0)
                canGenerateCheckpoint = true;

            //procedurally adds obstacles dependant on player position
            if(hasGeneratedUntil < startGenOffset) {
                for(int i = 0; i <= generationSegments; i++) //divides the full length of the current generating layer in X segments, where each one can generate a world segment
                {
                    float generationPositionX = ((levelWidth/(float)generationSegments * i) - levelWidth/2) + Random.Range(-randomPrefabXGenOffset/2, randomPrefabXGenOffset/2);
                    float generationPositionY = hasGeneratedUntil*5 + Random.Range(-randomPrefabYGenOffset/2, randomPrefabYGenOffset/2);

                    if(canGenerateCheckpoint && Random.value < ((1/(float)generationSegments)*i)) //if it can generate the checkpoint, spawns it somewhere on the current layer
                    {
                        Instantiate(checkPointIsland, new Vector3(generationPositionX, generationPositionY, 0), Quaternion.identity);
                        canGenerateCheckpoint = false;
                    }
                    if(Random.value < objectSpawnChance)
                    {
                        if(i == 0) //left side generation
                            Instantiate(sidePrefabs[Random.Range(0, sidePrefabs.Length)], new Vector3(((levelWidth/(float)generationSegments * i) - levelWidth/2), generationPositionY, 0), Quaternion.identity); //spawns left
                        else if(i == generationSegments) //right side generation
                        { 
                             Instantiate(sidePrefabsR[Random.Range(0, sidePrefabsR.Length)], new Vector3(((levelWidth/(float)generationSegments * i) - levelWidth/2), generationPositionY, 0), Quaternion.identity);
                            //hypotetically, should flip both object and children's rotation
                            /*GameObject prefab = Instantiate(sidePrefabs[Random.Range(0, sidePrefabs.Length)], new Vector3(((levelWidth/generationSegments * i) - levelWidth/2), generationPositionY, 0), Quaternion.identity); //spawns right
                            prefab.transform.localScale = new Vector3(-1, prefab.transform.localScale.y, prefab.transform.localScale.z); //modifies scale to make it look left
                            int children = prefab.transform.childCount;
                            for(int j=0; j < children; j++)
                            {
                                transform.GetChild(j).rotation = Quaternion.Euler(-transform.GetChild(j).rotation.eulerAngles);
                            }*/
                        }
                        else //middle generation
                        {
                            //TODO: add a way to spawn medium and small assets with more accuracy, like medium assets not overlapping others and so on
                            //For example, not generate a structure after or before a ledge
                            if(Random.value < 0.45f)
                                Instantiate(middlePrefabs_M[Random.Range(0, middlePrefabs_M.Length)], new Vector3(generationPositionX, generationPositionY, 0), Quaternion.identity);
                            else
                                Instantiate(middlePrefabs_S[Random.Range(0, middlePrefabs_S.Length)], new Vector3(generationPositionX, generationPositionY, 0), Quaternion.identity);
                        }
                    }
                    //the spawn chance for hurting items is double the chance for normal objects to spawn (spawns only if no object spawned before)
                    else if(Random.value < objectSpawnChance*2)
                    {
                        if(i == 0) //left side generation
                            Instantiate(sideDangerPrefabs[Random.Range(0, sideDangerPrefabs.Length)], new Vector3(((levelWidth/(float)generationSegments * i) - levelWidth/2) - genXOffset*3f, generationPositionY, 0), Quaternion.identity); //spawns left
                        else if(i == generationSegments) //right side generation
                        {
                            GameObject prefab = Instantiate(sideDangerPrefabs[Random.Range(0, sideDangerPrefabs.Length)], new Vector3(((levelWidth/(float)generationSegments * i) - levelWidth/2) + genXOffset*1.5f, generationPositionY, 0), Quaternion.identity); //spawns right
                            prefab.transform.localScale = new Vector3(-1, prefab.transform.localScale.y, prefab.transform.localScale.z); //modifies scale to make it look left
                        }
                    }
                }
            }

            hasGeneratedUntil--;
        }
        else if(-hasGeneratedUntil == maxWorldHeight)
        {
            //generates end of level
            Instantiate(layerPrefab, new Vector3(0, hasGeneratedUntil*5, 0), Quaternion.identity);
            Instantiate(endOfLevel, new Vector3(0, hasGeneratedUntil*5, 0), Quaternion.identity);
            hasGeneratedUntil--;
        }
    }
}
