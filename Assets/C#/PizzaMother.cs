using UnityEngine;

public class PizzaMother : MonoBehaviour
{
    public float rotationSpeed = 30f; // ����t��
    public float impactForce = 5f;    // ���������첾�O
    private Rigidbody rb;
    public Transform detectionCube;  // �����d�� Cube�]�������l����^

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // �������������z�v�T�A�u����
    }

    void Update()
    {
        // �������]�L�׬O�_�Q���^
        transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);

        // �O�� Cube ���۹��m�A�������Y��v�T
        detectionCube.position = transform.position; // �O���۹��m
        detectionCube.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // �u����Ӥ����Y��v�T
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pizza"))
        {
            Vector3 forceDirection = collision.contacts[0].point - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rb.isKinematic = false; // �}�l�����z�v�T
            rb.AddForce(forceDirection * impactForce, ForceMode.Impulse);
            Debug.Log("�j���ĳQ����F�A�}�l�첾�I");
        }
    }
}
