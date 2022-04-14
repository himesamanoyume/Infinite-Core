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

    ProEnum m_pro;

    float m_attack;

    float m_attackRange;

    float m_criticalHit;

    float m_criticalHitRate;

    float m_finalDamage;

    float receiveFinalAttack;

    float receiveFinalDamage;

    bool isAttack = false;

    #endregion

    #region Unity Functions

    void Start()
    {
        #region Init

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

            var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
            var point = Input.mousePosition - playerScreenPoint;
            var angle = Mathf.Atan2(point.x, point.y) * Mathf.Rad2Deg;
            playerModel.transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
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

        photonView.RPC("SpawnAttackCube", RpcTarget.AllViaServer, selfPos, modelRotation, attackCubeData, attackCubeData2);
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
    /// <param name="finalAttack"></param>
    /// <param name="finalDamage"></param>
    [PunRPC]
    public void PlayerDamaged(int actorNumber, float finalAttack, float finalDamage)
    {
        //if (actorNumber == PhotonNetwork.LocalPlayer.ActorNumber) return;

        Debug.LogWarning(actorNumber + " Damaged");

        receiveFinalAttack = finalAttack;
        receiveFinalDamage = finalDamage;

        if (actorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
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

    bool OnPlayerDamagedCheck(out object[] args)
    {
        args = new object[] { receiveFinalAttack, receiveFinalDamage };
        return true;
    }

    #endregion

}
