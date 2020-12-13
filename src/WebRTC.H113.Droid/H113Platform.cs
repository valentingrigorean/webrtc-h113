
using System;
using Android.App;
using Android.OS;
using WebRTC.Droid;
using WebRTC.H113.Schedulers;

namespace WebRTC.H113.Droid
{
    public static class H113Platform
    {
        private static bool _wasInit;
        
        public static void Init(Activity activity)
        {
            if (_wasInit)
                return;
            _wasInit = true;
            Platform.Init(activity);
            ExecutorServiceFactory.Factory = tag => new ExecutorServiceImpl(tag);
            ExecutorServiceFactory.MainExecutor = new MainExecutor();
        }


        public static void Cleanup()
        {
            
        }
        
        internal static bool HasApiLevelO =>
#if __ANDROID_26__
            HasApiLevel(BuildVersionCodes.O);
#else
            false;
#endif
        
        static int? _sdkInt;

        internal static int SdkInt
            => _sdkInt ??= (int)Build.VERSION.SdkInt;

        internal static bool HasApiLevel(BuildVersionCodes versionCode) =>
            SdkInt >= (int)versionCode;

        private class MainExecutor : IExecutor
        {
            private readonly Handler _handler = new Handler(Looper.MainLooper);
            public bool IsCurrentExecutor => Looper.MainLooper == Looper.MyLooper();
            public void Execute(Action action) => _handler.Post(action);
        }

        private class ExecutorServiceImpl : IExecutorService
        {

            private readonly HandlerThread _handlerThread;
            private readonly Handler _handler;

            public ExecutorServiceImpl(string tag)
            {
                _handlerThread = new HandlerThread(tag);
                _handlerThread.Start();
                _handler = new Handler(_handlerThread.Looper!);
            }


            public bool IsCurrentExecutor => _handlerThread.Looper == Looper.MyLooper();
            public void Execute(Action action) => _handler.Post(action);
        
            public void Release()
            {
                _handlerThread.QuitSafely();
            }
        }
    }
}