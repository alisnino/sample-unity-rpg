using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject rippleEffectPrefab;
    private InputSystem_Actions inputActions;
    private NavMeshAgent agent;
    private float lastClickTime = 0f;
    private const float CLICK_DEBOUNCE = 0.5f; // 100ms debounce
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void Start()
    {
        //Add listener to the click action
        agent = GetComponent<NavMeshAgent>();
        inputActions.UI.Enable();
        inputActions.UI.RightClick.performed += OnRightClick;
    }


    private void OnRightClick(InputAction.CallbackContext context)
    {   

        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hitInfo)) {
            if (Time.time - lastClickTime > CLICK_DEBOUNCE) {
                Vector3 ripplePosition = hitInfo.point;
                ripplePosition.y += 0.3f;
                GameObject rippleEffect = Instantiate(rippleEffectPrefab, ripplePosition, Quaternion.Euler(90, 0, 0));
                ParticleSystem ps = rippleEffect.GetComponent<ParticleSystem>();
                Destroy(rippleEffect, ps.main.duration);
                lastClickTime = Time.time;
            }
            agent.SetDestination(hitInfo.point);
        }
    }
    // Update is called once per frame
    void Update()
    {

        
    }

    void OnDestroy() {
        inputActions.UI.RightClick.performed -= OnRightClick;
        inputActions.UI.Disable();
    }
}
