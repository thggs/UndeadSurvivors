using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController : MonoBehaviour
{
    private UI_game_manager ui;

    private void Start(){
        ui = GameObject.FindGameObjectWithTag("UIToolkit").GetComponent<UI_game_manager>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ui.ChatBox(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ui.ChatBox(true);
        }
    }
}
