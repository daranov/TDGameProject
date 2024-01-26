using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [HideInInspector]
    public float speedMod = 1f;
    private Path thePath;
    private int currentPoint;
    private bool reachEnd;

    public float timeBetweenAttacks, damagePerAttack;
    private float attackCounter;

    private Castle theCastle;
    private int selectedAttackPoint;

    public bool isFlying;
    public float flyHeight;
    void Start()
    {
        if(thePath == null)
        {
            thePath = FindObjectOfType<Path>();
        }
        if(theCastle == null)
        {
            theCastle = FindObjectOfType<Castle>();
        }
        
        attackCounter = timeBetweenAttacks;

        if (isFlying)
        {
            transform.position += Vector3.up * flyHeight;
            currentPoint = thePath.points.Length - 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(LevelManager.instance.levelActive)
        {
            if (!reachEnd)
            {
                transform.LookAt(thePath.points[currentPoint]);

                if (!isFlying)
                {
                    transform.position = Vector3.MoveTowards(transform.position, thePath.points[currentPoint].position, moveSpeed * Time.deltaTime * speedMod);
                    if (Vector3.Distance(transform.position, thePath.points[currentPoint].position) < .01f)
                    {
                        currentPoint = currentPoint + 1;
                        if (currentPoint >= thePath.points.Length)
                        {
                            reachEnd = true;
                            selectedAttackPoint = Random.Range(0, theCastle.attactPoints.Length);
                        }
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, thePath.points[currentPoint].position + (Vector3.up * flyHeight), moveSpeed * Time.deltaTime * speedMod);
                    if (Vector3.Distance(transform.position, thePath.points[currentPoint].position + (Vector3.up * flyHeight)) < .01f)
                    {
                        currentPoint = currentPoint + 1;
                        if (currentPoint >= thePath.points.Length)
                        {
                            reachEnd = true;
                            selectedAttackPoint = Random.Range(0, theCastle.attactPoints.Length);
                        }
                    }
                }
            }
            else
            {
                if (!isFlying)
                {
                    transform.position = Vector3.MoveTowards(transform.position, theCastle.attactPoints[selectedAttackPoint].position, moveSpeed * Time.deltaTime * speedMod);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, theCastle.attactPoints[selectedAttackPoint].position + (Vector3.up * flyHeight), moveSpeed * Time.deltaTime * speedMod);
                }
                attackCounter -= Time.deltaTime;
                if(attackCounter <= 0)
                {
                    attackCounter = timeBetweenAttacks;
                    theCastle.TakeDamage(damagePerAttack);
                }
            }
        }
    }
    public void Setup(Castle newCastle, Path newPath)
    {
        theCastle = newCastle;
        thePath = newPath;
    }
}
