using UnityEngine;
using System.Collections;

public class WallBehaviour : MonoBehaviour
{
    void Start()
    {

    }

    void LateUpdate()
    {
        // Impide que la rotacion de la camara afecte a la posicion de los discos
        if (transform.rotation != Quaternion.Euler(0, 0, 0))
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
