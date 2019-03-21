using UnityEngine;

public class ResurgenceInterface : MonoBehaviour {

	//方法，复活界面入场动画完成之后的操作
    public void ResurgenceInterfaceStartCompleted()
    {
        //复活界面开始响应点击
        GameController.Instance.resurgenceInterfaceClickEnable = true;
    }

    //方法，复活界面离场动画完成之后的操作
    public void ResurgenceInterfaceEndCompleted()
    {
        //复活界面禁用
        gameObject.SetActive(false);

        //游戏结算
        GameController.Instance.GameAccount();
    }
}
