using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class PropertyListViewModel
    {
        /*
         * Etong ModelView na to yung mag hahawak ng data galing sa controller papunta sa view
         */

        //Eto hawak neton yung result ng query sa sa controller
        //public IEnumerable<SalesTransaction>? SalesTransactions { get; set; } // Changed from Properties to SalesTransactions
        public IEnumerable<SalesTransactionWithDocumentsViewModel>? SalesTransactions { get; set; }
        
        //Eto yung sa search bar
        public string? SearchTerm { get; set; }
    }
}
