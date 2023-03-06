using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scenes
    {
        Menu,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Loading
    }
    
    public static void Load(Scenes scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
