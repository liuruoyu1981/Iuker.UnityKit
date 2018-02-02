using UnityEngine;
using System.Collections.Generic;
using Iuker.Common;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine.UI;

public class TestMain : MonoBehaviour
{
    private readonly List<object> listItem = new List<object>();
    private IListView mListView;

    void Start()
    {
        //测试数据
        for (var i = 0; i < 50; i++)
        {
            var items = new Item("测试:" + Random.Range(1, 1000));
            listItem.Add(items);
        }

        //scrollView 相关所需注意接口
        mListView = gameObject.transform.GetComponentInChildren<IListView>();
        //warpContent.UpdateItem = onInitializeItem;
        //注意：目标init方法必须在warpContent.onInitializeItem之后
        mListView.SetItemTemplate(listItem, UpdateItem, "testmain");
    }

    private void UpdateItem(GameObject go, int dataIndex)
    {
        var text = go.transform.Find("Text").GetComponent<Text>();
        text.text = "i:" + dataIndex + "_N:" + listItem[dataIndex].As<Item>().Name();

        //add按钮监听【添加功能】
        Button addbutton = go.transform.Find("Add").GetComponent<Button>();
        addbutton.onClick.RemoveAllListeners();
        addbutton.onClick.AddListener(delegate
        {
            listItem.Insert(dataIndex + 1, new Item("Insert" + Random.Range(1, 1000)));
            mListView.AddItem(dataIndex + 1);
        });

        //sub按钮监听【删除功能】
        Button subButton = go.transform.Find("Sub").GetComponent<Button>();
        subButton.onClick.RemoveAllListeners();
        subButton.onClick.AddListener(delegate ()
        {
            listItem.RemoveAt(dataIndex);
            mListView.DeleteItem(dataIndex);
        });

    }



    //测试数据结构
    private class Item
    {
        private string name;
        public Item(string name)
        {
            this.name = name;
        }
        public string Name()
        {
            return name;
        }
        public void destroy()
        {
            name = null;
        }

    }
}
