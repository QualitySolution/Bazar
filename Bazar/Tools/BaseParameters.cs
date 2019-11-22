using System;
using QSSupportLib;

namespace bazar.Tools
{
	public static class BaseParameters
	{
		public static int ReportLastReadingDay => MainSupport.BaseParameters?.All != null && MainSupport.BaseParameters.All.ContainsKey(BaseParameterNames.ReportLastReadingDay.ToString ())
			? int.Parse (MainSupport.BaseParameters.All [BaseParameterNames.ReportLastReadingDay.ToString ()])
			: 15;
	}

	public enum BaseParameterNames
	{
		ReportLastReadingDay
	}
}
