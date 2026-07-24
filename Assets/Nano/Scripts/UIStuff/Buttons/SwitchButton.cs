using UnityEngine;

public class SwitchButton : Button
{
    public GameObject CurrentGame;
    public GameObject NextGame;

  void Start()
  {
    CurrentGame = this.gameObject.transform.parent.gameObject;
  }
}
