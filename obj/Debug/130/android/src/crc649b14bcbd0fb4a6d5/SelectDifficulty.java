package crc649b14bcbd0fb4a6d5;


public class SelectDifficulty
	extends androidx.appcompat.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onBackPressed:()V:GetOnBackPressedHandler\n" +
			"";
		mono.android.Runtime.register ("Nauka_angielskiego.SelectDifficulty, Nauka angielskiego", SelectDifficulty.class, __md_methods);
	}


	public SelectDifficulty ()
	{
		super ();
		if (getClass () == SelectDifficulty.class) {
			mono.android.TypeManager.Activate ("Nauka_angielskiego.SelectDifficulty, Nauka angielskiego", "", this, new java.lang.Object[] {  });
		}
	}


	public SelectDifficulty (int p0)
	{
		super (p0);
		if (getClass () == SelectDifficulty.class) {
			mono.android.TypeManager.Activate ("Nauka_angielskiego.SelectDifficulty, Nauka angielskiego", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
		}
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onBackPressed ()
	{
		n_onBackPressed ();
	}

	private native void n_onBackPressed ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
