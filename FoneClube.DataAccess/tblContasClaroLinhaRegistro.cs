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
    
    public partial class tblContasClaroLinhaRegistro
    {
        public int intId { get; set; }
        public int intContaId { get; set; }
        public string txtDDD { get; set; }
        public string txtTelefone { get; set; }
        public string txtSecao { get; set; }
        public string txtData { get; set; }
        public string txtHora { get; set; }
        public string txtOrigemUFDestino { get; set; }
        public string txtNumero { get; set; }
        public string txtDuracaoQuantidade { get; set; }
        public string txtTarifa { get; set; }
        public string txtValor { get; set; }
        public string txtValorCobrado { get; set; }
        public string txtNome { get; set; }
        public string txtMatricula { get; set; }
        public string txtSubSecao { get; set; }
        public string txtTipoImposto { get; set; }
        public string txtDescricao { get; set; }
        public string txtNomeLocalOrigem { get; set; }
        public string txtNomeLocalDestino { get; set; }
        public string txtCodigoLocalOrigem { get; set; }
        public string txtCodigoLocalDestino { get; set; }
    
        public virtual tblContasClaro tblContasClaro { get; set; }
    }
}
