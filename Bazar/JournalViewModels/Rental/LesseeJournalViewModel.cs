using System;
using System.Linq.Expressions;
using Bazar.Domain.Rental;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using QS.DomainModel.UoW;
using QS.Navigation;
using QS.Project.Journal;
using QS.Project.Journal.DataLoader;
using QS.Project.Journal.Search;
using QS.Services;

namespace Bazar.JournalViewModels.Rental
{
	public class LesseeJournalViewModel : JournalViewModelBase
	{
		public LesseeJournalViewModel(IUnitOfWorkFactory unitOfWorkFactory, IInteractiveService interactiveService, INavigationManager navigation) : base(unitOfWorkFactory, interactiveService, navigation)
		{

			var dataLoader = new ThreadDataLoader<LesseeJournalNode>(unitOfWorkFactory);
			dataLoader.AddQuery<Lessee>(ItemsQuery);
			DataLoader = dataLoader;

			TabName = "Арендаторы";

			CreateNodeActions();

			//Поиск
			Search.OnSearch += Search_OnSearch;
			searchHelper = new SearchHelper(Search);

		}

		protected IQueryOver<Lessee> ItemsQuery(IUnitOfWork uow)
		{
			LesseeJournalNode resultAlias = null;

			Lessee lesseeAlias = null;

			return uow.Session.QueryOver<Lessee>(() => lesseeAlias)
				.Where(GetSearchCriterion(
					() => lesseeAlias.Name,
					() => lesseeAlias.Inn
					))
				.SelectList((list) => list
					.SelectGroup(x => x.Id).WithAlias(() => resultAlias.Id)
					.Select(x => x.Name).WithAlias(() => resultAlias.Name)
					.Select(x => x.Inn).WithAlias(() => resultAlias.Inn)
				).TransformUsing(Transformers.AliasToBean<LesseeJournalNode>());
		}

		#region Поиск

		void Search_OnSearch(object sender, EventArgs e)
		{
			Refresh();
		}

		private readonly SearchHelper searchHelper;

		protected ICriterion GetSearchCriterion(params Expression<Func<object>>[] aliasPropertiesExpr)
		{
			return searchHelper.GetSearchCriterion(aliasPropertiesExpr);
		}

		protected ICriterion GetSearchCriterion<TRootEntity>(params Expression<Func<TRootEntity, object>>[] propertiesExpr)
		{
			return searchHelper.GetSearchCriterion(propertiesExpr);
		}

		#endregion
	}

	public class LesseeJournalNode
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string Inn { get; set; }
	}
}
