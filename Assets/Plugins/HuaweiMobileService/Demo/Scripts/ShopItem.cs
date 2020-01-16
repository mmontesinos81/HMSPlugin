using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{


	public string productID;

	private HMSCallbacksHandler callbacksHandler;

	private HMSManager hmsManager;

	private Sprite itemImage;

	private Image img;
	private Text nameText, priceText, descText;

	bool update;

	private void Start()
	{
		hmsManager = GameObject.Find("HMSManager").GetComponent<HMSManager>();
		callbacksHandler = GameObject.Find("HMSManager").GetComponent<HMSCallbacksHandler>();

		img = transform.Find("ItemImage").GetComponent<Image>();
		nameText = transform.Find("ItemName").GetComponent<Text>();
		priceText = transform.Find("ItemCost").GetComponent<Text>();
		descText = transform.Find("ItemDesc").GetComponent<Text>();

		//itemImage = Resources.Load<Sprite>("DefaultImage");

		img.sprite = itemImage;

		nameText.text = "";
		priceText.text = "";
		descText.text = "";
	}

	void Update()
	{
		if (update)
		{
			update = false;

			itemImage = Resources.Load<Sprite>(productID);
			img.sprite = itemImage;

			string name = callbacksHandler.getProductName(productID);
			nameText.text = name;
			priceText.text = callbacksHandler.getProductPrice(productID);
			descText.text = callbacksHandler.getProductDescription(productID);


		}
	}

	public void Load()
	{
		update = true;
	}

	public void Buy()
	{
		hmsManager.BuyProduct(productID);
	}
}

