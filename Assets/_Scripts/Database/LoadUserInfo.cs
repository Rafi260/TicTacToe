using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class LoadUserInfo : MonoBehaviour
{
    public DependencyStatus dependencyStatus;
    public DatabaseReference DBreference;

    public static string UserId ;  //your uid is set by authentication
    static string otherPlayerUid ;

    [Header("Plaery Info Ui ")]
    public TMP_Text nameText;
    public TMP_Text emailText;
    public TMP_Text totalMatchText;
    public TMP_Text winText;
    public TMP_Text loseText;
    public TMP_Text drawText;
    public TMP_Text winRateText;

    string name, email;
    int win, lose,draw, totalMatch;
    float winRate;


    [Header("Other Plaery Info")]
    public TMP_Text otherPlayerNameText ;
    public TMP_Text otherPlayerWinRateText;

    float  otherPlayerWinRate;

 
    void Start()
    {
        //Let firebase initialize and load user data
        StartCoroutine(WaitForFirebaseSetup());
    }

    private IEnumerator WaitForFirebaseSetup()
    {
        // Wait for Firebase setup to complete
        yield return new WaitUntil(() => DBreference != null);

        if (UserId != null)
        {
            StartCoroutine(LoadUserData());
        }
        else
        {
            print("userNull");
        }
    }

    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
   
    }

    private void InitializeFirebase()
    {
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    //you have received uid of opponent. Now his data in your Ui
    public void LoadReceivedUser(string UID )
    {
        Debug.Log("received uid" + UID);
        otherPlayerUid = UID;
        StartCoroutine(OtherPlayerData(otherPlayerUid));
    }

    public void Won()
    {
        totalMatch++;
        win++;
        StartCoroutine(UpdateUserData());
    }
    public void Lost()
    {
        totalMatch++;
        lose++;
        StartCoroutine(UpdateUserData());
    }
    public void Draw()
    {
        totalMatch++;
        StartCoroutine(UpdateUserData());
    }

    //Loading your data
    private IEnumerator LoadUserData()
    {
        //Get the currently logged in user data
        var DBTask = DBreference.Child("users").Child(UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet
            SetUserProfileText("Player01", "player@gmail.com", 0, 0, 0);
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            name = snapshot.Child("username").Value.ToString();
            email = snapshot.Child("email").Value.ToString();
            totalMatch = int.Parse(snapshot.Child("totalMatch").Value.ToString());
            win = int.Parse(snapshot.Child("win").Value.ToString()) ;
            lose = int.Parse(snapshot.Child("lose").Value.ToString());           

            SetUserProfileText(name,email, totalMatch, win, lose);
            
        }
    }


    //Loading opponents data
    private IEnumerator OtherPlayerData(string Uid)
    {
        var DBTask = DBreference.Child("users").Child(Uid).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {

            SetOtherUserProfiletext("Player02", 0);
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            string _name = snapshot.Child("username").Value.ToString();           
            int totalMatch = int.Parse(snapshot.Child("totalMatch").Value.ToString());
            int win = int.Parse(snapshot.Child("win").Value.ToString());
            if(totalMatch != 0) otherPlayerWinRate = ((float)win/(float)totalMatch)*100;
            else otherPlayerWinRate = 0;

            SetOtherUserProfiletext(_name, otherPlayerWinRate);
        }
    }

    //setting up user data in Ui
    void SetUserProfileText(string name,string email, int totalMatch, int win ,int lose)
    {
        if(nameText)nameText.text = name;
        if(emailText)emailText.text = email;
        if(totalMatchText) totalMatchText.text = totalMatch.ToString();
        if(winText) winText.text = win.ToString();
        if(loseText) loseText.text = lose.ToString();

        if (totalMatch != 0)
        {
            winRate = ((float)win / (float)totalMatch) * 100;
            draw = totalMatch - (win + lose);
        }
        else
        {
            winRate = 0f;
            draw = 0;
        }
        if (winRateText) winRateText.text = string.Format("{0:0.00}", winRate).ToString();
        if(drawText)drawText.text = draw.ToString();

    }

    void SetOtherUserProfiletext(string name, float winRate)
    {
        if (otherPlayerNameText) otherPlayerNameText.text = name;

        
        if(otherPlayerWinRateText) otherPlayerWinRateText.text = string.Format("{0:0.00}", winRate).ToString();
    }


    IEnumerator UpdateUserData()
    {
        User user = new User(name, email, totalMatch, win, lose);
        string json = JsonUtility.ToJson(user);

        //Set the currently logged in user username in the database
        var DBTask = DBreference.Child("users").Child(UserId).SetRawJsonValueAsync(json);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }


    //if rematch in online game, loading user data with updated value
    public void RestartLevel()
    {
        StartCoroutine(LoadUserData());
        StartCoroutine(OtherPlayerData(otherPlayerUid));
    }
}
