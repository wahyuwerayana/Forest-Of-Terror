using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    public static bool isPaused = false;

    private void Start() {
        isPaused = false;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) && !ShopManager.isOpened && !CardManager.Instance.GetCardUICondition()){
            PauseGame(!pausePanel.activeSelf);
        }
    }

    private void PauseGame(bool pausePanelBool){
        CardManager.Instance.GetCurrentWeapon();
        if(pausePanelBool){
            isPaused = true;
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            CardManager.Instance.currentActiveWeapon.showCrosshair = false;
            pausePanel.SetActive(true);
        } else{
            ResumeGame();
        }
    }

    public void ResumeGame(){
        isPaused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        CardManager.Instance.currentActiveWeapon.showCrosshair = true;
        pausePanel.SetActive(false);
    }
}
