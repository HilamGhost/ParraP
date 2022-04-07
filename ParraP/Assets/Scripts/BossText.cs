using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossText : MonoBehaviour
{
    public float delay = 0.1f;
    string currentText = "";
    // Update is called once per frame
    public IEnumerator ShowText(string fullText,BossManager boss, bool isDead)
    {        
            for (int i = 0; i < fullText.Length; i++)
            {
                currentText = fullText.Substring(0, i);
                this.GetComponent<Text>().text = currentText;
                yield return new WaitForSeconds(delay);
            }
            currentText = null;
      
        if (isDead)
        {
            StartCoroutine(LoseText(boss.tryAgainQuote, boss));
        }
        else 
        {
            if (boss.SafeZones.Count > 0 && !isDead)
            {
                StartCoroutine(boss.Spawn());
            }
            if (boss.SafeZones.Count <= 0 )
            {
                boss.BG.Stop();
                StartCoroutine(WinText(boss.goodEndQuotes, boss));
            }
        }
       
            

    }
    public IEnumerator StartText(string[] fullText, BossManager boss)
    {
        yield return new WaitForSeconds(2);
        for (int f = 0; f < fullText.Length; f++)
        {
            for (int i = 0; i < fullText[f].Length; i++)
            {
                currentText = fullText[f].Substring(0, i);
                this.GetComponent<Text>().text = currentText;
                yield return new WaitForSeconds(delay);
            }
            currentText = null;
            yield return new WaitForSeconds(2);
        }
        StartCoroutine(ShowText(" O halde sonuna hazýr ol! ", boss, false));

    }
    public IEnumerator WinText(string[] fullText, BossManager boss)
    {
        
            yield return new WaitForSeconds(2);
            for (int f = 0; f < fullText.Length; f++)
            {
                for (int i = 0; i < fullText[f].Length; i++)
                {
                    currentText = fullText[f].Substring(0, i);
                    this.GetComponent<Text>().text = currentText;
                    yield return new WaitForSeconds(delay);
                }
                currentText = null;
                yield return new WaitForSeconds(2);
            }
            boss.WinGame();
        
       
        
    }
    public IEnumerator LoseText(string fullText, BossManager boss)
    {
        yield return new WaitForSeconds(2);
        for (int f = 0; f < fullText.Length; f++)
        {
            
                currentText = fullText.Substring(0, f);
                this.GetComponent<Text>().text = currentText;
                yield return new WaitForSeconds(delay);
                            
        }
        currentText = null;
        yield return new WaitForSeconds(2);
        boss.LoseGame();

    }
}
