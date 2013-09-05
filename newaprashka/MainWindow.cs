using System;
using Gtk;
using MySql.Data.MySqlClient;
using bazar;
using QSProjectsLib;
using QSSupportLib;

public partial class MainWindow : Gtk.Window
{
	
	AccelGroup grup;
	
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();
		grup = new AccelGroup ();
		this.AddAccelGroup(grup);

		//Передаем лебл
		MainClass.StatusBarLabel = labelStatus;

		try
		{
			MainSupport.Param = new BaseParam(QSMain.connectionDB);
		}
		catch(MySqlException)
		{
			MessageDialog BaseError = new MessageDialog ( this, DialogFlags.DestroyWithParent,
	                                      MessageType.Warning, 
	                                      ButtonsType.Close, 
	                                      "Неизвестная база данных");
			BaseError.Run();
			BaseError.Destroy();
			Environment.Exit(0);
		}
		TestVersion();

		if(QSMain.User.Login == "root")
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

		if(QSMain.connectionDB.DataSource == "demo.qsolution.ru")
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
		if(QSMain.User.TestUserExistByLogin (true))
			QSMain.User.UpdateUserInfoByLogin ();
		UsersAction.Sensitive = QSMain.User.admin;
		labelUser.LabelProp = QSMain.User.Name;

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
		string place;
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
			itemid = (int) Contractfilter.GetValue(iter, 0);
			Contract winContract = new Contract();
			winContract.ContractFill(itemid);
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
				winIncome.SlipFill(itemid, false);
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
				winExpense.SlipFill(itemid, false);
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
				winAdvance.StatementFill(itemid, false);
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

		Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
		dialog.Version = String.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
		
		dialog.Logo = Gdk.Pixbuf.LoadFromResource ("bazar.icons.logo.png");
		
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
		QSMain.User.ChangeUserPassword (this);
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
		string place;
		
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
			itemid = Convert.ToInt32(Contractfilter.GetValue(iter, 1));
			winDelete.RunDeletion("contracts", itemid);
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
			ComboWorks.ComboFillReference(comboPlaceType,"place_types",1);
			ComboWorks.ComboFillReference(comboContractPlaceT,"place_types",1);
			ComboWorks.ComboFillReference(comboEventPlaceT,"place_types",1);
			break;
		case "organizations":
			ComboWorks.ComboFillReference(comboPlaceOrg,"organizations",1);
			ComboWorks.ComboFillReference(comboContractOrg, "organizations", 1);
			ComboWorks.ComboFillReference(comboAccrualOrg, "organizations", 1);
			ComboWorks.ComboFillReference(comboCashOrg, "organizations", 1);
			break;
		case "cash":
			ComboWorks.ComboFillReference(comboCashCash,"cash",1);
			break;
		case "income_items":
			if(notebookCash.CurrentPage == 0)
				ComboWorks.ComboFillReference(comboCashItem,"income_items",1);
			break;
		case "expense_items": 
			if(notebookCash.CurrentPage >= 1)
				ComboWorks.ComboFillReference(comboCashItem,"expense_items",1);
			break;
		case "classes":
			ComboWorks.ComboFillReference(comboEventType, "classes", 1);
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

	protected void OnAction40Activated (object sender, EventArgs e)
	{
		DailyCashReport WinReport = new DailyCashReport();
		WinReport.Show ();
		WinReport.Run ();
		WinReport.Destroy ();
	}	

	private void TestVersion()
	{
		string errors = "";
		string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString();

		string edition = "beta";
		if(MainSupport.Param.Edition != edition)
		{
			errors += "\nРедакция продукта не совпадает с редакцией базы данных.\n";
			errors += "Редакция продукта: " + edition + "\nРедакция базы данных: " + MainSupport.Param.Edition + "\n";
		}

		if(MainSupport.Param.Product != name)
			errors += "\nБаза данных не для того продукта.\n";

		Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
		string[] ver = MainSupport.Param.Version.Split('.');
		if(version.Major.ToString() != ver[0] || version.Minor.ToString() != ver[1])
		{
			errors += "\nВерсия продукта не совпадает с версией базы данных.\n";
			errors += "Версия продукта: " + String.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build); 
			errors += "\nВерсия базы данных: " + MainSupport.Param.Version + "\n";
		}

		if(errors != "")
		{
			MessageDialog VersionError = new MessageDialog ( this, DialogFlags.DestroyWithParent,
				                                      MessageType.Warning, 
				                                      ButtonsType.Close, 
				                                      errors);
			VersionError.Run();
			VersionError.Destroy();
			Environment.Exit(0);
		}
	}
}
