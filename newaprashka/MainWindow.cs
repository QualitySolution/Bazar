using System;
using System.Collections.Generic;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;
using QSSupportLib;
using bazar;

public partial class MainWindow : Gtk.Window
{
	private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
	AccelGroup grup;
	
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();
		grup = new AccelGroup ();
		this.AddAccelGroup(grup);

		//Передаем лебл
		MainClass.StatusBarLabel = labelStatus;
		this.Title = QSSupportLib.MainSupport.GetTitle();
		QSMain.MakeNewStatusTargetForNlog("StatusMessage", "bazar.MainClass, bazar");

		Reference.RunReferenceItemDlg += OnRunReferenceItemDialog;
		QSMain.ReferenceUpdated += OnReferenceUpdate;

		//Test version of base
		try
		{
			MainSupport.BaseParameters = new BaseParam(QSMain.connectionDB);
		}
		catch(MySqlException e)
		{
			Console.WriteLine(e.Message);
			MessageDialog BaseError = new MessageDialog ( this, DialogFlags.DestroyWithParent,
			                                             MessageType.Warning, 
			                                             ButtonsType.Close, 
			                                             "Не удалось получить информацию о версии базы данных.");
			BaseError.Run();
			BaseError.Destroy();
			Environment.Exit(0);
		}

		MainSupport.ProjectVerion = new AppVersion(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString(),
		                                           "gpl",
		                                           System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
		MainSupport.TestVersion(this); //Проверяем версию базы
		QSMain.CheckServer (this); // Проверяем настройки сервера
		MainClass.MinorDBVersionChange (); // При необходимости корректируем базу.

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

		//Настраиваем новости
		MainNewsFeed.NewsFeeds = new List<NewsFeed>(){
			new NewsFeed("bazarnews", "Новости программы", "http://news.qsolution.ru/bazar.atom")
		};
		MainNewsFeed.LoadReadFeed ();
		var newsmenu = new NewsMenuItem ();
		menubar1.Add (newsmenu);
		newsmenu.LoadFeed ();

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
			place = PlaceSort.GetValue(iter, (int)PlaceCol.place_no).ToString ();
			type = Convert.ToInt32(PlaceSort.GetValue(iter, (int)PlaceCol.type_place_id));
			Place winPlace = new Place(false);
			winPlace.PlaceFill(type,place);
			winPlace.Show();
			result = (ResponseType)winPlace.Run();
			winPlace.Destroy();
			if(result == ResponseType.Ok)
				UpdatePlaces();
		break;
		case 1:
			treeviewLessees.Selection.GetSelected(out iter);
			itemid = Convert.ToInt32(LesseesSort.GetValue(iter, (int)LesseesCol.id));
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
			itemid = (int) ContractSort.GetValue(iter, (int)ContractCol.id);
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
			itemid = Convert.ToInt32(AccrualSort.GetValue(iter, (int)AccrualCol.id));
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
				itemid = Convert.ToInt32(CashIncomeSort.GetValue(iter, (int)CashIncomeCol.id));
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
				itemid = Convert.ToInt32(CashExpenseSort.GetValue(iter, (int)CashExpenseCol.id));
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
				itemid = Convert.ToInt32(CashAdvanceSort.GetValue(iter, (int)CashAdvanceCol.id));
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
			Place winPlace = new Place(true);
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
		Reference winref = new Reference(true);
		winref.SetMode(true,false,true,true,true);
		winref.FillList("place_types","Тип места", "Типы мест");
		winref.Show();
		winref.Run();
		winref.Destroy();
		
	}
	
	protected virtual void OnAction6Activated (object sender, System.EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("goods","Группа товаров", "Группы товаров");
		winref.Show();
		winref.Run();
		winref.Destroy();

	}
	
	protected virtual void OnAction5Activated (object sender, System.EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("classes","Тип события", "Типы событий");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}
	
	protected virtual void OnAction10Activated (object sender, System.EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(false,false,true,true,true);
		winref.FillList("lessees","Арендатор", "Арендаторы");
		winref.Show();
		winref.Run();
		winref.Destroy();

	}
	
	protected virtual void OnAction3Activated (object sender, System.EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(false,false,true,true,true);
		winref.FillList("contact_persons","Контактное лицо", "Контактные лица");
		winref.Show();
		winref.Run();
		winref.Destroy();

	}

	protected void OnAction15Activated (object sender, EventArgs e)
	{
		Reference winref = new Reference();
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
		QSMain.RunAboutDialog ();
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
			place = PlaceSort.GetValue(iter, (int)PlaceCol.place_no).ToString ();
			type = Convert.ToInt32(PlaceSort.GetValue(iter, (int)PlaceCol.type_place_id));
			winDelete.RunDeletion("places", type, place);
			UpdatePlaces();
		break;
		case 1:
			treeviewLessees.Selection.GetSelected(out iter);
			itemid = Convert.ToInt32(LesseesSort.GetValue(iter, (int)LesseesCol.id));
			winDelete.RunDeletion("lessees", itemid);
			UpdateLessees();
		break;
		case 2:
			treeviewContract.Selection.GetSelected(out iter);
			itemid = Convert.ToInt32(ContractSort.GetValue(iter, (int)ContractCol.id));
			winDelete.RunDeletion("contracts", itemid);
			UpdateContract();
		break;
		case 3:
			treeviewAccrual.Selection.GetSelected(out iter);
			itemid = Convert.ToInt32(AccrualSort.GetValue(iter, (int)AccrualCol.id));
			winDelete.RunDeletion("accrual", itemid);
			UpdateAccrual();
			break;
		case 4:
			switch(notebookCash.CurrentPage)
			{
			case 0:
				treeviewIncome.Selection.GetSelected(out iter);
				itemid = Convert.ToInt32(CashIncomeSort.GetValue(iter, (int)CashIncomeCol.id));
				winDelete.RunDeletion("credit_slips", itemid);
				CalculateTotalCash();
				break;
			case 1:
				treeviewExpense.Selection.GetSelected(out iter);
				itemid = Convert.ToInt32(CashExpenseSort.GetValue(iter, (int)CashExpenseCol.id));
				winDelete.RunDeletion("debit_slips", itemid);
				CalculateTotalCash();
				break;
			case 2:
				treeviewAdvance.Selection.GetSelected(out iter);
				itemid = Convert.ToInt32(CashAdvanceSort.GetValue(iter, (int)CashAdvanceCol.id));
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
		Reference winref = new Reference();
		winref.SetMode(false,false,true,true,true);
		winref.FillList("services","Услуга", "Услуги");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}

	protected void OnAction18Activated (object sender, EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("units","Единица", "Единицы измерения");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}

	protected void OnAction19Activated (object sender, EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(false,false,true,true,true);
		winref.FillList("cash","Касса", "Кассы");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}	

	protected void OnAction20Activated (object sender, EventArgs e)
	{
		QSMain.RunChangeLogDlg (this);
	}	

	protected void OnAction24Activated (object sender, EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("expense_items","Статья расхода", "Статьи расходов");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}	

	protected void OnAction25Activated (object sender, EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("income_items","Статья дохода", "Статьи доходов");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}	

	protected void OnAction21Activated (object sender, EventArgs e)
	{
		Reference winref = new Reference();
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
		Reference winref = new Reference();
		winref.SetMode(true,false,true,true,true);
		winref.FillList("employees","Сотрудник", "Сотрудники");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}

	protected void OnRunReferenceItemDialog(object sender, Reference.RunReferenceItemDlgEventArgs e)
	{
		ResponseType Result;
		switch (e.TableName)
		{
		case "meter_types":
			MeterType MeterTypeEdit = new MeterType(e.NewItem);
			if(!e.NewItem)
				MeterTypeEdit.Fill(e.ItemId);
			MeterTypeEdit.Show();
			Result = (ResponseType)MeterTypeEdit.Run();
			MeterTypeEdit.Destroy();
			break;
		case "contact_persons":
			Contact ContactEdit = new Contact();
			ContactEdit.NewContact = e.NewItem;
			if(!e.NewItem)
				ContactEdit.ContactFill(e.ItemId);
			ContactEdit.Show();
			Result = (ResponseType)ContactEdit.Run();
			ContactEdit.Destroy();
			break;
		case "lessees":
			lessee LesseeEdit = new lessee();
			LesseeEdit.NewLessee = e.NewItem;
			if(!e.NewItem)
				LesseeEdit.LesseeFill(e.ItemId);
			LesseeEdit.Show();
			Result = (ResponseType)LesseeEdit.Run();
			LesseeEdit.Destroy();
			break;
		case "services":
			Service ServiceEdit = new Service();
			ServiceEdit.NewService = e.NewItem;
			if(!e.NewItem)
				ServiceEdit.ServiceFill(e.ItemId);
			ServiceEdit.Show();
			Result = (ResponseType)ServiceEdit.Run();
			ServiceEdit.Destroy();
			break;
		case "cash":
			Cash CashEdit = new Cash();
			CashEdit.NewItem = e.NewItem;
			if(!e.NewItem)
				CashEdit.Fill(e.ItemId);
			CashEdit.Show();
			Result = (ResponseType)CashEdit.Run();
			CashEdit.Destroy();
			break;

		default:
			Result = ResponseType.None;
			break;
		}
		e.Result = Result;
	}

	protected void OnReferenceUpdate(object sender, QSMain.ReferenceUpdatedEventArgs e)
	{
		switch (e.ReferenceTable) {
		case "place_types":
			ComboWorks.ComboFillReference(comboPlaceType,"place_types", ComboWorks.ListMode.WithAll);
			ComboWorks.ComboFillReference(comboContractPlaceT,"place_types", ComboWorks.ListMode.WithAll);
			ComboWorks.ComboFillReference(comboEventPlaceT,"place_types", ComboWorks.ListMode.WithAll);
			break;
		case "organizations":
			ComboWorks.ComboFillReference(comboPlaceOrg,"organizations", ComboWorks.ListMode.WithAll);
			ComboWorks.ComboFillReference(comboContractOrg, "organizations", ComboWorks.ListMode.WithAll);
			ComboWorks.ComboFillReference(comboAccrualOrg, "organizations", ComboWorks.ListMode.WithAll);
			ComboWorks.ComboFillReference(comboCashOrg, "organizations", ComboWorks.ListMode.WithAll);
			break;
		case "cash":
			ComboWorks.ComboFillReference(comboCashCash,"cash", ComboWorks.ListMode.WithAll);
			ComboWorks.ComboFillReference(comboAccrualCash,"cash", ComboWorks.ListMode.WithAll);
			break;
		case "income_items":
			if(notebookCash.CurrentPage == 0)
				ComboWorks.ComboFillReference(comboCashItem,"income_items", ComboWorks.ListMode.WithAll);
			break;
		case "expense_items": 
			if(notebookCash.CurrentPage >= 1)
				ComboWorks.ComboFillReference(comboCashItem,"expense_items", ComboWorks.ListMode.WithAll);
			break;
		case "classes":
			ComboWorks.ComboFillReference(comboEventType, "classes", ComboWorks.ListMode.WithAll);
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
		System.Diagnostics.Process.Start("bazar_ru.pdf");
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
			CalculateTotalCash ();
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
		ViewReportExt.Run ("Contracts", param);
	}

	protected void OnAction40Activated (object sender, EventArgs e)
	{
		DailyCashReport WinReport = new DailyCashReport();
		WinReport.Show ();
		WinReport.Run ();
		WinReport.Destroy ();
	}	
	protected void OnAction41Activated(object sender, EventArgs e)
	{
		Reference winref = new Reference();
		winref.SetMode(false,false,true,true,true);
		winref.FillList("meter_types","Тип счётчика", "Типы счётчиков");
		winref.Show();
		winref.Run();
		winref.Destroy();
	}
	
	protected void OnAction42Activated(object sender, EventArgs e)
	{
		MetersReport WinMeters = new MetersReport ();
		WinMeters.Show ();
		WinMeters.Run ();
		WinMeters.Destroy ();
	}
	
	protected void OnActionLesseeRentReportActivated (object sender, EventArgs e)
	{
		LesseeRentReport RentReport = new LesseeRentReport ();
		RentReport.Show ();
		RentReport.Run ();
		RentReport.Destroy ();
	}

}
