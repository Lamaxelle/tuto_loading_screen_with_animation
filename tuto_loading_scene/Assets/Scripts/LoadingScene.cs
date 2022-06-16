using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{

    // Boolean qui enregistre si une transition vient d'être faite
    public static bool fromLoadingScene = false;

    // Canvas de l'animation d'apparition
    public GameObject appearanceTransitionObject;

    void Start()
    {
        // Permet de charger une scène uniquement lorque l'on est dans la scène de chargement
        if (SceneManager.GetActiveScene().name=="Loading_Scene")
        {
            StartCoroutine (LoadAsyncOperation());
        }
    }

    void Update ()
    {
        // Active l'animation d'apparition
        if (fromLoadingScene && appearanceTransitionObject!=null)
        {
            if (!appearanceTransitionObject.activeInHierarchy)
            {
                appearanceTransitionObject.SetActive(true);
            }
        }
    }

    // Dit ce qu'il faut faire à la fin de l'animation d'apparition
    public void Appearance ()
    {
        fromLoadingScene = false;
        if (appearanceTransitionObject!=null)
        {
            appearanceTransitionObject.SetActive(false);
        }
        
    }

    // Enregistre le fait que l'on va avoir besoin d'une animation d'apparition à la prochaine scène
    public void Disappearance ()
    {
        fromLoadingScene = true;
    }

    // Charge la scene dont on lui donne le nom
    public void NextScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
        Disappearance ();
    }

    // Enregistre le nom d'une scene et le garde entre chaque scene tant qu'il n'est pas changé
        public void SceneName (string otherSceneName)
    {
        LoadingSceneDatas.LoadingSceneName = otherSceneName;
    }


    // Permet de charger une scène uniquement lorque l'on est dans la scène de chargement
    IEnumerator LoadAsyncOperation ()
    {
        if (SceneManager.GetActiveScene().name=="Loading_Scene")
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(LoadingSceneDatas.LoadingSceneName);
            while (asyncOperation.progress<0.9f)
            {
                asyncOperation.allowSceneActivation = false;
                yield return null;
            }
            if (asyncOperation.progress>=0.9f)
            {
                asyncOperation.allowSceneActivation = true;
                yield return null;
            }
        }
    }

}

public static class LoadingSceneDatas
{
    private static string loadingSceneName;
    public static string LoadingSceneName
    {
        get
        {
            return loadingSceneName;
        }
        set
        {
            loadingSceneName = value;
        }
    }
}
