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
    
    public partial class tblCieloPaymentStatus
    {
        public int intId { get; set; }
        public string txtPaymentId { get; set; }
        public string txtOrderId { get; set; }
        public Nullable<int> intAmount { get; set; }
        public string txtType { get; set; }
        public Nullable<int> intCustomerId { get; set; }
        public Nullable<int> intServiceTaxAmount { get; set; }
        public Nullable<int> intInstallments { get; set; }
        public string txtInterest { get; set; }
        public Nullable<bool> bitCapture { get; set; }
        public Nullable<bool> bitAuthenticate { get; set; }
        public string txtCardNumber { get; set; }
        public string txtHolder { get; set; }
        public string txtExpirationDate { get; set; }
        public string txtBrand { get; set; }
        public Nullable<bool> bitSaveCard { get; set; }
        public string txtAuthorizationCode { get; set; }
        public string txtProofOfSale { get; set; }
        public string txtCurrency { get; set; }
        public string txtCountry { get; set; }
        public Nullable<int> intStatus { get; set; }
        public Nullable<System.DateTime> dteChargingDate { get; set; }
        public string txtDemostrative { get; set; }
        public string txtUrl { get; set; }
        public string txtBoletoNumber { get; set; }
        public string txtBarCodeNumber { get; set; }
        public string txtDigitableLine { get; set; }
        public string txtAssignor { get; set; }
        public string txtAddress { get; set; }
        public string txtIdentification { get; set; }
        public string txtInstructions { get; set; }
    }
}