using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUDHandler : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    public UIDocument UIDoc;

    private VisualElement FuelMaskL;
    private VisualElement FuelMaskR;
    private VisualElement WinTextDefault;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        PlayerMovement.OnFuelChangeL += FuelChangedL;
        PlayerMovement.OnFuelChangeR += FuelChangedR;

        FuelMaskL = UIDoc.rootVisualElement.Q<VisualElement>("FuelMaskL");
        FuelMaskR = UIDoc.rootVisualElement.Q<VisualElement>("FuelMaskR");
        WinTextDefault = UIDoc.rootVisualElement.Q<VisualElement>("WinTextDefault");
        WinTextDefault.style.display = DisplayStyle.None;

        FuelChangedL();
        FuelChangedR();
    }

    void FuelChangedL()
    {
        float fuelRatio = (float)PlayerMovement.jetFuelL / PlayerMovement.jetFuelMaximum;
        float fuelPercent = Mathf.Lerp(15, 65, fuelRatio);
        FuelMaskL.style.height = Length.Percent(fuelPercent);
    }
    void FuelChangedR()
    {
        float fuelRatio = (float)PlayerMovement.jetFuelR / PlayerMovement.jetFuelMaximum;
        float fuelPercent = Mathf.Lerp(15, 65, fuelRatio);
        FuelMaskR.style.height = Length.Percent(fuelPercent);
    }

    public void ShowWinText()
    {
        WinTextDefault.style.display = DisplayStyle.Flex;
    }

}
