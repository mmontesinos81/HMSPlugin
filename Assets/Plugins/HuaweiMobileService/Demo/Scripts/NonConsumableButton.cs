using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonConsumableButton : MonoBehaviour
{
	public string productID;
	Text buyButtonText;

	public void Start()
	{
		buyButtonText = transform.Find("ItemBuyButtonText").GetComponent<Text>();
	}

	public void alreadyBought(string productIDBought)
	{
		if (productID == productIDBought)
		{
			buyButtonText.text = "BOUGHT";
			transform.GetComponentInParent<Button>().interactable = false;
		}

	}
}
