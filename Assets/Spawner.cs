using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject bulletPrefab;

    public GameObject gun;

    public int SpawnTime = 15;

    public Transform bulletSpawn;

    private int counter = 0;

    private int bulletSpeed = 30;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (counter > SpawnTime)
        {
            float xPosition = Random.Range(-8, 8);

            GameObject bullet = Instantiate(bulletPrefab);

            bullet.transform.position = new Vector3(xPosition, bulletSpawn.position.y, bulletSpawn.position.z);

            gun.transform.position = new Vector3(xPosition, gun.transform.position.y, gun.transform.position.z);

            Vector3 rotation = bullet.transform.eulerAngles;

            bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

            bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward * bulletSpeed, ForceMode.Impulse);

            Destroy(bullet, 4);
            counter = 0;
        }
        else
        {
            counter++;
        }

    }
}
