using UnityEngine;
using System.Collections;

public class AddedBallNumberText : MonoBehaviour {

	//出场动画完成之后的操作
    void AddedBallNumberTextStartCompleted()
    {
        //自身禁用
        gameObject.SetActive(false);
    }
}
