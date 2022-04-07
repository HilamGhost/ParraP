using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public CutsceneText text;
    public string[] startQuotes;
    public string[] endQuotes;
    public PlayerController playerController;
    bool canPlay;
    public bool isInside;
    public static AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        StartCutscene();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canPlay) playerController.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        if (!canPlay) playerController.isMoving = false;
        playerController.enabled = canPlay;
    }
    public void StartCutscene() 
    {
        Camera.main.transform.position = new Vector3(0, 0, 0);
        canPlay = false;
        StartCoroutine(text.StartCutscene(startQuotes,this));
       
        
    }
    public void ContinueCutScene() 
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);
        canPlay = true;       
    }
    public IEnumerator FinishCutscene()
    {
        if(!audioSource.isPlaying)audioSource.Play();
        playerController.transform.GetComponent<Animator>().SetTrigger("Stop");
        canPlay = false;
        yield return new WaitForSeconds(audioSource.clip.length);
        Camera.main.transform.position = new Vector3(0, 0, 0);
        
       
        StartCoroutine(text.FinishCutscene(endQuotes, this));

    }
    public void EndCutscene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
