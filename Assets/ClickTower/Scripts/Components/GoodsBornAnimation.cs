using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  DG.Tweening;


public class GoodsBornAnimation : MonoBehaviour
{
    public float offsetY = 0.35f;

    private Vector3 targetPos;
    public Vector3 TargetPos { get => new Vector3(InitialPos.x,InitialPos.y+offsetY,InitialPos.z); set => targetPos = value; }
    private Vector3 InitialPos;

    public void PlayBornAnimation(float duringTime, Vector3 initialPos)
    {
        InitialPos = initialPos;

        //Sequence mSequence = new Sequence();
        //transform.DOMoveY()
        transform.DOMove(TargetPos, duringTime/2f).OnComplete(() => { transform.DOMove(InitialPos, duringTime / 2f); });
    }
}
