using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusketUI : MonoBehaviour
{
    public GameObject musket;
    float alpha;

    public void Start()
    {
    }

    public void ChangeMusket(bool isActive)
    {
        if (isActive) 
        {
            musket.SetActive(true);
        }
        else 
        {
            musket.SetActive(false);
        }
    }
}
