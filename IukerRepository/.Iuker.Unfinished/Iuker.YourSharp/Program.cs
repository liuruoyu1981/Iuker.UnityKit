using System;
using YourSharp;

namespace YourSharpTest
{
    class Program
    {
        private static string code = @"
    namespace MySharp.Common.Test
    class MyClass
    
    void MyTest(int a, int b = 100)
        int c = a + b   // this is single quote!
        int d = c++ 
    end
";

        private static string mycode = @"
    import System System.Collections Jint UnityEngine
    class public MyClass inherit MySuperClass Implement interface1 interface2
    
    staticconstructor

    instanceconstructor

    instancefield 
        int maxRows maxColumns 
        string tableName
                            
    instancefunc 
        void SetLocalDataCalssName path

    staticfunc 
        void Print message = Console.WriteLine(message)

    funcbody 
        SetLocalDataCalssName
";


        static void Main(string[] args)
        {
            GC.Collect();
            var start = DateTime.Now;
            YourSharpTest();
            var end = DateTime.Now;
            var costTime = end - start;
        }


        private static void YourSharpTest()
        {
            YourSharpEngine yourSharp = new YourSharpEngine().Init();
            yourSharp.DoString(mycode);
        }
    }
}


namespace MyNamespace
{


}