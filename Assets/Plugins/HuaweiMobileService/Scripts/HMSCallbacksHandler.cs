using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using HuaweiMobileService;

public class HMSCallbacksHandler : MonoBehaviour
{

	protected List<ProductDetail> productList;

	
	public virtual void Start()
	{
        
        productList = new List<ProductDetail>();
	}


	public virtual void productListInitialiced()
	{
        Debug.Log("[HMS]: Products Initialiced.");
    }


	public virtual void onLoginSuccess(GameUserData gameUserData)
	{
		Debug.Log("[HMS]: logged successfully.");
		
	}

	public virtual void onLoginError(int resultCode)
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

	public virtual void onProductDetailSuccess(ProductDetailsResponse response)
	{

		productList.Add(response.productList[0]);
		Debug.Log("[HMS]: " + response.productList.Count + "product detail retrieved");

	}

	public string getProductName(string productID)
	{
		return productList.Find(productD => productD.productNo.Contains(productID)).productName;
	}

	public string getProductDescription(string productID)
	{
		return productList.Find(productD => productD.productNo.Contains(productID)).productDesc;
	}

	public string getProductPrice(string productID)
	{
		return productList.Find(productD => productD.productNo.Contains(productID)).price;
	}

	public string getProductCurrency(string productID)
	{
		return productList.Find(productD => productD.productNo.Contains(productID)).currency;
	}

	public virtual void onProductDetailError(int resultCode)
	{

		Debug.Log("[HMS]: log ERROR " + resultCode);
		//See Constants.cs>GameStatusCodes for all available responses
		switch ((HMSResponses.PayStatusCodes)resultCode)
		{
			case HMSResponses.PayStatusCodes.PAY_STATE_ERROR:
				Debug.LogError("[HMS]: Payment error, try again ");
				break;



			default:
				break;
		}
	}

	public virtual void OnPurchaseSuccess(ProductPayResponse response)
	{
		switch (response.productNo)
		{
            default:
                break;

		}
	}

	public virtual void OnPurchaseError(int resultCode)
	{
		Debug.LogError("[HMS]: log ERROR " + resultCode);
	}

	public virtual void OnRestorePurchasesSuccess(PurchaseInfoResponse response)
	{

		response.purchaseInfoList.ForEach(purchaseInfo => {
			Debug.Log("Restores purchase for " + purchaseInfo.productId);
			switch (purchaseInfo.productId)
			{
				
                default:
                    break;
			}
		});
	}

	public virtual void OnRestorePurchasesError(int resultCode)
	{
		Debug.LogError("[HMS]: Restore Purchases ERROR " + resultCode);
	}

    public virtual void OnSavePlayerSucccess()
    {
        Debug.Log("[HMS]: Player data saved.");
    }

    public virtual void OnSavePlayerError(int resultCode)
    {
        Debug.LogError("[HMS]: On Save Error: " +resultCode);
    }
    public virtual void OnCheckUpdateSuccess(int resultCode)
    {
        if (resultCode == 0)
        {
            Debug.Log("[HMS]: No updates available");
        } else if(resultCode == 1)
        {
            Debug.Log("[HMS]: Found available update");
        }
    }
    public virtual void OnCheckUpdateError(int resultCode)
    {
        Debug.Log("[HMS]: Update ERROR " + resultCode);
    }
}
