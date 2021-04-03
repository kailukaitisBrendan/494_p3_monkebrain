using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public void LoadScene(string scene) {
        StartCoroutine(OnAnimationComplete(scene));
    }

    IEnumerator OnAnimationComplete(string scene)
    {
        GetComponent<Animator>().SetTrigger("LoadScene");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}
