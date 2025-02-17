using UnityEngine;
using System.Linq;
using UnityEditor.Build;
using UnityEngine.UIElements;

public class IceWallScript : MonoBehaviour
{

    public float health;
    public float duration;
    public float raisSpeed;
    public float destroyDelay;
    public float destroyPushForce;
    public float destroyRotForce;
    public float destroySpeed;

    private SkinnedMeshRenderer rend;
    private MeshCollider col;
    private float blendAmount = 0;
    private bool isRaised = false;
    void Start()
    {
        rend = GetComponent<SkinnedMeshRenderer>();
        col = GetComponent<MeshCollider>();

        var iceWalls = GetComponentsInChildren<IceWallScript>();
        foreach(IceWallScript wall in iceWalls)
        {
            wall.transform.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRaised)
        {
            blendAmount += raisSpeed * Time.deltaTime;
            rend.SetBlendShapeWeight(0, blendAmount);
            Mesh bakeMesh = new Mesh();
            rend.BakeMesh(bakeMesh);
            col.sharedMesh = bakeMesh;
            if (blendAmount >= 100) { isRaised = true; }
        }

        if(health <= 0)
        {
            Component[] fractures = GetComponentsInChildren(typeof(Rigidbody), true);
            foreach(Rigidbody child  in fractures)
            {
                child.transform.SetParent(null);
                child.gameObject.SetActive(true);
                Destroy(child.gameObject, destroyDelay);

                var forceDir = child.position - transform.position;
                if(child != transform)
                {
                    Vector3 randomTorque;
                    randomTorque.x = Random.Range(-destroyRotForce, destroyRotForce);
                    randomTorque.y = Random.Range(-destroyRotForce, destroyRotForce);
                    randomTorque.z = Random.Range(-destroyRotForce, destroyRotForce);

                    child.AddTorque(randomTorque);
                    child.AddTorque(forceDir.normalized * destroyPushForce, ForceMode.VelocityChange);

                }
                if (child == fractures.Last()) { Destroy(gameObject);  }
            }
        }
        if(duration <= 0) { health = 0; } else { duration -= Time.deltaTime; }
    }
}
