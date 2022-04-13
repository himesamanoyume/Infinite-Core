using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    private CharacterController controller;

    Animator animator;

    GameObject playerModel;

    public GameObject attackCubePrefab;

    new PhotonView photonView;

    [SerializeField]
    float m_moveSpeed;

    [SerializeField]
    float gravity;

    public Transform groundCheck;

    public LayerMask layerMask;

    float checkRadius = 0.2f;

    bool isGround;

    Vector3 velocity = Vector3.zero;

    ProEnum m_pro;

    float m_attack;

    float m_attackRange;

    float m_criticalHit;

    float m_criticalHitRate;

    float m_finalDamage;

    float receiveFinalAttack;

    float receiveFinalDamage;

    void Start()
    {

        controller = GetComponent<CharacterController>();
        photonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        playerModel = CharManager.Instance.FindChildObjWithTag("Player", this.gameObject);
        m_moveSpeed = 8f;
        gravity = -19.8f;
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_PRO, out object pro);
        m_pro = (ProEnum)pro;

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_ATTACK_RANGE, out object attackRange);
        m_attackRange = (float)attackRange;

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_CRITICALHIT_HIT, out object criticalHit);
        m_criticalHit = (float)criticalHit;

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_CRITICAL_HIT_RATE, out object criticalHitRate);
        m_criticalHitRate = (float)criticalHitRate;

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_ATTACK,out object attack);
        m_attack = (float)attack;

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(InfiniteCoreGame.PLAYER_FINAL_DAMAGE, out object finalDamage);
        m_finalDamage = (float)finalDamage;

        #region Subscribe Event

        GameEventManager.SubscribeEvent(EventEnum.SendPlayerMoveSpeed, SendPlayerMoveSpeed);

        GameEventManager.SubscribeEvent(EventEnum.AllowPlayerMove, PlayerMove);

        GameEventManager.SubscribeEvent(EventEnum.AllowPlayerAttack, PlayerAttack);

        GameEventManager.SubscribeEvent(EventEnum.AllowPlayerTowardChanged, PlayerTowardChanged);

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerAttackRangeChanged, OnPlayerAttackRangeChanged);

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerCriticalHitChanged, OnPlayerCriticalHitChanged);

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerCriticalHitRateChanged, OnPlayerCriticalHitRateChanged);

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerFinalDamageChanged, OnPlayerFinalDamageChanged);

        #endregion

        #region Register Event

        if (photonView.IsMine)
        {
            GameEventManager.RegisterEvent(EventEnum.OnPlayerDamaged, OnPlayerDamagedCheck);

            
        }

        #endregion
    }

    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;

        PlayerGravity();
    }
    #region Player Control

    void PlayerAnimationControl()
    {

    }

    /// <summary>
    /// 玩家移动
    /// </summary>
    /// <param name="args"></param>
    private void PlayerMove(object[] args)
    {
        if (photonView.IsMine)
        {
            var hor = Input.GetAxis("Horizontal");
            var ver = Input.GetAxis("Vertical");


            Vector3 direction = new Vector3(hor, 0, ver).normalized;

            Vector3 move = direction * m_moveSpeed * Time.deltaTime;
            controller.Move(move);
        }
    }
    
    /// <summary>
    /// 玩家转向
    /// </summary>
    /// <param name="args"></param>
    private void PlayerTowardChanged(object[] args)
    {
        if (photonView.IsMine)
        {

            var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
            var point = Input.mousePosition - playerScreenPoint;
            var angle = Mathf.Atan2(point.x, point.y) * Mathf.Rad2Deg;
            playerModel.transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
        }
        
    }

    /// <summary>
    /// 玩家重力
    /// </summary>
    private void PlayerGravity()
    {
        if (photonView.IsMine)
        {

            isGround = Physics.CheckSphere(groundCheck.position, checkRadius, layerMask);
            if (isGround && velocity.y < 0)
            {
                velocity.y = 0;
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

    }

    public void PlayerAttack(object[] args)
    {

        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetTrigger("isAttack");
                //animator.SetBool("isAttack", true);
                //isAttack = true;
            }
        }
    }

    float GetFinalAttack(float t_attack, float t_criticalHit, float t_criticalHitRate, float t_ratio)
    {
        if (t_criticalHitRate == 1) return t_attack * 0.5f * t_criticalHit * t_ratio;
        if (Random.Range(0,1f)<=t_criticalHitRate)
        {
            return t_attack * 0.5f * t_criticalHit * t_ratio;
        }
        else
        {
            return t_attack * 0.5f * t_ratio;
        }

    }

    public void AnimationEventOnPlayerAttackEnd()
    {

    }
    
    public void AnimationEventOnPlayerAttack()
    {
        if (photonView.IsMine)
        {
            if (this.photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                return;
            }

            Vector3 selfPos = playerModel.transform.position;
            Quaternion modelRotation = playerModel.transform.rotation;
            Vector3[] attackCubeData;
            float[] attackCubeData2;
            GameEventManager.EnableEvent(EventEnum.AllowPlayerMove, false);

            float finalAttack = GetFinalAttack(m_attack, m_criticalHit, m_criticalHitRate, 1);

            switch (m_pro)
            {
                //Vector3 : initOffset initScale finalOffset finalScale
                //float : initToFinalTime finalDamage activeTime destory finalAttack
                case ProEnum.Soilder:

                    attackCubeData = new Vector3[] 
                    { 
                        modelRotation * Vector3.forward,
                        new Vector3(1.5f,1.5f,3) * m_attackRange * 0.1f, 
                        modelRotation * Vector3.forward,
                        new Vector3(1.5f, 1.5f, 3.5f) * m_attackRange * 0.1f
                    };

                    attackCubeData2 = new float[] 
                    {   
                        0.5f,
                        m_finalDamage,
                        0,
                        0.3f,
                        finalAttack
                    };

                    photonView.RPC("SpawnAttackCube", RpcTarget.AllViaServer, selfPos, modelRotation, attackCubeData, attackCubeData2);
                    break;

                case ProEnum.Archer:

                    attackCubeData = new Vector3[] 
                    { 
                        modelRotation * Vector3.forward,
                        Vector3.one * m_attackRange * 0.1f,
                        modelRotation * Vector3.forward,
                        Vector3.one  * m_attackRange * 0.1f
                    };

                    attackCubeData2 = new float[] 
                    { 
                        0.5f,
                        m_finalDamage,
                        0,
                        0.5f ,
                        finalAttack
                    };

                    photonView.RPC("SpawnAttackCube", RpcTarget.AllViaServer, selfPos, modelRotation, attackCubeData, attackCubeData2);
                    break;

                case ProEnum.Doctor:

                    attackCubeData = new Vector3[] 
                    { 
                        modelRotation * Vector3.forward * 3,
                        new Vector3(3, 3, 3) * m_attackRange * 0.1f,
                        modelRotation * Vector3.forward * 3,
                        new Vector3(3, 3, 3)  * m_attackRange * 0.1f
                    };

                    attackCubeData2 = new float[] 
                    { 
                        0.5f,
                        m_finalDamage,
                        0.3f,
                        0.6f,
                        finalAttack
                    };

                    photonView.RPC("SpawnAttackCube", RpcTarget.AllViaServer, selfPos, modelRotation, attackCubeData, attackCubeData2);
                    break;

                case ProEnum.Tanker:
                    
                    attackCubeData = new Vector3[] 
                    { 
                        modelRotation * Vector3.forward, 
                        new Vector3(1, 1, 2.5f) * m_attackRange * 0.1f,
                        modelRotation * Vector3.forward,
                        new Vector3(1, 1, 2.5f) * m_attackRange * 0.1f
                    };

                    attackCubeData2 = new float[] 
                    { 
                        0.5f,
                        m_finalDamage,
                        0,
                        0.3f ,
                        finalAttack
                    };

                    photonView.RPC("SpawnAttackCube", RpcTarget.AllViaServer, selfPos, modelRotation, attackCubeData, attackCubeData2);
                    break;
            }
            
        }
    }



    public void PlayerDamaged(float finalAttack, float finalDamage)
    {
        receiveFinalAttack = finalAttack;
        receiveFinalDamage = finalDamage;
        GameEventManager.EnableEvent(EventEnum.OnPlayerDamaged, true);
    }

    [PunRPC]
    public IEnumerator SpawnAttackCube(Vector3 position, Quaternion rotation, Vector3[] attackCubeData, float[] attackCubeData2, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        GameObject attackCube = Instantiate(attackCubePrefab, position, Quaternion.identity);

        //传入的attackCubeData应该为碰撞体的信息
        attackCube.GetComponent<AttackCube>().InitAttackCube(photonView.Owner, rotation * Vector3.forward, Mathf.Abs(lag), attackCubeData, attackCubeData2);

        yield return new WaitForSeconds(0.5f);
        GameEventManager.EnableEvent(EventEnum.AllowPlayerMove, true);
    }

    #endregion

    #region Event Response

    public void OnPlayerFinalDamageChanged(object[] args)
    {
        int actorNumber;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            m_finalDamage = (float)args[1];

            GameEventManager.EnableEvent(EventEnum.OnPlayerFinalDamageChanged, false);
        }
    }

    public void OnPlayerCriticalHitRateChanged(object[] args)
    {
        int actorNumber;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            m_criticalHitRate = (float)args[1];

            GameEventManager.EnableEvent(EventEnum.OnPlayerCriticalHitRateChanged, false);
        }
    }

    public void OnPlayerCriticalHitChanged(object[] args)
    {
        int actorNumber;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            m_criticalHit = (float)args[1];
            
            GameEventManager.EnableEvent(EventEnum.OnPlayerCriticalHitChanged, false);
        }
        
    }

    public void OnPlayerAttackRangeChanged(object[] args)
    {
        int actorNumber;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            m_attackRange = (float)args[1];

            GameEventManager.EnableEvent(EventEnum.OnPlayerAttackRangeChanged, false);
        }
        
    }

    public void OnPlayerAttackChanged(object[] args)
    {
        int actorNumber;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            m_attack = (float)args[1];


        }
    }

    /// <summary>
    /// 接收记录器的移动速度
    /// </summary>
    /// <param name="args"></param>
    public void SendPlayerMoveSpeed(object[] args)
    {
        
        int actorNumber;
        float moveSpeed;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            moveSpeed = (float)args[1];
            m_moveSpeed = moveSpeed;

            GameEventManager.EnableEvent(EventEnum.SendPlayerMoveSpeed, false);
        }
    }

    #endregion

    #region Event Check

    bool OnPlayerDamagedeCheck(out object[] args)
    {
        args = new object[] { receiveFinalAttack, receiveFinalDamage };
        return true;
    }

    bool OnPlayerDamagedCheck(out object[] args)
    {
        args = null;
        return true;
    }

    #endregion

}
