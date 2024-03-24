using System;
using System.Collections;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

namespace Database
{
  [Serializable]
  public class DataToSave
  {
    public string userName;
    public int totalMoney;
    public int currentLevel;
  }

  public class FirebaseDataManager : MonoBehaviour
  {
    public DataToSave dataToSave;
    public string userId;
    private DatabaseReference _databaseReference;

    private void Awake()
    {
      _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveData()
    {
      string json = JsonUtility.ToJson(dataToSave);
      _databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    public void LoadData()
    {
      StartCoroutine(LoadDataEnum());
    }

    public IEnumerator LoadDataEnum()
    {
      Task<DataSnapshot> serverData = _databaseReference.Child("users").
        Child(userId).GetValueAsync();
      yield return new WaitUntil(predicate: () => serverData.IsCompleted);
      Debug.Log("Process is complete.");
      
      DataSnapshot dataSnapshot = serverData.Result;
      string jsonData = dataSnapshot.GetRawJsonValue();
      
      if (jsonData!=null)
      {
       Debug.Log("Server data found.");
       dataToSave = JsonUtility.FromJson<DataToSave>(jsonData);
      }
      else
      {
        Debug.Log("No data found.");
      }
    }
       // private IEnumerator UpdateMoney()
    // {
    //   //Set the currently logged in user kills
    //   Task databaseTask = databaseReference.Child("userData").Child(user.UserId)
    //     .Child("money").SetValueAsync(200);
    //
    //   yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);
    //
    //   if (databaseTask.Exception != null)
    //   {
    //     Debug.LogWarning(message: $"Failed to register task with {databaseTask.Exception}");
    //   }
    //   else
    //   {
    //     Debug.Log("Kills are updated");
    //     //Kills are now updated
    //   }
    // }

  }
}
