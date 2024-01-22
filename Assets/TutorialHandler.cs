using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TutorialController.Instance.HandleTutorialDudeContact();
        Destroy(gameObject);
    }
}
