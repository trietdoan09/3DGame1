using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Image progressBar;
    public float progress;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingBar());
    }

    IEnumerator LoadingBar()
    {
        while (progress < 1)
        {
            progress += Random.RandomRange(0f, 3f) * Time.deltaTime;
            progressBar.fillAmount = progress;
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene("SampleScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
