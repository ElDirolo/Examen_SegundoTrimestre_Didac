using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    State currentState;
    NavMeshAgent agentAI;

    enum State
    {
        Patrullando,
        Perserguir,
        Golpeando,
    }
    public int destinos;
    public Transform[] puntos;
    
    public Transform player;
    public float rangoVision;
    public float rangoAtaque;
    

    void Awake()
    {
        agentAI = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Start()
    {
        currentState = State.Patrullando;
        destinos = Random.Range(0, puntos.Length);
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrullando:
                Patrulla();
            break;
            case State.Perserguir:
                Perseguiendo();
            break;
            case State.Golpeando:
                Golpe();
            break; 
            default:
                Patrulla();
            break;           
        }
    }



    void Patrulla()
    {
        agentAI.destination = puntos[destinos].position;
        
        if(Vector3.Distance(transform.position, puntos[destinos].position) < 1f)
        {
            destinos = Random.Range(0, puntos.Length);
        }

        if(DistanceToTarget(rangoVision))
        {
            currentState = State.Perserguir;
        }
    }

    void Perseguiendo()
    {
        agentAI.destination = player.position;

        if(!DistanceToTarget(rangoVision))
        {
            currentState = State.Patrullando;
        }
        if(DistanceToTarget(rangoAtaque))
        {
            currentState = State.Golpeando;
        }
    }

    void Golpe()
    {
        Debug.Log("Tocado");
        
        if(!DistanceToTarget(rangoAtaque))
        {
            currentState = State.Perserguir;
        }
    }

    bool DistanceToTarget(float distancia)
    {
        if(Vector3.Distance(transform.position, player.position) < distancia)
        {
            return true;
        }
        return false;
    }

    void OnDrawGizmos()
    {
        foreach (Transform point in puntos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(point.position, 1);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoVision);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}
