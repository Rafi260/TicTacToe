using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine.SceneManagement;

public class _Authentication : MonoBehaviour
{
    public static string UserID;

    public GameObject LoginBtn;
    public GameObject RegisterBtn;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseUser User;
    public FirebaseAuth auth;
    public DatabaseReference DBreference;

    [Header("Login")]
    public InputField emailLoginField;
    public InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    [Header("SighUp")]
    public InputField userNameSignupField;
    public InputField passwordSignupField;
    public InputField emailSignupField;
    public InputField passwordVerifyRegisterField;
    public TMP_Text warningRegisterText;

    public void Start()
    {
        Debug.Log(UserID);
        


    }

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = (DependencyStatus)task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.Log("could not resolve all Firebase dependencied: " + dependencyStatus);
            }
        });
    }

    void CheckUserId()
    {
        if(PlayerPrefs.HasKey("UserID"))
        {
            LoadUserInfo.UserId = PlayerPrefs.GetString("UserID");
            print(LoadUserInfo.UserId);
            SceneManager.LoadScene("LobbyScene");
        }
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        CheckUserId();
    }

    public void LoginButton()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    public void RegisterButton()
    {
        StartCoroutine(Register(emailSignupField.text, passwordSignupField.text, userNameSignupField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result.User;
            UserID = User.UserId;
            Debug.LogFormat(User.UserId);
            LoadUserInfo.UserId = UserID;
            PlayerPrefs.SetString("UserID", UserID);
            SceneManager.LoadScene("LobbyScene");


        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordSignupField.text != passwordVerifyRegisterField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result.User;
                UserID = User.UserId;
                PlayerPrefs.SetString("UserID", UserID);
                LoadUserInfo.UserId = UserID;
                StartCoroutine(UpdateUsernameDatabase(_username, _email));

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };



                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);



                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        StartCoroutine(UpdateUsernameDatabase(_username, _email));

                        SceneManager.LoadScene("LobbyScene");
                        //RegisterBtn.SetActive(false);


                        //LoginBtn.SetActive(true);

                        
                        warningRegisterText.text = "";
                    }
                }
            }
        }
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
        }
    }

    private IEnumerator UpdateUsernameDatabase(string _username, string _email)
    {
        User user = new User(_username, _email, 0, 0, 0);
        string json = JsonUtility.ToJson(user);

        PlayerPrefs.SetString("UserID", UserID);

        //Set the currently logged in user username in the database
        var DBTask = DBreference.Child("users").Child(UserID).SetRawJsonValueAsync(json);

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


    public void guest()
    {
        SceneManager.LoadScene("Loading");
    }

    public void signOut()
    {
        if (User != null)
        {
            auth.SignOut();
        }
    }

    public void Name()
    {
        Debug.Log(DBreference);
        //DBreference.Child("users").Child("123446").Child("username").SetValueAsync("rafi");
    }
}
