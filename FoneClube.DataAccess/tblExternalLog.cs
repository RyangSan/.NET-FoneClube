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
    
    public partial class tblExternalLog
    {
        public int intId { get; set; }
        public Nullable<int> intIdPerson { get; set; }
        public string txtSource { get; set; }
        public string txtLog { get; set; }
        public Nullable<System.DateTime> dteCreated { get; set; }
        public string txtTransactionId { get; set; }
    }
}
