using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;


public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [Header("Split")]
    public int splitCount = 0;
    public List<GameObject> Dimensions = new List<GameObject>();
    public GameObject dimension_object,dimensions_parent;
    public LayerMask propLayer;
    public Volume ppVolume;
    public VolumeProfile[] ppVolumeProfiles;
    public VolumeProfile transProfile;
    public float transision_lenght;
    Animator cameraAnimator;
    AudioSource audioSource,bg;
    public AudioClip changeDimesion_audioClip;
    public AudioClip isNotStable;
    bool isDead;




    [Header("Stable Situation")]
    public bool canConnect;
    public bool isStabile;
    public bool isStabileByProps;
    public bool playerIsInside;

    [Header("Stabiliy Bar")]
    public float stability = 120;
    float stability_reset;
    public float stability_decrease_multipler;
    public float splitStabilityMulpiter = 3.5f;
    public Slider stability_bar;
    public MusketUI musket_ui;
    public Image Vingette;
    bool gameOverTrans;
    
    [Header("GameOverThings")]
    public GameObject gameOverText;
    public GameObject gameWinText;

    [Header("NoPlace")]
    public bool haveNoPlace;

    [Header("Random Encounter")]
    public PropList propList;
    public int maxPropNumber;
    public int howManyPropsInScene;
    public List<int> randomInt = new List<int>();
    public List<int> randInts = new List<int>();
    public List<int> r = new List<int>();

    [SerializeField] GameObject[] Props;
    [SerializeField] GameObject[] Players;
    [SerializeField] GameObject[] Ega;
    [SerializeField] List<Transform> playerPoses = new List<Transform>();
    
    public List<bool> isSecronByID = new List<bool>();

    void Awake()
    {
        gameManager = this;   
    }
    // Start is called before the first frame update
    void Start()
    {
        Dimensions.Add(dimension_object);
        stability_reset = stability;
        stability_bar.maxValue = stability_reset;
        GetMaxPropNumber();
        if (haveNoPlace) GroundDestroyer();
        Invoke("FreezeProps", 1.5f);
        gameOverText.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        //VisualThings
        cameraAnimator = Camera.main.transform.GetComponent<Animator>();
        bg = ppVolume.transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (playerIsInside)
            {
                if (canConnect) 
                {
                    switch (splitCount)
                    {
                        case 0:
                            GameWon();
                            break;
                        case 1:

                            ConnectDimensions();
                            break;
                    }
                }
                else 
                {
                    switch (splitCount)
                    {
                        case 0:
                            SplitDimensions();
                            break;
                        case 1:

                            SplitDimensions();
                            break;
                    }
                }
            }
            else 
            {
                switch (splitCount)
                {
                    case 0:
                        SplitDimensions();
                        break;
                    case 1:

                        SplitDimensions();
                        break;
                }
            }
                
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            
        }
        
        isStabile = hasStability();
        isStabileByProps = HasStabilityByProps();

        /////STABILITY
        stability -= Time.deltaTime * stability_decrease_multipler;

        if (isStabile) stability_decrease_multipler = 1 * (splitCount + 1);
        else stability_decrease_multipler = 2 * (splitCount + 1);

        if (stability <= 0)
        {
            switch (splitCount) 
            {
                case 0:
                    SplitDimensions();                                        
                    break;
                case 1:
                    GameOver();
                    break;

            }
        }
        ////////////////

        ///FOR UI
        stability_bar.value = stability;
        musket_ui.ChangeMusket(canConnect);

        if(!isDead)
        {
            if (isStabile)
            {
                Vingette.color = new Color(255, 255, 255, 0);
                audioSource.clip = null;
                audioSource.loop = false;
                if (audioSource.clip != null) audioSource.Stop();
            }
            else
            {
                Vingette.color = new Color(255, 255, 255, 255);
                if (audioSource.clip == null)
                {
                    audioSource.clip = isNotStable;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }
        else 
        { 
            Vingette.color = new Color(255, 255, 255, 0);
            if (audioSource.clip != null) audioSource.Stop();
        }
       
        if (gameOverTrans) StartCoroutine(TransGameOver());



        ///////////////////////////
        ///FOR GAME END
        /////////////////////////

        if (isStabile && isStabileByProps) canConnect = true; else canConnect = false;
        ////////////////////////
        if (playerPoses.Count > 0) 
        {
            for (int i = 0; i < playerPoses.Count; i++)
            {
                if (playerPoses[i] == null) playerPoses.Remove(playerPoses[i]);
            }
        }
        DimensionsPropsEqualCheck();
        

    }
    public void SplitDimensions() 
    {
        splitCount++;
        
        
        //VISUAL AND AUDIO THINGS
        StartCoroutine(ChangePP());
        
        switch (splitCount) 
        {
            case 1:
                for (int i = 0; i < Dimensions.ToArray().Length; i++)
                {
                    Dimensions[i].transform.localScale = new Vector3(0.5f,1,1);
                    Dimensions[i].transform.position = new Vector3(3.55f, Dimensions[i].transform.position.y, Dimensions[i].transform.position.z);
                    if(i == Dimensions.ToArray().Length - 1)                       
                    {   
                        GameObject newDimention = Instantiate(dimension_object, Dimensions[i].transform.position - new Vector3((3.55f*2), 0,0),Quaternion.identity,dimensions_parent.transform);                 
                        Dimensions.Add(newDimention);
                        break;
                        
                    }
                }
                UpdateProps();
                UpdatePlayerPos(true);
                for (int i = 0; i < Props.Length; i++)
                {
                    Props[i].transform.localScale = new Vector3(Props[i].transform.localScale.x, Props[i].transform.localScale.y/2, Props[i].transform.localScale.z);                  
                }
                for (int i = 0; i < Players.Length; i++)
                {
                    Players[i].transform.localScale = new Vector3(Players[i].transform.localScale.x, Players[i].transform.localScale.y / 2, Players[i].transform.localScale.z);
                    Players[i].GetComponent<PlayerController>().JumpSpeed = Players[i].GetComponent<PlayerController>().JumpSpeed + 0.5f;
                }
                for (int i = 0; i < Ega.Length; i++)
                {
                    Ega[i].transform.localScale = new Vector3(Ega[i].transform.localScale.x, Ega[i].transform.localScale.y / 2, Ega[i].transform.localScale.z);
                }
                UnFreeze();
                
                stability_reset = stability_reset * splitStabilityMulpiter;
                stability_bar.maxValue = stability_reset;
                stability = stability_reset;
                
                break;
            case 2:
                GameOver();
                break;
        }
      
    }
    public void ConnectDimensions()
    {
        
        //VISUAL AND AUDIO THINGS
        StartCoroutine(ChangePP());
        

        if (splitCount == 1) 
           {
                for (int i = 0; i < Dimensions.ToArray().Length; i++)
                {
                    Dimensions[i].transform.localScale = new Vector3(1, 1, 1);
                    Dimensions[i].transform.position = new Vector3(Dimensions[i].transform.position.x-3.55f, Dimensions[i].transform.position.y, Dimensions[i].transform.position.z);
                    if (i == Dimensions.ToArray().Length - 1)
                    {
                    Destroy(Dimensions[i]);
                    Dimensions.Remove(Dimensions[i]);
                    }
                }
                 UpdateProps();
                 UpdatePlayerPos(false);
                 
                for (int i = 0; i < Props.Length; i++)
                {
                    Props[i].transform.localScale = new Vector3(Props[i].transform.localScale.x, Props[i].transform.localScale.y * 2, Props[i].transform.localScale.z);
                  
                }
               
                for (int i = 0; i < Players.Length; i++)
                {
                    Players[i].transform.localScale = new Vector3(Players[i].transform.localScale.x, Players[i].transform.localScale.y * 2, Players[i].transform.localScale.z);
                Players[i].GetComponent<PlayerController>().JumpSpeed = Players[i].GetComponent<PlayerController>().JumpSpeed - 0.5f;
            }
               
                for (int i = 0; i < Ega.Length; i++)
                {
                     Ega[i].transform.localScale = new Vector3(Ega[i].transform.localScale.x, Ega[i].transform.localScale.y * 2, Ega[i].transform.localScale.z);
                }
                splitCount--;
           }
           
    }
    public void UpdateProps() 
    {


        Props = null;
        GameObject[] newProps = GameObject.FindGameObjectsWithTag("Props") as GameObject[];
        Props = newProps;


        Players = null;
        GameObject[] newPlayers = GameObject.FindGameObjectsWithTag("Player") as GameObject[];
        Players = newPlayers;

        Ega = null;
        GameObject[] newEga= GameObject.FindGameObjectsWithTag("EndGameArea") as GameObject[];
        Ega = newEga;

    } 
    public bool hasStability() 
    {
        if (Dimensions.Count > 1)
        {
            for (int i = 0; i < Dimensions.ToArray().Length; i++)
            {
                for (int p = 0; p < Dimensions.Count; p++)
                {
                    if (i != p)
                    {
                        if (Dimensions[i].GetComponent<PositionFinder>().playerInPosition && Dimensions[p].GetComponent<PositionFinder>().playerInPosition)
                        {
                            if (IsDimensionsPlayerEqual())
                            {
                                return true;
                            }

                        }
                    }
                }
            }

            return false;
        }
        else
        {
            return Dimensions[0].GetComponent<PositionFinder>().playerInPosition;
        }
    }
    public bool IsDimensionsPlayerEqual() 
    {

        if (splitCount > 0)
        {

            for (int i = 0; i < playerPoses.Count; i++)
            {
                for (int p = 0; p < playerPoses.Count; p++)
                {
                    if (i != p)
                    {
                        if (playerPoses[p].localPosition.x - playerPoses[i].localPosition.x <= 0.25f && playerPoses[p].localPosition.x - playerPoses[i].localPosition.x >= -0.25f) return true;
                    }
                }
            }
            return false;
        }
        else
        {
            return false;
        }

    }
    public bool HasStabilityByProps() 
    {
        if (Dimensions.Count > 1)
        {
            for (int i = 0; i < Dimensions.ToArray().Length; i++)
            {
                for (int p = 0; p < Dimensions.Count; p++)
                {
                    if (i != p)
                    {
                        if (Dimensions[i].GetComponent<PropController>().isSecronized && Dimensions[p].GetComponent<PropController>().isSecronized)
                        {
                            #region Secronized Setting
                            bool isSecronized = false;
                            if (isSecronByID.Count > 0)
                            {
                                for (int j = 0; j < isSecronByID.Count; j++)
                                {
                                    if (!isSecronByID[j])
                                    {
                                        isSecronized = false;
                                        break;
                                    }
                                    else
                                    {
                                        if (j == isSecronByID.Count - 1) isSecronized = true;
                                    }
                                }
                            }
                            #endregion
                            if (isSecronized) return true;

                        }
                    }
                }
            }

            return false;
        }
        else
        {
            return Dimensions[0].GetComponent<PropController>().isSecronized;
        }
    }
    #region Checks Two Sides Dimension
    public void DimensionsPropsEqualCheck()
    {

        if (splitCount > 0)
        {

            for (int i = 0; i < Dimensions.ToArray().Length; i++)
            {
                if (i == 0) 
                {
                    for (int p = 0; p < Dimensions[i].GetComponent<PropController>().referance.Count; p++)
                    {
                        if(Dimensions[i].GetComponent<PropController>().referance[p].GetComponent<PropID>().ID == Dimensions[i + 1].GetComponent<PropController>().referance[p].GetComponent<PropID>().ID) 
                        {
                            SplitReferancePropsByID(Dimensions[i].GetComponent<PropController>().referance[p].GetComponent<PropID>().ID);
                        }
                    }

                }

            }

        }

    }
    public void SplitReferancePropsByID(int ID)
    {

        List<Transform> itemPosById = new List<Transform>();
        if (itemPosById.Count < 2 )
        {
            for (int i = 0; i < Dimensions.Count; i++)
            {
                for (int p = 0; p < Dimensions[i].GetComponent<PropController>().referance.Count; p++)
                {
                    if (Dimensions[i].GetComponent<PropController>().referance[p].GetComponent<PropID>().ID == ID)
                    {
                        itemPosById.Add(Dimensions[i].GetComponent<PropController>().referance[p]);
                    }
                }
            }
            
            
        }
        if (isSecronByID.Count > ID)
        {
            isSecronByID[ID] = CheckItemPositionsByID(itemPosById);
        }
        else
        {
            isSecronByID.Add(CheckItemPositionsByID(itemPosById));
        }
       
        

    }
    bool CheckItemPositionsByID(List<Transform> propListByID) 
    {
       
            for (int i = 0; i < propListByID.Count; i++)
            {
                for (int p = 0; p < propListByID.Count; p++)
                {
                    if (i != p)
                    {
                        if (propListByID[p].localPosition.x - propListByID[i].localPosition.x <= 0.35f && propListByID[p].localPosition.x - propListByID[i].localPosition.x >= -0.35f)
                        {
                            return true;
                        }

                    }
                }

            }
            return false;   
        
    }
    #endregion
    public void UpdatePlayerPos(bool add) 
    {
        if (add) 
        {
            for (int i = 0; i < Dimensions.Count; i++)
            {
                playerPoses.Add(Dimensions[i].GetComponent<PositionFinder>().playerPositions[0]);
            }
        }
        else 
        {
            for (int i = 0; i < Dimensions.Count; i++)
            {
                playerPoses.Remove(Dimensions[i].GetComponent<PositionFinder>().playerPositions[0]);
            }
        }
       
    }
    #region NoPlace
    void GroundDestroyer()
    {
        for (int i = 0; i < Dimensions[0].GetComponent<PropController>().dimensionProps[1].props.Count; i++)
        {
            Dimensions[0].GetComponent<PropController>().dimensionProps[1].props[i].GetComponent<DragObjects>().noGround = true;
        }
       
    }
    #endregion
    #region Random
    void GetMaxPropNumber() 
       {
           maxPropNumber = propList.propList.Count;
             for (int i = 0; i < howManyPropsInScene; i++)
             {
                 int r = Random.Range(0, maxPropNumber);
                while (randomInt.Contains(r)) 
                {
                    r= Random.Range(0, maxPropNumber);
                }
                 randomInt.Add(r);
                                                    
             }
        randomInt.Sort();
        SpawnObjects();
       }

       void SpawnObjects() 
       {
            for (int i = 0; i < Dimensions.Count; i++)
            {
                for (int p = 0; p < Dimensions[i].GetComponent<PropController>().dimensionProps.Count; p++)
                {
                    Dimensions[i].GetComponent<PropController>().dimensionProps[p].SpawnProps(propList, randomInt);
                }
            }
        UpdateProps();
        
       }
      void FreezeProps() 
      {
        
            if (Props.Length < 3)
            {
                int i = Random.Range(0, Props.Length);
                Props[i].GetComponent<DragObjects>().ChangeCantDrag();
        }
            else
            {
                int i = Mathf.RoundToInt(Props.Length / 3);
                if (i % 2 == 0) i -= 1;

                for (int p = 0; p < i; p++)
                {
                    int r = Random.Range(0, Props.Length);
                    while (randInts.Contains(r))
                    {
                        r = Random.Range(0, Props.Length);
                    }
                    randInts.Add(r);
                }
                randInts.Sort();
                for (int k = 0; k < randInts.Count; k++)
                {
                        Props[randInts[k]].GetComponent<DragObjects>().ChangeCantDrag();
                        r.Add(k);
                }
            }
        }       
        void UnFreeze() 
        {
             if (splitCount == 1)
             {
                for (int i = 0; i < r.Count; i++)
                {
                    StartCoroutine(Props[randInts[r[i]]].GetComponent<DragObjects>().FixFreeze());
                }
                
             }
        }
      

    #endregion
    public void GameOver() 
    {
       
        splitCount--; 
        Vingette.color = new Color(255, 255, 255, 0);
        ChangePPGameOver();
        cameraAnimator.SetTrigger("GameOver");
        for (int i = 0; i < Props.Length; i++)
        {
            Props[i].GetComponent<DragObjects>().enabled=false;
        }
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].GetComponent<PlayerController>().enabled = false;
        }
        stability = 0;
        Camera.main.transform.position = Vector3.zero;
        stability_bar.gameObject.SetActive(false);
        musket_ui.gameObject.SetActive(false);
        gameOverText.SetActive(true);
        bg.Stop();
    }
    public void GameWon() 
    {
        bg.Stop();
        isDead = true;
        Vingette.color = new Color(255, 255, 255, 0);
        ChangePPGameOver();
        cameraAnimator.SetTrigger("GameOver");
        playerIsInside = false;
        stability = stability_reset*10000000;
        Camera.main.transform.position = Vector3.zero;

        Debug.LogError("GAME WON");
        stability_bar.gameObject.SetActive(false);
        musket_ui.gameObject.SetActive(false);
        gameWinText.SetActive(true);
    }
    public void ChangeScene() 
    {
        Debug.Log("OLDU");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    #region VisualThings
    public IEnumerator ChangePP() 
    {
        audioSource.Stop();
        audioSource.PlayOneShot(changeDimesion_audioClip);
        cameraAnimator.SetTrigger("ChangeDimension");
        ppVolume.profile = transProfile;
        yield return new WaitForSeconds(transision_lenght);
        ppVolume.profile = ppVolumeProfiles[splitCount]; 
    }
    public void ChangePPGameOver()
    {

        audioSource.Stop();
        audioSource.PlayOneShot(changeDimesion_audioClip);

        gameOverTrans = true;
    }
    IEnumerator TransGameOver() 
    {
        ppVolume.profile = transProfile;
        yield return new WaitForSeconds(transision_lenght);
        ppVolume.profile = ppVolumeProfiles[1];
    }
    #endregion
}
