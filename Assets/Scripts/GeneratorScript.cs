using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratorScript : MonoBehaviour {

	// 包括所有的预制件，可以用其来生成实例
	public GameObject[] availableRooms;
	// 包括已经实例化的预制件对象，方便确定当前最后房间的位置，从而来判断是否需要增加更多的房间。一旦房间从玩家背后消失，则将其从列表中删掉。
	public List<GameObject> currentRooms;
	// 用来存储屏幕宽度
	private float screenWidthInPoints;


	// Use this for initialization
	void Start () {
		float height = 2.0f * Camera.main.orthographicSize;
		screenWidthInPoints = height * Camera.main.aspect;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/*
	* @parm farhtestRoomEndX 是当前最后侧房间的右边缘位置
	*/
	void AddRoom(float farhtestRoomEndX) {
		//1	随机生成一个预制件类型用来生成（本例中只有一种）。
		int randomRoomIndex = Random.Range(0, availableRooms.Length);

		//2 根据随机生成的类型，创建实例room。
		GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);

		//3 由于房间只是一个包括若干物品的空对象，不能够直接得到大小。可以通过找到天花板的宽度来得到房间的宽度，两者是一样大小的。
		float roomWidth = room.transform.FindChild("floor").localScale.x;

		//4 计算新创建房间实例的x位置，紧挨着当前最后一个房间
		float roomCenter = farhtestRoomEndX + roomWidth * 0.5f;

		//5 设置房间位置。仅需要设置x坐标，因为所有房间的y和z坐标都是0.
		room.transform.position = new Vector3(roomCenter, 0, 0);

		//6
		currentRooms.Add(room);         
	}
	/**
	*
	*/
	void GenerateRoomIfRequired() {
		//1 创建一个新数组用来存储需要被移除的房间。之所以需要单独创建一个数组，是因为当你在遍历的时候，暂时还不能移除。
		List<GameObject> roomsToRemove = new List<GameObject>();

		//2 addRooms 是一个bool型变量，标记是否需要增加新房间，默认值是ture。虽然在foreach循环中，大部分情况下，会被设置成false
		bool addRooms = true;		

		//3 保存当前玩家的位置：这里说的位置，其实特指玩家的x分量位置
		float playerX = transform.position.x;

		//4 计算一个removeRoomX位置，在这之前的房间是应该被删除的（为了考虑内存因素，看不到的房间要被删掉）
		float removeRoomX = playerX - screenWidthInPoints;		

		//5 计算addRoomX变量。如果在addRoomX位置之后没有房间存在，则需要新建房间。
		float addRoomX = playerX + screenWidthInPoints;

		//6 farthestRoomEndX变量存储的是当前游戏等级结束时候的位置，为了使得等级和等级之间无缝对接，需要严格的在这个位置添加新房间。
		float farthestRoomEndX = 0;

		foreach(var room in currentRooms) {
		  //7	使用foreach函数遍历当前的房间，使用天花板的宽度作为房间宽度，并且计算房间的起始位置roomStartX（房间的最左侧位置）以及结束位置（房间的最右侧）
		  float roomWidth = room.transform.FindChild("floor").localScale.x;
		  float roomStartX = room.transform.position.x - (roomWidth * 0.5f);	
		  float roomEndX = roomStartX + roomWidth;							
		   
		  //8 如果在addRoomX之后有房间，则现在不需要创建新房间。
		  if (roomStartX > addRoomX)
			addRooms = false;

		  //9 如果房间的右侧位置小于removeRoomX，则将该房间加到移除队列
		  if (roomEndX < removeRoomX)
			roomsToRemove.Add(room);

		  //10 计算得到 farthestRoomEndX，这个当前等级结束的位置。
		  farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
		}

		//11 删除掉移除数组中存储的房间。
		foreach(var room in roomsToRemove) {
		  currentRooms.Remove(room);
		  Destroy(room);			
		}
		 
		//12 如果addRooms变量为true，则证明需要增加房间。
		if (addRooms)
		  AddRoom(farthestRoomEndX);
	}
	void FixedUpdate(){
		GenerateRoomIfRequired();
	}
}

	
	

