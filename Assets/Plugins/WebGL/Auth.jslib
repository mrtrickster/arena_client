// Creating functions for the Unity
mergeInto(LibraryManager.library, {

   GetUserInfo: function () {
      sendParamsToUnity();
   },
   
   CopyToClipboard: function (newClipText) {
      copyFromUnity(UTF8ToString(newClipText));
   },
   
   LogIn: function () {
	  openGoogleLogInPage();
   },
   
   LogOut: function () {
      logOut();
   },
   
   getBaseURL: function() {
         return getBaseURL();
       }
});