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
    
    public partial class tblLinesForTopupPool
    {
        public int intId { get; set; }
        public string txtPhone { get; set; }
        public string txtICCID { get; set; }
        public string dteActivated { get; set; }
        public System.DateTime dteAdded { get; set; }
        public string txtData { get; set; }
        public Nullable<int> intRequiredTopup { get; set; }
        public Nullable<int> intRequiredSale { get; set; }
        public bool bitMovedToPool { get; set; }
    }
}