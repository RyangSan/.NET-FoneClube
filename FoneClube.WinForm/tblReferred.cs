//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FoneClube.WinForm
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblReferred
    {
        public int intId { get; set; }
        public int intIdComission { get; set; }
        public int intIdDad { get; set; }
        public int intIdCurrent { get; set; }
    
        public virtual tblCommissions tblCommissions { get; set; }
        public virtual tblPersons tblPersons { get; set; }
        public virtual tblPersons tblPersons1 { get; set; }
    }
}
