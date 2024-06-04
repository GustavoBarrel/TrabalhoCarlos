using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    
    [SerializeField] private GameObject painelPrincipal;
    [SerializeField] private GameObject painelGameOver;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera novaCamera;
    [SerializeField] private GameObject contador;

    public void AbrirGameOver() {

        painelPrincipal.SetActive(false);
        contador.SetActive(false);
        painelGameOver.SetActive(true);

        canvas.worldCamera = novaCamera;
    }

   public void RestartGame() {
        Debug.Log("ok");
        SceneManager.LoadScene(2);
    }
}
