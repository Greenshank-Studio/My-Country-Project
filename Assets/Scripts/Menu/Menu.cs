using UnityEngine;
using System;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject SelectBuildingMenu;
    public static event Action<bool> onMenuOpen;
    // [SerializeField] private GameObject _cameraRig;

    public void OpenSelectBuildingMenu(){
        // Debug.Log("HELLOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        if (SelectBuildingMenu.activeInHierarchy == false){
            SelectBuildingMenu.SetActive(true);
            // _cameraRig.SetActive(false);
            onMenuOpen?.Invoke(false); // run method
        }
        else {
            // Debug.Log("Google");
            SelectBuildingMenu.SetActive(false);
            // _cameraRig.SetActive(true);
            onMenuOpen?.Invoke(true);
        }
    }

}
