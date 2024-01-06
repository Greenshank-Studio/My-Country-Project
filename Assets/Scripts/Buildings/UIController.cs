using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Action<int> OnRoadPlacement, OnHousePlacement;
    public static Action<bool> OnMenuStateChanged;
    public Action OnStructureDelete;

    public Button PlaceRoadButton, PlaceHouseButton, DeleteButton;

    public Color outlineColor;
    List<Button> buttonList;

    [SerializeField] private GameObject _chooseBuildingMenu;

    private void Awake()
    {
        buttonList = new List<Button> { PlaceHouseButton, PlaceRoadButton, DeleteButton };
    }

    private void Start()
    {
        PlaceRoadButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(PlaceRoadButton);
            OnRoadPlacement?.Invoke(0);
        });



        PlaceHouseButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(PlaceHouseButton);
            OnHousePlacement?.Invoke(0);
        });



        DeleteButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(DeleteButton);
            OnStructureDelete?.Invoke();
        });
    }

    public void ChosenHouse()
    {
        _chooseBuildingMenu.SetActive(false);
        OnMenuStateChanged?.Invoke(false);
        ResetButtonColor();
        ModifyOutline(PlaceHouseButton);
        OnHousePlacement?.Invoke(0);
    }

    public void OpenShop()
    {
        _chooseBuildingMenu.SetActive(true);
        OnMenuStateChanged?.Invoke(true); // here we run all methods we've been subscribed
    }

    public void CloseShop()
    {
        _chooseBuildingMenu.SetActive(false);
        OnMenuStateChanged?.Invoke(false);
    }


    private void ModifyOutline(Button button)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    private void ResetButtonColor()
    {
        foreach (Button button in buttonList)
        {
            button.GetComponent<Outline>().enabled = false;
        }
    }
}
