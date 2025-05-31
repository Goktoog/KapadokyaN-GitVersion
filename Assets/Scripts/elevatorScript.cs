using UnityEngine;

public class elevatorScript : MonoBehaviour
{
    [Header("elevator parts")]
    [SerializeField] private Transform elevator;
    [SerializeField] private Transform pressurePlate;
    [SerializeField] private Transform playerCheck;

    [Header("Elevator settings")]
    [SerializeField] private float plateSinkSpeed = 0.1f;
    [SerializeField] private float plateSinkAmount = 0.1f;
    [SerializeField] private float elevatorMoveAmount = 3f;
    [SerializeField] private float elevatorMoveSpeed = 1.5f;

    private bool isPlayerOnPlate = false;
    private bool isElevatorDown = false;

    private Vector3 elevatorStartPos;
    private Vector3 elevatorTargetPos;

    private Vector3 plateStartPos;
    private Vector3 plateTargetPos;

    void Start()
    {
        plateStartPos = pressurePlate.localPosition;
        plateTargetPos = plateStartPos + Vector3.down * plateSinkAmount;

        elevatorStartPos = elevator.localPosition;
        elevatorTargetPos = elevatorStartPos + Vector3.down * elevatorMoveAmount;
    }

    private void Update()
    {
        Vector3 targetPlatePos = isPlayerOnPlate ? plateTargetPos : plateStartPos;
        pressurePlate.localPosition = Vector3.MoveTowards(pressurePlate.localPosition, targetPlatePos, Time.deltaTime * plateSinkSpeed);

        Vector3 targetElevatorPos = isElevatorDown ? elevatorTargetPos : elevatorStartPos;
        elevator.localPosition = Vector3.MoveTowards(elevator.localPosition, targetElevatorPos, Time.deltaTime * elevatorMoveSpeed);
    }

    private void ToggleElevator()
    {
        isElevatorDown = !isElevatorDown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlate = true;
            ToggleElevator();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlate = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pressurePlate.position, pressurePlate.position + Vector3.down * plateSinkAmount);
        Gizmos.DrawLine(elevator.position, elevator.position + Vector3.down * elevatorMoveAmount);
    }
}