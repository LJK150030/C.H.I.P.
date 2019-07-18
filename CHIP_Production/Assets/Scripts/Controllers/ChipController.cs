using System.Collections;
using Assets.Project_Assets_Folder.Scripts;
using Assets.Scripts.UI;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class ChipController : MonoBehaviour
    {
        enum ability
        {
            delay = -1,
            jump,
            duck,
            NumAbilities
        }

        [Header("Move")]
        public float xvelocity;
        public float initialScale;
        public bool right;

        [Header("Jump")]
        public float AdditionalXJump;
        public float HeightOfJump = 1.0f;
        private CapsuleCollider2D bodyCollider;
        private GroundDetector groundDetector;

        [Header("Duck")]
        public float DuckXSpeedScale = 1.0f;
        private CeilingDetector ceilingDetector;

        [Header("Ability")]
        public int NumOfSlots = 2;
        public float TimeToCompleteAbility;
        public AbilityLibrary Abilities;
        public float FractionTillComplete = 0.0f;
        public float[] FractionTillAbilityComplete;
        

        [Header("Chips Buttons")]
        public Button PlayButton;
        public Button ResetButton;
        public Timeline timeline;

        private Button[] _abilitiesButtons;
        private float _currentTime = 0.0f;
        private int _numAbilityChildCount = 0;
        private bool _startAbilities = false;
        private bool _isDucking = false;
        private int _slottingMachineSize;
        private int _onSlotID = -1;
        private Rigidbody2D _chipsRigidbody2D;
        private Animator _chipsAnimator;

        private void Awake()
        {
            _chipsAnimator = GetComponent<Animator>();
            //Physics2D.gravity = new Vector2(0, -2 * HeightOfJump / (TimeToCompleteAbility * TimeToCompleteAbility));
            _numAbilityChildCount = Abilities.transform.childCount;
            _chipsRigidbody2D = GetComponent<Rigidbody2D>();
            _abilitiesButtons = new Button[_numAbilityChildCount];

            for (int childID = 0; childID < _numAbilityChildCount; childID++)
            {
                _abilitiesButtons[childID] = Abilities.transform.GetChild(childID).GetComponent<Button>();
            }

            FractionTillAbilityComplete = new float[NumOfSlots];
            for (int fractionID = 0; fractionID < NumOfSlots; fractionID++)
            {
                FractionTillAbilityComplete[fractionID] = 0.0f;
            }

            if (PlayButton == null)
                PlayButton = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();

            if (ResetButton == null)
                ResetButton = GameObject.FindGameObjectWithTag("EjectButton").GetComponent<Button>();
        }

        void Start()
        {
            CheckPoint.InitialPlayerPosition();
            groundDetector = GetComponentInChildren<GroundDetector>();
            ceilingDetector = GetComponentInChildren<CeilingDetector>();
            bodyCollider = GetComponentInChildren<CapsuleCollider2D>();
            _chipsRigidbody2D = GetComponent<Rigidbody2D>();
            initialScale = transform.localScale.x;
        }

        void FixedUpdate()
        {
            //Move
            Vector2 velocity = _chipsRigidbody2D.velocity;
            if (right)
            {
                transform.localScale = new Vector3(initialScale, transform.localScale.y, transform.localScale.z);

                if (_isDucking)
                    velocity.x = xvelocity * DuckXSpeedScale;
                else
                    velocity.x = xvelocity;
            }

            if (!right)
            {
                transform.localScale = new Vector3(-initialScale, transform.localScale.y, transform.localScale.z);

                if (_isDucking)
                    velocity.x = -xvelocity * DuckXSpeedScale;
                else
                    velocity.x = -xvelocity;
            }

            //normalize ability time
            _chipsRigidbody2D.velocity = velocity;
            _currentTime += Time.fixedDeltaTime;
            FractionTillComplete = _currentTime / (_slottingMachineSize * TimeToCompleteAbility);

            if (_onSlotID >= 0)
            {
                float offsetCurrentTIme = _currentTime - (_onSlotID * TimeToCompleteAbility);
                FractionTillAbilityComplete[_onSlotID] = offsetCurrentTIme / TimeToCompleteAbility;
            }

            _chipsAnimator.SetBool("IsAirborne", !groundDetector.isGrounded);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.tag == "Ground")
            {
                ContactPoint2D[] allPoints = col.contacts;
                Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
                for (int i = 0; i < allPoints.Length; i++)
                {
                    Vector2 point = allPoints[i].point;
                    Vector2 dir = point - playerPos;
                    // check contact point direction
                    if (dir.x * (right ? 1 : -1) > 0)
                    {
                        // check contact point height
                        if (Mathf.Abs(dir.y) < bodyCollider.size.y / 2 - 0.1f)
                        {
                            // check contact point distance
                            if (Mathf.Abs(dir.x) > 0.1f)
                            {
                                //Move
                                right = !right;
                                break;
                            }
                        }
                    }
                }
            }

            if (col.gameObject.tag == "Die")
            {
                LevelManager.Instance.ReloadLevel();
            }
        }

        public void PressedPlay(PacketReader packetReader)
        {
            StartCoroutine(PerformAbilities(packetReader));
        }

        private IEnumerator PerformAbilities(PacketReader packetReader)
        {
            _startAbilities = true;

            timeline.StartTimeline(TimeToCompleteAbility);
            FractionTillComplete = 0.0f;
            for (int fractionID = 0; fractionID < NumOfSlots; fractionID++)
            {
                FractionTillAbilityComplete[fractionID] = 0.0f;
            }

            _currentTime = 0.0f;
            _onSlotID = 0;
            _slottingMachineSize = packetReader.GetPacketSize();


            for (int slotID = 0; slotID < _slottingMachineSize; slotID++)
            {
                _isDucking = false;
                _chipsAnimator.SetBool("IsDucking", _isDucking);

                var abilityID = packetReader.GetPackAbilityIDFromIndex(slotID);

                switch (abilityID)
                {
                    case (int) ability.delay:
                    {
                        break;
                    }

                    case (int) ability.jump:
                    {
                        _chipsAnimator.SetTrigger("IsJumping");
                        //_chipsRigidbody2D.velocity += (Vector2.up * -Physics2D.gravity.y * TimeToCompleteAbility);
                        ApplyJumpForce();
                        break;
                    }

                    case (int) ability.duck:
                    {
                        _isDucking = true;
                        _chipsAnimator.SetBool("IsDucking", _isDucking);
                        break;
                    }

                    default:
                        break;
                }

                yield return new WaitForSeconds(TimeToCompleteAbility);
                _onSlotID++;
            }

            _startAbilities = false;
            _isDucking = false;
            _chipsAnimator.SetBool("IsDucking", _isDucking);
            _onSlotID = -1;
            FractionTillComplete = 0.0f;
            for (int fractionID = 0; fractionID < NumOfSlots; fractionID++)
            {
                FractionTillAbilityComplete[fractionID] = 0.0f;
            }

        }

        private void DisableButtons()
        {
            PlayButton.interactable = false;
            ResetButton.interactable = false;

            for (int childID = 0; childID < _numAbilityChildCount; childID++)
            {
                _abilitiesButtons[childID].interactable = false;
            }
        }

        private void EnableButtons()
        {
            PlayButton.interactable = true;
            ResetButton.interactable = true;

            for (int childID = 0; childID < _numAbilityChildCount; childID++)
            {
                _abilitiesButtons[childID].interactable = true;
            }
        }

        private void IfHitOurHeadThenDie()
        {
            if(ceilingDetector.isHittingCeiling)
                Destroy(this.gameObject);
        }

        private void ApplyJumpForce()
        {
            //wanting to use a known height to reach within a certain amount of time
            float originalGravity = Physics2D.gravity.y;
            float desiredGravity = (2.0f * HeightOfJump) / (TimeToCompleteAbility * TimeToCompleteAbility);
            float desiredSpeed = Mathf.Sqrt(-2.0f * HeightOfJump * desiredGravity);

            Vector2 finalVelocity = new Vector2(0.0f, desiredSpeed);
            _chipsRigidbody2D.velocity += finalVelocity;
            _chipsRigidbody2D.gravityScale = desiredGravity / originalGravity;
        }
    }
}
