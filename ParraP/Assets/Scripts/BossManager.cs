using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class BossManager : MonoBehaviour
{
    [Header("isDead")]
    public bool winnedGame;
    public bool lostGame;
    bool isDead;

    [Header("Start")]
    public GameObject env;
    public GameObject mirror;
    public Volume ppVolume;
    public VolumeProfile bossProfile;
    public List<GameObject> SafeZones = new List<GameObject>();
    public List<GameObject> Props = new List<GameObject>();
    public GameObject ground;

    public int randomInt;
    public int howManyProps;
    public int phase;
    public AudioSource EvilSoundSource,BG;
    public AudioClip randomSZ, allSZ,glitch;

    PropList propList;
    Coroutine co;

    [Header("Text")]
    public BossText bossText;
    public string beginQuote;
    public string[] startQuotes;
    public string[] winQuotes;   
    public string[] loseQuotes;
    public string[] goodEndQuotes;
    public string tryAgainQuote;

    [Header("Spawn Random")]
    public Vector3 maxDistance = new Vector3(6f, 0.5f, 0);
    public Vector3 minDistance = new Vector3(-6f, -0.5f, 0);
    // Start is called before the first frame update
    void Start()
    {
        propList = GetComponent<PropList>();
        StartCoroutine(StartFight());
    }

    // Update is called once per frame
    void Update()
    {
            if(lostGame)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
            if (winnedGame) 
            {
            
            }            
    }
    public IEnumerator StartFight() 
    {
        StartCoroutine(bossText.StartText(startQuotes, this));
        yield return new WaitForSeconds(25);
        Destroy(env);
        yield return new WaitForSeconds(2);
        mirror.SetActive(true);
        EvilSoundSource.Play();
        yield return new WaitForSeconds(1);
        ppVolume.profile = bossProfile;




    }
    public IEnumerator Spawn() 
    {
        if (!isDead) 
        {
            if (phase == 0) BG.Play();
            EvilSoundSource.Play();
            OpenSafeZones(true);
            yield return new WaitForSeconds(3);

            RandomSafeZones();
            SpawnProps();

            yield return new WaitForSeconds(7);
            FreezeProps();

            ground.SetActive(false);
            yield return new WaitForSeconds(3);

            DestroyProps();
            OpenSafeZones(false);
            ground.SetActive(true);
            DestroySafeZone();
            phase++;
            StartCoroutine(bossText.ShowText(winQuotes[phase], this, false));
        } 
        


    }
    void OpenSafeZones(bool isSpawn) 
    {
        if (isSpawn) 
        {

            for (int i = 0; i < SafeZones.Count; i++)
            {
                SafeZones[i].SetActive(true);
                SafeZones[i].GetComponent<AudioSource>().PlayOneShot(allSZ);
            }
        }
        else 
        {
            for (int i = 0; i < SafeZones.Count; i++)
            {
                SafeZones[i].SetActive(false);
            }
        }
        
    }
    void DestroySafeZone() 
    {
        if (SafeZones.Count > 0)
        {
            Destroy(SafeZones[randomInt]);
            SafeZones.Remove(SafeZones[randomInt]);
        }

    }
    void RandomSafeZones() 
    {
        int p = Random.Range(0, SafeZones.Count);
        for (int i = 0; i < SafeZones.Count; i++)
        {
            if (i == p)
            {
                SafeZones[i].SetActive(true);
                randomInt = i;
                SafeZones[i].GetComponent<AudioSource>().PlayOneShot(randomSZ);
            }
            else SafeZones[i].SetActive(false);

        }
    }
    void SpawnProps() 
    {
        for (int a = 0; a < howManyProps; a++)
        {
            int p = Random.Range(0, propList.propList.Count);
            for (int i = 0; i < propList.propList.Count; i++)
            {
                if (i == p)
                {
                    float x = Random.Range(minDistance.x, maxDistance.x);
                    float y = Random.Range(minDistance.y, maxDistance.y);
                    Vector3 spawnPoint = new Vector3(x, y, 0);
                    GameObject newProp = Instantiate(propList.propList[i], transform.position ,Quaternion.identity);
                    newProp.GetComponent<DragObjects>().noGround = true;
                    
                    newProp.transform.localPosition = spawnPoint;
                    Props.Add(newProp);
                }

            }
        }
        
    }
    void DestroyProps() 
    {
        int p = Props.Count;
        for (int i = 0; i < p; i++)
        {
            GameObject nGO = Props[i];           
            Destroy(nGO);     
        }
        howManyProps--;
        Props.Clear();
    }
    void FreezeProps() 
    {
        for (int i = 0; i < Props.Count; i++)
        {
           Props[i].GetComponent<DragObjects>().ChangeCantDrag();
           Props[i].GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    public void Death() 
    {
        BG.Stop();
        EvilSoundSource.PlayOneShot(glitch);
        isDead = true;
        StartCoroutine(bossText.ShowText(loseQuotes[phase], this, true));
        DestroyProps();
        OpenSafeZones(false);
        

    }
    public void WinGame()
    {
        if (!isDead)
        {
            Debug.Log("Win");
            winnedGame = true;
        }
        else
        {
            LoseGame();
        }
       
    }
    public void LoseGame() 
    {
        Debug.Log("Lose");
        lostGame = true;
    }
}
