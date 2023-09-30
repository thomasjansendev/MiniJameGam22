using System;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Delay
{
    public static async void Method(Action method, float delay)
    {
        await Task.Delay((int)delay * 1000); // expects milliseconds 
        method();
    }

    public static async void DestroyObj(GameObject obj, float delay)
    {
        await Task.Delay((int)delay * 1000); // expects milliseconds 
        Object.Destroy(obj);
    }
}