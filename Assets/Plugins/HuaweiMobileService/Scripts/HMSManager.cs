#define DEBUG 
#define TRACE

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using HuaweiMobileService;



public class HMSManager : MonoBehaviour
{

    // private HMSCallbacksHandler callbacksHandler;
    public string key;


    public Product[] products;

    [HideInInspector]
    public int numberOfProductsRetrieved;


    void Start()
    {
#if DEBUG
        Debug.Log("[HMS]: HMS manager Init");
#endif

        // callbacksHandler = GetComponent<HMSCallbacksHandler>();
        InitialiceIAP();
    }


    public void login()
    {

        int isForceLogin = 1;
        GameAgent.NewInstance().Login(isForceLogin, new LoginHandlerImpl());

    }

    private void InitialiceIAP()
    {
        foreach (Product product in products)
        {
            GetProductDetail(product.productID);

            if (!product.isConsumable)
            {
                GetPurchaseInfo(product.productID);
            }
        }
    }


    public void CheckUpdate()
    {

        Purchasing.CheckUpdate(new CheckUpdateCallbackImpl());

    }

    /// <summary>
    /// Get the product info.!-- (Name, description, Price)
    /// </summary>
    /// <param name="productID">the product ID</param>
    public void GetProductDetail(string productID)
    {

        string requestId = DateTime.Now.ToString("yyyyMMddhhmmssfff");

        ProductDetailsRequest request = new ProductDetailsRequestBuild()
            .Info(requestId, productID)
            .Build();

        Purchasing.GetProductDetail(request, new ProductDetailImpl());

    }

    public void BuyProduct(string productID)

    {


        ProductPayRequest request = new ProductPayRequestBuild()
            .Info("Test 2", productID, DateTime.Now.ToString("yyyyMMddhhmmssfff"))
            .OptServiceCatalog("X6")
            .RsaSign(key)
            .Build();

        Purchasing.ProductPay(request, new ProductPayImpl());


    }

    public void GetPurchaseInfo(string productID)
    {
        PurchaseInfoRequest request = new PurchaseInfoRequestBuild()
            .RsaSign(key)
            .Build();



        Purchasing.GetPurchaseInfo(request, new PurchaseImpl());
    }


}

// *************  HANDLERS  *************//


/// <summary>
/// Class that handles the Login Callback
/// </summary>
class LoginHandlerImpl : ILoginHandler

{
    //private Text resultText;
    public HMSCallbacksHandler callbacksHandler;

    public void OnChange()

    {

    }

    /// <summary>
    /// The OnResult method fires when it recieves the answer from the server
    /// </summary>
    /// <param name="resultCode">Code</param>
    /// <param name="response">Response</param>
    public void OnResult(int resultCode, GameUserData response)

    {
#if DEBUG
        Debug.Log("Login callback");
#endif

        callbacksHandler = GameObject.Find("HMSManager").GetComponent<HMSCallbacksHandler>();


        if (resultCode == 0)
        {
            callbacksHandler.onLoginSuccess(response);

        }
        else
        {
            callbacksHandler.onLoginError(resultCode);
        }
    }

}

/// <summary>
/// Class that handles the check for updates Callback
/// </summary>
class CheckUpdateCallbackImpl : ICheckUpdateHandler

{
    private HMSCallbacksHandler callbacksHandler;

    /// <summary>
    /// The OnResult method fires when it recieves the answer from the server
    /// </summary>
    /// <param name="resultCode">Code</param>

    void ICheckUpdateHandler.OnResult(int resultCode)

    {
        callbacksHandler = GameObject.Find("HMSManager").GetComponent<HMSCallbacksHandler>();

        switch (resultCode)
        {
            case -1:
                callbacksHandler.OnCheckUpdateError(resultCode);
                break;

            case 0:


            case 1:
                callbacksHandler.OnCheckUpdateSuccess(resultCode);
                break;


        }
    }

}


/// <summary>
/// Class that handles the Payment Callback
/// </summary>
class ProductPayImpl : IProductPayHandler

{
    private HMSCallbacksHandler callbacksHandler;

    /// <summary>
    /// The OnResult method fires when it recieves the answer from the server
    /// </summary>
    /// <param name="resultCode">Code</param>
    /// <param name="response">Response</param>
    public void OnResult(int resultCode, ProductPayResponse response)

    {
#if DEBUG
        Debug.Log("Login callback");
#endif

        callbacksHandler = GameObject.Find("HMSManager").GetComponent<HMSCallbacksHandler>();


        if (resultCode == 0)
        {
            callbacksHandler.OnPurchaseSuccess(response);

        }
        else
        {
            callbacksHandler.OnPurchaseError(resultCode);
        }

    }

}

/// <summary>
/// Class that handles the Get Purchases Callback
/// </summary>
class PurchaseImpl : IPurchaseInfoHandler

{
    private HMSCallbacksHandler callbacksHandler;

    /// <summary>
    /// The OnResult method fires when it recieves the answer from the server
    /// </summary>
    /// <param name="resultCode">Code</param>
    /// <param name="response">Response</param>
    public void OnResult(int resultCode, PurchaseInfoResponse response)

    {
#if DEBUG
        Debug.Log("get Purchase Info callback");
#endif

        callbacksHandler = GameObject.Find("HMSManager").GetComponent<HMSCallbacksHandler>();

        if (resultCode == 0)
        {
            callbacksHandler.OnRestorePurchasesSuccess(response);
        }
        else
        {
            callbacksHandler.OnRestorePurchasesError(resultCode);
        }

    }

}


/// <summary>
/// Class that handles the get the product detail Callback
/// </summary>
class ProductDetailImpl : IProductDetailHandler

{
    private HMSCallbacksHandler callbacksHandler;
    HMSManager hmsManager;

    /// <summary>
    /// The OnResult method fires when it recieves the answer from the server
    /// </summary>
    /// <param name="resultCode">Code</param>
    /// <param name="response">Response</param>
    public void OnResult(int resultCode, ProductDetailsResponse response)

    {

        hmsManager = GameObject.Find("HMSManager").GetComponent<HMSManager>();
        callbacksHandler = GameObject.Find("HMSManager").GetComponent<HMSCallbacksHandler>();


        if (resultCode == 0)
        {
            hmsManager.numberOfProductsRetrieved++;
            callbacksHandler.onProductDetailSuccess(response);

            if (hmsManager.numberOfProductsRetrieved == hmsManager.products.Length)
            {
#if DEBUG
                Debug.Log("[HMS]: all products retrieved");
#endif
                callbacksHandler.productListInitialiced();
            }

        }
        else
        {
            callbacksHandler.onProductDetailError(resultCode);
        }
    }


}

/// <summary>
/// Class that handles the Save Player Info Callback
/// </summary>
class SavePlayerInfoHandlerImpl : ISavePlayerInfoHandler

{

    private HMSCallbacksHandler callbacksHandler;

    /// <summary>
    /// The OnResult method fires when it recieves the answer from the server
    /// </summary>
    /// <param name="resultCode">Code</param>
    public void OnResult(int resultCode)

    {
        callbacksHandler = GameObject.Find("HMSManager").GetComponent<HMSCallbacksHandler>();

#if DEBUG
        Debug.Log("Saving players info callback");
#endif


        if (resultCode == 0)
        {
            callbacksHandler.OnSavePlayerSucccess();
        }
        else
        {
            callbacksHandler.OnSavePlayerError(resultCode);
        }
    }

}

[System.Serializable]
public class Product
{
    [SerializeField]
    public string productID;
    [SerializeField]
    public bool isConsumable;
}
