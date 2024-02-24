using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockBehaviour : MonoBehaviour
{
    public GameObject manager;
    public Flock flock;
    public Vector2 location = Vector2.zero;
    public Vector2 velocity;
    
    private Rigidbody2D boidrBody;
    private RaycastHit2D[] hits;
    private Vector2 mousePointer = Vector2.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        boidrBody = this.GetComponent<Rigidbody2D>();
        manager = transform.parent.gameObject;
        flock = manager.GetComponent<Flock>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FLockController();
        Avoidance();
        FacingFront();
        mousePointer = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    }

    private void FacingFront()
    {
        transform.up = boidrBody.velocity;
    }

    private Vector2 Seek (Vector2 target)
    {
        return (target - location);
    }

    private Vector2 Alignment()
    {
        float neighborRadius = flock.neighborRadius;
        Vector2 sum = Vector2.zero;
        hits = Physics2D.CircleCastAll(location, neighborRadius, Vector2.zero);
        int count = 0;
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject == this.gameObject) continue;
            if(hit.transform.gameObject.GetComponent<FlockBehaviour>().manager == manager)
            {
                count++;
                sum += hit.transform.GetComponent<Rigidbody2D>().velocity;
            }
        }

        if(count > 0)
        {
            sum /= count;
            return (sum - velocity);
        }
        else
        {
            return sum;
        }
    }

    private void Avoidance()
    {
        hits = Physics2D.CircleCastAll(location, 1f, Vector2.zero);
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.transform.gameObject.GetComponent<FlockBehaviour>().manager !=  manager)
            {
                hit.rigidbody.AddForce(hit.transform.position - this.transform.position);
            }
        }
    }

    private Vector2 Cohesion()
    {
        float neighborRadius = flock.neighborRadius;
        Vector2 sum = Vector2.zero;
        hits = Physics2D.CircleCastAll(location, neighborRadius, Vector2.zero);
        int count = 0;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject == this.gameObject) continue;
            if (hit.transform.gameObject.GetComponent<FlockBehaviour>().manager == manager)
            {
                count++;
                sum += hit.transform.GetComponent<Rigidbody2D>().velocity;
            }
        }

        if (count > 0)
        {
            sum /= count;
            return Seek(sum);
        }
        else
        {
            return sum;
        }
    }

    private void ApplyForce(Vector2 force)
    {
        if(force.magnitude > flock.driveFactor)
        {
            force = force.normalized * manager.GetComponent<Flock>().driveFactor;
        }
        boidrBody.AddForce(force);

        if(boidrBody.velocity.magnitude > flock.driveFactor)
        {
            boidrBody.velocity = boidrBody.velocity.normalized * flock.maxSpeed;
        }

        if(Seek(mousePointer).magnitude < 1)
        {
            boidrBody.drag = 1f;
        }
        else
        {
            boidrBody.drag = 0;
        }
    }

    private void FLockController()
    {
        location = this.transform.position;
        velocity = boidrBody.velocity;
        Vector2 positionToMove = Seek(mousePointer);

        Vector2 boidMovement = Alignment() + Cohesion() + positionToMove;

        ApplyForce(boidMovement.normalized);
    }
}
