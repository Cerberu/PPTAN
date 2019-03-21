using UnityEngine;
using System.Collections;

public class FlyingCoin : MonoBehaviour {

    //金币移动
    public void FlyingCoinMove()
    {
        //从起始点飞到终点
        iTween.MoveTo(gameObject, iTween.Hash("position", 0.01F * MenuController.Instance.flyingCoinEndPosition, "time", 0.5F, "oncomplete", "FlyingCoinMoveEnd"));
    }

    //金币移动结束之后的操作
    public void FlyingCoinMoveEnd()
    {
        //自身禁用
        gameObject.SetActive(false);

        //金币增长
        MenuController.Instance.CoinIncrease();
    }
}
