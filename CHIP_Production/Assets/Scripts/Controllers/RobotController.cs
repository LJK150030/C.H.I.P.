using System.Collections;
using Assets.Project_Assets_Folder.Scripts;
using Assets.Scripts.Components;
using Assets.Scripts.UI;
using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class RobotController : MonoBehaviour
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
        public bool justTurned;
        public bool directionLastFrame;
        public AudioClip SFX_turnaround;

        [Header("Jump")]
        public float HeightOfJump = 1.0f;
        private CapsuleCollider2D bodyCollider;
        private GroundDetector groundDetector;
        public AudioClip SFX_Jumping;
        public AudioClip[] SFX_Landing;
        private bool _isJumping = false;


        [Header("Duck")]
        public float DuckXAcceleration = 1.0f;
        public float DuckXDeacceleration = 2.0f;
        public float MaxXSpeed = 5.0f;
        private CeilingDetector ceilingDetector;
        public AudioClip SFX_Ducking;

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

        [Header("Dead")]
        public bool _isDead = false;
        public AudioClip[] SFX_Dying;


        private Button[] _abilitiesButtons;
        private float _currentTime = 0.0f;
        private int _numAbilityChildCount = 0;
        private bool _startAbilities = false;
        private bool _isDucking = false;
        private int _slottingMachineSize;
        private int _onSlotID = -1;
        private Rigidbody2D _chipsRigidbody2D;
        private Animator _chipsAnimator;
        private CapsuleCollider2D _capsuleCollider2D;
        private bool startKillAnimation = false;
        private SlottingMachine _slottingMachine;
        private PacketReader _packetReader;
        private Vector2 _duckingLastFrame = Vector2.zero;
        private Vector2 _duckingCurrentFrame = Vector2.zero;



        private void Awake()
        {
            CameraFollow.SetTarget(GameObject.FindGameObjectWithTag("TargetPlayer"));
            //Physics2D.gravity = new Vector2(0, -2 * HeightOfJump / (TimeToCompleteAbility * TimeToCompleteAbility));
            _slottingMachine = GameObject.FindGameObjectWithTag("SlottingMachine").GetComponent<SlottingMachine>();
            _packetReader = GameObject.FindGameObjectWithTag("PacketReader").GetComponent<PacketReader>();
            _chipsAnimator = GetComponent<Animator>();
            Abilities = GameObject.FindGameObjectWithTag("AbilityLibrary").GetComponent<AbilityLibrary>();
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

            directionLastFrame = right;
        }

        void Start()
        {
            CheckPoint.InitialPlayerPosition();
            groundDetector = GetComponentInChildren<GroundDetector>();
            ceilingDetector = GetComponentInChildren<CeilingDetector>();
            bodyCollider = GetComponentInChildren<CapsuleCollider2D>();
            _chipsRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _capsuleCollider2D = GetComponentInChildren<CapsuleCollider2D>();
            initialScale = transform.localScale.x;
        }

        void FixedUpdate()
        {
            if (_isDead)
            {
                _chipsRigidbody2D.velocity = Vector2.zero;
                return;
            }

            justTurned = false;

            CameraFollow.SwitchCorner(!right);

            if (directionLastFrame != right)
            {
                directionLastFrame = right;
                justTurned = true;
                //RegionLockedCamera.SwitchCorner(right);
                /*CameraFollow.SwitchCorner(!right);*/
            }

            //if I am moving right
            Vector2 velocity = _chipsRigidbody2D.velocity;
            if (right)
            {
                transform.localScale = new Vector3(initialScale, transform.localScale.y, transform.localScale.z);

                if (_isDucking)
                {
                    velocity.x += DuckXAcceleration * Time.fixedDeltaTime;
                    if (velocity.x >= MaxXSpeed)
                        velocity.x = MaxXSpeed;
                    _duckingCurrentFrame = transform.position;
                }
                else if (groundDetector.isGrounded)
                {
                    velocity.x -= DuckXDeacceleration * Time.fixedDeltaTime;
                    if (velocity.x <= xvelocity)
                        velocity.x = xvelocity;
                }

                if(ceilingDetector.isHittingCeiling)
                    velocity.x = xvelocity;

                if (justTurned)
                {
                    velocity.x = xvelocity;
                }

            }

            //if I am moving left
            if (!right)
            {
                transform.localScale = new Vector3(-initialScale, transform.localScale.y, transform.localScale.z);

                if (_isDucking)
                {
                    velocity.x -= DuckXAcceleration * Time.fixedDeltaTime;
                    if (velocity.x <= -MaxXSpeed)
                        velocity.x = -MaxXSpeed;
                    _duckingCurrentFrame = transform.position;
                }
                else if (groundDetector.isGrounded)
                {
                    velocity.x += DuckXDeacceleration * Time.fixedDeltaTime;
                    if (velocity.x >= -xvelocity)
                        velocity.x = -xvelocity;
                }

                if (ceilingDetector.isHittingCeiling)
                    velocity.x = -xvelocity;

                if (justTurned)
                {
                    velocity.x = -xvelocity;
                }
            }

            if (_isDucking && _duckingCurrentFrame == _duckingLastFrame)
            {
                right = !right;
                velocity = -velocity;
            }

            _duckingLastFrame = _duckingCurrentFrame;
                
            //normalize ability time
            _chipsRigidbody2D.velocity = velocity;

            _currentTime += Time.fixedDeltaTime;
            FractionTillComplete = _currentTime / (_slottingMachineSize * TimeToCompleteAbility);
            if (_onSlotID >= 0)
            {
                float offsetCurrentTIme = _currentTime - (_onSlotID * TimeToCompleteAbility);
                FractionTillAbilityComplete[_onSlotID] = offsetCurrentTIme / TimeToCompleteAbility;
            }

            bool enableResetButton = _slottingMachine.EnableOrDisablePlayButton();
            bool enablePlayButton = _slottingMachine.EnableOrDisablePlayButton();

            //Check if in the air
            _chipsAnimator.SetBool("IsAirborne", !groundDetector.isGrounded);

            if (!groundDetector.isGrounded)
                enablePlayButton = false;

            PlayButton.interactable = enablePlayButton;
            ResetButton.interactable = enableResetButton;

            Abilities.EnableOrDisableButton();

            //check if I'm on the ground and ducking
            if (groundDetector.enterGrounded && _isDucking)
            {
                _isDucking = false;
                _chipsAnimator.SetBool("IsDucking", _isDucking);
            }

            if (groundDetector.enterGrounded)
            {
                int randomNumber = Random.Range(0, SFX_Landing.Length);
                AudioUtil.PlayOneOff(SFX_Landing[randomNumber]);
            }
        }

        private void OnCollisionStay2D(Collision2D col)
        {
            if (col.gameObject.tag == "Ground")
            {
                ContactPoint2D[] allPoints = col.contacts;
                Vector2 playerPos = new Vector2(_capsuleCollider2D.transform.position.x + _capsuleCollider2D.offset.x, _capsuleCollider2D.transform.position.y + _capsuleCollider2D.offset.y);
                for(int i = 0; i < allPoints.Length; i++)
                {
                    Vector2 point = allPoints[i].point;
                    Vector2 dir = point - playerPos;
                    // check contact point direction
                    if (dir.x * (right ? 1 : -1) > 0)
                    {
                        if (!_isDucking)
                        {
                            if (Mathf.Abs(dir.y) < (bodyCollider.size.y / 2.0f - 0.1f))
                            {
                                // check contact point distance
                                if (Mathf.Abs(dir.x) > 0.1f)
                                {
                                    //Move
                                    right = !right;
                                    AudioUtil.PlayOneOff(SFX_turnaround);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //Debug.Log("Mathf.Abs(dir.y) < (bodyCollider.size.y - 0.1f) : " + dir.y + " < " + (bodyCollider.size.y - 0.1f));

                            if (dir.y < (bodyCollider.size.y - 0.1f) && dir.y > 0.0f)
                            {
                                // check contact point distance
                                if (Mathf.Abs(dir.x) > 0.1f)
                                {
                                    //Move
                                    right = !right;
                                    AudioUtil.PlayOneOff(SFX_turnaround);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (col.gameObject.tag == "Die")
            {
                StartCoroutine(Kill());
            }
        }
        
        public void PressedPlay()
        {
            if (_isDead) return;
            _packetReader.ReadPacket();
            _slottingMachine.ResetSlots();
            StartCoroutine(PerformAbilities(_packetReader));
        }

        private IEnumerator PerformAbilities(PacketReader packetReader)
        {

            // updating the time of completing from 0 to 1
            _startAbilities = true;
            //timeline.StartTimeline(TimeToCompleteAbility);
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
                var abilityID = packetReader.GetPackAbilityIDFromIndex(slotID);

                switch (abilityID)
                {
                    case (int)ability.delay:
                        {
                            _isDucking = false;
/*                            _chipsAnimator.SetBool("IsDucking", _isDucking);*/
                            continue;
                            //break;
                        }

                    case (int)ability.jump:
                        {
                            // Debug.Log("Jump Action");
                            //_chipsAnimator.speed /= TimeToCompleteAbility;
                            //_chipsRigidbody2D.AddForce(new Vector2(0, _forceAmountWithDefaultValues), ForceMode2D.Impulse);
                            //_chipsRigidbody2D.velocity += (Vector2.up * -Physics2D.gravity.y * TimeToCompleteAbility);
                            _isDucking = false;
                            _isJumping = true;
                            _chipsAnimator.SetTrigger("IsJumping");
/*                            _chipsAnimator.SetBool("IsDucking", _isDucking);*/
                            ApplyJumpForce();
                            AudioUtil.PlayOneOff(SFX_Jumping);
                            break;
                        }

                    case (int)ability.duck:
                        {
                            _isDucking = true;
                            AudioUtil.PlayOneOff(SFX_Ducking);
                            /*                            _chipsAnimator.SetBool("IsDucking", _isDucking);*/
                            break;
                        }

                    default:
                        break;
                }
                _chipsAnimator.SetBool("IsDucking", _isDucking);
                yield return new WaitForSeconds(TimeToCompleteAbility);
                _isJumping = false;
                _onSlotID++;
            }

            _startAbilities = false;

            if (groundDetector.isGrounded && _isDucking)
                _isDucking = false;

            _chipsAnimator.SetBool("IsDucking", _isDucking);
            _onSlotID = -1;
            FractionTillComplete = 0.0f;
            for (int fractionID = 0; fractionID < NumOfSlots; fractionID++)
            {
                FractionTillAbilityComplete[fractionID] = 0.0f;
            }

            _packetReader.ClearSlots();
        }

        private void IfHitOurHeadThenDie()
        {
            if (ceilingDetector.isHittingCeiling)
            {
                StartCoroutine(Kill());
                _isDead = true;
            }
        }

        private void ApplyJumpForce()
        {
            //wanting to use a known height to reach within a certain amount of time
            float originalGravity = Physics2D.gravity.y;
            /*Debug.Log("originalGravity " + originalGravity);*/

            float desiredGravity = (2.0f * HeightOfJump) / (TimeToCompleteAbility * TimeToCompleteAbility);
            /*Debug.Log("desiredGravity " + desiredGravity);*/

            float desiredSpeed = Mathf.Sqrt(2.0f * HeightOfJump * desiredGravity);
            /*Debug.Log("desiredSpeed " + desiredSpeed);*/
            
            Vector2 finalVelocity = new Vector2(0.0f, desiredSpeed);

            _chipsRigidbody2D.gravityScale = desiredGravity / -originalGravity;
            _chipsRigidbody2D.velocity += finalVelocity;
        }

        private IEnumerator Kill()
        {
            if (startKillAnimation) yield break;

            startKillAnimation = true;
            _chipsAnimator.SetTrigger("Die");

            int randomNumber = Random.Range(0, SFX_Dying.Length);
            AudioUtil.PlayOneOff(SFX_Dying[randomNumber]);
            
            yield return new WaitForSeconds(1.0f);

            LevelManager.Instance.ReloadLevel();
        }

    }
}
