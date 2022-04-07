using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverText : MonoBehaviour
{

    public float delay = 0.1f;
    public string[] fullText;
    string currentText = "";
    // Use this for initialization
    void Start()
    {
        StartCoroutine(ShowText());
    }

    // Update is called once per frame
    IEnumerator ShowText()
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
            if (p == fullText.Length - 1) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
}
