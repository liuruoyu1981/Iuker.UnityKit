//using System.Collections.Generic;
//using Haste;
//using Iuker.Common;
//using UnityEditor;
//using UnityEngine;

//public class TipWIndow : EditorWindow
//{

//    //[MenuItem("Iuker/Tip")]
//    public static void ShowWindow()
//    {
//        MyOpen();
//    }

//    private static TipWIndow GetInstance { get; set; }

//    private static Rect GetPosition()
//    {
//        Vector2 vector2 = HasteSettings.WindowPosition;
//        if (vector2 == Vector2.zero)
//            vector2 = new Vector2((float)((Screen.currentResolution.width - HasteStyles.WindowWidth) / 2), (float)((Screen.currentResolution.height - HasteStyles.WindowHeight) / 2));
//        return new Rect(vector2.x, vector2.y, (float)HasteStyles.WindowWidth, (float)HasteStyles.WindowHeight);
//    }

//    private static void MyOpen()
//    {
//        EditorApplication.LockReloadAssemblies();
//        //if (GetInstance == null)
//        //{
//        //    MyInit();
//        //}
//        //else
//        //{
//        //    GetWindow<TipWIndow>();
//        //}
//        Extensions.IfElse(GetInstance.IsNull(), MyInit, () => { GetWindow<TipWIndow>(); });
//    }

//    private static bool IsOpen => GetInstance != null;

//    private static void MyInit()
//    {
//        ++HasteSettings.UsageCount;
//        if (HasteSettings.ShowHandle)
//        {
//            if (IsOpen)
//            {
//                GetInstance.Close();
//            }
//            var rect = GetPosition();
//            TipWIndow myinstance = GetWindowWithRect<TipWIndow>(rect, true);
//            myinstance.MyInitializeInstance();
//        }
//        else
//        {
//            var tempInstance = ScriptableObject.CreateInstance<TipWIndow>();
//            tempInstance.MyInitializeInstance();
//            tempInstance.ShowPopup();
//            tempInstance.Focus();
//        }
//    }

//    //private const int RESULT_COUNT = 100;
//    //private const double loadingDelay = 0.25;
//    //private TipWIndowState windowState;
//    [SerializeField]
//    private UnityEngine.Object[] prevSelection;
//    [SerializeField]
//    private HashSet<UnityEngine.Object> nextSelection;
//    [SerializeField]
//    private HasteQuery queryInput;
//    [SerializeField]
//    private HasteEmpty empty;
//    [SerializeField]
//    private HasteLoading loading;
//    [SerializeField]
//    private HasteIntro intro;
//    [SerializeField]
//    private HasteList resultList;
//    private Rect selectionPosition;
//    private HasteUpdateStatus prevUpdateStatus;
//    private bool wasIndexing;
//    private bool wasSearching;
//    private HasteSchedulerNode searching;
//    private double searchStart;

//    private void MyInitializeInstance()
//    {
//        //this.title = "Haste";
//        position = TipWIndow.GetPosition();
//        //Vector2 vector2 = new Vector2(this.position.width, this.position.height);
//        //maxSize = vector2;
//        //minSize = vector2;
//        //selectionPosition = new Rect((float)(HasteStyles.WindowWidth - 90), 24f, 80f, 80f);
//        //if (Selection.objects != null)
//        //{
//        //    this.prevSelection = new UnityEngine.Object[Selection.objects.Length];
//        //    Array.Copy((Array)Selection.objects, (Array)this.prevSelection, Selection.objects.Length);
//        //}
//        //this.nextSelection = new HashSet<UnityEngine.Object>();
//        //this.queryInput = ScriptableObject.CreateInstance<HasteQuery>();
//        //this.queryInput.Changed += new QueryChangedHandler(this.OnQueryChanged);
//        //this.resultList = ScriptableObject.CreateInstance<HasteList>().Init(HasteStyles.ListHeight);
//        //this.resultList.ItemDrag += new HasteListEvent(this.OnItemDrag);
//        //this.resultList.ItemMouseDown += new HasteListEvent(this.OnItemHighlight);
//        //this.resultList.ItemClick += new HasteListEvent(this.OnItemSelect);
//        //this.resultList.ItemDoubleClick += new HasteListEvent(this.OnItemAction);
//        //this.RestoreRecommendations();
//        //string random = HasteTips.Random;
//        //this.intro = ScriptableObject.CreateInstance<HasteIntro>().Init(random);
//        //this.empty = ScriptableObject.CreateInstance<HasteEmpty>().Init(random);
//        //this.loading = ScriptableObject.CreateInstance<HasteLoading>();
//    }

//}
