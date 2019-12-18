using System;
using Bazar.Domain.Application;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Application
{
	public class DocumentLastNumberMap : ClassMap<DocumentLastNumber>
	{
		public DocumentLastNumberMap()
		{
			Table("document_last_numbers");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.Year).Column("year");
			Map(x => x.LastNumber).Column("number");
			Map(x => x.DocumentType).Column("doc_type").CustomType<DocumentTypeStringType>();
		}
	}
}
