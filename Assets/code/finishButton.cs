using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class finishButton : MonoBehaviour
{
    public Button myButton;
    void Start()
    {
        myButton.gameObject.SetActive(false);
        StartCoroutine(WaitForSeconds());
    }
    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(2f);
        myButton.gameObject.SetActive(true);
    }
}
