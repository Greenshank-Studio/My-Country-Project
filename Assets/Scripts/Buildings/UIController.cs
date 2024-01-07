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
    [SerializeField] private GameObject _shopMenuChanger;
    [SerializeField] private GameObject _shopMenuText;

    private RectTransform _shopTransform;
    private Vector3 _initialShopPosition, _newShopPosition;

    private float _movementTime;
    private bool _isShopOpened;

    private void Awake()
    {
        _movementTime = 10f;
        _shopTransform = _shopMenuChanger.GetComponent<RectTransform>();
        _newShopPosition = _shopTransform.localPosition;
        _initialShopPosition = _shopTransform.localPosition;

        buttonList = new List<Button> { PlaceHouseButton, PlaceRoadButton, DeleteButton };
    }

    private void Start()
    {
        PlaceRoadButton.onClick.AddListener(() =>
        {
            // ResetButtonColor();
            ModifyOutline(PlaceRoadButton);
            OnRoadPlacement?.Invoke(0);
        });



        // PlaceHouseButton.onClick.AddListener(() =>
        // {
        //     // ResetButtonColor();
        //     ModifyOutline(PlaceHouseButton);
        //     OnHousePlacement?.Invoke(0);
        // });



        DeleteButton.onClick.AddListener(() =>
        {
            // ResetButtonColor();
            ModifyOutline(DeleteButton);
            OnStructureDelete?.Invoke();
        });
    }


    private void LateUpdate()
    {
        if(_isShopOpened)
        {
            ChangeShopMenuPosition();
        }
    }

    private void ChangeShopMenuPosition()
    {
        _shopTransform.localPosition = Vector3.Lerp(_shopTransform.localPosition, _newShopPosition, Time.deltaTime * _movementTime); // Vector3 should be updated as a whole thing, not just by position.x only, it won't work
    }

    public void ChosenHouse(int houseIndex)
    {
        _chooseBuildingMenu.SetActive(false);
        OnMenuStateChanged?.Invoke(false);
        // ResetButtonColor();
        // ModifyOutline(PlaceHouseButton);
        OnHousePlacement?.Invoke(houseIndex);
    }

    public void OpenShop()
    {
        _chooseBuildingMenu.SetActive(true);
        _isShopOpened = true;
        // _chooseBuildingMenu.transform.GetChild(1).GetComponent<Scrollbar>().value = 1;
        OnMenuStateChanged?.Invoke(true); // here we run all methods we've been subscribed
    }

    public void CloseShop()
    {
        _chooseBuildingMenu.SetActive(false);
        _isShopOpened = false;
        OnMenuStateChanged?.Invoke(false);
    }

    public void ChangeHorizontalSlidingMenu(int menuIndex)
    {
        _newShopPosition.x =_initialShopPosition.x + menuIndex * -1500;
    }

    public void ChangeText(string text)
    {
        _shopMenuText.GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }


    private void ModifyOutline(Button button)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    // private void ResetButtonColor()
    // {
    //     foreach (Button button in buttonList)
    //     {
    //         button.GetComponent<Outline>().enabled = false;
    //     }
    // }
}
