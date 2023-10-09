var MyPlugin = {
   IsMobile: function()
   {
alert('123');
      return UnityLoader.SystemInfo.mobile;
   }
};  
mergeInto(LibraryManager.library, MyPlugin);