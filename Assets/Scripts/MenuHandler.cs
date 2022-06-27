using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuHandler : MonoBehaviour
{
    public TextMeshProUGUI tiempoRestante;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        string tAux = TiempoHastaElSiguienteDia().ToString();
        tiempoRestante.text = "S I G U I E N T E   E N:   "+tAux[0].ToString() +" " +tAux[1].ToString() + " " + tAux[2].ToString() + " " + tAux[3].ToString() + " " + tAux[4].ToString() + " " + tAux[5].ToString() + " " + tAux[6].ToString() + " " + tAux[7].ToString();
    }

    public void LoadClassicGame() {
        SceneManager.LoadScene("ClassicMode");
    }

    public void LoadDailyGame()
    {
        SceneManager.LoadScene("Diaria");
    }

    public void Load4letras()
    {
        //SceneManager.LoadScene();
    }

    public void LoadMemoriza()
    {
        //SceneManager.LoadScene();
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("TutorialMenu");
    }

    public void SalirJuego() {
        Application.Quit();
    }

    private System.TimeSpan TiempoHastaElSiguienteDia()
    {
        var dateToday = System.DateTime.UtcNow;
        var dateTomorrow = System.DateTime.UtcNow.AddDays(1).Date;
        System.TimeSpan diff = dateTomorrow - dateToday;
        return diff;
    }
}
