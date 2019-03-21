using UnityEngine;

public class SoundOnButton : MonoBehaviour {

    //SoundOff按钮
    public GameObject soundOffButton;

    //方法，执行该按钮被点击之后的操作
    public void SoundOnButtonClick()
    {
        //如果设置界面响应点击
        if (MenuController.Instance.settingInterfaceClickEnable)
        {
            #if (IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("SoundOnButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(GameObject.Find("SoundPlayer").GetComponent<AudioSource>(),
                              Resources.Load<AudioClip>("Audio/button"),
                              MyClass.soundEnable);

            //音效开关关闭
            MyClass.soundEnable = 0;

            //将音效开关状态存入玩家偏好中
            PlayerPrefs.SetInt("soundEnable", MyClass.soundEnable);

            //SoundOff按钮激活
            soundOffButton.SetActive(true);

            //本按钮禁用
            gameObject.SetActive(false);
        }
    }
}
