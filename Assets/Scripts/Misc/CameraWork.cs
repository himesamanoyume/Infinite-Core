// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraWork.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in PUN Basics Tutorial to deal with the Camera work to follow the player
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using Photon.Pun;
using UnityEngine;


/// <summary>
/// Camera work. Follow a target
/// </summary>
public class CameraWork : MonoBehaviourPun
{
	#region Private Fields

	[Tooltip("The distance in the local x-z plane to the target")]
	[SerializeField]
	private float distance = 7.0f;

	[Tooltip("The height we want the camera to be above the target")]
	[SerializeField]
	private float height = 3.0f;

	[Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
	[SerializeField]
	private Vector3 centerOffset = Vector3.zero;

	[Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
	[SerializeField]
	private bool followOnStart = false;

	[Tooltip("The Smoothing for the camera to follow the target")]
	[SerializeField]
	private float smoothSpeed = 0.125f;

	//public CinemachineVirtualCamera cinemachine;
	//PlayerMoveController controller;

	// cached transform of the target
	Transform cameraTransform;

	// maintain a flag internally to reconnect if target is lost or camera is switched
	bool isFollowing;

	// Cache for camera offset
	Vector3 cameraOffset = Vector3.zero;

	public float Speed = 10f;

	#endregion

	#region MonoBehaviour Callbacks

	/// <summary>
	/// MonoBehaviour method called on GameObject by Unity during initialization phase
	/// </summary>
	void Start()
	{
  //      if (photonView.IsMine)
  //      {
		//	cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
		//	cinemachine.m_Follow = gameObject.transform;
		//}
		
		// Start following the target if wanted.
		if (followOnStart)
		{
			OnStartFollowing();
		}
		//controller = gameObject.GetComponent<PlayerMoveController>();
	}


	void LateUpdate()
	{
		// The transform target may not destroy on level load, 
		// so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
		if (cameraTransform == null && isFollowing)
		{
			OnStartFollowing();
		}

        // only follow is explicitly declared
        if (isFollowing)
        {
            Follow();
        }
    }

	#endregion

	#region Public Methods

	/// <summary>
	/// Raises the start following event. 
	/// Use this when you don't know at the time of editing what to follow, typically instances managed by the photon network.
	/// </summary>
	public void OnStartFollowing()
	{
		cameraTransform = Camera.main.transform;
		isFollowing = true;
		// we don't smooth anything, we go straight to the right camera shot
		Cut();
	}

    #endregion

    #region Private Methods

    /// <summary>
    /// Follow the target smoothly
    /// </summary>
    void Follow()
    {
        cameraOffset.z = -distance;
        cameraOffset.y = height;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position + this.transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);

        cameraTransform.LookAt(this.transform.position + centerOffset);

        //Vector3 wDirection = new Vector3(1, 0, 1);
        //Vector3 sDirection = new Vector3(-1, 0, -1);
        //Vector3 aDirection = new Vector3(-1, 0, 1);
        //Vector3 dDirection = new Vector3(1, 0, -1);

        //Vector3 direction = new Vector3(hor, 0, ver).normalized;

        //if (Input.GetKey(KeyCode.W))
        //{
        //    Vector3 move = wDirection * Speed * Time.deltaTime;
        //    controller.Move(move);
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    Vector3 move = sDirection * Speed * Time.deltaTime;
        //    controller.Move(move);
        //}
        //else if (Input.GetKey(KeyCode.A))
        //{
        //    Vector3 move = aDirection * Speed * Time.deltaTime;
        //    controller.Move(move);
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    Vector3 move = dDirection * Speed * Time.deltaTime;
        //    controller.Move(move);
        //}
    }


    void Cut()
	{
		cameraOffset.z = -distance;
		cameraOffset.y = height;

		cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);

		cameraTransform.LookAt(this.transform.position + centerOffset);
	}
	#endregion
}
