using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobManager : MonoBehaviour
{
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
    }

    void Update()
    {
    }
}