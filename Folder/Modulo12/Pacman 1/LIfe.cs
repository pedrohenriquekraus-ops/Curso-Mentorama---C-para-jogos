using System;
using UnityEngine;

public class LIfe : MonoBehaviour
{

    public int Lives;
    public event Action<int> OnLifeRemoved;




    public void RemoveLife()
    {
        Lives--;
        OnLifeRemoved?.Invoke(Lives);
    }

}
