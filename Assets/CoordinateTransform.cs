using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
namespace IndoorAtlas
{
    public class CoordinateTransform : MonoBehaviour
    {
        // Conversion factors
        //const float degreesLatitudeInMeters = 111132;     // 1 緯度距離
        //const float degreesLongitudeInMetersAtEquator = 111319.9f;        // 在赤道 1 經度距離
        //const float originLat = 25.151063f, originLon = 121.780095f;        // 設定原點
        // Conversion factors
        const double degreesLatitudeInMeters = 111132;     // 1 緯度距離
        const double degreesLongitudeInMetersAtEquator = 111319.9f;        // 在赤道 1 經度距離
        const double originLat = 25.150958563f, originLon = 121.77993876f;        // 設定原點
        public Text campostext;
        public Text arsopostext;
        public GameObject aRCamera;
        public ARSessionOrigin aRSessionOrigin;
        public Canvas canvas;
        public Text postext;
        // gpsLat, gpsLon 表示現在所處經緯度資訊
        double gpsLat, gpsLon;
        double latOffset, lonOffset;
        float accuracy = float.MaxValue;
        string regionName;
        bool lockOrientation = false, lockLocation = true;
        Quaternion sdkRot;
        Vector3 cameraPosition;
        private IEnumerator enu;
        // Start is called before the first frame update

        void Start()
        {
            //aRSessionOrigin.transform.position = new Vector3(0, 1.5f, 0);
            enu = positioningFunc();
            StartCoroutine(enu);
        }

        // Update is called once per frame
        void Update()
        {

            //(double, int, double) offset = (latOffset, 0, lonOffset);
            //campostext.text = location.position.coordinate.latitude.ToString() + ", " + location.position.coordinate.longitude.ToString();//aRCamera.transform.localPosition.ToString();
            //arsopostext.text = aRSessionOrigin.transform.localPosition.ToString();
            //currentCoordinate.text = aRCamera.transform.localPosition.ToString() + " - " + offset.ToString() + "\n" + aRSessionOrigin.transform.position.ToString();
        }
        public void CancelButton()
        {
            StopCoroutine(enu);
            canvas.enabled = false;
        }
        public void ButtonAction()
        {
            StartCoroutine(enu);
        }
        void IndoorAtlasOnLocationChanged(IndoorAtlas.Location location)
        {
            //campostext.text = location.position.coordinate.latitude.ToString() + ", " + location.position.coordinate.longitude.ToString(); 
               //currentCoordinate.text = "(" + lonOffset.ToString() + ", " + latOffset.ToString() + ")\n" + sdkRot.eulerAngles.ToString();
               gpsLat = (float)location.position.coordinate.latitude;
            gpsLon = (float)location.position.coordinate.longitude;
            accuracy = location.accuracy;
            cameraPosition = aRCamera.transform.localPosition;
            //lockLocation = true;
            if (lockLocation == false) { GeoToUnityCoordinates(); }
            newPosition = true;
            //if (lockOrientation == true) { GenerateScene(); }
        }

        void IndoorAtlasOnEnterRegion(IndoorAtlas.Region region)
        {
            regionName = region.name;
            arsopostext.text = regionName;
            if (regionName == "1f")
            {
                //currentCoordinate.text = "1f";
            }
            else if (regionName == "2f")
            {
                //currentCoordinate.text = "2f";
            }
        }
        private IEnumerator positioningFunc()
        {
            canvas.enabled = true;
            campostext.text = "wait for moving";
            yield return new WaitForSeconds(5);
            //yield return new WaitWhile(() => aRCamera.transform.localPosition.z <2);
            canvas.enabled = false;
            campostext.text = "after moving";
           /*Vector3 OriginPos = aRSessionOrigin.transform.position;
            aRSessionOrigin.transform.position = new Vector3(aRSessionOrigin.transform.position.x + aRCamera.transform.localPosition.x, aRSessionOrigin.transform.position.y + aRCamera.transform.localPosition.y, aRSessionOrigin.transform.position.z + aRCamera.transform.localPosition.z);
            aRSessionOrigin.transform.rotation = Quaternion.Euler(0, 180f, 0);
            aRSessionOrigin.transform.position = OriginPos;*/
        }
        /*void IndoorAtlasOnOrientationChanged(Quaternion rot)
        {
            currentCoordinate.text = "OrientationChanged." ;
            sdkRot = rot;
            if (lockOrientation == false)
            {
                aRSessionOrigin.transform.rotation = rot;

                lockOrientation = true;
                lockLocation = false;
            }
        }*/
        private bool newPosition;
        private IEnumerator AdjustOrientation()
        {
            canvas.enabled = true;
            newPosition = false;
            postext.text = "before wait";
            yield return new WaitWhile(() => newPosition == false);
            // 按下 Button 後記錄起點位置，等待使用者移動3公尺
            double startLat = gpsLat, startLon = gpsLon;
            float startX = aRCamera.transform.localPosition.x, startZ = aRCamera.transform.localPosition.z;
            postext.text = "before walk";
            yield return new WaitWhile(() => Mathf.Pow(aRCamera.transform.localPosition.x - startX, 2) + Mathf.Pow(aRCamera.transform.localPosition.z - startZ, 2) < 9);
            // 移動3公尺後，等待取得新的位置資訊
            newPosition = false;
            yield return new WaitWhile(() => newPosition == false);
            //currentCoordinate.text = "after waiting";
            // 得到新的位置資訊後，做方位校正
            double endLat = gpsLat, endLon = gpsLon;
            float localEulerAngle = 0, globalEulerAngle = 0;

            localEulerAngle = aRCamera.transform.rotation.eulerAngles[1];
            postext.text = "calculating";
            latOffset = (endLat - startLat) * degreesLatitudeInMeters;
            lonOffset = (endLon - startLon) * GetLongitudeDegreeDistance(gpsLat);
            // 計算世界座標方位
            // 預設使用者位置在東半球
            // 若在西半球則東西對調
            if (latOffset == 0f)
            {
                if (lonOffset < 0) globalEulerAngle = 90;         // 正西
                else if (lonOffset > 0) globalEulerAngle = -90;       // 正東
            }
            else if (lonOffset == 0f)
            {
                if (latOffset < 0) globalEulerAngle = 180;         // 正南
                else globalEulerAngle = 0;      // 正北
            }
            else
            {
                float rotation = Mathf.Atan((float)latOffset / (float)lonOffset);
                if (latOffset > 0 && lonOffset > 0) globalEulerAngle = 360 - rotation * Mathf.Rad2Deg;
                else if (latOffset < 0 && lonOffset > 0) globalEulerAngle = 180 - rotation * Mathf.Rad2Deg;
                else if (latOffset < 0 && lonOffset < 0) globalEulerAngle = 180 - rotation * Mathf.Rad2Deg;
                else if (latOffset > 0 && lonOffset < 0) globalEulerAngle = -rotation * Mathf.Rad2Deg;
            }
            //currentCoordinate.text = "after if";
            Quaternion rot = Quaternion.identity;
            rot.eulerAngles = new Vector3(0, 180 - (globalEulerAngle - localEulerAngle), 0);
            aRSessionOrigin.transform.rotation = rot;
            //currentCoordinate.text = localEulerAngle.ToString() + " " + globalEulerAngle.ToString() + " " + latOffset.ToString() + "/" + lonOffset.ToString();
            GeoToUnityCoordinates();
            postext.text = "positioning finished";
            canvas.enabled = false;
        }
        private double GetLongitudeDegreeDistance(double latitude)
        {
            return degreesLongitudeInMetersAtEquator * System.Math.Cos(latitude * (Mathf.PI / 180));
            //return degreesLongitudeInMetersAtEquator * Mathf.Cos(latitude * (Mathf.PI / 180f));
        }

        // 「經緯度」差異轉換為「公尺」距離差異
        void GeoToUnityCoordinates()
        {
            latOffset = (gpsLat - originLat) * degreesLatitudeInMeters;
            lonOffset = (gpsLon - originLon) * GetLongitudeDegreeDistance(gpsLat);

            // 設定 ARSessionOrigin 在空間中的位置（會一同影響 aRCamera 位置，因為 aRCamera 為 ARSessionOrigin 之子物件），可用 y 調整手機預設高度
            aRSessionOrigin.transform.position = new Vector3(cameraPosition[0] - (float)lonOffset, aRSessionOrigin.transform.localPosition[1], cameraPosition[2] - (float)latOffset);
            //line 147:最後修改處
            lockLocation = true;
            //aRCamera.transform.localPosition = new Vector3((float)lonOffset, aRCamera.transform.localPosition[1], (float)latOffset);
        }
        void GenerateScene()
        {
            Vector3 position = new Vector3(aRCamera.transform.localPosition[0] - (float)lonOffset, 1.5f, aRCamera.transform.localPosition[2] - (float)latOffset);
            //Instantiate(scenePrefab, position, sdkRot);
        }
    }
}