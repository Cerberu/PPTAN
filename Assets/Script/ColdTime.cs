using UnityEngine;
using UnityEngine.UI;
using System;

public class ColdTime : MonoBehaviour {

    //游戏体，对立按钮
    public GameObject oppositeButton;

    //冷却的开始时间
    public float startColdTime;

    //bool值，是否允许倒计时
    public bool timerEnable;

    //冷却计时
    float coldTimer;

    //自身的文本组件
    Text selfText;

	void OnEnable () {

        //获得自身的文本组件
        selfText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        //如果允许倒计时
        if (timerEnable)
        {
            //计时递减
            coldTimer -= Time.deltaTime;

            //将该时间转换为TimeSpan格式
            TimeSpan tempTimeSpan = new TimeSpan(0, 0, 0, (int)coldTimer);

            //显示剩余时间
            selfText.text = tempTimeSpan.Minutes.ToString("00") + ":" + tempTimeSpan.Seconds.ToString("00");

            //如果计时为0
            if (coldTimer <= 0)
            {
                //计时修正为0
                coldTimer = 0;

                //倒计时结束
                timerEnable = false;

                //冷却图片禁用
                transform.parent.gameObject.SetActive(false);

                //对立按钮激活
                oppositeButton.SetActive(true);
            }
        }
	}

    //方法，执行倒计时
    public void TimeCountDown()
    {
        //初始时间
        coldTimer = startColdTime;

        //标志位改变
        timerEnable = true;
    }
}
