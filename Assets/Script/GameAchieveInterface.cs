using UnityEngine;

public class GameAchieveInterface : MonoBehaviour {

    //方法，执行成就界面入场动画结束之后的操作
    public void AchieveInterfaceStartCompleted()
    {
        //成就界面响应点击
        GameController.Instance.achieveInterfaceClickEnable = true;
    }

    //方法，执行成就界面离场动画结束之后的操作
    public void AchieveInterfaceEndCompleted()
    {
        //成就界面暂时禁用
        gameObject.SetActive(false);
    }
}
