using UnityEngine;
using UnityEngine.UI;

public class CharactorInterface : MonoBehaviour {

    //金币Logo
    public GameObject propertyLogo;

    //角色组
    public Image charactorGroup;

    //角色界面激活时调用
    void OnEnable()
    {
        //更新道具点数
        MenuController.Instance.RefreshPropertyPoints();
    }

    //方法，执行角色界面入场动画结束之后的操作
    void CharactorInterfaceStartCompleted()
    {
        //角色界面响应点击
        MenuController.Instance.charactorInterfaceClickEnable = true;

        //角色组的Image组件激活，角色区域可以被滑动
        charactorGroup.enabled = true;
    }

    //方法，执行角色界面离场动画结束之后的操作
    void CharactorInterfaceEndCompleted()
    {
        //角色界面暂时禁用
        gameObject.SetActive(false);

        //首页界面允许点击
        MenuController.Instance.menuInterfaceClickEnable = true;

        //设置界面允许点击
        MenuController.Instance.settingInterfaceClickEnable = true;
    }
}
