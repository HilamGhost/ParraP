using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFinder : MonoBehaviour
{
    [Header("IsPosition")]
    public bool playerInPosition;
    
    [Header("Lists")]
    public List<Transform> playerPositions = new List<Transform>();
    

    // Update is called once per frame
    void Update()
    {
        playerInPosition = isPlayersPositionEqual();
    }
    public void AddPlayerController(Transform player) 
    {
        ClearVariables();
        playerPositions.Add(player);        
    }
    public void ClearVariables()
    {
        if (playerPositions.Count > 2) 
        {
            for (int i = 0; i < playerPositions.Count; i++)
            {
                playerPositions.Remove(playerPositions[i]);
            }
        }
        
    }
    public bool isPlayersPositionEqual()
    {
        for (int i = 0; i < playerPositions.Count; i++)
        {
            for (int p = 0; p < playerPositions.Count; p++)
            {
                if (i != p)
                {
                    if (playerPositions[p].position.x - playerPositions[i].position.x <= 0.25f && playerPositions[p].position.x - playerPositions[i].position.x >= -0.25f)
                    {
                        return true;
                    }

                }
            }

        }
        return false;
    }
   
    
}
