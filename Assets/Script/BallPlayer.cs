using UnityEngine;
using System.Collections;

public class BallPlayer : MonoBehaviour {

    //临时计时器
    float tempTimer;
	
	// Update is called once per frame
	void Update () {

        //如果撞击播放器处于繁忙状态
        if(GameController.Instance.ballPlayerBusyState == 1)
        {
            //计时
            tempTimer += Time.deltaTime;

            //如果计时到达指定时间
            if(tempTimer >= MyClass.ballPlayerCD)
            {
                //计时器清0
                tempTimer = 0;

                //撞击播放器退出繁忙状态
                GameController.Instance.ballPlayerBusyState = 0;
            }
        }
	}

    //方法，撞击播放器进入繁忙状态
    public void BallPlayerEnterBusyState()
    {
        //临时计时器清0
        tempTimer = 0;

        //撞击播放器进入繁忙状态
        GameController.Instance.ballPlayerBusyState = 1;
    }
}
