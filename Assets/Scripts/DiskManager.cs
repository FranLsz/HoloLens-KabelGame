using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DiskManager : MonoBehaviour
{
    public int ForceSpeed;
    public GameObject UsedDiskContainer;
    public Camera MainCamera;
    public Text Score;

    // controla si el disco ya ha sido pulsado
    private bool _pulsed;

    void Start()
    {
        _pulsed = false;
    }

    void Update()
    {

        if (!_pulsed)
            // nos aseguramos de que el disco siempre mire a la camara mientras no haya sido pulsado
            transform.LookAt(transform.position + MainCamera.transform.rotation * Vector3.forward,
                MainCamera.transform.rotation * Vector3.up);


    }

    void OnSelect()
    {
        if (_pulsed) return;
        if (this.GetComponent<Rigidbody>()) return;

        _pulsed = true;

        // incrementamos la puntuación en 1
        Score.text = (int.Parse(Score.text) + 1).ToString();

        // modificamos el padre (para que el disco deje de moverse en relacion a la camara)
        gameObject.transform.parent = UsedDiskContainer.transform;

        // añadimos el componente Rigidbody para generar efecto gravedad
        var ridbod = this.gameObject.AddComponent<Rigidbody>();
        ridbod.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // impulsamos el objeto contra el entorno en la direccion en la que ha sido pulsado
        ridbod.AddForce(transform.forward * ForceSpeed);

        SendMessageUpwards("OnDiskPulsed");
    }
}
