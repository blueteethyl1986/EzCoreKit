using System;
using EzCoreKit.MIME.Attributes;

namespace EzCoreKit.MIME {
    public static partial class DeclareMIME {
		/// <summary>
		/// ChemDraw eXchange file
		/// </summary>
		[FileExtName(".cdx")]
		public const string ChemDraw_eXchange_file = "chemical/x-cdx";

		/// <summary>
		/// Chemical Markup Language
		/// </summary>
		[FileExtName(".cml")]
		public const string Chemical_Markup_Language = "chemical/x-cml";

		/// <summary>
		/// Chemical Style Markup Language
		/// </summary>
		[FileExtName(".csml")]
		public const string Chemical_Style_Markup_Language = "chemical/x-csml";

		/// <summary>
		/// Crystallographic Interchange Format
		/// </summary>
		[FileExtName(".cif")]
		public const string Crystallographic_Interchange_Format = "chemical/x-cif";

		/// <summary>
		/// CrystalMaker Data Format
		/// </summary>
		[FileExtName(".cmdf")]
		public const string CrystalMaker_Data_Format = "chemical/x-cmdf";

		/// <summary>
		/// XYZ File Format
		/// </summary>
		[FileExtName(".xyz")]
		public const string XYZ_File_Format = "chemical/x-xyz";

	}
}
