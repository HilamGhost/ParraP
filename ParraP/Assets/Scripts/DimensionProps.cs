using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DimensionProps : MonoBehaviour
{  

    public List<GameObject> props = new List<GameObject>();

    [Header("Spawn Random")]
    public Vector3 maxDistance = new Vector3(6f, 0.5f,0);
    public Vector3 minDistance = new Vector3(-6f, -0.5f,0);

    // Start is called before the first frame update
    void Start()
    {
        maxDistance = new Vector3(6f, 0.5f, 0);
        minDistance = new Vector3(-6f, -0.5f, 0);
        

        gameObject.transform.parent.parent.GetComponent<PropController>().AddPropList(this);
    }

    // Update is called once per frame
    void Update()
    {
        props = props.Distinct().ToList();
    }
    public void UpdateProps()
    {          
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.tag == "Props" && child.gameObject.GetComponent<PlayerController>() == null)
            {
                props.Add(child.gameObject);
            }
        }
    }
    public void SpawnProps(PropList propList,List<int> randomints) 
    {
        for (int i = 0; i < randomints.Count; i++)
        {
            float x = Random.Range(minDistance.x,maxDistance.x);
            float y = Random.Range(minDistance.y, maxDistance.y);
            Vector3 spawnPoint = new Vector3(x, y, 0);
            GameObject newProp= Instantiate(propList.propList[randomints[i]].gameObject, transform.position,Quaternion.identity,gameObject.transform);
            newProp.transform.localPosition = spawnPoint;
            newProp.GetComponent<PropID>().ID = i;
        }

        UpdateProps();
    }
   
}
