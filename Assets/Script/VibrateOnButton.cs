using UnityEngine;

public class VibrateOnButton : MonoBehaviour {

    //振动关闭按钮
    public GameObject vibrateOffButton;

	//方法，执行振动打开按钮被点击之后的操作
    public void VibrateOnButtonClick()
    {
        //如果设置界面响应点击
        if (MenuController.Instance.settingInterfaceClickEnable)
        {
            #if (IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("VibrateOnButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(GameObject.Find("SoundPlayer").GetComponent<AudioSource>(),
                              Resources.Load<AudioClip>("Audio/button"),
                              MyClass.soundEnable);

            //振动开关关闭
            MyClass.vibrateEnable = 0;

            //将振动开关状态存入玩家偏好中
            PlayerPrefs.SetInt("vibrateEnable", MyClass.vibrateEnable);

            //VibrateOff按钮激活
            vibrateOffButton.SetActive(true);

            //本按钮禁用
            gameObject.SetActive(false);
        }
    }
}
