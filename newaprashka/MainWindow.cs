using System;
using Gtk;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using bazar;

public partial class MainWindow : Gtk.Window
{
	Dialog changePassword;
	Entry inputPassword;
	Entry inputPassword2;
	Button changePasswordOk;
	
	AccelGroup grup;
	
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();
		
		grup = new AccelGroup ();
		this.AddAccelGroup(grup);

		//Передаем лебл
		MainClass.StatusBarLabel = labelStatus;

		if(MainClass.User.Login == "root")
		{
			string Message = "Вы зашли в программу под администратором базы данных. У вас есть только возможность создавать других пользователей.";
			MessageDialog md = new MessageDialog ( this, DialogFlags.DestroyWithParent,
			                                      MessageType.Info, 
			                                      ButtonsType.Ok,
			                                      Message);
			md.Run ();
			md.Destroy();
			Users WinUser = new Users();
			WinUser.Show();
			WinUser.Run ();
			WinUser.Destroy ();
			return;
		}

		if(MainClass.connectionDB.DataSource == "demo.qsolution.ru")
		{
			string Message = "Вы подключились к демонстрационному серверу. Сервер предназначен для оценки " +
				"возможностей программы, не используйте его для работы, так как ваши данные будут доступны " +
				"любому пользователю через интернет.\n\nДля полноценного использования программы вам необходимо " +
				"установить собственный сервер. Для его установки обратитесь к документации.\n\nЕсли у вас возникнут " +
				"вопросы вы можете задать их на форуме программы: https://groups.google.com/forum/?fromgroups#!forum/bazarsoft " +
				"или обратится в нашу тех. поддержку.";
			MessageDialog md = new MessageDialog ( this, DialogFlags.DestroyWithParent,
			                                      MessageType.Info, 
			                                      ButtonsType.Ok,
			                                      Message);
			md.Run ();
			md.Destroy();
			dialogAuthenticationAction.Sensitive = false;
		}

		//Загружаем информацию о пользователе
		if(MainClass.User.TestUserExistByLogin (true))
			MainClass.User.UpdateUserInfoByLogin ();
		UsersAction.Sensitive = MainClass.User.admin;
		labelUser.LabelProp = MainClass.User.Name;
		
		PreparePlaces();
		PrepareLessee();
		PrepareContract();
		PrepareAccrual ();
		PrepareEvent();
		PrepareCash();
		notebookMain.CurrentPage = 0;
		UpdatePlaces ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	
	protected virtual void OnButtonViewClicked (object sender, System.EventArgs e)
	{
		TreeIter iter;
		int type, itemid;
		string ItemNumber, place;
		ResponseType result;
		
		switch (notebookMain.CurrentPage) {
		case 0:
			treeviewPlaces.Selection.GetSelected(out iter);
			place = Placefilter.GetValue(iter,2).ToString ();
			type = Convert.ToInt32(Placefilter.GetValue(iter,0));
			Place winPlace = new Place();
			winPlace.PlaceFill(type,place);
			winPlace.Show();
			result = (ResponseType)winPlace.Run();
			winPlace.Destroy();
			if(result == ResponseType.Ok)
				UpdatePlaces();
		break;
		case 1:
			treeviewLessees.Selection.GetSelected(out iter);
			itemid = Convert.ToInt32(Lesseesfilter.GetValue(iter,0));
			lessee winLessee = new lessee();
			winLessee.LesseeFill(itemid);
			winLessee.Show();
			result = (ResponseType)winLessee.Run();
			winLessee.Destroy();
			if(result == ResponseType.Ok)
				UpdateLessees();
		break;
		case 2:
			treeviewContract.Selection.GetSelected(out iter);
			ItemNumber = Contractfilter.GetValue(iter,1).ToString();
			Contract winContract = new Contract();
			winContract.ContractFill(ItemNumber);
			winContract.Show();
			result = (ResponseType)winContract.Run();
			winContract.Destroy();
			if(result == ResponseType.Ok)
				UpdateContract();
			break;
		case 3:
			treeviewAccrual.Selection.GetSelected(out iter);
			itemid = Convert.ToInt32(Accrualfilter.GetValue(iter,0));
			Accrual winAccrual = new Accrual();
			winAccrual.AccrualFill(itemid);
			winAccrual.Show();
			result = (ResponseType)winAccrual.Run();
			winAccrual.Destroy();
			if(result == ResponseType.Ok)
				UpdateAccrual();
			break;
		case 4:
			switch(notebookCash.CurrentPage)
			{
			case 0:
				treeviewIncome.Selection.GetSelected(out iter);
				itemid = Convert.ToInt32(CashIncomeFilter.GetValue(iter,0));
				IncomeSlip winIncome = new IncomeSlip();
				winIncome.SlipFill(itemid);
				winIncome.Show();
				result = (ResponseType)winIncome.Run();
				winIncome.Destroy();
				if(result == ResponseType.Ok)
				{
					UpdateCashIncome();
					CalculateTotalCash ();
				}
				break;
			case 1:
				treeviewExpense.Selection.GetSelected(out iter);
				itemid = Convert.ToInt32(CashExpenseFilter.GetValue(iter,0));
				ExpenseSlip winExpense = new  ExpenseSlip();
				winExpense.SlipFill(itemid);
				winExpense.Show();
				result = (ResponseType)winExpense.Run();
				winExpense.Destroy();
				if(result == ResponseType.Ok)
				{
					UpdateCashExpense();
					CalculateTotalCash ();
				}
				break;
			case 2:
				treeviewAdvance.Selection.GetSelected(out iter);
				itemid = Convert.ToInt32(CashAdvanceFilter.GetValue(iter,0));
				AdvanceStatement winAdvance = new AdvanceStatement();
				winAdvance.StatementFill(itemid);
				winAdvance.Show();
				result = (ResponseType)winAdvance.Run();
				winAdvance.Destroy();
				if(result == ResponseType.Ok)
					UpdateCashAdvance();
				break;
			}
			break;
		case 5:
			treeviewEvents.Selection.GetSelected(out iter);
			itemid = Convert.ToInt32(EventsListStore.GetValue(iter,0));
			Event winEvent = new Event();
			winEvent.EventFill(itemid);
			winEvent.Show();
			result = (ResponseType)winEvent.Run();
			winEvent.Destroy();
			if(result == ResponseType.Ok)
				UpdateEvents();
		break;
		default:
		break;
		}

	}
	
	protected virtual void OnButtonAddClicked (object sender, System.EventArgs e)
	{
		switch (notebookMain.CurrentPage) {
		case 0:
			Place winPlace = new Place();
			winPlace.NewPlace = true;
			winPlace.Show();
			winPlace.Run();
			winPlace.Destroy();
			UpdatePlaces();
		break;
		case 1:
			lessee winLessee = new lessee();
			winLessee.NewLessee = true;
			winLessee.Show();
			winLessee.Run();
			winLessee.Destroy();
			UpdateLessees();
		break;
		case 2:
			Contract winContract = new Contract();
			winContract.NewContract = true;
			winContract.Show();
			winContract.Run();
			winContract.Destroy();
			UpdateContract();
		break;
		case 3:
			Accrual winAccrual = new Accrual();
			winAccrual.NewAccrual = true;
			winAccrual.Show();
			winAccrual.Run();
			winAccrual.Destroy();
			UpdateAccrual();
			break;
		case 4:
			switch(notebookCash.CurrentPage)
			{
			case 0:
				IncomeSlip winIncomeSlip = new IncomeSlip();
				winIncomeSlip.NewSlip = true;
				winIncomeSlip.Show();
				winIncomeSlip.Run();
				winIncomeSlip.Destroy();
				UpdateCashIncome();
				CalculateTotalCash();
				break;
			case 1:
				ExpenseSlip winExpenseSlip = new ExpenseSlip();
				winExpenseSlip.NewSlip = true;
				winExpenseSlip.Show();
				winExpenseSlip.Run();
				winExpenseSlip.Destroy();
				UpdateCashExpense();
				CalculateTotalCash();
				break;
			case 2:
				AdvanceStatement winAdvance = new AdvanceStatement();
				winAdvance.NewStatement = true;
				winAdvance.Show();
				winAdvance.Run();
				winAdvance.Destroy();
				UpdateCashAdvance();
				break;
			}
			break;
		case 5:
			Event winEvent = new Event();
			winEvent.NewEvent = true;
			winEvent.Show();
			winEvent.Run();
			winEvent.Destroy();
			UpdateEvents();
		break;
		default:
		break;
		}
	}
	
	protected virtual void OnAction7Activated (object sender, System.EventArgs e)
	{
		reference winref = new reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("place_types","Тип места", "Типы мест");
		winref.Show();
		winref.Run();
		winref.Destroy();
		
	}
	
	protected virtual void OnAction6Activated (object sender, System.EventArgs e)
	{
		reference winref = new reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("goods","Группа товаров", "Группы товаров");
		winref.Show();
		winref.Run();
		winref.Destroy();

	}
	
	protected virtual void OnAction5Activated (object sender, System.EventArgs e)
	{
		reference winref = new reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("classes","Тип события", "Типы событий");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}
	
	protected virtual void OnAction10Activated (object sender, System.EventArgs e)
	{
		reference winref = new reference();
		winref.SetMode(false,false,true,true,true);
		winref.FillList("lessees","Арендатор", "Арендаторы");
		winref.Show();
		winref.Run();
		winref.Destroy();

	}
	
	protected virtual void OnAction3Activated (object sender, System.EventArgs e)
	{
		reference winref = new reference();
		winref.SetMode(false,false,true,true,true);
		winref.FillList("contact_persons","Контактное лицо", "Контактные лица");
		winref.Show();
		winref.Run();
		winref.Destroy();

	}

	protected void OnAction15Activated (object sender, EventArgs e)
	{
		reference winref = new reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("organizations","Организация", "Организации");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}

	protected virtual void OnNotebookMainSwitchPage (object o, Gtk.SwitchPageArgs args)
	{
		switch (notebookMain.CurrentPage) {
		case 0:
			UpdatePlaces ();
			OnTreeviewPlacesCursorChanged(o, EventArgs.Empty);
			labelSum.Visible = true;
			break;
		case 1:
			UpdateLessees();
			OnTreeviewLesseesCursorChanged(o, EventArgs.Empty);
			labelSum.Visible = false;
			break;
		case 2:
			UpdateContract();
			OnTreeviewContractCursorChanged(o, EventArgs.Empty);
			labelSum.Visible = false;
			break;
		case 3:
			UpdateAccrual();
			OnTreeviewAccrualCursorChanged(o, EventArgs.Empty);
			labelSum.Visible = true;
			break;
		case 4:
			switch(notebookCash.CurrentPage)
			{
			case 0:
				OnTreeviewIncomeCursorChanged(o, EventArgs.Empty);
				break;
			case 1:
				OnTreeviewExpenseCursorChanged(o, EventArgs.Empty);
				break;
			case 2:
				OnTreeviewAdvanceCursorChanged(o, EventArgs.Empty);
				break;
			}
			UpdateCash ();
			CalculateTotalCash ();
			labelSum.Visible = true;
			break;
		case 5:
			OnTreeviewEventsCursorChanged(o, EventArgs.Empty);
			labelSum.Visible = false;
			break;
		default:
		break;
		}
	}
	

	protected virtual void OnAction12Activated (object sender, System.EventArgs e)
	{
		AboutDialog dialog = new AboutDialog ();
		dialog.ProgramName = "БазАр (База Арендаторов)";
		
		dialog.Version = "2.0.3";
		
		dialog.Logo = Gdk.Pixbuf.LoadFromResource ("bazar.logo.png");
		
		dialog.Comments = "Программа позволяет вести учет арендаторов, кассы и т.п. " +
			"\nРазработана на MonoDevelop с использованием открытых технологий Mono, GTK#, MySQL." +
			"\nТелефон тех. поддержки +7(812)575-79-44";
		
		dialog.Copyright = "Quality Solution 2013";
		
		dialog.Authors = new string [] {"Ганьков Андрей <gav@qsolution.ru>"};
		
		dialog.Website = "http://www.qsolution.ru/";
		
		dialog.Run ();
		dialog.Destroy();
	}
	
	protected virtual void OnDialogAuthenticationActionActivated (object sender, System.EventArgs e)
	{
		changePassword = new Dialog("Смена своего пароля", this, Gtk.DialogFlags.DestroyWithParent);
    	changePassword.Modal = true;
		changePassword.AddButton ("Отмена", ResponseType.Cancel);
    	changePasswordOk =(Button) changePassword.AddButton ("Ok", ResponseType.Ok);
		Label changepassLabel = new Label ("Пароль должен содержать не менее 6 символов.\nНе используйте русские буквы в пароле.\nПароль не может состоять только из цифр.");
		changepassLabel.Wrap = true;
		changePassword.VBox.Add(changepassLabel);
		Table tablepassword = new Table (2, 2, true);
		changePassword.VBox.Add(tablepassword);
		Label newpassword = new Label ("Новый пароль:");
		newpassword.Justify = Justification.Right;
		Label newpassword2 = new Label ("Еще раз:");
		newpassword2.Justify = Justification.Right;
		tablepassword.Attach(newpassword,0,1,0,1);
		tablepassword.Attach(newpassword2,0,1,1,2);
		inputPassword = new Entry();
		inputPassword.Visibility = false;
		inputPassword.Changed += new EventHandler (OnInputPasswordChanged);
		changePassword.SetResponseSensitive(ResponseType.Ok,false);
		tablepassword.Attach(inputPassword,1,2,0,1);
		inputPassword2 = new Entry();
		inputPassword2.Visibility = false;
		inputPassword2.Changed += new EventHandler (OnInputPasswordChanged);
		changePassword.SetResponseSensitive(ResponseType.Ok,false);
		tablepassword.Attach(inputPassword2,1,2,1,2);
    	changePassword.Response += new ResponseHandler (OnChangePasswordResponse);
		changePassword.ShowAll();
    	changePassword.Run ();
		newpassword.Destroy();
		newpassword2.Destroy();
		inputPassword.Destroy();
		inputPassword2.Destroy();
		tablepassword.Destroy();
    	changePassword.Destroy ();
	}
	
	void OnInputPasswordChanged (object sender, System.EventArgs e)
	{
		long tempout;
		bool CanSaveEmpty = inputPassword.Text != "" || inputPassword2.Text != "";
		bool CanSaveEqual = inputPassword.Text == inputPassword2.Text;
		bool CanSaveLength = inputPassword.Text.Length > 4;
		bool CanSaveSpace = inputPassword.Text.IndexOf(' ') == -1;
		bool CanSaveNumbers = !long.TryParse(inputPassword.Text, out tempout);
		bool CanSaveCyrillic = !System.Text.RegularExpressions.Regex.IsMatch (inputPassword.Text, "\\p{IsCyrillic}");
		if(!CanSaveCyrillic) changePasswordOk.TooltipText = "Пароль не может содержать русские буквы";
		if(!CanSaveNumbers) changePasswordOk.TooltipText = "Пароль не может состоять только из цифр";
		if(!CanSaveLength) changePasswordOk.TooltipText = "Пароль должен быть длиннее 4 символов";
		if(!CanSaveSpace) changePasswordOk.TooltipText = "Пароль не может содержать пробелов";
		if(!CanSaveEqual) changePasswordOk.TooltipText = "Оба введенных пароля должны совпадать";
		if(!CanSaveEmpty) changePasswordOk.TooltipText = "Сначала заполните оба поля";
		
		bool CanSave = CanSaveEmpty && CanSaveEqual && CanSaveLength && CanSaveNumbers && CanSaveSpace && CanSaveCyrillic;
		if(CanSave) changePasswordOk.TooltipText = "Сохранить новый пароль";
		changePassword.SetResponseSensitive(ResponseType.Ok, CanSave);
	}
	
	void OnChangePasswordResponse (object obj, ResponseArgs args)
    {
        if(args.ResponseId == ResponseType.Ok)
		{
			MainClass.StatusMessage("Отправляем новый пароль на сервер...");
			string sql;
			sql = "SET PASSWORD = PASSWORD('" + inputPassword.Text + "')";
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.ExecuteNonQuery();
				MainClass.StatusMessage("Пароль изменен. Ok");
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка установки пароля!");
				MainClass.ErrorMessage(this,ex);
			}
		}
    }
	
	protected virtual void OnQuitActionActivated (object sender, System.EventArgs e)
	{
		Application.Quit();
	}
	
	protected void OnButtonDelClicked (object sender, System.EventArgs e)
	{
		// Удаление
		TreeIter iter;
		int type, itemid;
		Delete winDelete = new Delete();
		string ItemNumber, place;
		
		switch (notebookMain.CurrentPage) {
		case 0:
			treeviewPlaces.Selection.GetSelected(out iter);
			place = Placefilter.GetValue(iter,2).ToString ();
			type = Convert.ToInt32(Placefilter.GetValue(iter,0));
			winDelete.RunDeletion("places", type, place);
			UpdatePlaces();
		break;
		case 1:
			treeviewLessees.Selection.GetSelected(out iter);
			itemid = Convert.ToInt32(Lesseesfilter.GetValue(iter,0));
			winDelete.RunDeletion("lessees", itemid);
			UpdateLessees();
		break;
		case 2:
			treeviewContract.Selection.GetSelected(out iter);
			ItemNumber = Contractfilter.GetValue(iter, 1).ToString();
			winDelete.RunDeletion("contracts", ItemNumber);
			UpdateContract();
		break;
		case 3:
			treeviewAccrual.Selection.GetSelected(out iter);
			itemid = Convert.ToInt32(Accrualfilter.GetValue(iter,0));
			winDelete.RunDeletion("accrual", itemid);
			UpdateAccrual();
			break;
		case 4:
			switch(notebookCash.CurrentPage)
			{
			case 0:
				treeviewIncome.Selection.GetSelected(out iter);
				itemid = Convert.ToInt32(CashIncomeFilter.GetValue(iter,0));
				winDelete.RunDeletion("credit_slips", itemid);
				CalculateTotalCash();
				break;
			case 1:
				treeviewExpense.Selection.GetSelected(out iter);
				itemid = Convert.ToInt32(CashExpenseFilter.GetValue(iter,0));
				winDelete.RunDeletion("debit_slips", itemid);
				CalculateTotalCash();
				break;
			case 2:
				treeviewAdvance.Selection.GetSelected(out iter);
				itemid = Convert.ToInt32(CashAdvanceFilter.GetValue(iter,0));
				winDelete.RunDeletion("advance", itemid);
				break;
			}
			UpdateCash ();
			break;
		case 5:
			treeviewEvents.Selection.GetSelected(out iter);
			itemid = Convert.ToInt32(EventsListStore.GetValue(iter,0));
			winDelete.RunDeletion("events", itemid);
			UpdateEvents();
		break;
		default:
		break;
		}
		winDelete.Destroy();
	}
	
	protected void OnAction17Activated (object sender, EventArgs e)
	{
		reference winref = new reference();
		winref.SetMode(false,false,true,true,true);
		winref.FillList("services","Услуга", "Услуги");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}

	protected void OnAction18Activated (object sender, EventArgs e)
	{
		reference winref = new reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("units","Единица", "Единицы измерения");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}

	protected void OnAction19Activated (object sender, EventArgs e)
	{
		reference winref = new reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("cash","Касса", "Кассы");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}	

	protected void OnAction20Activated (object sender, EventArgs e)
	{
		Dialog HistoryDialog = new Dialog("История версий программы", this, Gtk.DialogFlags.DestroyWithParent);
    	HistoryDialog.Modal = true;
		HistoryDialog.AddButton ("Закрыть", ResponseType.Close);

		System.IO.StreamReader HistoryFile = new System.IO.StreamReader( "changes.txt");
		TextView HistoryTextView = new TextView();
		HistoryTextView.WidthRequest = 700;
		HistoryTextView.WrapMode = WrapMode.Word;
		HistoryTextView.Sensitive = false;
		HistoryTextView.Buffer.Text = HistoryFile.ReadToEnd();
		Gtk.ScrolledWindow ScrollW = new ScrolledWindow();
		ScrollW.HeightRequest = 500;
		ScrollW.Add (HistoryTextView);
		HistoryDialog.VBox.Add (ScrollW);

		HistoryDialog.ShowAll ();
		HistoryDialog.Run ();
		HistoryDialog.Destroy ();
	}	

	protected void OnAction24Activated (object sender, EventArgs e)
	{
		reference winref = new reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("expense_items","Статья расхода", "Статьи расходов");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}	

	protected void OnAction25Activated (object sender, EventArgs e)
	{
		reference winref = new reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("income_items","Статья дохода", "Статьи доходов");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}	

	protected void OnAction21Activated (object sender, EventArgs e)
	{
		reference winref = new reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("contractors","Контрагент", "Контрагенты");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}
	
	protected void OnUsersActionActivated (object sender, EventArgs e)
	{
		Users winUsers = new Users();
		winUsers.Show();
		winUsers.Run();
		winUsers.Destroy();
	}

	protected void OnAction27Activated (object sender, EventArgs e)
	{
		reference winref = new reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("employees","Сотрудник", "Сотрудники");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}

	public void ReferenceUpdated(string RefTable)
	{
		switch (RefTable)
		{
		case "place_types":
			MainClass.ComboFillReference(comboPlaceType,"place_types",1);
			MainClass.ComboFillReference(comboContractPlaceT,"place_types",1);
			MainClass.ComboFillReference(comboEventPlaceT,"place_types",1);
			break;
		case "organizations":
			MainClass.ComboFillReference(comboPlaceOrg,"organizations",1);
			MainClass.ComboFillReference(comboContractOrg, "organizations", 1);
			MainClass.ComboFillReference(comboAccrualOrg, "organizations", 1);
			MainClass.ComboFillReference(comboCashOrg, "organizations", 1);
			break;
		case "cash":
			MainClass.ComboFillReference(comboCashCash,"cash",1);
			break;
		case "income_items":
			if(notebookCash.CurrentPage == 0)
				MainClass.ComboFillReference(comboCashItem,"income_items",1);
			break;
		case "expense_items": 
			if(notebookCash.CurrentPage >= 1)
				MainClass.ComboFillReference(comboCashItem,"expense_items",1);
			break;
		case "classes":
			MainClass.ComboFillReference(comboEventType, "classes", 1);
			break;
		default:
				break;
		}
	}
	

	protected void OnAction31Activated (object sender, EventArgs e)
	{
		AccountableDebts WinDebts = new AccountableDebts();
		WinDebts.ShowAll ();
		WinDebts.Run ();
		WinDebts.Destroy ();
	}

	protected void OnAction32Activated (object sender, EventArgs e)
	{
		AccountableSlips winSlips = new AccountableSlips();
		winSlips.Show();
		winSlips.Run();
		winSlips.Destroy();
	}

	protected void OnAction30Activated (object sender, EventArgs e)
	{
		CashBalance winBalance = new CashBalance();
		winBalance.Show();
		winBalance.Run();
		winBalance.Destroy();
	}	

	protected void OnHelpActionActivated (object sender, EventArgs e)
	{
		System.Diagnostics.Process.Start("UserGuide_ru.pdf");
	}
	
	protected void OnAction33Activated (object sender, EventArgs e)
	{
		LesseeDebtsReport WinReport = new LesseeDebtsReport();
		WinReport.Show ();
		WinReport.Run ();
		WinReport.Destroy ();
	}
	protected void OnAction36Activated (object sender, EventArgs e)
	{
		System.Diagnostics.Process.Start ("http://bazar.qsolution.ru");
	}
	
	protected void OnAction37Activated (object sender, EventArgs e)
	{
		System.Diagnostics.Process.Start ("https://groups.google.com/forum/?fromgroups#!forum/bazarsoft");
	}

	protected void OnAction38Activated (object sender, EventArgs e)
	{
		DocRegister win = new DocRegister();
		win.Show ();
		win.Run ();
		win.Destroy ();
	}
	
	protected void OnButtonRefreshTableClicked (object sender, EventArgs e)
	{
		switch (notebookMain.CurrentPage) {
		case 0:
			UpdatePlaces();
			break;
		case 1:
			UpdateLessees();
			break;
		case 2:
			UpdateContract();
			break;
		case 3:
			UpdateAccrual();
			break;
		case 4:
			UpdateCash ();
			break;
		case 5:
			UpdateEvents();
			break;
		default:
			break;
		}
	}

	protected void OnAction39Activated (object sender, EventArgs e)
	{
		string param = "";
		ReportsExt.ViewReport ("Contracts", param);
	}

}
