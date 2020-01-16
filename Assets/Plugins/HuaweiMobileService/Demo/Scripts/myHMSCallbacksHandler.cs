using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using HuaweiMobileService;

public class myHMSCallbacksHandler : HMSCallbacksHandler {

	public GameObject shop;
	ShopManager shopManager;

	// Use this for initialization
	public override void Start () {
		productList = new List<ProductDetail>();
		shopManager = shop.GetComponent<ShopManager>();
	}
	
	public override void productListInitialiced()
	{
		shopManager.activateProducts();
	}

	public override void onLoginSuccess(GameUserData gameUserData)
	{
		Debug.Log("[HMS]: logged successfully.");
		shopManager.setUser(gameUserData.displayName);
	}

	public override void onLoginError(int resultCode)
	{
		Debug.Log("[HMS]: log ERROR " + resultCode);
		switch ((HMSResponses.GameStatusCodes)resultCode)
		{
			//See Constants.cs>GameStatusCodes for all available responses
			case HMSResponses.GameStatusCodes.GAME_STATE_ERROR:
				Debug.LogWarning("[HMS]: Login error, try again ");
				break;



			default:
				break;
		}
	}

	public override void onProductDetailSuccess(ProductDetailsResponse response)
	{

		productList.Add(response.productList[0]);
		Debug.Log("[HMS]: " + response.productList.Count + "product detail retrieved");

	}

	public override void onProductDetailError(int resultCode)
	{

		Debug.Log("[HMS]: log ERROR " + resultCode);
		//See Constants.cs>GameStatusCodes for all available responses
		switch ((HMSResponses.PayStatusCodes)resultCode)
		{
			// TODO: Product List errors
			case HMSResponses.PayStatusCodes.PAY_STATE_ERROR:
				Debug.LogWarning("[HMS]: Payment error, try again ");
				break;



			default:
				break;
		}
	}

	public override void OnPurchaseSuccess(ProductPayResponse response)
	{
		switch (response.productNo)
		{
			case "coins100":
				shopManager.addCoins(100);
				break;
			case "coins1000":
				shopManager.addCoins(1000);
				break;
			case "no_ads":
				shopManager.setNoAds(true);
				break;
			case "premium":
				shopManager.setPremium(true);
				break;

		}
	}

	public override void OnPurchaseError(int resultCode)
	{
		Debug.Log("[HMS]: log ERROR " + resultCode);
	}

	public override void OnRestorePurchasesSuccess(PurchaseInfoResponse response)
	{

		response.purchaseInfoList.ForEach(purchaseInfo => {
			Debug.Log("you bought " + purchaseInfo.productId);
			switch (purchaseInfo.productId)
			{
				case "no_ads":
					shopManager.setNoAds(true);
					break;
				case "premium":
					shopManager.setPremium(true);
					break;
			}
		});
	}

	public override void OnRestorePurchasesError(int resultCode)
	{
		Debug.Log("[HMS]: log ERROR " + resultCode);
	}
}
