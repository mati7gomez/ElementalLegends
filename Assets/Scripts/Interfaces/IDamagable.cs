using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void ReceiveDamage(Transform enemyTransform, float damage, float energy, float forceX, float forceY); //Recibimos el transform enemigo para poder calcular en que direccion sera golpeado y lanzado el jugador
}
