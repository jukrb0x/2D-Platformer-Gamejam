using System;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    private GameObject board1;
    private GameObject board2;
    private bool isFlag = true;

    private void Start()
    {
        // get boards
        board1 ??= gameObject.transform.Find("Board1").gameObject;
        board2 ??= gameObject.transform.Find("Board2").gameObject;
        // reset active board
        board1.SetActive(isFlag);
        board2.SetActive(!isFlag);
    }

    public void SwitchBoard()
    {
        isFlag = !isFlag;
        board1.SetActive(isFlag);
        board2.SetActive(!isFlag);
    }
}