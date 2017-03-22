using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Model;
using POS.ViewModel;

namespace POS.Repo
{
    public class AddTransferItemRepo
    {
        public static List<AddTransferItemViewModel> GetAddTransferItem()
        {
            List<AddTransferItemViewModel> result = new List<AddTransferItemViewModel>();
            using (var context = new POSContext())
            {
                result = (from Item in context.MstItems
                          join ItemVariant in context.MstItemVariants on Item.id equals ItemVariant.itemId into j0

                          from j00 in j0.DefaultIfEmpty()
                          join ItemInventory in context.MstItemInventorys on j00.id equals ItemInventory.variantId into j1

                          from j10 in j1.DefaultIfEmpty()

                          where j10.outletId == 1

                          select new AddTransferItemViewModel
                          {
                              variantId = j10.id,
                              itemName = Item.name,
                              endingQty = j10.endingQty,
                              variantName = j00.name,
                          }).ToList();
            }
            return result;
        }

        public static List<AddTransferItemViewModel> GetAddTransferItemViewBerdasarkanID(int id)
        {
            List<AddTransferItemViewModel> result = new List<AddTransferItemViewModel>();
            using (var context = new POSContext())
            {
                result = (
                          from Item in context.MstItems
                          from ItemVariant in context.MstItemVariants
                          from TransferStockDetail in context.TrxTransferStockDetails

                          where Item.id == ItemVariant.itemId
                          where ItemVariant.id == TransferStockDetail.variantId
                          where TransferStockDetail.headerId == id

                          select new AddTransferItemViewModel
                          {
                              variantId = ItemVariant.id,
                              itemName = Item.name,
                              variantName = ItemVariant.name,
                              transferQty = TransferStockDetail.transferQty,
                              instock = TransferStockDetail.instock

                          }).ToList();
            }
            return result;
        }
    }
}
