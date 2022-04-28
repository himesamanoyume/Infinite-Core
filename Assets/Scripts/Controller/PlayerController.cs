using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    #region Various

    CharacterController controller;

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

    CharBase m_charBase;

    ProEnum m_pro;

    [SerializeField]
    float m_attack;

    float m_attackRange;

    float m_criticalHit;

    float m_criticalHitRate;

    float m_finalDamage;

    float receiveFinalAttack;

    float receiveFinalDamage;

    int enemyActorNumber;

    bool isAttack = false;

    Camera m_camera;

    #endregion

    #region Unity Functions

    void Start()
    {
        #region Init

        controller = GetComponent<CharacterController>();
        photonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        CharManager charManager = GameObject.Find("CharManager").GetComponent<CharManager>();
        m_camera = Camera.main;
        playerModel = charManager.FindChildObjWithTag("Player", this.gameObject);

        
        gravity = -19.8f;

        charManager.FindPlayerRecorder(PhotonNetwork.LocalPlayer.ActorNumber, out GameObject recorder, out CharBase charBase);
        m_charBase = charBase;

        if (m_charBase)
        {
            m_pro = m_charBase.Pro;
            m_attackRange = m_charBase.AttackRange;
            m_criticalHit = m_charBase.CriticalHit;
            m_criticalHitRate = m_charBase.CriticalHitRate;
            m_attack = m_charBase.Attack;
            m_finalDamage = m_charBase.FinalDamage;
            m_moveSpeed = m_charBase.MoveSpeed;
        }
        

        #endregion

        #region Subscribe Event

        GameEventManager.SubscribeEvent(EventEnum.SendPlayerMoveSpeed, SendPlayerMoveSpeed);

        GameEventManager.SubscribeEvent(EventEnum.AllowPlayerMove, PlayerMove);

        GameEventManager.SubscribeEvent(EventEnum.AllowPlayerAttack, PlayerAttack);

        GameEventManager.SubscribeEvent(EventEnum.AllowPlayerTowardChanged, PlayerTowardChanged);

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerAttackRangeChanged, OnPlayerAttackRangeChanged);

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerCriticalHitChanged, OnPlayerCriticalHitChanged);

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerCriticalHitRateChanged, OnPlayerCriticalHitRateChanged);

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerFinalDamageChanged, OnPlayerFinalDamageChanged);

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerKilled, OnPlayerKilled);

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerDead, OnPlayerDead);

        GameEventManager.SubscribeEvent(EventEnum.OnPlayerLeftRoom, OnPlayerLeftRoom);

        #endregion

        #region Register Event

        if (photonView.IsMine)
        {
            GameEventManager.RegisterEvent(EventEnum.OnPlayerDamaged, OnPlayerDamagedCheck);
            

        }

        #endregion
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;

        PlayerGravity();

        if (m_charBase)
        {
            m_attackRange = m_charBase.AttackRange;
            m_criticalHit = m_charBase.CriticalHit;
            m_criticalHitRate = m_charBase.CriticalHitRate;
            m_attack = m_charBase.Attack;
            m_finalDamage = m_charBase.FinalDamage;
            m_moveSpeed = m_charBase.MoveSpeed;
        }
        
    }

    #endregion

    #region Player Control

    void PlayerMove(object[] args)
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
    
    void PlayerTowardChanged(object[] args)
    {
        if (photonView.IsMine)
        {

            var playerScreenPoint = m_camera.WorldToScreenPoint(this.transform.position);
            var point = Input.mousePosition - playerScreenPoint;
            var angle = Mathf.Atan2(point.x, point.y) * Mathf.Rad2Deg;

            playerModel.transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
        }
        
    }

    bool isSafe;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InfiniteCoreArea"))
        {
            isSafe = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InfiniteCoreArea"))
        {
            isSafe = false;
            StartCoroutine(NotSafe());
        }
    }

    IEnumerator NotSafe()
    {
        yield return new WaitForSeconds(1);
        if (!isSafe)
        {
            m_charBase.CurrentHealth -= m_charBase.MaxHealth * 0.05f;
            StartCoroutine(NotSafe());
        }
    }

    void PlayerGravity()
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
                isAttack = true;
            }
        }
    }

    float GetFinalAttack(float t_attack, float t_criticalHit, float t_criticalHitRate, float t_ratio)
    {
        if (t_criticalHitRate == 1) return t_attack * 0.5f * t_criticalHit * t_ratio;

        if (Random.Range(0,1f)<=t_criticalHitRate)
        {
            return t_attack * 0.5f * (1 + t_criticalHit) * t_ratio;
        }
        else
        {
            return t_attack * 0.5f * t_ratio;
        }

    }

    /// <summary>
    /// 创建float数组传递AttackCube的生成数据
    /// </summary>
    /// <param name="initToFinalTime">销毁时间 时间结束时自动销毁</param>
    /// <param name="finalDamage">最终伤害倍率</param>
    /// <param name="activeTime">设置Cube生效时间 为0即为立刻生效</param>
    /// <param name="finalAttack">计算完爆伤后的攻击力</param>
    /// <returns></returns>
    float[] SetAttackCubeData2(float initToFinalTime,float finalDamage, float activeTime, float finalAttack)
    {
        float[] data = new float[4]
        {
            initToFinalTime,
            finalDamage,
            activeTime,
            finalAttack
        };

        return data;
    }

    /// <summary>
    /// 创建Vector3数组传递AttackCube的生成数据
    /// </summary>
    /// <param name="initOffset">初始Cube 坐标偏移</param>
    /// <param name="initScale">初始Cube 大小</param>
    /// <param name="finalOffset">最终Cube 坐标偏移</param>
    /// <param name="finalScale">最终Cube 大小</param>
    /// <returns></returns>
    Vector3[] SetAttackCubeData(Vector3 initOffset, Vector3 initScale, Vector3 finalOffset, Vector3 finalScale)
    {
        Vector3[] data = new Vector3[4]
        {
            initOffset,
            initScale,
            finalOffset,
            finalScale
        };
        return data;
    }

    void InitSoilderNormalAttack(Vector3 selfPos, Quaternion modelRotation, float finalAttack)
    {
        Vector3[] attackCubeData = SetAttackCubeData(
                        modelRotation * Vector3.forward,
                        new Vector3(1.5f, 1.5f, 3) * m_attackRange * 0.1f,
                        modelRotation * Vector3.forward,
                        new Vector3(1.5f, 1.5f, 3.5f) * m_attackRange * 0.1f
                        );

        float[] attackCubeData2 = SetAttackCubeData2(
            0.3f,
            m_finalDamage,
            0,
            finalAttack
            );

        photonView.RPC("SpawnAttackCube", RpcTarget.All, selfPos, modelRotation, attackCubeData, attackCubeData2);
    }

    void InitArcherNormalAttack(Vector3 selfPos, Quaternion modelRotation, float finalAttack)
    {
        Vector3[] attackCubeData = SetAttackCubeData(
                        modelRotation * Vector3.forward,
                        Vector3.one * m_attackRange * 0.1f,
                        modelRotation * Vector3.forward * 10,
                        Vector3.one * m_attackRange * 0.1f
                        );

        float[] attackCubeData2 = SetAttackCubeData2(
            0.5f,
            m_finalDamage,
            0,
            finalAttack
            );

        photonView.RPC("SpawnAttackCube", RpcTarget.AllViaServer, selfPos, modelRotation, attackCubeData, attackCubeData2);
    }

    void InitDoctorNormalAttack(Vector3 selfPos, Quaternion modelRotation, float finalAttack)
    {
        Vector3[] attackCubeData = SetAttackCubeData(
                        modelRotation * Vector3.forward * 3,
                        new Vector3(3, 3, 3) * m_attackRange * 0.1f,
                        modelRotation * Vector3.forward * 3,
                        new Vector3(3, 3, 3) * m_attackRange * 0.1f
                        );

        float[] attackCubeData2 = SetAttackCubeData2(
            0.6f,
            m_finalDamage,
            0.3f,
            finalAttack
            );

        photonView.RPC("SpawnAttackCube", RpcTarget.AllViaServer, selfPos, modelRotation, attackCubeData, attackCubeData2);
    }

    void InitTankerNormalAttack(Vector3 selfPos, Quaternion modelRotation, float finalAttack)
    {
        Vector3[] attackCubeData = SetAttackCubeData(
                        modelRotation * Vector3.forward,
                        new Vector3(1, 1, 2.5f) * m_attackRange * 0.1f,
                        modelRotation * Vector3.forward,
                        new Vector3(1, 1, 2.5f) * m_attackRange * 0.1f
                        );

        float[] attackCubeData2 = SetAttackCubeData2(
            0.3f,
            m_finalDamage,
            0,
            finalAttack
            );

        photonView.RPC("SpawnAttackCube", RpcTarget.AllViaServer, selfPos, modelRotation, attackCubeData, attackCubeData2);
    }

    public void AnimationEventOnPlayerAttackEnd()
    {
        isAttack = false;
    }
    
    public void AnimationEventOnPlayerAttack()
    {
        if (photonView.IsMine)
        {
            if (this.photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber) return;

            Vector3 selfPos = playerModel.transform.position;
            Quaternion modelRotation = playerModel.transform.rotation;

            GameEventManager.EnableEvent(EventEnum.AllowPlayerMove, false);

            float finalAttack = GetFinalAttack(m_attack, m_criticalHit, m_criticalHitRate, 1);

            switch (m_pro)
            {
                case ProEnum.Soilder:
                    InitSoilderNormalAttack(selfPos, modelRotation, finalAttack);
                    break;
                case ProEnum.Archer:
                    InitArcherNormalAttack(selfPos, modelRotation, finalAttack);
                    break;
                case ProEnum.Doctor:
                    InitDoctorNormalAttack(selfPos, modelRotation, finalAttack);
                    break;
                case ProEnum.Tanker:
                    InitTankerNormalAttack(selfPos, modelRotation, finalAttack);
                    break;
            }
        }
    }

    #endregion

    #region PUN Functions

    /// <summary>
    /// 用于被敌方Cube碰撞后调用的接收伤害函数
    /// </summary>
    /// <param name="otherActorNumber">被碰撞的物体ActorNumber</param>
    /// <param name="finalAttack"></param>
    /// <param name="finalDamage"></param>
    [PunRPC]
    public void PlayerDamaged(int otherActorNumber, int ownerActorNumber, float finalAttack, float finalDamage)
    {
        if (otherActorNumber != PhotonNetwork.LocalPlayer.ActorNumber) return;

        enemyActorNumber = ownerActorNumber;
        receiveFinalAttack = finalAttack;
        receiveFinalDamage = finalDamage;
        
        if (otherActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            GameEventManager.EnableEvent(EventEnum.OnPlayerDamaged, true);
        }
    }

    [PunRPC]
    public IEnumerator SpawnAttackCube(Vector3 position, Quaternion rotation, Vector3[] attackCubeData, float[] attackCubeData2, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        GameObject attackCube = Instantiate(attackCubePrefab, position, Quaternion.identity);

        //传入的attackCubeData应该为碰撞体的信息
        attackCube.GetComponent<AttackCube>().InitAttackCube(photonView.Owner, rotation * Vector3.forward, Mathf.Abs(lag), attackCubeData, attackCubeData2);

        yield return new WaitForSeconds(0.5f);

        if (!isAttack)
        {
            GameEventManager.EnableEvent(EventEnum.AllowPlayerMove, true);
        }
    }

    [PunRPC]
    public void ThreeMinRule()
    {
        StartCoroutine(NotSafe());
    }

    #endregion

    #region Event Response

    public void OnPlayerLeftRoom(object[] args)
    {
        GameEventManager.UnsubscribeEvent(EventEnum.AllowPlayerTowardChanged, PlayerTowardChanged);
        GameEventManager.UnsubscribeEvent(EventEnum.AllowPlayerMove, PlayerMove);
        GameEventManager.UnsubscribeEvent(EventEnum.AllowPlayerAttack, PlayerAttack);

        GameEventManager.EnableEvent(EventEnum.OnPlayerLeftRoom, false);
    }

    public void OnPlayerKilled(object[] args)
    {
        int actorNumber;
        int killerActorNumber;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            killerActorNumber = (int)args[1];

            GameEventManager.EnableEvent(EventEnum.OnPlayerKilled, false);

            GameEventManager.UnsubscribeEvent(EventEnum.SendPlayerMoveSpeed, SendPlayerMoveSpeed);

            GameEventManager.UnsubscribeEvent(EventEnum.AllowPlayerMove, PlayerMove);

            GameEventManager.UnsubscribeEvent(EventEnum.AllowPlayerAttack, PlayerAttack);

            GameEventManager.UnsubscribeEvent(EventEnum.AllowPlayerTowardChanged, PlayerTowardChanged);

            GameEventManager.UnsubscribeEvent(EventEnum.OnPlayerAttackRangeChanged, OnPlayerAttackRangeChanged);

            GameEventManager.UnsubscribeEvent(EventEnum.OnPlayerCriticalHitChanged, OnPlayerCriticalHitChanged);

            GameEventManager.UnsubscribeEvent(EventEnum.OnPlayerCriticalHitRateChanged, OnPlayerCriticalHitRateChanged);

            GameEventManager.UnsubscribeEvent(EventEnum.OnPlayerFinalDamageChanged, OnPlayerFinalDamageChanged);

            GameEventManager.UnsubscribeEvent(EventEnum.OnPlayerKilled, OnPlayerKilled);

            GameEventManager.UnsubscribeEvent(EventEnum.OnPlayerDead, OnPlayerDead);
            
        }
    }

    public void OnPlayerDead(object[] args)
    {
        int actorNumber;
        int killerActorNumber;
        if (args.Length == 2)
        {
            actorNumber = (int)args[0];
            killerActorNumber = (int)args[1];

            GameEventManager.EnableEvent(EventEnum.OnPlayerDead, false);

            GameEventManager.UnsubscribeEvent(EventEnum.SendPlayerMoveSpeed, SendPlayerMoveSpeed);
            GameEventManager.UnsubscribeEvent(EventEnum.SendPlayerMoveSpeed, SendPlayerMoveSpeed);

            GameEventManager.UnsubscribeEvent(EventEnum.AllowPlayerMove, PlayerMove);

            GameEventManager.UnsubscribeEvent(EventEnum.AllowPlayerAttack, PlayerAttack);

            GameEventManager.UnsubscribeEvent(EventEnum.AllowPlayerTowardChanged, PlayerTowardChanged);

            GameEventManager.UnsubscribeEvent(EventEnum.OnPlayerAttackRangeChanged, OnPlayerAttackRangeChanged);

            GameEventManager.UnsubscribeEvent(EventEnum.OnPlayerCriticalHitChanged, OnPlayerCriticalHitChanged);

            GameEventManager.UnsubscribeEvent(EventEnum.OnPlayerCriticalHitRateChanged, OnPlayerCriticalHitRateChanged);

            GameEventManager.UnsubscribeEvent(EventEnum.OnPlayerFinalDamageChanged, OnPlayerFinalDamageChanged);

            GameEventManager.UnsubscribeEvent(EventEnum.OnPlayerKilled, OnPlayerKilled);

            GameEventManager.UnsubscribeEvent(EventEnum.OnPlayerDead, OnPlayerDead);

        }
    }

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

    bool OnPlayerDamagedCheck(out object[] args)
    {
        if (enemyActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            args = new object[] { enemyActorNumber, receiveFinalAttack, receiveFinalDamage };
            return true;
        }
        else if(enemyActorNumber == -1)
        {
            args = new object[] { -1 };
            return true;

        }else
        {
            args = new object[] { -2 };
            return true;
        }

    }

    #endregion

}
