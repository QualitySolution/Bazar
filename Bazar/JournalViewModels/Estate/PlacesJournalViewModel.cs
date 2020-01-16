using System;
using System.Linq.Expressions;
using Bazar.Domain.Estate;
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

namespace Bazar.JournalViewModels.Estate
{
	public class PlacesJournalViewModel : JournalViewModelBase
	{
		public PlacesJournalViewModel(IUnitOfWorkFactory unitOfWorkFactory, IInteractiveService interactiveService, INavigationManager navigation) : base(unitOfWorkFactory, interactiveService, navigation)
		{

			var dataLoader = new ThreadDataLoader<PlaceJournalNode>(unitOfWorkFactory);
			dataLoader.AddQuery<Place>(ItemsQuery);
			DataLoader = dataLoader;

			TabName = "Места";

			CreateNodeActions();

			//Поиск
			Search.OnSearch += Search_OnSearch;
			searchHelper = new SearchHelper(Search);

		}

		protected IQueryOver<Place> ItemsQuery(IUnitOfWork uow)
		{
			Place placeAlias = null;
			PlaceType placeTypeAlias = null;
			Organization organizationAlias = null;
			ContractItem contractItemAlias = null;
			Contract contractAlias = null;
			Lessee lesseeAlias = null;

			var contractSubquery = QueryOver.Of<ContractItem>(() => contractItemAlias)
				.JoinAlias(() => contractItemAlias.Contract, () => contractAlias)
				.JoinAlias(() => contractAlias.Lessee, () => lesseeAlias)
				.Where(x => x.Place_NhOnly.Id == placeAlias.Id)
				.Where(() => contractAlias.BeginDate < DateTime.Now)
				.Where(Restrictions.GeProperty(
					Projections.SqlFunction("COALESCE", NHibernateUtil.DateTime,
						Projections.Property(() => contractAlias.CancelDate),
						Projections.Property(() => contractAlias.EndDate)),
					Projections.Constant(DateTime.Now)))
				.Select(Projections.SqlFunction("CONCAT", NHibernateUtil.String,
						Projections.Property(() => lesseeAlias.Name),
						Projections.Constant("|"),
						Projections.Property(() => contractAlias.BeginDate),
						Projections.Constant("|"),
						Projections.SqlFunction("COALESCE", NHibernateUtil.DateTime,
						Projections.Property(() => contractAlias.CancelDate),
						Projections.Property(() => contractAlias.EndDate))
					))
				.Take(1);

			PlaceJournalNode resultAlias = null;
			return uow.Session.QueryOver<Place>(() => placeAlias)
				.Where(GetSearchCriterion(
					() => placeAlias.PlaceNumber,
					() => placeTypeAlias.Name
					))
				.JoinAlias(x => x.PlaceType, () => placeTypeAlias)
				.JoinAlias(x => x.Organization, ()=> organizationAlias, NHibernate.SqlCommand.JoinType.LeftOuterJoin)
				.SelectList((list) => list
					.SelectGroup(x => x.Id).WithAlias(() => resultAlias.Id)
					.Select(x => x.PlaceNumber).WithAlias(() => resultAlias.Number)
					.Select(x => x.Area).WithAlias(() => resultAlias.Area)
					.Select(() => placeTypeAlias.Name).WithAlias(() => resultAlias.TypeName)
					.Select(() => organizationAlias.Name).WithAlias(() => resultAlias.Organization)
					.SelectSubQuery(contractSubquery).WithAlias(() => resultAlias.LeaseInfo)
				).TransformUsing(Transformers.AliasToBean<PlaceJournalNode>());
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

	public class PlaceJournalNode : JournalEntityNodeBase<Place>
	{
		public string TypeName { get; set; }
		public string Number { get; set; }
		public string PlaceName => $"{TypeName}-{Number}";
		public decimal Area { get; set; }
		public string AreaText => Area > 0 ? String.Format("{0} м<sup>2</sup>", Area) : String.Empty;
		public string Organization { get; set; }

		public string LeaseInfo { get; set; } // Здесь информация передается в виде 'арендатор|2019-01-01|2019-01-01'
		public string Lessee => String.IsNullOrEmpty(LeaseInfo) ? null : LeaseInfo.Split('|')[0];
		public string LeaseDates => String.IsNullOrEmpty(LeaseInfo) ? null 
			: DateTime.Parse(LeaseInfo.Split('|')[1]).ToShortDateString() + "—" + DateTime.Parse(LeaseInfo.Split('|')[2]).ToShortDateString();
	}
}
