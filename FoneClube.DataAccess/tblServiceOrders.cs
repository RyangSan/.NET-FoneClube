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
    
    public partial class tblServiceOrders
    {
        public Nullable<int> intIdPerson { get; set; }
        public System.DateTime dteRegister { get; set; }
        public string txtDescription { get; set; }
        public Nullable<int> intIdAgent { get; set; }
        public string txtAgentName { get; set; }
        public Nullable<bool> bitPendingInteraction { get; set; }
        public Nullable<bool> bitPending { get; set; }
        public int intIdServiceOrder { get; set; }
    }
}