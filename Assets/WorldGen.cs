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

    public int maxWorldHeight = 40; //still calculated in multiples of 5.

    public GameObject layerPrefab;

    public GameObject[] sideDanger; //the prefabs for the lateral spikes and harmful objects
    public GameObject[] sidePrefabs; //the prefabs that generate only at the sides (they are all large)
    public GameObject[] middlePrefabs_S; //the prefabs that generate only at the middle, small size
    public GameObject[] middlePrefabs_M; //the prefabs that generate only at the middle, medium size
    //the size values of the variuous prefabs. Smaller prefabs will have smaller values, and larger will have larger ones. They will scale percentage-wise
    /*private float largePrefabSizeValue = 3;
    private float mediumPrefabSizeValue = 2;
    private float smallPrefabSizeValue = 1;*/

    private int hasGeneratedUntil = 4; //the generation height it already generated. Each level equals to 5 tiles in the y, downwards. Starts at 20 because the player starts satarts at 0 0 so it generates a bit higher.

    private float objectSpawnChance = 0.2f; //generation chance in each segment of spawning
    private float genXOffset = 0f; //default x offset position for the assets

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hasGeneratedUntil + "," + objectSpawnChance);
        //procedurally spawns the layer dependant on player position
        if(playerTransform.position.y - 25 < hasGeneratedUntil*5)
        {
            //increases the spawn chance of objects a little, with a cap;
            if(objectSpawnChance < 0.5f)
                objectSpawnChance += 0.1f/(float)maxWorldHeight;

            Instantiate(layerPrefab, new Vector3(0, hasGeneratedUntil*5, 0), Quaternion.identity); //generates the base layer prefab (bg and sides)

            //procedurally adds obstacles dependant on player position
            if(hasGeneratedUntil < startGenOffset) {
                for(int i = 0; i <= generationSegments; i++) //divides the full length of the current generating layer in X segments, where each one can generate a world segment
                {
                    float spawnChance = Random.value; //spawn randomness chance in each segment
                    if(spawnChance < objectSpawnChance)
                    {
                        float generationPositionX = ((levelWidth/generationSegments * i) - levelWidth/2) + Random.Range(-randomPrefabXGenOffset/2, randomPrefabXGenOffset/2);
                        float generationPositionY = hasGeneratedUntil*5 + Random.Range(-randomPrefabYGenOffset/2, randomPrefabYGenOffset/2);

                        if(i == 0) //left side generation
                            Instantiate(sidePrefabs[Random.Range(0, sidePrefabs.Length)], new Vector3(((levelWidth/generationSegments * i) - levelWidth/2) + genXOffset, generationPositionY, 0), Quaternion.identity); //spawns left
                        else if(i == generationSegments) //right side generation
                        {
                            GameObject prefab = Instantiate(sidePrefabs[Random.Range(0, sidePrefabs.Length)], new Vector3(((levelWidth/generationSegments * i) - levelWidth/2) + genXOffset, generationPositionY, 0), Quaternion.identity); //spawns right
                            prefab.transform.localScale = new Vector3(-1, prefab.transform.localScale.y, prefab.transform.localScale.z); //modifies scale to make it look left
                        }
                        else //middle generation
                        {
                            //TODO: add a way to spawn medium and small assets with more accuracy, like medium assets not overlapping others and so on
                            //For example, not generate a structure after or before a ledge
                            if(Random.value < 0.45f)
                                Instantiate(middlePrefabs_M[Random.Range(0, middlePrefabs_M.Length)], new Vector3(generationPositionX + genXOffset, generationPositionY, 0), Quaternion.identity);
                            else
                                Instantiate(middlePrefabs_S[Random.Range(0, middlePrefabs_S.Length)], new Vector3(generationPositionX + genXOffset, generationPositionY, 0), Quaternion.identity);
                        }
                    }
                }

                /*if(random > 4.5f) //here it generates the side structures
                {
                    float yOffset = Random.Range(-randomPrefabYGenOffset/2, randomPrefabYGenOffset/2); //generates the y offset
                    int randomSidePrefab = Random.Range(0, sidePrefabs.Length);
                    if(random <= 5.25f){
                        Instantiate(sidePrefabs[randomSidePrefab], new Vector3(-levelWidth/2 + genXOffset, (hasGeneratedUntil*5)+yOffset, 0), Quaternion.identity); //spawns left
                    }
                    else {
                        GameObject prefab = Instantiate(sidePrefabs[randomSidePrefab], new Vector3(levelWidth/2 + genXOffset, (hasGeneratedUntil*5)+yOffset, 0), Quaternion.identity); //spawns right
                        prefab.transform.localScale = new Vector3(-1, prefab.transform.localScale.y, prefab.transform.localScale.z); //modifies scale to make it look left
                    }
                }

                if(random > 3.5f) //here it generates medium structures
                {
                    float yOffset = Random.Range(-randomPrefabYGenOffset/2, randomPrefabYGenOffset/2); //generates the y offset
                    int randomSidePrefab = Random.Range(0, middlePrefabs_M.Length);
                    float randomXPos = Random.Range(-levelWidth/2, levelWidth/2);

                    GameObject prefab = Instantiate(middlePrefabs_M[randomSidePrefab], new Vector3(randomXPos + genXOffset, (hasGeneratedUntil*5)+yOffset, 0), Quaternion.identity);
                }

                if(random > 2f) //here it generates small structures
                {
                    float yOffset = Random.Range(-randomPrefabYGenOffset/2, randomPrefabYGenOffset/2); //generates the y offset
                    int randomSidePrefab = Random.Range(0, middlePrefabs_S.Length);
                    float randomXPos = Random.Range(-levelWidth/2, levelWidth/2);

                    GameObject prefab = Instantiate(middlePrefabs_S[randomSidePrefab], new Vector3(randomXPos + genXOffset, (hasGeneratedUntil*5)+yOffset, 0), Quaternion.identity);
                }*/
            }

            hasGeneratedUntil--;
        }
    }
}
