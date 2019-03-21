using UnityEngine;
using System.Collections;

public class VibrateOffButton : MonoBehaviour {

    //振动打开按钮
    public GameObject vibrateOnButton;

    //方法，执行该按钮被按下之后的操作
    public void VibrateOffButtonClick()
    {
        //如果设置界面响应点击
        if (MenuController.Instance.settingInterfaceClickEnable)
        {
            #if (IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("VibrateOffButton");

            #endif

            //振动开关打开
            MyClass.vibrateEnable = 1;

            //将振动开关状态存入玩家偏好中
            PlayerPrefs.SetInt("vibrateEnable", MyClass.vibrateEnable);

            //VibrateOn按钮激活
            vibrateOnButton.SetActive(true);

            //本按钮禁用
            gameObject.SetActive(false);
        }
    }
}
