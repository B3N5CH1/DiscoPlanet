/**
*   Filename: IceCreamSapwner.cs
*   Author: Flückiger, Graf
*   
*   Description:
*       This script is used to enable the Ice Cream.
*   
**/
using UnityEngine;


public class IceCreamSpawner : MonoBehaviour
{

    public GameObject _IceCream;

    public void activateIceCream()
    {
        _IceCream.SetActive(true);
    }



}