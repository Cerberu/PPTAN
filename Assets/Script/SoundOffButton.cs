using UnityEngine;

public class SoundOffButton : MonoBehaviour {

    //SoundOn按钮
    public GameObject soundOnButton;

    //方法，执行该按钮被按下之后的操作
    public void SoundOffButtonClick()
    {
        //如果设置界面响应点击
        if (MenuController.Instance.settingInterfaceClickEnable)
        {
            #if (IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("SoundOffButton");

            #endif

            //音效开关打开
            MyClass.soundEnable = 1;

            //将音效开关状态存入玩家偏好中
            PlayerPrefs.SetInt("soundEnable", MyClass.soundEnable);

            //SoundOn按钮激活
            soundOnButton.SetActive(true);

            //本按钮禁用
            gameObject.SetActive(false);
        }
    }
}
