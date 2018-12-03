﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MLAgents;

public class ZombieHordeAgent : Agent {

    public float speed = 5;
    public float visionRange = 30; //el rango de percepción del zombie, si el humano está dentro del rango lo perseguirá
    public float maxCoordinate = 10; //la mayor coordenada x o z del mapa
    public Transform human; //el humano al que perseguir
    public Vector3 originalPosition;
    //private float lastDist2Human;
    private GameObject[] zombies;

    private NavMeshAgent myNavAgent;
    private Vector3 goal;
    private int walkStep = 0;
    public float walk = 3;

    public void Start()
    {
        originalPosition = transform.position;
        //lastDist2Human = Vector3.Distance(originalPosition, human.position);
        zombies = GameObject.FindGameObjectsWithTag("Zombie");

        myNavAgent = transform.GetComponent<NavMeshAgent>();
    }

    public override void CollectObservations()
    {
        //My position relative
        Vector3 myPosition = transform.position;
        float xNorm = myPosition.x / maxCoordinate;
        float zNorm = myPosition.z / maxCoordinate;
        AddVectorObs(xNorm);
        AddVectorObs(zNorm);
        
        //Human relative position
        Vector3 humanRelativePosition = human.position - transform.position;
        xNorm = humanRelativePosition.x / maxCoordinate;
        zNorm = humanRelativePosition.z / maxCoordinate;
        AddVectorObs(xNorm);
        AddVectorObs(zNorm);

        //The direction im facing
        Vector3 forwardNorm = Vector3.Normalize(transform.forward);
        float xForw = forwardNorm.x;
        float zForw = forwardNorm.z;
        AddVectorObs(xForw);
        AddVectorObs(zForw);
        
        //The 2 closest zombies position
        Transform[] closestZombies = get2ClosestZombies();
        
        if(closestZombies[0] != null)
        {
            Vector3 zombie1RelativePosition = closestZombies[0].position - transform.position;
            AddVectorObs(zombie1RelativePosition.x / maxCoordinate);
            AddVectorObs(zombie1RelativePosition.z / maxCoordinate);

            if(closestZombies[1] != null)
            {
                Vector3 zombie2RelativePosition = closestZombies[1].position - transform.position;
                AddVectorObs(zombie2RelativePosition.x / maxCoordinate);
                AddVectorObs(zombie2RelativePosition.z / maxCoordinate);
            }
            else
            {
                AddVectorObs(0);
                AddVectorObs(0);
            }
        }
        else
        {
            AddVectorObs(0);
            AddVectorObs(0);
            AddVectorObs(0);
            AddVectorObs(0);
        }

        //The zombie vision
        Vector3 forward = transform.forward;
        Vector3 leftInt = rotateVector(45 * Mathf.PI / 180, forward);
        Vector3 rightInt = rotateVector(-45 * Mathf.PI / 180, forward);

        Ray rayCentral = new Ray(transform.position, forward);
        Ray rayLeftInt = new Ray(transform.position, leftInt);
        Ray rayLeftExt = new Ray(transform.position, -transform.right);
        Ray rayRightInt = new Ray(transform.position, rightInt);
        Ray rayRightExt = new Ray(transform.position, transform.right);

        //Debug.DrawRay(transform.position, leftInt);
        //Debug.DrawRay(transform.position, transform.forward);
        //Debug.DrawRay(transform.position, rightInt);

        AddVectorObs(shoot(rayCentral));
        AddVectorObs(shoot(rayLeftInt));
        AddVectorObs(shoot(rayLeftExt));
        AddVectorObs(shoot(rayRightInt));
        AddVectorObs(shoot(rayRightExt));
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float dist2Human = Vector3.Distance(transform.position, human.position);

        ////to achive optimal time 
        //AddReward(-0.001f);

        ////has it fell somehow?
        //if (transform.position.y < -1)
        //{
        //    AddReward(-1.0f);
        //    transform.position = originalPosition;
        //}

        //advance?
        int advance = (int)vectorAction[0];

        //obtain direction
        int direction = (int)vectorAction[1];

        //si está en el rango nos movemos
        if (dist2Human < visionRange)
        {
            //paramos el nav agent
            myNavAgent.isStopped = true;
            walkStep = 0;

            //set direction
            //we need a zero to not act
            switch (direction)
            {
                case 1:
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 2:
                    transform.rotation = Quaternion.Euler(0, 45, 0);
                    break;
                case 3:
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case 4:
                    transform.rotation = Quaternion.Euler(0, 135, 0);
                    break;
                case 5:
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case 6:
                    transform.rotation = Quaternion.Euler(0, 225, 0);
                    break;
                case 7:
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                case 8:
                    transform.rotation = Quaternion.Euler(0, 315, 0);
                    break;
            }

            //advance
            if (advance == 1)
            {
                transform.position += transform.forward * Time.deltaTime * speed;
            }
        } 
        else
        {
            //nos movemos a una dirección aleatoria
            myNavAgent.isStopped = false;
            //Debug.Log("Goal " + goal);
            //Debug.Log("Step " + walkStep);

            switch (walkStep)
            {
                case 0:
                    //primera dirección aleatoria
                    goal = randomGoal();
                    walkStep++;
                    break;
                case 1:
                    //segunda dirección aleatoria
                    if (Vector3.Distance(transform.position, goal) < 0.5)
                    {
                        goal = randomGoal();
                        walkStep++;
                    }
                    break;
                case 2:
                    //nueva dirección aleatoria
                    if (Vector3.Distance(transform.position, goal) < 0.5)
                    {
                        goal = randomGoal();
                        walkStep++;
                    }
                    break;
                case 3:
                    //ahora el barrido contrario
                    if (Vector3.Distance(transform.position, goal) < 0.5)
                    {
                        goal = originalPosition - (goal - originalPosition);
                        walkStep = 2;
                    }
                    break;
            }

            myNavAgent.destination = goal;
        }

        ////is distance lower?
        ////reward based on distance 0 - 0.1
        //if(dist2Human < lastDist2Human)
        //{
        //    float reward =  -(dist2Human - maxCoordinate) / ( maxCoordinate * 10); //the priority is to get there
        //    AddReward(reward);
        //    //Monitor.Log("Agent distance reward", reward.ToString(), null);
        //}

        ////has human been captured?
        //foreach(GameObject zombie in zombies)
        //{
        //    if (Vector3.Distance(zombie.transform.position, human.position) < 0.5)
        //    {
        //        AddReward(1f);
        //    }
        //}
    }

    //public override void AgentReset()
    //{
    //    transform.position = originalPosition;
    //}

    Transform[] get2ClosestZombies()
    {
        Transform zombie1 = null;
        Transform zombie2 = null;

        float closestDistanceSqr = Mathf.Infinity;
        float secondClosestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject zombie in zombies)
        {
            Vector3 directionToZombie = zombie.transform.position - currentPosition;
            float dSqrToZombie = directionToZombie.sqrMagnitude;
            if (dSqrToZombie < secondClosestDistanceSqr)
            {
                if(dSqrToZombie < closestDistanceSqr)
                {
                    //new scnd closest
                    secondClosestDistanceSqr = closestDistanceSqr;
                    zombie2 = zombie1;

                    //new closest
                    closestDistanceSqr = dSqrToZombie;
                    zombie1 = zombie.transform;
                }
                else
                {
                    //new scnd closest
                    secondClosestDistanceSqr = dSqrToZombie;
                    zombie2 = zombie.transform;
                }
            }
        }

        Transform[] closestZombies = new Transform[2];

        closestZombies[0] = zombie1;
        closestZombies[1] = zombie2;

        return closestZombies;
    }

    Vector3 rotateVector(float angle, Vector3 vector)
    {
        float x = vector.x;
        float z = vector.z;
        float zp = z * Mathf.Cos(angle) - x * Mathf.Sin(angle);
        float xp = z * Mathf.Sin(angle) + x * Mathf.Cos(angle);

        return new Vector3(xp, 0, zp);
    }

    float shoot(Ray ray)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 3))
        {
            return hitInfo.distance;
        }
        else
        {
            return -1;
        }
    }

    Vector3 randomGoal()
    {
        return new Vector3(originalPosition.x + Random.Range(-walk, walk), 1, originalPosition.z + Random.Range(-walk, walk));
    }
}
