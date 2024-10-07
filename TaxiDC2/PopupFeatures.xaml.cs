using System.Collections.ObjectModel;
using System.ComponentModel;
namespace TaxiDC2;
///<summary>
///PopupFeatures class
///</summary>
public partial class PopupFeatures : ContentPage
{
	///<summary>
    ///PopupFeatures constructor
    ///</summary>
	public PopupFeatures()
	{
		InitializeComponent();
	}	
	 private void Button_Clicked(object sender, EventArgs e)
    {
        this.Popup.Show();        
    }  
}
