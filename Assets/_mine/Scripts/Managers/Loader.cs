using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private class loaderMonoBehaviour: MonoBehaviour{ }

    public enum Scenes
    {
        Menu,
        Level_1,
        Level_2,
        Level_3,
        Level_4,
        Level_5,
        Loading
    }

    private static Action onLoaderCallBack;
    private static AsyncOperation operation;
    
    
    public static void Load(Scenes scene)
    {
        onLoaderCallBack = () =>
        {
            GameObject loaderGameObject = new GameObject("Loading Game Object");
            loaderGameObject.AddComponent<loaderMonoBehaviour>().StartCoroutine(loadSceneAsync(scene));
        };
        
        SceneManager.LoadScene(Scenes.Loading.ToString());
        
    }

    public static Scenes GetCurrentScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        Enum.TryParse(activeScene.name, out Scenes currScene);

        return currScene;
    }

    public static Scenes GetNextLevel()
    {
        Scenes currScene = GetCurrentScene();
        Scenes nextScene = currScene.Next();
        if (nextScene == Scenes.Loading)
        {
            return Scenes.Menu;
        }
        
        return nextScene;
    }
    
    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length==j) ? Arr[0] : Arr[j];            
    }

    private static IEnumerator loadSceneAsync(Scenes scene)
    {
        yield return null;
                
        operation = SceneManager.LoadSceneAsync(scene.ToString());

        if (operation == null)
        {
            SceneManager.LoadScene(Scenes.Menu.ToString());
        }        
        
        while (!operation.isDone)
        {
            yield return null;
        }

        operation = null;
    }

    public static float GetLoadingProgress()
    {
        if (operation == null)
            return 1f;

        return operation.progress;

    }

    public static void LoaderCallBack()
    {
        onLoaderCallBack?.Invoke();
        onLoaderCallBack = null;
    }
}
