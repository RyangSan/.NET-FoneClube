﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FebrabanContext : DbContext
    {
        public FebrabanContext()
            : base("name=FebrabanContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblBilhetacoesFebraban> tblBilhetacoesFebraban { get; set; }
        public virtual DbSet<tblContasFebraban> tblContasFebraban { get; set; }
        public virtual DbSet<tblDescontosFebraban> tblDescontosFebraban { get; set; }
        public virtual DbSet<tblEnderecosFebraban> tblEnderecosFebraban { get; set; }
        public virtual DbSet<tblResumosFebraban> tblResumosFebraban { get; set; }
        public virtual DbSet<tblServicosFebraban> tblServicosFebraban { get; set; }
        public virtual DbSet<tblTotalizadoresFebraban> tblTotalizadoresFebraban { get; set; }
        public virtual DbSet<tblContasClaro> tblContasClaro { get; set; }
        public virtual DbSet<tblContasClaroLinhaRegistro> tblContasClaroLinhaRegistro { get; set; }
        public virtual DbSet<tblContasClaroRegistroNotaFiscal> tblContasClaroRegistroNotaFiscal { get; set; }
        public virtual DbSet<tblHeadsFebraban> tblHeadsFebraban { get; set; }
    }
}
