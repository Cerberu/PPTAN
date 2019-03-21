using UnityEngine;
using System.Collections;

public class BandPlayer : MonoBehaviour
{
    //临时计时器
    float tempTimer;

    // Update is called once per frame
    void Update()
    {
        //如果光带播放器处于繁忙状态
        if (GameController.Instance.bandPlayerBusyState == 1)
        {
            //计时
            tempTimer += Time.deltaTime;

            //如果计时到达指定时间
            if (tempTimer >= MyClass.bandPlayerCD)
            {
                //计时器清0
                tempTimer = 0;

                //光带播放器退出繁忙状态
                GameController.Instance.bandPlayerBusyState = 0;
            }
        }
    }

    //方法，光带播放器进入繁忙状态
    public void BandPlayerEnterBusyState()
    {
        //临时计时器清0
        tempTimer = 0;

        //光带播放器进入繁忙状态
        GameController.Instance.bandPlayerBusyState = 1;
    }
}
