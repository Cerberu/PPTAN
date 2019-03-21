using UnityEngine;

public class VibrateButton : MonoBehaviour {

    //当该游戏体激活时
    void OnEnable()
    {
        //根据手机振动的开关状态，激活或禁用相关的按钮
        transform.GetChild(1 - MyClass.vibrateEnable).gameObject.SetActive(true);
        transform.GetChild(MyClass.vibrateEnable).gameObject.SetActive(false);
    }
}
