using UnityEngine;
using System.Collections;


public delegate bool Bool_Void ();
public delegate void Void_Void ();
public delegate void Void_GameObject (GameObject go);
public delegate void Void_Bool (bool bo);
public delegate void Void_GO_Bool (GameObject go,bool bo);
public delegate void Void_Int (int a);
public delegate void Void_String (string str);
public delegate void Void_String_String (string a, string b);
public delegate void Void_StringBool (string str,bool bo);
public delegate void Void_Int_Int (int a,int b);
public delegate void Void_Long_Int(long a, int b);
public delegate void Void_Int_Int_Int (int a,int b,int c);
public delegate void Void_Object (Object obj);
public delegate int Int_Void ();
public delegate string String_Void ();
public delegate void Void_String_Int (string s, int a);
public delegate void Void_String_Bool_String(string a,bool b,string c);
public delegate void Void_Str_Obj(string str, object obj);


public delegate void Void_String_Object(string path,Object obj);
public delegate void Void_STR_UnityEngineObject(string str, UnityEngine.Object obj);
public delegate void Void_UnityEngineObject(UnityEngine.Object obj);