using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deplacement : MonoBehaviour
{
    PlayerInventory playerInv;
    public GameObject inventory;
    public GameObject storage;
    public GameObject equipmentSystem;
    public GameObject shop;
    public GameObject skills;
    public GameObject quest;

    public float rotateSpeed;
    public AudioSource shotSound;

    public float attackCooldown;
    private bool isAttacking = false;
    private float currentCooldown = 1.2f;
    public float attackRange;
    private GameObject rayHit;

    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;
    public string inputSpell;

    Animation animations;
    CapsuleCollider PlayerCollider;

    [HideInInspector]
    public bool isInShop = false;

    [HideInInspector]
    public bool isDead = false;
    public float jumpSpeed;

    [Header("Paramètres des sorts")]
    public int totalSpell;
    private GameObject raySpell;
    private GameObject spellHolderImg;
    private int currentSpell = 1;

    //Lightning Spell
    [Header("Paramètres du sort de foudre")]
    public float lightningSpellCost;
    public GameObject lightningSpellGO;
    public float lightningSpellSpeed;
    public float lightningSpellID;
    public Sprite lightningSpellImage;

    //Heal Spell
    [Header("Paramètres du sort de soin")]
    public float healSpellCost;
    public GameObject healSpellGO;
    public float healSpellAmount;
    public float healSpellID;
    public Sprite healSpellImage;

    //bool isGrounded;

    public Vector3 velocity { get; private set; }
    public bool isJumping { get; private set; }
    public bool isGrounded { get; private set; }
    public bool previouslyGrounded { get; private set; }

    [Header("Movement Settings")]
    [SerializeField] float forwardSpeed = 5f;
    [SerializeField] float backwardSpeed = 4f;
    [SerializeField] float strafeSpeed = 5f;
    [SerializeField] float runMultiplier = 1.8f;
    [SerializeField] float acceleration = 18f;
    [SerializeField] float deceleration = 12f;
    [SerializeField] public float movementEnergy = 6f;

    [Header("Jump Settings")]
    [SerializeField] float jumpBaseSpeed = 5f;
    [SerializeField] float jumpExtraSpeed = 1f;
    [SerializeField] float gravity = -20f;
    [SerializeField] [Range(0f, 1f)] float airControl = 0.2f;

    // References
    Transform thisTransform;
    CharacterController thisCharacterController;

    // System
    Vector3 targetDirection;
    Vector2 movementInput;
    float targetSpeed;
    float currentSpeed;
    float remainedExtraJumpSpeed;

    // States
    bool jump = true;

    // Start is called before the first frame update
    void Start()
    {
        animations = GetComponent<Animation>();
        PlayerCollider = GetComponent<CapsuleCollider>();
        playerInv = GetComponent<PlayerInventory>();
        rayHit = GameObject.Find("RayHit");
        raySpell = GameObject.Find("RaySpell");
        spellHolderImg = GameObject.Find("SpellHolderImg");
        thisTransform = transform;
        thisCharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            HandleUserInput();
        }

        if (!inventory.active || !storage.active || !equipmentSystem.active || !shop.active || !skills.active || !quest.active)
            LockCursor();

        if (inventory.active || storage.active || equipmentSystem.active || shop.active || skills.active || quest.active)
            UnlockCursor();

        if (isAttacking)
        {
            currentCooldown -= Time.deltaTime;
        }

        if (currentCooldown <= 0.4f)
        {
            if (!shotSound.isPlaying)
                shotSound.Play();
        }

        if (currentCooldown <= 0)
        {
            currentCooldown = attackCooldown;
            isAttacking = false;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentSpell <= totalSpell && currentSpell != 1)
            {
                currentSpell -= 1;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentSpell >= 0 && currentSpell != totalSpell)
            {
                currentSpell += 1;
            }
        }

        //Changer de Spell

        if (currentSpell == lightningSpellID)
        {
            spellHolderImg.GetComponent<Image>().sprite = lightningSpellImage;
        }

        if (currentSpell == healSpellID)
        {
            spellHolderImg.GetComponent<Image>().sprite = healSpellImage;
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (!thisCharacterController.enabled)
                GetComponent<CharacterController>().enabled = true;
            previouslyGrounded = isGrounded;
            isGrounded = thisCharacterController.isGrounded;
            velocity = thisCharacterController.velocity;

            float accelRate = movementInput.sqrMagnitude > 0f ? acceleration : deceleration;
            float controlModifier = (isGrounded ? 1f : airControl);

            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (Time.fixedDeltaTime * accelRate * controlModifier));
            Vector3 targetVelocity = targetDirection.normalized * currentSpeed;
            targetVelocity.y = thisCharacterController.velocity.y + gravity * Time.fixedDeltaTime;

            if (jump && isGrounded)
            {
                // Jumping
                targetVelocity = new Vector3(targetVelocity.x, jumpBaseSpeed, targetVelocity.z);
                isJumping = true;
            }
            else if (isGrounded && !previouslyGrounded && IsGrounded())
            {
                if (isJumping) isJumping = false;

                remainedExtraJumpSpeed = jumpExtraSpeed;
            }

            if (jump && thisCharacterController.velocity.y > 0f)
            {
                float jumpSpeedIncrement = remainedExtraJumpSpeed * Time.fixedDeltaTime;
                remainedExtraJumpSpeed -= jumpSpeedIncrement;

                if (jumpSpeedIncrement > 0f)
                {
                    targetVelocity.y += jumpSpeedIncrement;
                }
            }

            Vector3 vel = Vector3.MoveTowards(thisCharacterController.velocity, targetVelocity, Time.fixedDeltaTime * movementEnergy);
            vel.y = targetVelocity.y;
            thisCharacterController.Move(vel * Time.fixedDeltaTime);

            jump = false;
        }
    }

    bool IsGrounded()
    {
        return Physics.CheckCapsule(PlayerCollider.bounds.center, new Vector3(PlayerCollider.bounds.center.x, PlayerCollider.bounds.min.y - 0.1f, PlayerCollider.bounds.center.z), 0.1f);
    }


    public void Attack()
    {
        if (!isAttacking)
        {
            animations.Play("attack");

            RaycastHit hit;

            if (Physics.Raycast(rayHit.transform.position, transform.TransformDirection(Vector3.forward), out hit, attackRange))
            {
                Debug.DrawLine(rayHit.transform.position, hit.point, Color.red);

                if (hit.transform.tag == "Enemy" | hit.transform.tag == "EnemyQuest")
                {
                    hit.transform.GetComponent<EnemyAI>().ApplyDamage(playerInv.currentDamage);
                }

                if (hit.transform.tag == "Terrain")
                {
                    if (hit.transform.GetComponent<DestroyObject>())
                    {
                        hit.transform.GetComponent<DestroyObject>().ApplyDamage(playerInv.currentDamage);
                    }
                }
            }
            isAttacking = true;
        }
    }

    public void AttackSpell()
    {
        if (currentSpell == lightningSpellID && !isAttacking && playerInv.currentMana >= lightningSpellCost)
        {
            animations.Play("attack");
            GameObject theSpell = Instantiate(lightningSpellGO, raySpell.transform.position, transform.rotation);
            theSpell.GetComponent<Rigidbody>().AddForce(transform.forward * lightningSpellSpeed);
            playerInv.currentMana -= lightningSpellCost;
            isAttacking = true;
        }

        if (currentSpell == healSpellID && !isAttacking && playerInv.currentMana >= healSpellCost && playerInv.currentHealth < playerInv.maxHealth)
        {
            Instantiate(healSpellGO, raySpell.transform.position, transform.rotation);
            playerInv.currentMana -= healSpellCost;
            playerInv.currentHealth += healSpellAmount;
        }
    }

    void HandleUserInput()
    {
        if (Input.GetKey(inputFront) && !Input.GetKey(KeyCode.LeftShift))
        {
            targetSpeed = forwardSpeed;
            movementInput = new Vector2(0, 1);

            DirectionCamera();

            if (!isAttacking)
            {
                animations.Play("walk");
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }

            if (Input.GetKeyDown(inputSpell))
            {
                AttackSpell();
            }
        }

        if (Input.GetKey(inputFront) && Input.GetKey(KeyCode.LeftShift))
        {
            targetSpeed = runMultiplier;
            movementInput = new Vector2(0, 1);

            DirectionCamera();

            animations.Play("run");
        }

        if (Input.GetKey(inputBack))
        {
            targetSpeed = backwardSpeed;
            movementInput = new Vector2(0, -1);

            DirectionCamera();

            if (!isAttacking)
            {
                animations.Play("walk");
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }

            if (Input.GetKeyDown(inputSpell))
            {
                AttackSpell();
            }
        }

        if (Input.GetKey(inputLeft))
        {
            thisTransform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
        }

        if (Input.GetKey(inputRight))
        {
            thisTransform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }

        if (!Input.GetKey(inputFront) && !Input.GetKey(inputBack))
        {
            targetSpeed = strafeSpeed;
            movementInput = new Vector2(0, 0);

            if (!isAttacking)
            {
                animations.Play("idle");
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }

            if (Input.GetKeyDown(inputSpell))
            {
                AttackSpell();
            }
        }
        jump = Input.GetButton("Jump");
        targetDirection = thisTransform.forward * movementInput.y;
    }


    void DirectionCamera()
    {
        Transform m_Cam = Camera.main.transform;

        Vector3 m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = movementInput.y * m_CamForward + movementInput.x * m_Cam.right;

        if (m_Move.magnitude > 1f) m_Move.Normalize();
        m_Move = transform.InverseTransformDirection(m_Move);
        Vector3 m_GroundNormal = Vector3.zero;
        m_Move = Vector3.ProjectOnPlane(m_Move, m_GroundNormal);
        float m_TurnAmount = Mathf.Atan2(m_Move.x, m_Move.z);

        float turnSpeed = Mathf.Lerp(360, 180, forwardSpeed);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    public void UnlockCursor()
    {
        if (Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void LockCursor()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}