using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CutsceneText : MonoBehaviour
{
    public float delay = 0.1f;
    string currentText = "";

    public AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    IEnumerator ShowText(string[] fullText)
    {
        for (int p = 0; p < fullText.Length; p++)
        {
            for (int i = 0; i < fullText[p].Length; i++)
            {
                currentText = fullText[p].Substring(0, i);
                this.GetComponent<Text>().text = currentText;
                yield return new WaitForSeconds(delay);
            }
            currentText = null;
            yield return new WaitForSeconds(2);

        }
    }
    public IEnumerator StartCutscene(string[] fullText, CutsceneManager cutscene) 
    {
        audioSource.Play();
        for (int p = 0; p < fullText.Length; p++)
        {
            for (int i = 0; i < fullText[p].Length; i++)
            {
                currentText = fullText[p].Substring(0, i);
                this.GetComponent<Text>().text = currentText;
                yield return new WaitForSeconds(delay);
            }
            currentText = null;
            yield return new WaitForSeconds(2);
            if (p == fullText.Length - 1) cutscene.ContinueCutScene();
        }
        audioSource.Stop();
    }
    public IEnumerator FinishCutscene(string[] fullText, CutsceneManager cutscene)
    {
        audioSource.Play();
        for (int p = 0; p < fullText.Length; p++)
        {
            for (int i = 0; i < fullText[p].Length; i++)
            {
                currentText = fullText[p].Substring(0, i);
                this.GetComponent<Text>().text = currentText;
                yield return new WaitForSeconds(delay);
            }
            currentText = null;
            yield return new WaitForSeconds(2);
            if (p == fullText.Length - 1) cutscene.EndCutscene();
        }
        audioSource.Stop();
    }
}
