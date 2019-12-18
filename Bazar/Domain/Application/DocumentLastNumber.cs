using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Application
{
	public class DocumentLastNumber : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private DocumentType documentType;
		[Display(Name = "Вид документа")]
		public virtual DocumentType DocumentType {
			get => documentType;
			set => SetField(ref documentType, value);
		}

		private uint year;
		[Display(Name = "Год")]
		public virtual uint Year {
			get => year;
			set => SetField(ref year, value);
		}

		private uint lastNumber;
		[Display(Name = "Последний номер")]
		public virtual uint LastNumber {
			get => lastNumber;
			set => SetField(ref lastNumber, value);
		}

		#endregion
	}

	public enum DocumentType
	{
		Invoice
	}

	public class DocumentTypeStringType : NHibernate.Type.EnumStringType
	{
		public DocumentTypeStringType() : base(typeof(DocumentType))
		{
		}
	}
}
