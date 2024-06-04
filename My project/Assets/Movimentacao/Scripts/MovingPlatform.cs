using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;  // Ponto A para o movimento da plataforma
    public Transform pointB;  // Ponto B para o movimento da plataforma
    public float speed = 2f;   // Velocidade da plataforma

    private Vector3 target;    // Destino atual da plataforma

    private void Start()
    {
        target = pointA.position;
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        // Move a plataforma em direção ao destino atual
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Troca o destino quando a plataforma atinge o ponto A ou B
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            if (target == pointA.position)
            {
                target = pointB.position;
            }
            else
            {
                target = pointA.position;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered platform collision!");
            // Torna o jogador filho da plataforma
            collision.transform.SetParent(transform);
            collision.gameObject.GetComponent<PlayerMovement>().IsOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exited platform collision!");
            // Remove o jogador como filho da plataforma
            collision.transform.SetParent(null);
            collision.gameObject.GetComponent<PlayerMovement>().IsOnPlatform = false;
        }
    }
}
