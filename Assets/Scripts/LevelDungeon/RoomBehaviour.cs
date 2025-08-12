using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private GameObject stairs;
    [SerializeField] private GameObject endChest;
    [SerializeField] private GameObject normalChest;
    
    private bool isNormalRoom;

    private void Awake()
    {
        isNormalRoom = false;
    }

    public void UpdateRoom(bool[] status, bool isStartRoom, bool isEndRoom)
    {
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
            if (isStartRoom)
            {
                stairs.SetActive(true);
            }
            else if (isEndRoom)
            {
                endChest.SetActive(true);
                RotateEndChestToTheDoor();
            }
            else
            {
                ActivateRandomObstacle();
                isNormalRoom = true;
            }

        }
    }

    private void RotateEndChestToTheDoor()
    {
        GameObject target = null;

        foreach (GameObject door in doors)
        {
            if (door.activeSelf)
            {
                target = door;
                break;
            }
        }
        if(target == null) return;

        endChest.transform.LookAt(target.transform.position);
    }

    private void ActivateRandomObstacle()
    {
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.SetActive(false);
        }
        int randomObstaclesAmount = Random.Range(0, obstacles.Length);

        if(randomObstaclesAmount >= 3)
        {
            foreach (GameObject obstacle in obstacles)
            {
                obstacle.SetActive(true);
            }
        }else if (randomObstaclesAmount == 0)
        {
            int randomIndex = Random.Range(0, obstacles.Length);
            obstacles[randomIndex].SetActive(true);
        }
        else
        {
            int actives = 0;
            while (actives < randomObstaclesAmount)
            {
                int randomIndex = Random.Range(0, obstacles.Length);
                if (!obstacles[randomIndex].activeSelf)
                {
                    obstacles[randomIndex].SetActive(true);
                    actives++;
                }
            }
        }

    }

    public void PlaceChest()
    {
        normalChest.SetActive(true);
    }

    public bool IsNormalChestActivated()
    {
        return normalChest.activeSelf;
    }

    public bool IsNormalRoom()
    {
        return isNormalRoom;
    }
}
