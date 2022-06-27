using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class Memoriza : MonoBehaviour
{
    // Primer intento
    public TextMeshProUGUI letra1;
    public TextMeshProUGUI letra2;
    public TextMeshProUGUI letra3;
    public TextMeshProUGUI letra4;
    public TextMeshProUGUI letra5;
    private string palabraFinal;

    // Informacion adicional in game
    public TextMeshProUGUI palabraNoValida;

    // Maneja estadisticas
    [SerializeField] private GameObject menuEstadisticas;
    public TextMeshProUGUI estadisticasGanaPierde;
    public TextMeshProUGUI palabraRevelar;

    // Otras variables
    private string hiddenWord;
    private int numIntento;
    private bool ganar = false;
    List<string> listadoPalabras = new List<string>();

    // Botones de letras
    private IDictionary<string, Button> buttonsHash = new Dictionary<string, Button>();
    public Button buttonQ; public Button buttonW; public Button buttonE; public Button buttonR; public Button buttonT;
    public Button buttonY; public Button buttonU; public Button buttonI; public Button buttonO; public Button buttonP;
    public Button buttonA; public Button buttonS; public Button buttonD; public Button buttonF; public Button buttonG;
    public Button buttonH; public Button buttonJ; public Button buttonK; public Button buttonL; public Button buttonZ;
    public Button buttonX; public Button buttonC; public Button buttonV; public Button buttonB; public Button buttonN;
    public Button buttonM;

    // JSON
    public TextAsset jsonFile;
    Words wordsInJson;

    // Start is called before the first frame update
    void Start()
    {
        // Semilla
        Random.InitState(ObtenerSemilla());

        InicializarHashMapButtons();
        menuEstadisticas.SetActive(false);
        RellenarPalabras();
        GenerateWord();
        Debug.Log(hiddenWord);
        numIntento = 1;

        // Palabras intentadas
        palabraFinal = "";
        // Intento 1
        letra1.text = "";
        letra2.text = "";
        letra3.text = "";
        letra4.text = "";
        letra5.text = "";
        
        palabraRevelar.text = "";


        // Adicionales
        palabraNoValida.text = "";

        // Refered to JSON
        wordsInJson = JsonUtility.FromJson<Words>(jsonFile.text);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateTry(string word)
    {
        // Primer intento
        if (numIntento == 1)
        {
            if (palabraFinal.Length < 5)
            {
                palabraFinal += word;
                if (palabraFinal.Length == 1)
                {
                    letra1.text += palabraFinal[0];
                }
                if (palabraFinal.Length == 2)
                {
                    letra2.text += palabraFinal[1];
                }
                if (palabraFinal.Length == 3)
                {
                    letra3.text += palabraFinal[2];
                }
                if (palabraFinal.Length == 4)
                {
                    letra4.text += palabraFinal[3];
                }
                if (palabraFinal.Length == 5)
                {
                    letra5.text += palabraFinal[4];
                }
            }
        }

    }

    public void EraseWord()
    {
        // Intento 1
        if (numIntento == 1)
        {
            letra1.text = "";
            letra2.text = "";
            letra3.text = "";
            letra4.text = "";
            letra5.text = "";
            palabraFinal = "";
        }
    }

    public void GenerateWord()
    {
        hiddenWord = listadoPalabras[Random.Range(0, listadoPalabras.Count)];
    }

    public void CheckWord()
    {
        bool palabraValida = false;
        // Comprobamos que la palabra es válida
        // Intento 1
        if (palabraFinal.Length == 5 && numIntento == 1)
            palabraValida = buscarEnBaseDeDatos(palabraFinal);

        // Si la palabra introducida es válida, hacemos las comprobaciones pertinentes
        if (palabraValida)
        {
            palabraNoValida.text = " ";

            // Primer intento
            if (palabraFinal.Length == 5 && numIntento == 1)
            {
                //Poner a gris todas, incluidos los botones
                letra1.color = Color.grey;
                TurnGrey(buttonsHash[letra1.text]);
                letra2.color = Color.grey;
                TurnGrey(buttonsHash[letra2.text]);
                letra3.color = Color.grey;
                TurnGrey(buttonsHash[letra3.text]);
                letra4.color = Color.grey;
                TurnGrey(buttonsHash[letra4.text]);
                letra5.color = Color.grey;
                TurnGrey(buttonsHash[letra5.text]);

                // Lo siguiente es comprobar las verdes
                string[] wordAux1 = new string[5];
                wordAux1[0] = hiddenWord[0].ToString();
                wordAux1[1] = hiddenWord[1].ToString();
                wordAux1[2] = hiddenWord[2].ToString();
                wordAux1[3] = hiddenWord[3].ToString();
                wordAux1[4] = hiddenWord[4].ToString();
                // Verdes
                if (letra1.text.Equals(hiddenWord[0].ToString()))
                {
                    letra1.color = Color.green;
                    TurnGreen(buttonsHash[letra1.text]);
                    wordAux1[0] = "1";
                }
                if (letra2.text.Equals(hiddenWord[1].ToString()))
                {
                    letra2.color = Color.green;
                    TurnGreen(buttonsHash[letra2.text]);
                    wordAux1[1] = "2";
                }
                if (letra3.text.Equals(hiddenWord[2].ToString()))
                {
                    letra3.color = Color.green;
                    TurnGreen(buttonsHash[letra3.text]);
                    wordAux1[2] = "3";
                }
                if (letra4.text.Equals(hiddenWord[3].ToString()))
                {
                    letra4.color = Color.green;
                    TurnGreen(buttonsHash[letra4.text]);
                    wordAux1[3] = "4";
                }
                if (letra5.text.Equals(hiddenWord[4].ToString()))
                {
                    letra5.color = Color.green;
                    TurnGreen(buttonsHash[letra5.text]);
                    wordAux1[4] = "5";
                }

                // Amarillas
                string word = wordAux1[0] + wordAux1[1] + wordAux1[2] + wordAux1[3] + wordAux1[4];
                Debug.Log(word);
                if (word.Contains(letra1.text) && (wordAux1[0] != "1"))
                {
                    letra1.color = Color.yellow;
                    TurnYellow(buttonsHash[letra1.text]);

                    // Hay que sacar el indice correcto
                    for (int i = 0; i < 5; i++)
                    {
                        if (word[i].ToString() == letra1.text)
                        {
                            wordAux1[i] = "6";
                            continue;
                        }
                    }
                }
                word = wordAux1[0] + wordAux1[1] + wordAux1[2] + wordAux1[3] + wordAux1[4];
                if (word.Contains(letra2.text) && (wordAux1[1] != "2"))
                {
                    letra2.color = Color.yellow;
                    TurnYellow(buttonsHash[letra2.text]);
                    // Hay que sacar el indice correcto
                    for (int i = 0; i < 5; i++)
                    {
                        if (word[i].ToString() == letra2.text)
                        {
                            wordAux1[i] = "7";
                            continue;
                        }
                    }
                }
                word = wordAux1[0] + wordAux1[1] + wordAux1[2] + wordAux1[3] + wordAux1[4];
                if (word.Contains(letra3.text) && (wordAux1[2] != "3"))
                {
                    letra3.color = Color.yellow;
                    TurnYellow(buttonsHash[letra3.text]);
                    // Hay que sacar el indice correcto
                    for (int i = 0; i < 5; i++)
                    {
                        if (word[i].ToString() == letra3.text)
                        {
                            wordAux1[i] = "8";
                            continue;
                        }
                    }
                }
                word = wordAux1[0] + wordAux1[1] + wordAux1[2] + wordAux1[3] + wordAux1[4];
                if (word.Contains(letra4.text) && (wordAux1[3] != "4"))
                {
                    letra4.color = Color.yellow;
                    TurnYellow(buttonsHash[letra4.text]);
                    // Hay que sacar el indice correcto
                    for (int i = 0; i < 5; i++)
                    {
                        if (word[i].ToString() == letra4.text)
                        {
                            wordAux1[i] = "9";
                            continue;
                        }
                    }
                }
                word = wordAux1[0] + wordAux1[1] + wordAux1[2] + wordAux1[3] + wordAux1[4];
                if (word.Contains(letra5.text) && (wordAux1[4] != "5"))
                {
                    letra5.color = Color.yellow;
                    TurnYellow(buttonsHash[letra5.text]);
                    // Hay que sacar el indice correcto
                    for (int i = 0; i < 5; i++)
                    {
                        if (word[i].ToString() == letra5.text)
                        {
                            wordAux1[i] = "0";
                            continue;
                        }
                    }
                }
            }

            // Check condicion de victoria
            ganar = CheckCondicionVictoria();
            if (ganar == true)
            {
                palabraNoValida.text = "";
                // Parar juego
                GoToEstadisticas(true, numIntento);
                numIntento = -1; // -1 para que no pertenezca a ningun movimiento posible.
            }

            if (numIntento == 7)
            {
                GoToEstadisticas(false, numIntento);
            }
        }
        // En caso contrario, avisamos en el juego
        else
        {
            palabraNoValida.text = "P A L A B R A   N O   V Á L I D A";
        }
    }


    private void TurnGrey(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.grey;
        colors.highlightedColor = Color.grey;
        colors.pressedColor = Color.grey;
        colors.selectedColor = Color.grey;
        button.colors = colors;
    }
    private void TurnGreen(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.green;
        colors.highlightedColor = Color.green;
        colors.pressedColor = Color.green;
        colors.selectedColor = Color.green;
        button.colors = colors;
    }

    private void TurnYellow(Button button)
    {
        if (button.colors.normalColor != Color.green)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.yellow;
            colors.highlightedColor = Color.yellow;
            colors.pressedColor = Color.yellow;
            colors.selectedColor = Color.yellow;
            button.colors = colors;
        }
    }

    private bool CheckCondicionVictoria()
    {

        // Intento 1
        if ((numIntento == 1) && (string.Equals(palabraFinal, hiddenWord)))
        {
            return true;
        }
        
        if ((palabraFinal.Length == 5 && numIntento == 1) || // Solo suma el intento completado cuando haya palabras completas en los respectivos turnos
            (palabraFinal.Length == 5 && numIntento == 2) ||
            (palabraFinal.Length == 5 && numIntento == 3) ||
            (palabraFinal.Length == 5 && numIntento == 4) ||
            (palabraFinal.Length == 5 && numIntento == 5) ||
            (palabraFinal.Length == 5 && numIntento == 6))
        {
            numIntento++;
        }
        return false;
    }

    public void VuelveMenuPrincipal()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void GoToEstadisticas(bool ganar, int numIntentoAux)
    {
        menuEstadisticas.SetActive(true);
        if (ganar == true)
        {
            estadisticasGanaPierde.text = "G A N A S T E";
        }
        else
        {
            estadisticasGanaPierde.text = "P E R D I S T E";
            palabraRevelar.text = "L A   P A L A B R A   E R A :   " + hiddenWord[0].ToString() + " " + hiddenWord[1].ToString() + " " + hiddenWord[2].ToString() + " " + hiddenWord[3].ToString() + " " + hiddenWord[4].ToString();
        }
    }
    public void VolverPartida()
    {
        menuEstadisticas.SetActive(false);
    }
    private void RellenarPalabras()
    {
        // https://www.listasdepalabras.es/palabras5letraspagina6.htm
        // A
        listadoPalabras.Add("ABETO"); listadoPalabras.Add("AGRIO"); listadoPalabras.Add("ACERO"); listadoPalabras.Add("AMIGO"); listadoPalabras.Add("ABUSO");
        listadoPalabras.Add("AGUDO"); listadoPalabras.Add("ACTOR"); listadoPalabras.Add("ALIJO"); listadoPalabras.Add("AMENO"); listadoPalabras.Add("ATROZ");
        listadoPalabras.Add("ABACO");
        // B
        listadoPalabras.Add("BACON"); listadoPalabras.Add("BELGA"); listadoPalabras.Add("BANCO"); listadoPalabras.Add("BEMOL"); listadoPalabras.Add("BIZCO");
        listadoPalabras.Add("BOLSA"); listadoPalabras.Add("BLUSA"); listadoPalabras.Add("BOCIN"); listadoPalabras.Add("BUJIA"); listadoPalabras.Add("BRAZO");
        // C
        listadoPalabras.Add("CURVA"); listadoPalabras.Add("CALVO"); listadoPalabras.Add("CAMPO"); listadoPalabras.Add("CARDO"); listadoPalabras.Add("CERDO");
        listadoPalabras.Add("CHEPA"); listadoPalabras.Add("CINTA"); listadoPalabras.Add("CISNE"); listadoPalabras.Add("CLASE"); listadoPalabras.Add("COMER");
        listadoPalabras.Add("COMBA"); listadoPalabras.Add("CREDO");
        // D
        listadoPalabras.Add("DARDO"); listadoPalabras.Add("DOMAR"); listadoPalabras.Add("DOSEL"); listadoPalabras.Add("DIQUE"); listadoPalabras.Add("DROGA");
        listadoPalabras.Add("DULCE"); listadoPalabras.Add("DISCO"); listadoPalabras.Add("DUELO"); listadoPalabras.Add("DECIR"); listadoPalabras.Add("DIETA");
        // E
        listadoPalabras.Add("EBANO"); listadoPalabras.Add("EPICO"); listadoPalabras.Add("EPOCA"); listadoPalabras.Add("EUROS"); listadoPalabras.Add("EXTRA");
        listadoPalabras.Add("ENANO"); listadoPalabras.Add("ESPIA"); listadoPalabras.Add("ETNIA"); listadoPalabras.Add("ETICA"); listadoPalabras.Add("ETANO");
        // F
        listadoPalabras.Add("FACIL"); listadoPalabras.Add("FERIA"); listadoPalabras.Add("FAGOT"); listadoPalabras.Add("FARDO"); listadoPalabras.Add("FALSO");
        listadoPalabras.Add("FEMUR"); listadoPalabras.Add("FIERO"); listadoPalabras.Add("FICHA"); listadoPalabras.Add("FANGO"); listadoPalabras.Add("FILME");
        listadoPalabras.Add("FINTA"); listadoPalabras.Add("FIRMA"); listadoPalabras.Add("FLORA"); listadoPalabras.Add("FLUIR"); listadoPalabras.Add("FOVEA");
        listadoPalabras.Add("FORJA"); listadoPalabras.Add("FLACO"); listadoPalabras.Add("FRUTA"); listadoPalabras.Add("FUSIL"); listadoPalabras.Add("FUSTA");
        listadoPalabras.Add("FUTIL"); listadoPalabras.Add("FUTON");
        // G
        listadoPalabras.Add("GANSO"); listadoPalabras.Add("GAITA"); listadoPalabras.Add("GARBO"); listadoPalabras.Add("GATOS"); listadoPalabras.Add("GORDA");
        listadoPalabras.Add("GEMIR"); listadoPalabras.Add("GENIO"); listadoPalabras.Add("GIBON"); listadoPalabras.Add("GRADO"); listadoPalabras.Add("GLIFO");
        listadoPalabras.Add("GOZAR"); listadoPalabras.Add("GRAFO"); listadoPalabras.Add("GRIFO"); listadoPalabras.Add("GUAPO"); listadoPalabras.Add("GUASA");
        listadoPalabras.Add("GUISO"); listadoPalabras.Add("GUSTO");
        // H
        listadoPalabras.Add("HABIL"); listadoPalabras.Add("HACER"); listadoPalabras.Add("HABER"); listadoPalabras.Add("HEBRA"); listadoPalabras.Add("HEDOR");
        listadoPalabras.Add("HELAR"); listadoPalabras.Add("HIJOS"); listadoPalabras.Add("HIMNO"); listadoPalabras.Add("HIMEN"); listadoPalabras.Add("HILAR");
        listadoPalabras.Add("HORDA"); listadoPalabras.Add("HONRA"); listadoPalabras.Add("HORCA"); listadoPalabras.Add("HUECA"); listadoPalabras.Add("HUECO");
        listadoPalabras.Add("HUESO"); listadoPalabras.Add("HUMOR"); listadoPalabras.Add("HURTO"); listadoPalabras.Add("HUEVO");
        // I
        listadoPalabras.Add("ICTUS"); listadoPalabras.Add("IDEAL"); listadoPalabras.Add("IDEAR"); listadoPalabras.Add("IGNEO"); listadoPalabras.Add("ILUSO");
        listadoPalabras.Add("ISLAS"); listadoPalabras.Add("ISLAM");
        // J
        listadoPalabras.Add("JABON"); listadoPalabras.Add("JADEO"); listadoPalabras.Add("JALEO"); listadoPalabras.Add("JAMBO"); listadoPalabras.Add("JAQUE");
        listadoPalabras.Add("JUDIA"); listadoPalabras.Add("JUDIO"); listadoPalabras.Add("JERGA"); listadoPalabras.Add("JUEZA"); listadoPalabras.Add("JUBON");
        listadoPalabras.Add("JUGAR");
        // K
        // L
        listadoPalabras.Add("LAMER"); listadoPalabras.Add("LABIO"); listadoPalabras.Add("LABOR"); listadoPalabras.Add("LACIO"); listadoPalabras.Add("LACON");
        listadoPalabras.Add("LAICO"); listadoPalabras.Add("LARGO"); listadoPalabras.Add("LAPIZ"); listadoPalabras.Add("LASER"); listadoPalabras.Add("LECHO");
        listadoPalabras.Add("LEGAL"); listadoPalabras.Add("LEJIA"); listadoPalabras.Add("LEGUA"); listadoPalabras.Add("LEJOS"); listadoPalabras.Add("LEMUR");
        listadoPalabras.Add("LENTO"); listadoPalabras.Add("LEONA"); listadoPalabras.Add("LERDO"); listadoPalabras.Add("LICOR"); listadoPalabras.Add("LIDER");
        listadoPalabras.Add("LIGAR"); listadoPalabras.Add("LIGON"); listadoPalabras.Add("LIJAR"); listadoPalabras.Add("LIMAR"); listadoPalabras.Add("LIMBO");
        listadoPalabras.Add("LIMON"); listadoPalabras.Add("LINCE"); listadoPalabras.Add("LINDO"); listadoPalabras.Add("LISTO"); listadoPalabras.Add("LITRO");
        listadoPalabras.Add("LORZA"); listadoPalabras.Add("LUCIR"); listadoPalabras.Add("LUCRO"); listadoPalabras.Add("LUGAR");
        // M
        listadoPalabras.Add("MACHO"); listadoPalabras.Add("MADRE"); listadoPalabras.Add("MIGAS"); listadoPalabras.Add("MAGNO"); listadoPalabras.Add("MAGRO");
        listadoPalabras.Add("MANCO"); listadoPalabras.Add("MANDO"); listadoPalabras.Add("MANGO"); listadoPalabras.Add("MANSO"); listadoPalabras.Add("MAREO");
        listadoPalabras.Add("MARZO"); listadoPalabras.Add("MAYOR"); listadoPalabras.Add("MEADO"); listadoPalabras.Add("MEDIA"); listadoPalabras.Add("MEDIO");
        listadoPalabras.Add("MEDIR"); listadoPalabras.Add("MEJOR"); listadoPalabras.Add("MELON"); listadoPalabras.Add("MENOR"); listadoPalabras.Add("MESON");
        listadoPalabras.Add("METAL"); listadoPalabras.Add("METRO"); listadoPalabras.Add("MICRA"); listadoPalabras.Add("MIEDO"); listadoPalabras.Add("MINGA");
        listadoPalabras.Add("MIOPE"); listadoPalabras.Add("MITAD"); listadoPalabras.Add("MIXTO"); listadoPalabras.Add("MOFAR"); listadoPalabras.Add("MOHIN");
        listadoPalabras.Add("MOJAR"); listadoPalabras.Add("MOLAR"); listadoPalabras.Add("MOLDE"); listadoPalabras.Add("MONJA"); listadoPalabras.Add("MONJE");
        listadoPalabras.Add("MONTE"); listadoPalabras.Add("MORAL"); listadoPalabras.Add("MORAS"); listadoPalabras.Add("MORSA"); listadoPalabras.Add("MOSCA");
        listadoPalabras.Add("MOTEL"); listadoPalabras.Add("MOTIN"); listadoPalabras.Add("MOVER"); listadoPalabras.Add("MOVIL"); listadoPalabras.Add("MUDAR");
        listadoPalabras.Add("MUECA"); listadoPalabras.Add("MUGIR"); listadoPalabras.Add("MUGRE"); listadoPalabras.Add("MUJER"); listadoPalabras.Add("MUSLO");
        listadoPalabras.Add("MULTA"); listadoPalabras.Add("MURAL"); listadoPalabras.Add("MUSGO");
        // N
        listadoPalabras.Add("NACER"); listadoPalabras.Add("NADIE"); listadoPalabras.Add("NADIR"); listadoPalabras.Add("NARCO"); listadoPalabras.Add("NARDO");
        listadoPalabras.Add("NARIZ"); listadoPalabras.Add("NAVIO"); listadoPalabras.Add("NECIO"); listadoPalabras.Add("NEGRO"); listadoPalabras.Add("NEVAR");
        listadoPalabras.Add("NIETO"); listadoPalabras.Add("NIVEL"); listadoPalabras.Add("NOBEL"); listadoPalabras.Add("NOBLE"); listadoPalabras.Add("NOCHE");
        listadoPalabras.Add("NOGAL"); listadoPalabras.Add("NORIA"); listadoPalabras.Add("NORTE"); listadoPalabras.Add("NOVIA"); listadoPalabras.Add("NUERA");
        listadoPalabras.Add("NUERO"); listadoPalabras.Add("NUEVO");
        // O
        listadoPalabras.Add("OBLEA"); listadoPalabras.Add("ODIAR"); listadoPalabras.Add("OJEAR"); listadoPalabras.Add("OLIVA"); listadoPalabras.Add("OMEGA");
        listadoPalabras.Add("OPERA"); listadoPalabras.Add("OPTAR"); listadoPalabras.Add("ORDEN"); listadoPalabras.Add("OREJA"); listadoPalabras.Add("ORGIA");
        listadoPalabras.Add("ORINA"); listadoPalabras.Add("ORUGA"); listadoPalabras.Add("OSTIA"); listadoPalabras.Add("OSTRA"); listadoPalabras.Add("OTEAR");
        listadoPalabras.Add("OVALO"); listadoPalabras.Add("OVEJA");
        // P
        listadoPalabras.Add("PACTO"); listadoPalabras.Add("PADRE"); listadoPalabras.Add("PALCO"); listadoPalabras.Add("PARDO"); listadoPalabras.Add("PARED");
        listadoPalabras.Add("PAREO"); listadoPalabras.Add("PASTO"); listadoPalabras.Add("PATIN"); listadoPalabras.Add("PATIO"); listadoPalabras.Add("PECAR");
        listadoPalabras.Add("PECHO"); listadoPalabras.Add("PECIO"); listadoPalabras.Add("PEDIR"); listadoPalabras.Add("PEGAR"); listadoPalabras.Add("PELAR");
        listadoPalabras.Add("PELTA"); listadoPalabras.Add("PENAL"); listadoPalabras.Add("PERAL"); listadoPalabras.Add("PERLA"); listadoPalabras.Add("PERNO");
        listadoPalabras.Add("PERSA"); listadoPalabras.Add("PESAR"); listadoPalabras.Add("PESCA"); listadoPalabras.Add("PEZON"); listadoPalabras.Add("PIANO");
        listadoPalabras.Add("PICAR"); listadoPalabras.Add("PICHA"); listadoPalabras.Add("PICOR"); listadoPalabras.Add("PIEZA"); listadoPalabras.Add("PILON");
        listadoPalabras.Add("PINAR"); listadoPalabras.Add("PINZA"); listadoPalabras.Add("PISAR"); listadoPalabras.Add("PISTA"); listadoPalabras.Add("PITAR");
        listadoPalabras.Add("PIXEL"); listadoPalabras.Add("PLANO"); listadoPalabras.Add("PLATO"); listadoPalabras.Add("PLUMA"); listadoPalabras.Add("POBRE");
        listadoPalabras.Add("PODAR"); listadoPalabras.Add("PODER"); listadoPalabras.Add("POEMA"); listadoPalabras.Add("POETA"); listadoPalabras.Add("POLAR");
        listadoPalabras.Add("POLEN"); listadoPalabras.Add("PONER"); listadoPalabras.Add("POSAR"); listadoPalabras.Add("POSTE"); listadoPalabras.Add("POTAR");
        listadoPalabras.Add("PRADO"); listadoPalabras.Add("PRESA"); listadoPalabras.Add("PRESO"); listadoPalabras.Add("PRISA"); listadoPalabras.Add("PROLE");
        listadoPalabras.Add("PROSA"); listadoPalabras.Add("PUBIS"); listadoPalabras.Add("PUDIN"); listadoPalabras.Add("PUDOR"); listadoPalabras.Add("PUJAR");
        listadoPalabras.Add("PULGA"); listadoPalabras.Add("PULIR"); listadoPalabras.Add("PULSO"); listadoPalabras.Add("PUNTO"); listadoPalabras.Add("PUNTA");
        // Q
        listadoPalabras.Add("QUEJA"); listadoPalabras.Add("QUESO");
        // R
        listadoPalabras.Add("RADIO"); listadoPalabras.Add("RAIDO"); listadoPalabras.Add("RANGO"); listadoPalabras.Add("RASGO"); listadoPalabras.Add("RATON");
        listadoPalabras.Add("RAUDO"); listadoPalabras.Add("RAZON"); listadoPalabras.Add("RECIO"); listadoPalabras.Add("RECTA"); listadoPalabras.Add("REGIO");
        listadoPalabras.Add("REGLA"); listadoPalabras.Add("REINA"); listadoPalabras.Add("REINO"); listadoPalabras.Add("RELAX"); listadoPalabras.Add("RELOJ");
        listadoPalabras.Add("RAMEN"); listadoPalabras.Add("RESTA"); listadoPalabras.Add("REUMA"); listadoPalabras.Add("REZAR"); listadoPalabras.Add("RIEGO");
        listadoPalabras.Add("RISCO"); listadoPalabras.Add("RITMO"); listadoPalabras.Add("RIZAR"); listadoPalabras.Add("ROBLE"); listadoPalabras.Add("RODIL");
        listadoPalabras.Add("ROGAR"); listadoPalabras.Add("RONDA"); listadoPalabras.Add("ROSAL"); listadoPalabras.Add("RUBIO"); listadoPalabras.Add("RUEDA");
        listadoPalabras.Add("RUEGO"); listadoPalabras.Add("RUGBY"); listadoPalabras.Add("RUIDO"); listadoPalabras.Add("RUINA"); listadoPalabras.Add("RUMBA");
        listadoPalabras.Add("RUMBO"); listadoPalabras.Add("RUPIA");
        // S
        listadoPalabras.Add("SABER"); listadoPalabras.Add("SABIO"); listadoPalabras.Add("SABLE"); listadoPalabras.Add("SABOR"); listadoPalabras.Add("SALDO");
        listadoPalabras.Add("SALIR"); listadoPalabras.Add("SALUD"); listadoPalabras.Add("SANTO"); listadoPalabras.Add("SAQUE"); listadoPalabras.Add("SATEN");
        listadoPalabras.Add("SAUCE"); listadoPalabras.Add("SECAR"); listadoPalabras.Add("SECTA"); listadoPalabras.Add("SEDAL"); listadoPalabras.Add("SELVA");
        listadoPalabras.Add("SEPIA"); listadoPalabras.Add("SERIO"); listadoPalabras.Add("SEXTO"); listadoPalabras.Add("SIDRA"); listadoPalabras.Add("SIFON");
        listadoPalabras.Add("SIGLA"); listadoPalabras.Add("SIGLO"); listadoPalabras.Add("SIGNO"); listadoPalabras.Add("SOBRE"); listadoPalabras.Add("SPRAY");
        listadoPalabras.Add("SUAVE"); listadoPalabras.Add("SUBIR"); listadoPalabras.Add("SUCIO"); listadoPalabras.Add("SUDAR"); listadoPalabras.Add("SUDOR");
        listadoPalabras.Add("SUELA"); listadoPalabras.Add("SUELO"); listadoPalabras.Add("SUMAR"); listadoPalabras.Add("SURCO");
        // T
        listadoPalabras.Add("TACON"); listadoPalabras.Add("TALCO"); listadoPalabras.Add("TALON"); listadoPalabras.Add("TANGO"); listadoPalabras.Add("TAPIZ");
        listadoPalabras.Add("TAPON"); listadoPalabras.Add("TARDE"); listadoPalabras.Add("TAURO"); listadoPalabras.Add("TAZON"); listadoPalabras.Add("TECHO");
        listadoPalabras.Add("TECLA"); listadoPalabras.Add("TELAR"); listadoPalabras.Add("TELON"); listadoPalabras.Add("TEMOR"); listadoPalabras.Add("TEMPO");
        listadoPalabras.Add("TENAZ"); listadoPalabras.Add("TENOR"); listadoPalabras.Add("TERCO"); listadoPalabras.Add("TIFON"); listadoPalabras.Add("TIGRE");
        listadoPalabras.Add("TILDE"); listadoPalabras.Add("TIMAR"); listadoPalabras.Add("TIMON"); listadoPalabras.Add("TOCAR"); listadoPalabras.Add("TORAX");
        listadoPalabras.Add("TORPE"); listadoPalabras.Add("TRAGO"); listadoPalabras.Add("TRAJE"); listadoPalabras.Add("TRAPO"); listadoPalabras.Add("TRENA");
        listadoPalabras.Add("TRIBU"); listadoPalabras.Add("TRIGO"); listadoPalabras.Add("TRIPA"); listadoPalabras.Add("TROPA"); listadoPalabras.Add("TRUCO");
        listadoPalabras.Add("TRUFA"); listadoPalabras.Add("TUMBA"); listadoPalabras.Add("TUMOR");
        // U
        listadoPalabras.Add("ULTRA"); listadoPalabras.Add("UMBRA"); listadoPalabras.Add("UNICO"); listadoPalabras.Add("UNTAR");
        // V
        listadoPalabras.Add("VACIO"); listadoPalabras.Add("VACUO"); listadoPalabras.Add("VAGON"); listadoPalabras.Add("VALET"); listadoPalabras.Add("VALOR");
        listadoPalabras.Add("VAPOR"); listadoPalabras.Add("VARON"); listadoPalabras.Add("VEJAR"); listadoPalabras.Add("VELOZ"); listadoPalabras.Add("VENDA");
        listadoPalabras.Add("VERBO"); listadoPalabras.Add("VERGA"); listadoPalabras.Add("VERJA"); listadoPalabras.Add("VERSO"); listadoPalabras.Add("VETAR");
        listadoPalabras.Add("VIAJE"); listadoPalabras.Add("VIDEO"); listadoPalabras.Add("VIEJO"); listadoPalabras.Add("VIGOR"); listadoPalabras.Add("VIRAL");
        listadoPalabras.Add("VIRGO"); listadoPalabras.Add("VIRUS"); listadoPalabras.Add("VISTA"); listadoPalabras.Add("VIUDO"); listadoPalabras.Add("VOLAR");
        listadoPalabras.Add("VORAZ"); listadoPalabras.Add("VOTAR");
        // W
        // X
        // Y
        listadoPalabras.Add("YEGUA"); listadoPalabras.Add("YELMO"); listadoPalabras.Add("YERMO"); listadoPalabras.Add("YOGUR");
        // Z
        listadoPalabras.Add("ZAFIO"); listadoPalabras.Add("ZOMBI"); listadoPalabras.Add("ZUECO"); listadoPalabras.Add("ZUMBA"); listadoPalabras.Add("ZURDO");
    }

    private void InicializarHashMapButtons()
    {
        buttonsHash.Add("Q", buttonQ);
        buttonsHash.Add("W", buttonW);
        buttonsHash.Add("E", buttonE);
        buttonsHash.Add("R", buttonR);
        buttonsHash.Add("T", buttonT);
        buttonsHash.Add("Y", buttonY);
        buttonsHash.Add("U", buttonU);
        buttonsHash.Add("I", buttonI);
        buttonsHash.Add("O", buttonO);
        buttonsHash.Add("P", buttonP);
        buttonsHash.Add("A", buttonA);
        buttonsHash.Add("S", buttonS);
        buttonsHash.Add("D", buttonD);
        buttonsHash.Add("F", buttonF);
        buttonsHash.Add("G", buttonG);
        buttonsHash.Add("H", buttonH);
        buttonsHash.Add("J", buttonJ);
        buttonsHash.Add("K", buttonK);
        buttonsHash.Add("L", buttonL);
        buttonsHash.Add("Z", buttonZ);
        buttonsHash.Add("X", buttonX);
        buttonsHash.Add("C", buttonC);
        buttonsHash.Add("V", buttonV);
        buttonsHash.Add("B", buttonB);
        buttonsHash.Add("N", buttonN);
        buttonsHash.Add("M", buttonM);
    }

    private bool buscarEnBaseDeDatos(string guess)
    {
        foreach (Word wordAux in wordsInJson.words)
        {
            if (wordAux.word.Equals(guess))
            {
                Debug.Log("Word in database");
                return true;
            }
        }
        return false;
    }

    private int ObtenerSemilla()
    {
        var segundos = System.DateTime.UtcNow.Second;
        var minutos = System.DateTime.UtcNow.Minute;
        var horas = System.DateTime.UtcNow.Hour;
        return segundos + minutos + horas;
    }
}
