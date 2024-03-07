using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using xmaolol.com;
using QFramework;
using Lean.Pool;

public class Coin : MonoBehaviour
{

    int num;
    private float RateSpeed = 600f;
    private float moveSpeed = 10f;
    private GoodsBornAnimation goodsBornAnimation;

    public Vector3 target;
    //代表金币的颜色L1等级最高
    public Sprite L1;
    public Sprite L2;
    public Sprite L3;
    public Sprite L4;

    SpriteRenderer sr;
    public int Num
    {
        get => num; set
        {
            num = value;
            if (value >= 0 && value <= 100)
            {
                sr.sprite = L4;
            }
            else

            if (value > 100 && value <= 1000)
            {
                sr.sprite = L3;
            }
            else

            if (value > 1000 && value <= 10000)
            {
                sr.sprite = L2;
            }
            else
            if (value > 10000)
            {
                sr.sprite = L1;
            }
        }
    }

    private void Awake()
    {
        goodsBornAnimation = GetComponent<GoodsBornAnimation>();
        sr = GetComponent<SpriteRenderer>();
        target = Camera.main.ScreenToWorldPoint(new Vector3(18, Screen.height - 18));
    }

    public void Init(int money)
    {
        this.Num = money;
        goodsBornAnimation.PlayBornAnimation(0.34f, transform.position);
        this.Delay(0.35f, ClickSelf);
    }

    public void ClickSelf()
    {
        //增加钱
        transform.DOMove(target, 0.8f).OnComplete(() =>
        {
            UIMain.Instance.InitialMoney += num;
            LeanPool.Despawn(this.gameObject);
        });
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, RateSpeed * Time.deltaTime, Space.Self);
    }

}
