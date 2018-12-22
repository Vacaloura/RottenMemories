using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MLAgents;

public class ZombieHordeAgent : Agent
{
    public float speed = 5;
    public float visionRange = 30; //el rango de percepción del zombie, si el humano está dentro del rango lo perseguirá
    public float maxCoordinate = 10; //la mayor coordenada x o z del mapa
    public Transform human; //el humano al que perseguir
    public Vector3 originalPosition;
    private GameObject[] zombies;

    [HideInInspector] public NavMeshAgent myNavAgent;
    private Vector3 goal;
    private int walkStep = 0;
    public float walk = 3;
    public float timeCounter = 0;

    public float maxAtackRange = 5f;
    public float minAtackRange = 1f;

    public void Start()
    {
        originalPosition = transform.position;
        //lastDist2Human = Vector3.Distance(originalPosition, human.position);
        zombies = GameObject.FindGameObjectsWithTag("Zombie");

        myNavAgent = transform.GetComponent<NavMeshAgent>();

        //configuración en base a la dificultad
	/*
        GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        speed = gameController.currentGameData.zombieSpeed;
        maxAtackRange = gameController.currentGameData.zombieMaxAtackRange;
        visionRange = gameController.currentGameData.zombieVisionRange;
        Debug.Log("speed " + speed);
        Debug.Log("maxAtackRange " + maxAtackRange);
        Debug.Log("visionRange " + visionRange);
    	*/
    }

    public override void CollectObservations()
    {
        //My position relative
        Vector3 myPosition = transform.position;
        float xNorm = 0; //hack para el escenario final
        float zNorm = 0;
        AddVectorObs(xNorm);
        AddVectorObs(zNorm);

        //Human relative position
        Vector3 humanRelativePosition = human.position - transform.position;
        xNorm = humanRelativePosition.x / visionRange;
        zNorm = humanRelativePosition.z / visionRange;
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

        if (closestZombies[0] != null)
        {
            Vector3 zombie1RelativePosition = closestZombies[0].position - transform.position;
            AddVectorObs(zombie1RelativePosition.x / maxCoordinate);
            AddVectorObs(zombie1RelativePosition.z / maxCoordinate);

            if (closestZombies[1] != null)
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

        AddVectorObs(shoot(rayCentral));
        AddVectorObs(shoot(rayLeftInt));
        AddVectorObs(shoot(rayLeftExt));
        AddVectorObs(shoot(rayRightInt));
        AddVectorObs(shoot(rayRightExt));
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float dist2Human = Vector3.Distance(transform.position, human.position);
        bool decide = false;

        //obtain direction
        int direction = (int)vectorAction[1];

        //si está en el rango nos movemos
        if (dist2Human < visionRange) //caza
        {
            
            if (Vector3.Distance(human.position, this.transform.position) < maxAtackRange)
            {
                if (Vector3.Distance(human.position, this.transform.position) > minAtackRange)
                {
                    myNavAgent.isStopped = false;
                    myNavAgent.destination = human.position;
                    return;
                }
                else
                {
                    myNavAgent.isStopped = true;
                    return;
                }
            }

            //parche para limitar aumentar la continuidad del movimiento 2467
            //hay que añdir el contador time
            if (walkStep != 0)
            {
                //venimos de deambular por lo que hay que decidir una dirección objetivo
                timeCounter = 0; //reseteamos el contador temporal
                decide = true;
            }
            else
            {
                timeCounter += Time.deltaTime; //no venimos de deambular así que actualizamos el contador
                if (timeCounter > 0.66f)
                { //el contador ha pasado el umbral así que resteamos el contardor de tiempo
                    timeCounter = timeCounter - 0.66f;
                    decide = true;
                }
            }

            walkStep = 0;

            //set direction
            //we need a zero to not act
            if (decide)
            {

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

                Vector3 goal = transform.position + transform.forward * 3;
                myNavAgent.destination = goal;
            }
        }
        else //deambula
        {
            switch (walkStep)
            {
                case 0:
                    //dirección aleatoria inicial
                    goal = randomGoal();
                    walkStep++;
                    break;
                case 1:
                    //nueva dirección aleatoria
                    if (Vector3.Distance(transform.position, goal) < 0.5)
                    {
                        goal = randomGoal();
                        walkStep++;
                    }
                    break;
                case 2:
                    //segunda dirección aleatoria
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
                        walkStep = 1;
                    }
                    break;
            }

            myNavAgent.destination = goal;
        }
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
                if (dSqrToZombie < closestDistanceSqr)
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
