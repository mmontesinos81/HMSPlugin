using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemName : MonoBehaviour
{
	private HMSCallbacksHandler callbacksHandler;
	private Text itemName;
	// Start is called before the first frame update
	void Start()
	{
		callbacksHandler = GameObject.Find("HMSManager").GetComponent<HMSCallbacksHandler>();
		itemName = GetComponent<Text>();
		itemName.text = callbacksHandler.getProductName("coins100");
	}

	// Update is called once per frame
	void Update()
	{

	}
}
