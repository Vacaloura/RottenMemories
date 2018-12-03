using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapingHuman : MonoBehaviour {

    private GameObject[] zombies;
    public float speed = 5;
    public float maxCoordinate = 10;

    // Use this for initialization
    void Start () {
        zombies = GameObject.FindGameObjectsWithTag("Zombie");
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 escapeDir = new Vector3();

		foreach(GameObject zombie in zombies)
        {
            Vector3 dir2Zombie = transform.position - zombie.transform.position;
            escapeDir += dir2Zombie;
        }

        escapeDir = Vector3.Normalize(escapeDir);
        escapeDir.y = 0;
        transform.position +=  escapeDir * Time.deltaTime * speed;

        bool captured = false;

        foreach (GameObject zombie in zombies)
        {
            float dist2Human = Vector3.Distance(transform.position, zombie.transform.position);
            if (dist2Human < 1.5)
            {
                captured = true;
            }
        }

        if (captured)
        {
            reallocate();
        }
    }

    private void reallocate()
    {
        // Move the human to a new spot        
        //The human should be a litle far from the borders
        //The human begins at one corner and has to get to the other
        bool tryAgain = true;
        Vector3 position = new Vector3();
        while (tryAgain)
        {
            //random x or z?
            float xOrZ = Random.value;

            float a = (Random.value * 2 * (maxCoordinate - 1)) - (maxCoordinate - 1);
            float b = round(Random.value) * (maxCoordinate - 1);

            if (xOrZ < 0.5)
            {
                //random x
                position = new Vector3(a, 1.1f, b);
            }
            else
            {
                //random z
                position = new Vector3(b, 1.1f, a);
            }

            tryAgain = false;
            foreach (GameObject zombie in zombies)
            {
                if (Vector3.Distance(position, zombie.transform.position) < 1.5)
                {
                    tryAgain = true;
                }
            }
        }

        transform.position = position;
    }

    float round(float a)
    {
        if (a > 0.5)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
