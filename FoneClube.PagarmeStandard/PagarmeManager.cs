using PagarMe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

using System.Web;
using System.Xml;

namespace FoneClube.PagarmeStandard
{
    public class PagarmeManager
    {
        public Transaction CardCheckout(Transaction transaction)
        {
            PagarMeService.DefaultApiKey = "ak_test_rIMnFMFbwNJR1A5RuTmSULl9xxDdoM"; //mover pra config
            PagarMeService.DefaultEncryptionKey = "ek_test_5rLvyIU3tqMGHKAj94kpCuqSWT37Ps"; //mover pra config

            //transaction.Amount = 100;

            //var creditcard = new CardHash();
            //creditcard.CardHolderName = "Jose da Silvaa";
            //creditcard.CardNumber = "5433229077370451";
            //creditcard.CardExpirationDate = "1038";
            //creditcard.CardCvv = "018";
            
            //transaction.CardHash = creditcard.Generate();

            //transaction.Customer = new PagarMe.Customer()
            //{
            //    Name = "Teste Silva",
            //    Email = "aardvarkirkd@pagarre.me",
            //    DocumentNumber = "51623928478",
            //    BornAt = DateTime.ParseExact("1996-05-08 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture),
            //    Address = new PagarMe.Address()
            //    {
            //        Street = "Avenida Brigadeiro Faria Lima",
            //        StreetNumber = "123",
            //        Neighborhood = "Jardim Paulistano",
            //        Zipcode = "01451001",
            //        State = "Rio de Jneiro",
            //        City = "Rio de Janeiro"
            //    },

            //    Phone = new PagarMe.Phone
            //    {
            //        Ddi = "55",
            //        Ddd = "21",
            //        Number = "23456789"
            //    }
            //};

            transaction.Customer.Save();
            transaction.Save();
            return transaction;
        }
    }
}
