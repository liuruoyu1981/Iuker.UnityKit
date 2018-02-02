/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/10 22:18:06
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System;
using System.Collections.Generic;
using Iuker.Common.Base.Interfaces;
using Iuker.Common.Serialize;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base.Config;
using UnityEngine;

namespace Iuker.UnityKit.Run.Base
{
    /// <summary>
    /// 默认的Protobuf序列化器
    /// </summary>
    public class DefaultU3dProtobufSerializer : ISerializer
    {
        private IU3dFrame mU3DFrame;

        /// <summary>
        /// Protobuf协议消息对象和Protobuf消息序列化器的映射关系字典
        /// </summary>
        private readonly Dictionary<string, ISerializer> mProtoMapTypeModelDictionary = new Dictionary<string, ISerializer>();

        public byte[] Serialize(object value)
        {
            var typeFullName = value.GetType().FullName;
            GetSerializerType(value, typeFullName);
            var serializer = mProtoMapTypeModelDictionary[typeFullName];
            var buffer = serializer.Serialize(value);
            return buffer;
        }

        private void GetSerializerType(object value, string typeFullName)
        {
            if (mProtoMapTypeModelDictionary.ContainsKey(typeFullName))
            {
                return;
            }

            foreach (var t in mSerializers)
            {
                ISerializer serializer;
                try
                {
                    serializer = t;
                    serializer.Serialize(value);
                }
                catch (Exception)
                {
                    continue;
                }

                if (!mProtoMapTypeModelDictionary.ContainsKey(typeFullName))
                {
                    mProtoMapTypeModelDictionary.Add(typeFullName, serializer);
                }
            }
        }

        private void GetDeSerializerType<T>(byte[] value, string typeFullName) where T : class, new()
        {
            if (mProtoMapTypeModelDictionary.ContainsKey(typeFullName))
            {
                return;
            }

            foreach (var t in mSerializers)
            {
                ISerializer serializer;
                try
                {
                    serializer = t;
                    serializer.DeSerialize<T>(value);
                }
                catch (Exception)
                {
                    continue;
                }

                if (!mProtoMapTypeModelDictionary.ContainsKey(typeFullName))
                {
                    mProtoMapTypeModelDictionary.Add(typeFullName, serializer);
                }
            }
        }

        public T DeSerialize<T>(byte[] buffer) where T : class, new()
        {
            if (buffer == null) return default(T);
            var typeFullName = typeof(T).FullName;

            GetDeSerializerType<T>(buffer, typeFullName);
            if (!mProtoMapTypeModelDictionary.ContainsKey(typeFullName))
            {
                throw new Exception(string.Format("不存在目标类型{0}的序列化器！", typeFullName));
            }

            var serializer = mProtoMapTypeModelDictionary[typeFullName];
            var message = serializer.DeSerialize<T>(buffer);
            return message;
        }

        public ISerializer Init(IFrame frame)
        {
            mU3DFrame = (IU3dFrame)frame;
            InitAllSonProjectSerializer();
            return this;
        }


        private void InitAllSonProjectSerializer()
        {
            //  手动添加默认的Protobuf序列化器，用于序列化基础Protobuf消息
            mSerializers.Add(new IukBaseProtobufSerializer_Proxy());
            var allTypes = ReflectionUitlity.GetAllType(mU3DFrame.ProjectAssemblys);
            Bootstrap.Instance.GetCombinProject().ForEach(p => InitSonProjectSerializer(p, allTypes));
        }


        private void InitSonProjectSerializer(Project project, List<Type> allTypes)
        {
            Debug.Log(project.ProjectName);
            var sons = project.AllSonProjects;
            foreach (var sonProject in sons)
            {
                var typename = string.Format("{0}_{1}_ProtobufSerializer_Proxy", project.ProjectName,
                    sonProject.ProjectName);
                var targetType = allTypes.Find(t => t.Name == typename);
                if (targetType == null)
                {
#if UNITY_EDITOR
                    Debug.LogWarning(string.Format("子项目{0}的Protobuf序列化器没有找到！", sonProject.ProjectName));
#endif
                    continue;
                }
                var instance = Activator.CreateInstance(targetType) as ISerializer;

                mSerializers.Add(instance);
            }
        }

        private readonly List<ISerializer> mSerializers = new List<ISerializer>();
    }
}