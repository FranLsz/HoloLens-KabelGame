using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject BaseDisk;
    public GameObject DiskContainer;
    public GameObject Cursor;
    public double DiskGenerationRadius;
    public Text Score;
    public Text HighScore;
    public Text Timer;
    public Text FirstCounter;
    public Text InstructionsLabel;
    public Text NewGameLabel;
    public Text RepeatNewGameLabel;
    public int SecondsLimit;

    private bool _gameStarted = false;
    private bool _gameEndend = false;
    private int _intervalSeconds = 1;
    private float _nextTime = 3;

    //void Awake()
    //{
    //    Application.targetFrameRate = 120;
    //}

    void Start()
    {
        PrepareScene(true);
    }

    private void PrepareScene(bool startGame = false)
    {
       

        Score.text = "0";
        Timer.text = SecondsLimit.ToString();
        FirstCounter.text = "3";
        _gameStarted = false;
        _gameEndend = false;

        RepeatNewGameLabel.gameObject.SetActive(false);
        InstructionsLabel.gameObject.SetActive(true);

        // Nos cargamos todos los discos 
        foreach (var t in GameObject.FindGameObjectsWithTag("Disk"))
            Destroy(t);

        if (startGame)
            StartCoroutine(StartGame());
    }

    public void OnStartVoice()
    {
        if (_gameEndend)
            PrepareScene(true);
    }

    private IEnumerator StartGame()
    {

        yield return new WaitForSeconds(1);
        FirstCounter.text = "2";

        yield return new WaitForSeconds(1);
        FirstCounter.text = "1";

        yield return new WaitForSeconds(1);
        FirstCounter.text = string.Empty;

        InstructionsLabel.gameObject.SetActive(false);

        _gameStarted = true;
        Cursor.SetActive(true);
        addDisk();
    }

    public void OnDiskPulsed()
    {
        if (_gameStarted)
            addDisk();
    }

    public void addDisk()
    {
        // generamos una posicion aleatoria en una circunferencia en relacion al radio (DiskGenerationRadius)
        double rDouble = new System.Random().NextDouble() * 1;
        var angle = rDouble * Math.PI * 2;

        var x = (float)(Math.Cos(angle) * DiskGenerationRadius);
        var z = (float)(Math.Sin(angle) * DiskGenerationRadius);

        // instanciamos un disco a partir del disco base, en la posicion resuelta a partir del calculo anterior y sin rotacion
        var newDisk = Instantiate(BaseDisk, new Vector3(x, 0, z), Quaternion.identity) as GameObject;

        // le damos el tag de "Disk" (para poder encontrarlo a la hora de ser borrado)
        newDisk.tag = "Disk";

        // modificamos el padre de dicho disco, para que sea DiskContainer (de tal forma que se controle la posicion respecto a la camara)
        newDisk.transform.parent = DiskContainer.transform;

        // activamos el elemento (en caso de que no lo estuviera ya)
        newDisk.SetActive(true);
    }

    private void decreaseTimer()
    {
        if (Timer.text != "0")
            Timer.text = (int.Parse(Timer.text) - 1).ToString();
        else
        {
            _gameStarted = false;
            _gameEndend = true;
        }
    }



    void Update()
    {

        if (Time.time >= _nextTime)
        {
            if (_gameStarted)
                Invoke("decreaseTimer", 1);

            _nextTime += _intervalSeconds;
        }


        if (_gameEndend)
        {
            Cursor.SetActive(false);
            RepeatNewGameLabel.gameObject.SetActive(true);

            var score = int.Parse(Score.text);
            var highScore = int.Parse(HighScore.text);

            if (score > highScore)
                HighScore.text = Score.text;


        }
    }
}
