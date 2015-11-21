using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

	public float jetpackForce = 75.0f;
	public float forwardMovementSpeed = 3.0f;
	public Transform groundCheckTransform;	//存储之前创建的用于groundCheck的空对象
	private bool grounded;	//表示小老鼠是否着陆。
	public LayerMask groundCheckLayerMask;	//具体着陆的定义。
	Animator animator;	//包含了Animator组件的引用。
	public ParticleSystem jetpack;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 newVelocity = GetComponent<Rigidbody2D>().velocity;
		newVelocity.x = forwardMovementSpeed;
		GetComponent<Rigidbody2D>().velocity = newVelocity;
	}

	void FixedUpdate () {
		bool jetpackActive = Input.GetButton("Fire1");
		if (jetpackActive)
		{
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jetpackForce));
		}
		UpdateGroundedStatus();
		AdjustJetpack(jetpackActive);
	}

	void UpdateGroundedStatus(){
		//1	为检测小老鼠是否着陆，以groundCheck对象的位置为圆心，新建一个半径为0.1的圆。如果圆与在groundCheckLayerMask中定义的层有重叠，则小老鼠着陆
		grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

		//2
		animator.SetBool("grounded", grounded);
	}

	void AdjustJetpack (bool jetpackActive) {
		jetpack.enableEmission = !grounded;
		jetpack.emissionRate = jetpackActive ? 300.0f : 75.0f; 
	}
}
