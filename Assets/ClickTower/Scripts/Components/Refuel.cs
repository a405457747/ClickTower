using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using xmaolol.com;

public class Refuel : MonoBehaviour
{
    void OnEnable()
    {
        if (MySaveManager.Instance.SaveMapping.IsVip == true)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
}
