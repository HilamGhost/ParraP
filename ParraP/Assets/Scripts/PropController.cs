using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropController : MonoBehaviour
{
    public List<DimensionProps> dimensionProps = new List<DimensionProps>();
    public List<bool> isSecronByID = new List<bool>();
    public List<Transform> referance = new List<Transform>();
    public bool isSecronized;

    private void Start()
    {
      
    }
    private void Update()
    {
        if(isSecronByID.Count > 0) 
        {
            for (int i = 0; i < isSecronByID.Count; i++)
            {
                if (!isSecronByID[i]) 
                {
                    isSecronized = false;
                    break;
                }
                else 
                {
                    if (i == isSecronByID.Count-1) isSecronized = true;
                }
            }
        }
        SeperateObjectsByID();
    }
    public void AddPropList(DimensionProps proplist)
    {
        RemovePropList();
        if (dimensionProps.Count > 0)
        {
            for (int i = 0; i < dimensionProps.Count; i++)
            {
                if (dimensionProps[i] != proplist) dimensionProps.Add(proplist);
            }
        }
        else
        {
            dimensionProps.Add(proplist);
        }
    }
    public void RemovePropList()
    {
        if (dimensionProps.Count > 2)
        {
            for (int i = 0; i < dimensionProps.Count; i++)
            {
                dimensionProps.Remove(dimensionProps[i]);
            }
        }
    }
    public void SeperateObjectsByID()
    {
        for (int i = 0; i < dimensionProps.Count; i++)
        {
            if (i == 0)
            {
                for (int p = 0; p < dimensionProps[i].props.Count; p++)
                {
                    if (dimensionProps[i].props[p].GetComponent<PropID>().ID == dimensionProps[i + 1].props[p].GetComponent<PropID>().ID)
                    {
                        dimensionProps[i].props[p].transform.name = dimensionProps[i].props[p].GetComponent<PropID>().ID.ToString();
                        dimensionProps[i + 1].props[p].transform.name = dimensionProps[i].props[p].GetComponent<PropID>().ID.ToString()+ dimensionProps[i].props[p].GetComponent<PropID>().ID.ToString();
                        TakeItemPositionsByID(dimensionProps[i].props[p].GetComponent<PropID>().ID);
                    }
                }
            }

        }
    }
    public void TakeItemPositionsByID(int id)
    {
        
        List<Transform> itemPosById = new List<Transform>();
        if(itemPosById.Count < 2) 
        {
            for (int i = 0; i < dimensionProps.Count; i++)
            {
                for (int p = 0; p < dimensionProps[i].props.Count; p++)
                {
                    if (dimensionProps[i].props[p].GetComponent<PropID>().ID == id)
                    { 
                        itemPosById.Add(dimensionProps[i].props[p].transform);
                        if(referance.Count <= id) 
                        {
                            referance.Add(dimensionProps[0].props[p].transform);
                        }
                        
                        
                    }
                }
            }
            if(isSecronByID.Count > id) 
            {
                isSecronByID[id] = CheckItemPositionsByID(itemPosById);               
            }
            else 
            {
                isSecronByID.Add(CheckItemPositionsByID(itemPosById));              
            }
            
        }
        
    }
    public bool CheckItemPositionsByID(List<Transform> propListByID) 
    {
        for (int i = 0; i < propListByID.Count; i++)
        {
            for (int p = 0; p < propListByID.Count; p++)
            {
                if (i != p)
                {
                    if (propListByID[p].position.x - propListByID[i].position.x <= 0.25f && propListByID[p].position.x - propListByID[i].position.x >= -0.25f)
                    {
                        return true;
                    }
                    if (propListByID[p].GetComponent<DragObjects>().cantDrag && propListByID[i].GetComponent<DragObjects>().cantDrag) return true;

                }
            }

        }
        return false;
    }

   
 
    
}
