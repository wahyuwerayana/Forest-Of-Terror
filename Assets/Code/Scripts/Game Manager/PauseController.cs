using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            PauseGame(!pausePanel.activeSelf);
        }
    }

    private void PauseGame(bool pausePanelBool){
        CardManager.Instance.GetCurrentWeapon();
        if(pausePanelBool){
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            CardManager.Instance.currentActiveWeapon.showCrosshair = false;
            pausePanel.SetActive(true);
        } else{
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            CardManager.Instance.currentActiveWeapon.showCrosshair = true;
            pausePanel.SetActive(false);
        }
    }

    public void ResumeGame(){
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        CardManager.Instance.currentActiveWeapon.showCrosshair = true;
        pausePanel.SetActive(false);
    }
}
