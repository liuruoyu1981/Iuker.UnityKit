//Created by Poq Xert (poqxert@gmail.com)

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Enum = System.Enum;

namespace GUIBuilder
{
	enum SnapType
	{
		Default = 0,
		Top,
		TopRight,
		Left,
		Center,
		Right,
		BottomLeft,
		Bottom,
		BottomRight
	}
	
	public class GUIBuilderX : EditorWindow
	{
		private int _namesPopup = 0;											//ID in elements list
		private string _className = "MyScript";									//Class name
		private int _group = 0;													//ID group of panel
		private Rect _rectElement;												//Position and size of element
		private SnapType _snapTypeElement = SnapType.Default;					//Snap type of element
		private int _type = 0;													//ID group of property
		private string _element = "";											//Type of element
		private string _nameElement = "Element_01";								//Name of element
		private int _maxLeight = 10;											//Max number of characters in field
		private string _maskChar = "*";											//Password character
		private float _sliderValue = 0;											//Value of slider
		private float _minValue = 0;											//Min value of slider
		private float _maxValue = 5;											//Max value of slider
		private int _idWin = 0;													//Window ID
		private bool _dragWin = false;											//Window is drag?
	 
		#region Content
		private string _text = "";												//Text of element
		private Texture2D _texture = null;										//Image of element
		private string _tooltip = "";											//Tooltip of element
		#endregion
		#region lists
		private List<string> _typesElements = new List<string>();
		private List<string> _namesElements = new List<string>();
		private List<GUIContent> _contentElements = new List<GUIContent>();
		List<SnapType> _snapTypeElements = new List<SnapType>();
		private List<Rect> _rectElements = new List<Rect>();
		private List<int> _maxLeights = new List<int>();
		private List<string> _maskChars = new List<string>();
		private List<float> _slidersValues = new List<float>();
		private List<float> _minValues = new List<float>();
		private List<float> _maxValues = new List<float>();
		private List<int> _idWindows = new List<int>();
		private List<bool> _typeWin = new List<bool>();
		#endregion
		private int _scriptType = 0;
		private bool _createElement = false;
		private GUISkin _guiSkin;
			
		//Меню для открытия	
		[MenuItem("Window/GUI Builder")]
		static void CreateWindow(){
			GUIBuilderX window = GetWindow<GUIBuilderX>("GUI Builder");
			Object.DontDestroyOnLoad(window);
			window.AutoLoad();
		}
		
		bool _mousePress = false;
		Vector2 _startPos;
		void OnGUI(){
			VisibleGUI();
			if(_element != null && _createElement){
				Event e = Event.current;
				if(!_mousePress)
				{
					_rectElement.x = e.mousePosition.x;
					_rectElement.y = e.mousePosition.y;
					if(e.type == EventType.MouseDown && e.isMouse && e.button == 0)
					{
						_mousePress = true;
						_startPos = new Vector2(_rectElement.x, _rectElement.y);
					}
				}
				//Иначе...
				else
				{
					if(_startPos.x < e.mousePosition.y)
					{
						_rectElement.x = _startPos.x;
						_rectElement.width = e.mousePosition.x - _startPos.x;
					}
					else
					{
						_rectElement.x = e.mousePosition.x;
						_rectElement.width = _startPos.x - e.mousePosition.x;
					}
					if(_startPos.y < e.mousePosition.y)
					{
						_rectElement.y = _startPos.y;
						_rectElement.height = e.mousePosition.y - _startPos.y;
					}
					else
					{
						_rectElement.y = e.mousePosition.y;
						_rectElement.height =  _startPos.y - e.mousePosition.y;
					}
					if(e.type == EventType.MouseUp && e.isMouse && e.button == 0)
					{
						_mousePress = false;
						AddElement();
					}
				}
				if(e.button == 1 && e.isMouse)
					_createElement = false;
				_SwitchElement();
				Repaint();
			}
			_group = GUI.Toolbar(new Rect(Screen.width - 210, 10, 200, 30), _group, new string[]{"Options", "Elements"});
			EditorGUI.BeginChangeCheck();
			switch(_group){
				case 0:
					ShowSettings();
					break;
				case 1:
					ShowElementsList();
					break;
			}
		}

		private void ShowSettings()
		{
			_snapTypeElement = (SnapType)EditorGUI.EnumPopup(new Rect(Screen.width - 110, 45, 100, 15), _snapTypeElement);
			_rectElement = EditorGUI.RectField(new Rect(Screen.width - 210, 45, 200, 30),"Rect", _rectElement);
			if(_rectElement.width < 3) _rectElement.width = 3;
			if(_rectElement.height < 3) _rectElement.height = 3;
			EditorGUI.LabelField(new Rect(Screen.width - 210, 105, 100, 15), "Name");
			_nameElement = EditorGUI.TextField(new Rect(Screen.width - 110, 105, 100, 15), _nameElement);
			EditorGUI.LabelField(new Rect(Screen.width - 210, 130, 100, 15), "Window ID");
			string str = _idWin.ToString();
			str = EditorGUI.TextField(new Rect(Screen.width - 110, 130, 100, 15), str);
			_idWin = (str != "") ? int.Parse(str) : 0;
			switch(_type){
			case 0:
				EditorGUI.LabelField(new Rect(Screen.width - 210, 150, 100, 15), "Text");
				_text = EditorGUI.TextField(new Rect(Screen.width - 110, 150, 100, 15), _text);
				EditorGUI.LabelField(new Rect(Screen.width - 210, 170, 100, 15), "Tooltip");
				_tooltip = EditorGUI.TextField(new Rect(Screen.width - 110, 170, 100, 15), _tooltip);
				if(_element == "Window"){
					EditorGUI.LabelField(new Rect(Screen.width - 210, 190, 100, 15), "Drag Window");
					_dragWin = EditorGUI.Toggle(new Rect(Screen.width - 110, 190, 100, 15), _dragWin);
				}
				if(_element == "Toggle"){
					EditorGUI.LabelField(new Rect(Screen.width - 210, 190, 100, 15), "Check");
					_dragWin = EditorGUI.Toggle(new Rect(Screen.width - 110, 190, 100, 15), _dragWin);
				}
				EditorGUI.LabelField(new Rect(Screen.width - 210, 210, 100, 15), "Texture");
				_texture = EditorGUI.ObjectField(new Rect(Screen.width - 110, 210, 100, 100), _texture, typeof(Texture2D), false) as Texture2D;
				
				break;
			case 1:
				EditorGUI.LabelField(new Rect(Screen.width - 210, 150, 100, 15), "Text");
				_text = EditorGUI.TextField(new Rect(Screen.width - 110, 150, 100, 15), _text);
				EditorGUI.LabelField(new Rect(Screen.width - 210, 170, 100, 15), "Max Leight");
				_maxLeight = EditorGUI.IntField(new Rect(Screen.width - 110, 170, 100, 15), _maxLeight);
				EditorGUI.LabelField(new Rect(Screen.width - 210, 195, 100, 15), "Mask Char");
				_maskChar = EditorGUI.TextField(new Rect(Screen.width - 110, 195, 100, 15), _maskChar);
				break;
			case 2:
				EditorGUI.LabelField(new Rect(Screen.width - 210, 150, 100, 15), "Value");
				_sliderValue = EditorGUI.FloatField(new Rect(Screen.width - 110, 150, 100, 15), _sliderValue);
				EditorGUI.LabelField(new Rect(Screen.width - 210, 170, 100, 15), "Min Value");
				_minValue = EditorGUI.FloatField(new Rect(Screen.width - 110, 170, 100, 15), _minValue);
				EditorGUI.LabelField(new Rect(Screen.width - 210, 190, 100, 15), "Max Value");
				_maxValue = EditorGUI.FloatField(new Rect(Screen.width - 110, 190, 100, 15), _maxValue);
				break;
			case 3:
				EditorGUI.LabelField(new Rect(Screen.width - 210, 150, 100, 15), "Texture");
				_texture = EditorGUI.ObjectField(new Rect(Screen.width - 110, 150, 100, 100), _texture, typeof(Texture2D), false) as Texture2D;
				break;
			}
			if(EditorGUI.EndChangeCheck()){
				Undo.RegisterUndo(GetWindow<GUIBuilderX>(), "Edit Element - GUI Builder");
				EditElement();
			}
			if(GUI.Button(new Rect(Screen.width - 140, 320, 90, 20), "Add") && _element != "")
				_createElement = true;
			EditorGUI.BeginChangeCheck();
			_namesPopup = EditorGUI.Popup(new Rect(Screen.width - 210, 345, 200, 15), "Element", _namesPopup, _namesElements.ToArray());
			if(EditorGUI.EndChangeCheck()) SelectElem();
			EditorGUI.LabelField(new Rect(Screen.width - 210, 395, 100, 15), "GUI Skin");
			_guiSkin = EditorGUI.ObjectField(new Rect(Screen.width - 110, 395, 100, 15), _guiSkin, typeof(GUISkin), false) as GUISkin;
			if(GUI.Button(new Rect(Screen.width - 140, 370, 90, 20), "Delete") && _namesElements.Count != 0){
				DelElement();
			}
		}

		private void ShowElementsList()
		{
			if(GUI.Button(new Rect(Screen.width - 210, 50, 90, 20), "Button")){
				NewElement("Button", 0,	0);
			}
			if(GUI.Button(new Rect(Screen.width - 110, 50, 90, 20), "Label")){
				NewElement("Label", 0, 0);
			}
			if(GUI.Button(new Rect(Screen.width - 210, 75, 90, 20), "PasswordField")){
				NewElement("PasswordField", 1, 0);
			}
			if(GUI.Button(new Rect(Screen.width - 110, 75, 90, 20), "TextField")){
				NewElement("TextField", 1, 0);
			}
			if(GUI.Button(new Rect(Screen.width - 210, 100, 90, 20), "Box")){
				NewElement("Box", 0, 0);
			}
			if(GUI.Button(new Rect(Screen.width - 110, 100, 90, 20), "RepeatButton")){
				NewElement("RepeatButton", 0, 0);
			}
			if(GUI.Button(new Rect(Screen.width - 210, 125, 90, 20), "TextArea")){
				NewElement("TextArea", 1, 0);
			}
			if(GUI.Button(new Rect(Screen.width - 210, 150, 90, 20), "HorizontalSlider")){
				NewElement("HorizontalSlider", 2, 0);
			}
			if(GUI.Button(new Rect(Screen.width - 110, 125, 90, 20), "VerticalSlider")){
				NewElement("VerticalSlider", 2, 0);
			}
			if(GUI.Button(new Rect(Screen.width - 110, 150, 90, 20), "DrawTexture")){
				NewElement("DrawTexture", 3, 0);
			}
			if(GUI.Button(new Rect(Screen.width - 210, 175, 90, 20), "Window")){
				NewElement("Window", 0, 0);
			}
			if(GUI.Button(new Rect(Screen.width - 110, 175, 90, 20), "Toggle")){
				NewElement("Toggle", 0, 0);
			}
			//if(GUI.Button(new Rect(Screen.width - 210, 150, 90, 20), "Toolbar")){
			//	_element = "Toolbar";
			//}
			//if(GUI.Button(new Rect(Screen.width - 110, 150, 90, 20), "SelectionGrid")){
			//	_element = "SelectionGrid";
			//}
			//if(GUI.Button(new Rect(Screen.width - 210, 200, 90, 20), "_group")){
			//	_element = "_group";
			//}
			//if(GUI.Button(new Rect(Screen.width - 110, 200, 90, 20), "ScrollView")){
			//	_element = "ScrollView";
			//}
			EditorGUI.LabelField(new Rect(Screen.width - 210, 250, 100, 15), "Class Name");                                                                                                                                                                                                   
			_className = EditorGUI.TextField(new Rect(Screen.width - 110, 250, 100, 15), _className);
			EditorGUI.LabelField(new Rect(Screen.width - 210, 275, 100, 15), "Script Type");   
			_scriptType = EditorGUI.Popup( new Rect(Screen.width - 110, 275, 100, 15), _scriptType, new string[]{"Original (GBX)", "C Sharp (C#)", " Java Script (JS)"});
			if(GUI.Button(new Rect(Screen.width - 140, 300, 90, 20), "Save Script")){
				if(_scriptType == 0)
					SaveOriginal(false);
				else if(_scriptType == 1)
					SaveScriptSharp();
				else
					SaveScriptJS();
			}
			if(GUI.Button(new Rect(Screen.width - 140, 325, 90, 20), "Load")){
				LoadOriginal(false);
			}
			if(GUI.Button(new Rect(Screen.width - 140, 350, 90, 20), "Clear")){
				ClearView();
			}
			if(GUI.Button(new Rect(Screen.width - 140, 375, 90, 20), "Donate"))
			{
				Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=GPPJYJ7GTE4LG");
			}
		}
		private void NewElement(string elemName, int elemType, int elem_group)
		{
			_element = elemName;
			_type = elemType;
			_group = elem_group;
			AddEmptyElement();
			
		}
		GUISkin _defSkin;
		private void VisibleGUI(){
			if(_guiSkin != null)
			{
				_defSkin = GUI.skin;
				GUI.skin = _guiSkin;
			}
			for(int i = 0; i < _namesElements.Count; i++){
				if(_typesElements[i] != "Window" && _idWindows[i] != 0) continue;
				switch(_typesElements[i]){
				case "Button":
						GUI.Button(GetRectWithSnap(_rectElements[i], _snapTypeElements[i]), _contentElements[i]);
					break;
				case "Label":
						GUI.Label(GetRectWithSnap(_rectElements[i], _snapTypeElements[i]), _contentElements[i]);
					break;
				case "PasswordField":
						GUI.PasswordField(GetRectWithSnap(_rectElements[i], _snapTypeElements[i]), _contentElements[i].text, _maskChars[i].ToCharArray(0, 1)[0], _maxLeights[i]);
					break;
				case "TextField":
						GUI.TextField(GetRectWithSnap(_rectElements[i], _snapTypeElements[i]), _contentElements[i].text, _maxLeights[i]);
					break;
				case "Box":
						GUI.Box(GetRectWithSnap(_rectElements[i], _snapTypeElements[i]), _contentElements[i]);
					break;
				case "RepeatButton":
						GUI.RepeatButton(GetRectWithSnap(_rectElements[i], _snapTypeElements[i]), _contentElements[i]);
					break;
				case "TextArea":
						GUI.TextArea(GetRectWithSnap(_rectElements[i], _snapTypeElements[i]), _contentElements[i].text, _maxLeights[i]);
					break;
				case "HorizontalSlider":
					GUI.HorizontalSlider(GetRectWithSnap(_rectElements[i], _snapTypeElements[i]), _slidersValues[i], _minValues[i], _maxValues[i]);
					break;
				case "VerticalSlider":
					GUI.VerticalSlider(GetRectWithSnap(_rectElements[i], _snapTypeElements[i]), _slidersValues[i], _minValues[i], _maxValues[i]);
					break;
				case "DrawTexture":
						GUI.DrawTexture(GetRectWithSnap(_rectElements[i], _snapTypeElements[i]), _contentElements[i].image);
					break;
				case "Toggle":
					GUI.Toggle(GetRectWithSnap(_rectElements[i], _snapTypeElements[i]), _typeWin[i], _contentElements[i]);
				break;
				case "Window":
						BeginWindows();
						GUI.Window(_idWindows[i], GetRectWithSnap(_rectElements[i], _snapTypeElements[i]), WinF, _contentElements[i]);
						EndWindows();
					break;
				}
			}
			if(_guiSkin != null)
			{
				GUI.skin = _defSkin;
			}
		}
		
		private void VisibleGUIWindow(int winid){
			for(int i = 0; i < _namesElements.Count; i++){
				if( winid != _idWindows[i]) continue;
				switch(_typesElements[i]){
				case "Button":
						GUI.Button(_rectElements[i], _contentElements[i]);
					break;
				case "Label":
						GUI.Label(_rectElements[i], _contentElements[i]);
					break;
				case "PasswordField":
						GUI.PasswordField(_rectElements[i], _contentElements[i].text, _maskChars[i].ToCharArray(0, 1)[0], _maxLeights[i]);
					break;
				case "TextField":
						GUI.TextField(_rectElements[i], _contentElements[i].text, _maxLeights[i]);
					break;
				case "Box":
						GUI.Box(_rectElements[i], _contentElements[i]);
					break;
				case "RepeatButton":
						GUI.RepeatButton(_rectElements[i], _contentElements[i]);
					break;
				case "TextArea":
						GUI.TextArea(_rectElements[i], _contentElements[i].text, _maxLeights[i]);
					break;
				case "HorizontalSlider":
					GUI.HorizontalSlider(_rectElements[i], _slidersValues[i], _minValues[i], _maxValues[i]);
					break;
				case "VerticalSlider":
					GUI.VerticalSlider(_rectElements[i], _slidersValues[i], _minValues[i], _maxValues[i]);
					break;
				case "DrawTexture":
						GUI.DrawTexture(_rectElements[i], _contentElements[i].image);
					break;
				case "Toggle":
					GUI.Toggle(GetRectWithSnap(_rectElements[i], _snapTypeElements[i]), _typeWin[i], _contentElements[i]);
				break;
				}
			}
		}
			
		void OnDestroy(){
			if(_namesElements.Count != 0)
				SaveOriginal(!EditorUtility.DisplayDialog("Save", "Do you want to save GUI?", "Save", "Cancel"));	
		}
		private void AddEmptyElement()
		{
			int index = _typesElements.IndexOf("----");
			_nameElement = "New_Element_"+_namesElements.Count;
			_idWin = 0;
			if(index < 0)
			{
				_typesElements.Add("----");
				_namesElements.Add("----");
				_contentElements.Add(new GUIContent(_text, _texture));
				_snapTypeElements.Add(_snapTypeElement);
				_rectElements.Add(_rectElement);
				_maxLeights.Add(_maxLeight);
				_maskChars.Add(_maskChar);
				_slidersValues.Add(_sliderValue);
				_minValues.Add(_minValue);
				_maxValues.Add(_maxValue);
				_namesPopup = _namesElements.Count - 1;
				_idWindows.Add(_idWin);
				_typeWin.Add(_dragWin);
			}
			else
			{
				_typesElements[index] = "----";
				_namesElements[index] = "----";
				_contentElements[index] = new GUIContent(_text, _texture);
				_snapTypeElements[index] = _snapTypeElement;
				_rectElements[index] = _rectElement;
				_maxLeights[index] = _maxLeight;
				_maskChars[index] = _maskChar;
				_slidersValues[index] = _sliderValue;
				_minValues[index] = _minValue;
				_maxValues[index] = _maxValue;
				_namesPopup = index;
				_idWindows[index] = _idWin;
				_typeWin[index] = _dragWin;
			}
		}
		private void AddElement(){
			_createElement = false;
			Undo.RegisterUndo(GetWindow<GUIBuilderX>(), "Add Element - GUI Builder");
			_typesElements.Add(_element);
			_namesElements.Add(_nameElement);
			_contentElements.Add(new GUIContent(_text, _texture));
			_snapTypeElements.Add(_snapTypeElement);
			_rectElements.Add(_rectElement);
			_maxLeights.Add(_maxLeight);
			_maskChars.Add(_maskChar);
			_slidersValues.Add(_sliderValue);
			_minValues.Add(_minValue);
			_maxValues.Add(_maxValue);
			_idWindows.Add(_idWin);
			_typeWin.Add(_dragWin);
			DelElement();
			_namesPopup = _namesElements.Count - 1;
		}
		private void _SwitchElement(){
				switch(_element){
				case "Button":
						GUI.Button(_rectElement, new GUIContent(_text, _texture));
					break;
				case "Label":
						GUI.Label(_rectElement, new GUIContent(_text, _texture));
					break;
				case "PasswordField":
						GUI.PasswordField(_rectElement, _text, _maskChar.ToCharArray(0, 1)[0]);
					break;
				case "TextField":
						GUI.TextField(_rectElement, _text, _maxLeight);
					break;
				case "Box":
						GUI.Box(_rectElement, new GUIContent(_text, _texture));
					break;
				case "RepeatButton":
						GUI.RepeatButton(_rectElement, new GUIContent(_text, _texture));
					break;
				case "TextArea":
						GUI.TextArea(_rectElement, _text, _maxLeight);
					break;
				case "HorizontalSlider":
						GUI.HorizontalSlider(_rectElement, _sliderValue, _minValue, _maxValue);
					break;
				case "VerticalSlider":
						GUI.VerticalSlider(_rectElement, _sliderValue, _minValue, _maxValue);
					break;
				case "DrawTexture":
					if(_texture  == null) _texture = new Texture2D(64, 64);
						GUI.DrawTexture(_rectElement, _texture);
					break;
				case "Window":
						BeginWindows();
						GUI.Window(_idWin, _rectElement, WinF, new GUIContent(_text, _texture));
						EndWindows();
					break;
			}
		}
	
		private Rect GetRectWithSnap(Rect rectElem, SnapType snap)
		{
			Rect newRectElem = rectElem;
			switch(snap)
			{
				case SnapType.Default:
					return newRectElem;
				case SnapType.Top:
					newRectElem.x += (Screen.width - 210 - rectElem.width)/2;
					break;
				case SnapType.TopRight:
					newRectElem.x += Screen.width - 210 - rectElem.width;
					break;
				case SnapType.Left:
					newRectElem.y += (Screen.height - rectElem.height)/2;
					break;
				case SnapType.Center:
					newRectElem.x += (Screen.width - 210 - rectElem.width)/2;
					newRectElem.y += (Screen.height - rectElem.height)/2;
					break;
				case SnapType.Right:
					newRectElem.x += Screen.width - 210 - rectElem.width;
					newRectElem.y += (Screen.height - rectElem.height)/2;
					break;
				case SnapType.BottomLeft:
					newRectElem.y += Screen.height - rectElem.height;
					break;
				case SnapType.Bottom:
					newRectElem.x += (Screen.width - 210 - rectElem.width)/2;
					newRectElem.y += Screen.height - rectElem.height;
					break;
				case SnapType.BottomRight:
					newRectElem.x += Screen.width - 210 - rectElem.width;
					newRectElem.y += Screen.height - rectElem.height;
					break;
			}
			return newRectElem;
		}
		
		private string GetStringRectWithSnap(Rect rectElem, SnapType snap)
		{
			string resRect = "";
			switch(snap)
			{
				case SnapType.Default:
					resRect = rectElem.x + "f, " + rectElem.y + "f, " + rectElem.width + "f, " + rectElem.height + "f";
					break;
				case SnapType.Top:
					resRect = "(Screen.width - " + rectElem.width + ")/2 + " + rectElem.x + "f, " + rectElem.y + "f, " + rectElem.width + "f, " + rectElem.height + "f";
					break;
				case SnapType.TopRight:
					resRect = "Screen.width - " + rectElem.width + " + " + rectElem.x + "f, " + rectElem.y + "f, " + rectElem.width + "f, " + rectElem.height + "f";
					break;
				case SnapType.Left:
					resRect = rectElem.x + "f, (Screen.height - " + rectElem.height + ")/2 + " + rectElem.y + "f, " + rectElem.width + "f, " + rectElem.height + "f";
					break;
				case SnapType.Center:
					resRect = "(Screen.width - " + rectElem.width + ")/2 + " + rectElem.x + "f, (Screen.height - " + rectElem.height + ")/2 + " + rectElem.y + "f, " + rectElem.width + "f, " + rectElem.height + "f";
					break;
				case SnapType.Right:
					resRect = "Screen.width - " + rectElem.width + " + " + rectElem.x + "f, (Screen.height - " + rectElem.height + ")/2 + " + rectElem.y + "f, " + rectElem.width + "f, " + rectElem.height + "f";
					break;
				case SnapType.BottomLeft:
					resRect = rectElem.x + "f, Screen.height - " + rectElem.height + " + " + rectElem.y + "f, " + rectElem.width + "f, " + rectElem.height + "f";
					break;
				case SnapType.Bottom:
					resRect = "(Screen.width - " + rectElem.width + ")/2 + " + rectElem.x + "f, Screen.height - " + rectElem.height + " + " + rectElem.y + "f, " + rectElem.width + "f, " + rectElem.height + "f";
					break;
				case SnapType.BottomRight:
					resRect = "Screen.width - " + rectElem.width + " + " + rectElem.x + "f, Screen.height - " + rectElem.height + " + " + rectElem.y + "f, " + rectElem.width + "f, " + rectElem.height + "f";
					break;
			}
			return resRect;
		}
		
		private string GetStringRectWithSnapJS(Rect rectElem, SnapType snap)
		{
			return GetStringRectWithSnap(rectElem, snap).Replace("f", "");
		}

		private void SaveScriptSharp(){
			if(_className != ""){
				string scriptText = "";
				string scriptPublicVar = "";
				string scriptPrivateVar = "";
				string scriptElem = "";
				string nameVar = "";
				string[] scriptWindow = new string[MaxValueInList(_idWindows.ToArray()) + 1];
				for(int i = 0; i < scriptWindow.Length; i++)
						scriptWindow[i] = "";
				bool winB = false;
				
					
				if(_guiSkin != null){
					scriptPublicVar += "	public GUISkin SkinGUI;\n";
					scriptElem += "		if(SkinGUI != null)\n			GUI.skin = SkinGUI;\n";
				}
				for(int i = 0; i < _namesElements.Count; i++){
					if(_typesElements[i] != "Window" && _idWindows[i] != 0) continue;
					switch(_typesElements[i]){
					case "Button":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	public Texture2D " + nameVar + ";\n";
							}
							else
								nameVar = "null";
							scriptElem += "		if(GUI.Button( new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"))){}\n";
						break;
					case "Label":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	public Texture2D " + nameVar + ";\n";
							}
							else
								nameVar = "null";
							scriptElem += "		GUI.Label( new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
						break;
					case "PasswordField":
							scriptPrivateVar += "	private string " + _namesElements[i] + " = \"" + _contentElements[i].text + "\";\n";
							scriptElem += "		" + _namesElements[i] + " = GUI.PasswordField(new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", '" + _maskChars[i].ToCharArray(0, 1)[0] + "', " + _maxLeights[i] + ");\n";
						break;
					case "TextField":
							scriptPrivateVar += "	private string " + _namesElements[i] + " = \"" + _contentElements[i].text + "\";\n";
							scriptElem += "		" + _namesElements[i] + " = GUI.TextField(new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", " + _maxLeights[i] + ");\n";
						break;
					case "Box":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	public Texture2D " + nameVar + ";\n";
							}
							else
								nameVar = "null";
							scriptElem += "		GUI.Box( new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
						break;
					case "RepeatButton":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	public Texture2D " + nameVar + ";\n";
							}
							else
								nameVar = "null";
							scriptElem += "		if(GUI.RepeatButton( new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"))){}\n";
						break;
					case "TextArea":
							scriptPrivateVar += "	private string " + _namesElements[i] + " = \"" + _contentElements[i].text + "\";\n";
							scriptElem += "		" + _namesElements[i] + " = GUI.TextArea(new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", " + _maxLeights[i] + ");\n";
						break;
					case "HorizontalSlider":
							scriptPrivateVar += "	private float " + _namesElements[i] + " = " + _slidersValues[i] + "f;\n";
							scriptElem += "		" + _namesElements[i] + " = GUI.HorizontalSlider(new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", " + _minValues[i] + "f, " + _maxValues[i] + "f);\n";
						break;
						case "VerticalSlider":
							scriptPrivateVar += "	private float " + _namesElements[i] + " = " + _slidersValues[i] + "f;\n";
							scriptElem += "		" + _namesElements[i] + " = GUI.VerticalSlider(new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", " + _minValues[i] + "f, " + _maxValues[i] + "f);\n";
						break;
					case "DrawTexture":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	public Texture2D " + nameVar + ";\n";
							}
							else
								nameVar = "new Texture2D(64, 64)";
							scriptElem += "		GUI.DrawTexture( new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + nameVar + ");\n";
						break;
					case "Toggle":
							scriptPrivateVar += "	private bool " + _namesElements[i] + " = " + _typeWin[i].ToString().ToLower() + ";\n";
							scriptElem += "		" + _namesElements[i] + " = GUI.Toggle(new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
						break;
					case "Window":
							winB = true;
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	public Texture2D " + nameVar + ";\n";
							}
							else
								nameVar = "null";
							scriptElem += "		";
							scriptPrivateVar += "	private Rect " + _namesElements[i] + "_rect = new Rect(" + _rectElements[i].x + "f, " + _rectElements[i].y + "f, " +
								+ _rectElements[i].width + "f, " + _rectElements[i].height + "f);\n";
							scriptElem += _namesElements[i] + "_rect = ";
							scriptElem += "	GUI.Window( " + _idWindows[i] + ", " + _namesElements[i] + "_rect, WindowFunc, new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
									
						break;
					}
				}
				
				bool addDrag = false;
					
				for(int i = 0; i < _namesElements.Count; i++){
					if(_typeWin[i] && !addDrag && _typesElements[i] == "Window" && _idWindows[i] != 0){ scriptWindow[_idWindows[i]] += "\n			GUI.DragWindow();\n"; addDrag = true;}
					if(_typesElements[i] == "Window" || _idWindows[i] == 0) continue;
					switch(_typesElements[i]){
					case "Button":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	public Texture2D " + nameVar + ";\n";
							}
							else
								nameVar = "null";
							scriptWindow[_idWindows[i]] += "			if(GUI.Button( new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"))){}\n";
						break;
					case "Label":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	public Texture2D " + nameVar + ";\n";
							}
							else
								nameVar = "null";
							scriptWindow[_idWindows[i]] += "			GUI.Label( new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
						break;
					case "PasswordField":
							scriptPrivateVar += "	private string " + _namesElements[i] + " = \"" + _contentElements[i].text + "\";\n";
							scriptWindow[_idWindows[i]] += "			" + _namesElements[i] + " = GUI.PasswordField(new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", '" + _maskChars[i].ToCharArray(0, 1)[0] + "', " + _maxLeights[i] + ");\n";
						break;
					case "TextField":
							scriptPrivateVar += "	private string " + _namesElements[i] + " = \"" + _contentElements[i].text + "\";\n";
							scriptWindow[_idWindows[i]] += "			" + _namesElements[i] + " = GUI.TextField(new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", " + _maxLeights[i] + ");\n";
						break;
					case "Box":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	public Texture2D " + nameVar + ";\n";
							}
							else
								nameVar = "null";
							scriptWindow[_idWindows[i]] += "			GUI.Box( new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
						break;
					case "RepeatButton":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	public Texture2D " + nameVar + ";\n";
							}
							else
								nameVar = "null";
							scriptWindow[_idWindows[i]] += "			if(GUI.RepeatButton( new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"))){}\n";
						break;
					case "TextArea":
							scriptPrivateVar += "	private string " + _namesElements[i] + " = \"" + _contentElements[i].text + "\";\n";
							scriptWindow[_idWindows[i]] += "			" + _namesElements[i] + " = GUI.TextArea(new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", " + _maxLeights[i] + ");\n";
						break;
					case "HorizontalSlider":
							scriptPrivateVar += "	private float " + _namesElements[i] + " = " + _slidersValues[i] + "f;\n";
							scriptWindow[_idWindows[i]] += "			" + _namesElements[i] + " = GUI.HorizontalSlider(new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", " + _minValues[i] + "f, " + _maxValues[i] + "f);\n";
						break;
					case "VerticalSlider":
							scriptPrivateVar += "	private float " + _namesElements[i] + " = " + _slidersValues[i] + "f;\n";
							scriptWindow[_idWindows[i]] += "			" + _namesElements[i] + " = GUI.VerticalSlider(new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", " + _minValues[i] + "f, " + _maxValues[i] + "f);\n";
						break;
					case "DrawTexture":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	public Texture2D " + nameVar + ";\n";
							}
							else
								nameVar = "new Texture2D(64, 64)";
							scriptWindow[_idWindows[i]] += "			GUI.DrawTexture( new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + nameVar + ");\n";
						break;
					case "Toggle":
							scriptPrivateVar += "	private bool " + _namesElements[i] + " = " + _typeWin[i].ToString().ToLower() + ";\n";
							scriptElem += "		" + _namesElements[i] + " = GUI.Toggle(new Rect(" + GetStringRectWithSnap(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
						break;
					}
					if(_typeWin[i] && !addDrag){ scriptWindow[_idWindows[i]] += "\nGUI.DragWindow();\n"; addDrag = true;}
				}
				scriptText = "using UnityEngine;\nusing System.Collections;\n\npublic class " + _className + " : MonoBehaviour{\n" + scriptPublicVar + "\n\n" + scriptPrivateVar + "\n\n	void OnGUI(){\n" + scriptElem + "	}\n";
				if(winB){
					scriptText += "\n	void WindowFunc(int winID){\n";
					for(int i = 0; i < scriptWindow.Length; i++){
						if(scriptWindow[i] == "") continue;
						scriptText += "		if(winID == " + i + "){\n" + scriptWindow[i];
						scriptText += "		}\n";
					}
					scriptText += "	}\n";
				}
				scriptText += "}";
				string path = EditorUtility.SaveFilePanel("Save Script", Application.dataPath, _className, "cs");
				if(path.Length != 0)
					File.WriteAllText(path, scriptText);
				AssetDatabase.Refresh();
			}
			else
				GetWindow<GUIBuilderX>().ShowNotification(new GUIContent("Class name = \"\""));
		}
		private void SaveScriptJS(){
			if(_className != ""){
				string scriptText = "";
				string scriptPublicVar = "";
				string scriptPrivateVar = "";
				string scriptElem = "";
				string nameVar = "";
				string[] scriptWindow = new string[MaxValueInList(_idWindows.ToArray()) + 1];
				for(int i = 0; i < scriptWindow.Length; i++)
						scriptWindow[i] = "";
				bool winB = false;
				
				if(_guiSkin != null){
					scriptPublicVar += "var SkinGUI : GUISkin;\n";
					scriptElem += "GUI.skin = SkinGUI;\n";
				}
				for(int i = 0; i < _namesElements.Count; i++){
					if(_typesElements[i] != "Window" && _idWindows[i] != 0) continue;
					switch(_typesElements[i]){
					case "Button":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	var " + nameVar + " : Texture2D;\n";
							}
							else
								nameVar = "null";
							scriptElem += "	if(GUI.Button( new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"))){}\n";
						break;
					case "Label":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	var " + nameVar + " : Texture2D;\n";
							}
							else
								nameVar = "null";
							scriptElem += "	GUI.Label( new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
						break;
					case "PasswordField":
							scriptPrivateVar += "	private var " + _namesElements[i] + " : String = \"" + _contentElements[i].text + "\";\n";
							scriptElem += "	" + _namesElements[i] + " = GUI.PasswordField(new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), " + _namesElements[i] + ", '" + _maskChars[i].ToCharArray(0, 1)[0] + "'[0], " + _maxLeights[i] + ");\n";
						break;
					case "TextField":
							scriptPrivateVar += "	private var " + _namesElements[i] + " = \"" + _contentElements[i].text + "\";\n";
							scriptElem += "	" + _namesElements[i] + " = GUI.TextField(new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), " + _namesElements[i] + ", " + _maxLeights[i] + ");\n";
						break;
					case "Box":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	var " + nameVar + " : Texture2D;\n";
							}
							else
								nameVar = "null";
							scriptElem += "	GUI.Box( new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
						break;
					case "RepeatButton":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	var " + nameVar + " : Texture2D;\n";
							}
							else
								nameVar = "null";
							scriptElem += "	if(GUI.RepeatButton( new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"))){}\n";
						break;
					case "TextArea":
							scriptPrivateVar += "	private var " + _namesElements[i] + " = \"" + _contentElements[i].text + "\";\n";
							scriptElem += "	" + _namesElements[i] + " = GUI.TextArea(new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), " + _namesElements[i] + ", " + _maxLeights[i] + ");\n";
						break;
					case "HorizontalSlider":
							scriptPrivateVar += "	private var " + _namesElements[i] + " : float = " + _slidersValues[i] + "f;\n";
							scriptElem += "	" + _namesElements[i] + " = GUI.HorizontalSlider(new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), " + _namesElements[i] + ", " + _minValues[i] + "f, " + _maxValues[i] + "f);\n";
						break;
					case "VerticalSlider":
							scriptPrivateVar += "	private var " + _namesElements[i] + " : float = " + _slidersValues[i] + "f;\n";
							scriptElem += "	" + _namesElements[i] + " = GUI.VerticalSlider(new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), " + _namesElements[i] + ", " + _minValues[i] + "f, " + _maxValues[i] + "f);\n";
						break;
					case "DrawTexture":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	var " + nameVar + " : Texture2D;\n";
							}
							else
								nameVar = "new Texture2D(64, 64)";
							scriptElem += "	GUI.DrawTexture( new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), " + nameVar + ");\n";
						break;
					case "Toggle":
							scriptPrivateVar += "	private var " + _namesElements[i] + " : boolean = " + _typeWin[i].ToString().ToLower() + ";\n";
							scriptElem += "		" + _namesElements[i] + " = GUI.Toggle(new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
						break;
					case "Window":
							winB = true;
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	var " + nameVar + " : Texture2D;\n";
							}
							else
								nameVar = "null";
							scriptPrivateVar += "	private var " + _namesElements[i] + "_rect : Rect = new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) + ");\n";
							scriptElem += _namesElements[i] + " = ";
							scriptElem += "	GUI.Window( " + _idWindows[i] + ", " + _namesElements[i] + "_rect, WindowFunc, new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
							break;
					}
				}
				
				bool addDrag = false;
					
				for(int i = 0; i < _namesElements.Count; i++){
					if(_typeWin[i] && !addDrag && _typesElements[i] == "Window" && _idWindows[i] != 0){ scriptWindow[_idWindows[i]] += "\n			GUI.DragWindow();\n"; addDrag = true;}
					if(_typesElements[i] == "Window" || _idWindows[i] == 0) continue;
					switch(_typesElements[i]){
					case "Button":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	var " + nameVar + " : Texture2D;\n";
							}
							else
								nameVar = "null";
							scriptWindow[_idWindows[i]] += "		if(GUI.Button( new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"))){}\n";
						break;
					case "Label":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	var " + nameVar + " : Texture2D;\n";
							}
							else
								nameVar = "null";
							scriptWindow[_idWindows[i]] += "		GUI.Label( new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
						break;
					case "PasswordField":
							scriptPrivateVar += "	private var " + _namesElements[i] + " : String = \"" + _contentElements[i].text + "\";\n";
							scriptWindow[_idWindows[i]] += "		" + _namesElements[i] + " = GUI.PasswordField(new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), " + _namesElements[i] + ", '" + _maskChars[i].ToCharArray(0, 1)[0] + "'[0], " + _maxLeights[i] + ");\n";
						break;
					case "TextField":
							scriptPrivateVar += "	private var " + _namesElements[i] + " = \"" + _contentElements[i].text + "\";\n";
							scriptWindow[_idWindows[i]] += "		" + _namesElements[i] + " = GUI.TextField(new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), " + _namesElements[i] + ", " + _maxLeights[i] + ");\n";
						break;
					case "Box":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	var " + nameVar + " : Texture2D;\n";
							}
							else
								nameVar = "null";
							scriptWindow[_idWindows[i]] += "		GUI.Box( new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
						break;
					case "RepeatButton":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	var " + nameVar + " : Texture2D;\n";
							}
							else
								nameVar = "null";
							scriptWindow[_idWindows[i]] += "		if(GUI.RepeatButton( new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"))){}\n";
						break;
					case "TextArea":
							scriptPrivateVar += "	private var " + _namesElements[i] + " = \"" + _contentElements[i].text + "\";\n";
							scriptWindow[_idWindows[i]] += "		" + _namesElements[i] + " = GUI.TextArea(new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), " + _namesElements[i] + ", " + _maxLeights[i] + ");\n";
						break;
					case "HorizontalSlider":
							scriptPrivateVar += "	private var " + _namesElements[i] + " : float = " + _slidersValues[i] + "f;\n";
							scriptWindow[_idWindows[i]] += "		" + _namesElements[i] + " = GUI.HorizontalSlider(new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), " + _namesElements[i] + ", " + _minValues[i] + "f, " + _maxValues[i] + "f);\n";
						break;
					case "VerticalSlider":
							scriptPrivateVar += "	private var " + _namesElements[i] + " : float = " + _slidersValues[i] + "f;\n";
							scriptWindow[_idWindows[i]] += "		" + _namesElements[i] + " = GUI.VerticalSlider(new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), " + _namesElements[i] + ", " + _minValues[i] + "f, " + _maxValues[i] + "f);\n";
						break;
					case "DrawTexture":
							if(_contentElements[i].image != null){
								nameVar = _namesElements[i];
								scriptPublicVar += "	var " + nameVar + " : Texture2D;\n";
							}
							else
								nameVar = "new Texture2D(64, 64)";
							scriptWindow[_idWindows[i]] += "		GUI.DrawTexture( new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) +
							"), " + nameVar + ");\n";
						break;
					case "Toggle":
							scriptPrivateVar += "	private var " + _namesElements[i] + " : boolean = " + _typeWin[i].ToString().ToLower() + ";\n";
							scriptElem += "		" + _namesElements[i] + " = GUI.Toggle(new Rect(" + GetStringRectWithSnapJS(_rectElements[i], _snapTypeElements[i]) + "), " + _namesElements[i] + ", new GUIContent(\"" + _contentElements[i].text + "\", " + nameVar + ", \"" + _contentElements[i].tooltip + "\"));\n";
						break;
					}
					if(_typeWin[i] && !addDrag){ scriptWindow[_idWindows[i]] += "\nGUI.DragWindow();\n"; addDrag = true;}
				}
				scriptText = scriptPublicVar + "\n" + scriptPrivateVar + "\nfunction OnGUI(){\n" + scriptElem + "}\n";
				if(winB){
					scriptText += "\n	function WindowFunc(winID : int){\n";
					for(int i = 0; i < scriptWindow.Length; i++){
						if(scriptWindow[i] == "") continue;
						scriptText += "		if(winID == " + i + "){\n" + scriptWindow[i];
						scriptText += "		}\n";
					}
					scriptText += "	}\n";
				}
				string path = EditorUtility.SaveFilePanel("Save Script", Application.dataPath, _className, "js");
				if(path.Length != 0)
					File.WriteAllText(path, scriptText);
				AssetDatabase.Refresh();
			}
			else
				GetWindow<GUIBuilderX>().ShowNotification(new GUIContent("Class name = \"\""));
		}
		private void DelElement(){
			if(_namesElements.Count <= 0) return;
			Undo.RegisterUndo(GetWindow<GUIBuilderX>(), "Delete Element - GUI Builder");
			_typesElements.RemoveAt(_namesPopup);
			_namesElements.RemoveAt(_namesPopup);
			_contentElements.RemoveAt(_namesPopup);
			_snapTypeElements.RemoveAt(_namesPopup);
			_rectElements.RemoveAt(_namesPopup);
			_maxLeights.RemoveAt(_namesPopup);
			_maskChars.RemoveAt(_namesPopup);
			_slidersValues.RemoveAt(_namesPopup);
			_minValues.RemoveAt(_namesPopup);
			_maxValues.RemoveAt(_namesPopup);
			_typeWin.RemoveAt(_namesPopup);
		}
		private void ClearView(){
			Undo.RegisterUndo(GetWindow<GUIBuilderX>(), "Clear - GUI Builder");
			_typesElements.Clear();
			_namesElements.Clear();
			_contentElements.Clear();
			_snapTypeElements.Clear();
			_rectElements.Clear();
			_maxLeights.Clear();
			_maskChars.Clear();
			_slidersValues.Clear();
			_minValues.Clear();
			_maxValues.Clear();
			_typeWin.Clear();
		}
		private void EditElement(){
			if(_namesElements.Count <= 0) return;
			if(_typesElements[_namesPopup] != "----")
			{
				_namesElements[_namesPopup] = _nameElement;
			}
			_contentElements[_namesPopup] = new GUIContent(_text, _texture);
			_snapTypeElements[_namesPopup] = _snapTypeElement;
			_rectElements[_namesPopup] = _rectElement;
			_maxLeights[_namesPopup] = _maxLeight;
			if(_maskChar.Length == 0) _maskChar = "*";
			_maskChars[_namesPopup] = _maskChar;
			_slidersValues[_namesPopup] = _sliderValue;
			_minValues[_namesPopup] = _minValue;
			_maxValues[_namesPopup] = _maxValue;
			_idWindows[_namesPopup] = _idWin;
			_typeWin[_namesPopup] = _dragWin;
		}
		private void SelectElem(){
			if(_namesElements.Count <= 0) return;
			_nameElement = _namesElements[_namesPopup];
			_text = _contentElements[_namesPopup].text;
			_texture = _contentElements[_namesPopup].image as Texture2D;
			_snapTypeElement = _snapTypeElements[_namesPopup];
			_rectElement = _rectElements[_namesPopup];
			_maxLeight = _maxLeights[_namesPopup];
			_maskChar = _maskChars[_namesPopup];
			_sliderValue = _slidersValues[_namesPopup];
			_minValue = _minValues[_namesPopup];
			_maxValue = _maxValues[_namesPopup];
			_idWin = _idWindows[_namesPopup];
			_dragWin = _typeWin[_namesPopup];
			_element = _typesElements[_namesPopup];
				
		}
			
		private void SaveOriginal(bool auto){
				string types = "[Types]\n";
				string names = "[Names]\n";
				string content = "[Content]\n";
				string snap = "[Snaps]\n";
				string rect = "[Rects]\n";
				string leights = "[Leights]\n";
				string mask = "[Mask]\n";
				string sliders = "[Sliders]\n";
				string mValues = "[Min Values]\n";
				string mxValues = "[Max Values]\n";
				string windowId = "[Window ID]\n";
				string windowDrag = "[Win Drag]\n";
				
				for(int i = 0; i < _namesElements.Count; i++){
					types += _typesElements[i] + "\n";
					names += _namesElements[i] + "\n";
					content += _contentElements[i].text + "\n";
					snap += _snapTypeElements[i] + "\n";
					rect += _rectElements[i].x + "," + _rectElements[i].y + "," + _rectElements[i].width + "," + _rectElements[i].height + "\n";
					leights += _maxLeights[i] + "\n";
					mask += _maskChars[i] + "\n";
					sliders += _slidersValues[i] + "\n";
					mValues += _minValues[i] + "\n";
					mxValues += _maxValues[i] + "\n";
					windowId += _idWindows[i] + "\n";
					windowDrag += _typeWin[i] +"\n";
				}
				string scriptText = _namesElements.Count + "\n" + types + names + content + snap + rect + leights + mask + sliders + mValues + mxValues + windowId + windowDrag;
				string path = Application.dataPath + "/Poq Xert/GUIBuilder/Public/AutoSave.gbx";
				if(!auto)
					path = EditorUtility.SaveFilePanel("Save GUI", Application.dataPath + "/Poq Xert/GUIBuilder/Public", _className, "gbx");
				if(path.Length != 0)
					File.WriteAllText(path, scriptText);
			AssetDatabase.Refresh();
		}
		private void LoadOriginal(bool auto)
		{
			string path = Application.dataPath + "/Poq Xert/GUIBuilder/Public/AutoSave.gbx";
			if(!auto)
				path = EditorUtility.OpenFilePanel("Open GUI", Application.dataPath + "/Poq Xert/GUIBuilder/Public", "gbx");
			if(path.Length == 0 || !File.Exists(path)) return;
			Undo.RegisterUndo(GetWindow<GUIBuilderX>(), "Load GUI - GUI Builder");
			ClearView();
			StreamReader sr = new StreamReader(path);
			_className = Path.GetFileNameWithoutExtension(path);
			int count = int.Parse(sr.ReadLine());
			while(!sr.EndOfStream){
				string line = sr.ReadLine();
				if(line == "[Types]"){
					for(int i = 0; i < count; i++){
						_typesElements.Add(sr.ReadLine());
					}
				}
				if(line == "[Names]"){
					for(int i = 0; i < count; i++){
						_namesElements.Add(sr.ReadLine());
					}
				}
				if(line == "[Content]"){
					for(int i = 0; i < count; i++){
						if(_typesElements[i] != "DrawTexture")
							_contentElements.Add(new GUIContent(sr.ReadLine(), (Texture2D)null));
						else
							_contentElements.Add(new GUIContent(sr.ReadLine(), new Texture2D(64, 64)));
					}
				}
				if(line == "[Snaps]"){
					for(int i = 0; i < count; i++){
						_snapTypeElements.Add((SnapType)Enum.Parse(typeof(SnapType), sr.ReadLine()));
					}
				}
				if(line == "[Rects]"){
					for(int i = 0; i < count; i++){
						string[] str = sr.ReadLine().Split(new char[]{','});
						_rectElements.Add(new Rect(float.Parse(str[0]), float.Parse(str[1]), float.Parse(str[2]), float.Parse(str[3])));
					}
				}
				if(line == "[Leights]"){
					for(int i = 0; i < count; i++){
						_maxLeights.Add(int.Parse(sr.ReadLine()));
					}
				}
				if(line == "[Mask]"){
					for(int i = 0; i < count; i++){
						_maskChars.Add(sr.ReadLine());
					}
				}
				if(line == "[Sliders]"){
					for(int i = 0; i < count; i++){
						_slidersValues.Add(float.Parse(sr.ReadLine()));
					}
				}
				if(line == "[Min Values]"){
					for(int i = 0; i < count; i++){
						_minValues.Add(float.Parse(sr.ReadLine()));
					}
				}
				if(line == "[Max Values]"){
					for(int i = 0; i < count; i++){
						_maxValues.Add(float.Parse(sr.ReadLine()));
					}
				}
				if(line == "[Window ID]"){
					for(int i = 0; i < count; i++){
						_idWindows.Add(int.Parse(sr.ReadLine()));
					}
				}
				if(line == "[Win Drag]"){
					for(int i = 0; i < count; i++){
						_typeWin.Add(bool.Parse(sr.ReadLine()));
					}
				}
			}
			if(_snapTypeElements.Count == 0)
			{
				for(int i = 0; i < count; i++)
				{
					_snapTypeElements.Add(SnapType.Default);
				}
			}
			sr.Close();
	}
		private void AutoSave(){
			SaveOriginal(true);
			
		}
		private void AutoLoad(){
			LoadOriginal(true);
		}

		void WinF(int ID){
			EditorGUIUtility.LookLikeControls ();
			VisibleGUIWindow(ID);
		}
		
		int MaxValueInList(int[] arrInt)
		{
			int max = int.MinValue;
			foreach(int i in arrInt)
			{
				if(max < i)
					max = i;
			}
			return max;
		}
	}
}