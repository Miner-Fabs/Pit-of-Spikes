using System;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UIElements;

public class FuelStationScript : MonoBehaviour
{
    public string stationType;
    public Collider2D stationCollider;
    private bool stationEntered;

    public PlayerMovement PlayerMovement;
    public LayerMask playerLayer;

    public UIDocument UIDoc;
    private VisualElement RefuelPopup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        stationEntered = false;
        RefuelPopup = UIDoc.rootVisualElement.Q<VisualElement>("RefuelPopup");
        RefuelPopup.style.display = DisplayStyle.None;
    }

    // Update is called once per frame
    private void Update()
    {
        if (stationCollider.IsTouchingLayers(playerLayer) && !PlayerMovement.isFlying && !PlayerMovement.isStunned)
        {
            if (!stationEntered)
            {
                RefuelPopup.style.display = DisplayStyle.Flex;
                stationEntered = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Refuel();
            }
        }
        else if (stationEntered)
        {
            RefuelPopup.style.display = DisplayStyle.None;
            stationEntered = false;
        }
    }

    private void Refuel()
    {
        switch (stationType)
        {
            case "both":
                PlayerMovement.jetFuelL = PlayerMovement.jetFuelMaximum;
                PlayerMovement.jetFuelR = PlayerMovement.jetFuelMaximum;
                PlayerMovement.OnFuelChangeL?.Invoke();
                PlayerMovement.OnFuelChangeR?.Invoke();
                break;
            case "left":
                PlayerMovement.jetFuelL = PlayerMovement.jetFuelMaximum;
                PlayerMovement.OnFuelChangeL?.Invoke();
                break;
            case "right":
                PlayerMovement.jetFuelR = PlayerMovement.jetFuelMaximum;
                PlayerMovement.OnFuelChangeR?.Invoke();
                break;
        }
    }

}
