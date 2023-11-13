using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Action<int> OnRoadPlacement, OnHousePlacement;
    public Action OnStructureDelete;
    public Button PlaceRoadButton, PlaceHouseButton, DeleteButton;

    public Color outlineColor;
    List<Button> buttonList;

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
