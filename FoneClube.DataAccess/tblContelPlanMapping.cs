//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FoneClube.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblContelPlanMapping
    {
        public int intId { get; set; }
        public string txtPlanName { get; set; }
        public int intIdPlan { get; set; }
        public string txtContelPlanName { get; set; }
        public string txtPrice { get; set; }
        public string txtContelPrice { get; set; }
        public bool bitActive { get; set; }
        public int intIdPerson { get; set; }
        public Nullable<int> intDataMB { get; set; }
    }
}