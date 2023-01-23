using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameCounter : MonoBehaviour
{
    public GameObject LastButton;

    public bool BuildingPlayed,
        ApartmentPlayed,
        SubwayPlayed,
        TablePlayed,
        FencePlayed,
        PathPlayed,
        RoadPlayed,
        TreePlayed,
        LampPlayed;

  void Start()
  {
      LastButton.SetActive(false);

  }

    void ShowFinalButton()
    {
        if (BuildingPlayed && ApartmentPlayed && SubwayPlayed && TablePlayed && FencePlayed && PathPlayed &&
            RoadPlayed && TreePlayed && LampPlayed)
        {
            LastButton.SetActive(true);
        }
    }

  public void TruthSetterBuilding()
    {
        BuildingPlayed = true;
        ShowFinalButton();
    }
    
  public void TruthSetterApartment()
    {
        ApartmentPlayed = true;
        ShowFinalButton();
    }
  public void TruthSetterSubway()
    {
        SubwayPlayed = true;
        ShowFinalButton();
    }
  public void TruthSetterTable()
    {
        TablePlayed = true;
        ShowFinalButton();
    }
  public void TruthSetterFence()
    {
        FencePlayed = true;
        ShowFinalButton();
    }
  public void TruthSetterPath()
    {
        PathPlayed = true;
        ShowFinalButton();
    }
  public void TruthSetterRoad()
    {
        RoadPlayed = true;
        ShowFinalButton();
    }
  public void TruthSetterTree()
    {
        TreePlayed = true;
        ShowFinalButton();
    }
  public void TruthSetterLamp()
    {
        LampPlayed = true;
        ShowFinalButton();
    }
}
 