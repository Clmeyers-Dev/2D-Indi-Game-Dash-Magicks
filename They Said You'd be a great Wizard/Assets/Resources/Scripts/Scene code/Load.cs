using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using System;
public static class Load
{

   
    private static Action onLoaderCallBack;
    public static void LoadThis(SceneEnum scene){
       
        onLoaderCallBack = ()=>{

        SceneManager.LoadScene(scene.ToString());
        };
        SceneManager.LoadScene(SceneEnum.Loading.ToString());
    }
 public static void LoaderCallback(){
     if(onLoaderCallBack !=null){
         onLoaderCallBack();
         onLoaderCallBack = null;
     }
 }
}
 public enum SceneEnum{
       
        Loading,
        
    }