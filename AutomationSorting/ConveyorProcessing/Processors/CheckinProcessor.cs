using AutomationSorting.Data;
using AutomationSorting.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationSorting.ConveyorProcessing
{
    public class CheckinProcessor
    {
        IProductRepository _productRepository = RepositoryFactory.GetProductRepository();
        public void Process(ProcessingUnit unit)
        {
            var upcs = unit.Scanned.Where(u => u.UPC != null).ToArray();

            if(upcs.Length > 1)
            {
                unit.Errors.Add(new ProcessingError()
                {
                    Code = ProcessingErrorCodeEnum.MultipleUPC,
                    Message = "Multiple UPCs were found on product: " + string.Join(", ", upcs.Select(u => u.UPC.UPC).ToArray())
                });

                return;
            }

            var slps = unit.Scanned.Where(s => s.SLP != null).ToArray();

            if(slps.Length > 1)
            {
                unit.Errors.Add(new ProcessingError()
                {
                    Code = ProcessingErrorCodeEnum.MultipleSLP,
                    Message = "Multiple SLPs were found on product: " + string.Join(", ", slps.Select(u => u.SLP.SLP).ToArray())
                });

                return;
            }

            if(upcs.Length == 0)
            {
                if(slps.Length == 0)
                {
                    unit.Errors.Add(new ProcessingError()
                    {
                        Code = ProcessingErrorCodeEnum.NoUPC,
                        Message = "UPC was not recoginzed"
                    });

                    return;
                }
            }

            if(slps.Length == 1 && slps[0].SLP.CheckedIn)
            {
                unit.Errors.Add(new ProcessingError()
                {
                    Code = ProcessingErrorCodeEnum.DuplicateSLP,
                    Message = "SLP " + slps[0].SLP.SLP + " was already checked in"
                });

                return;
            }

            unit.Product = new Model.ProductDataModel();

            if(slps.Length == 1)
            {
                unit.Product.CompanyProductID = slps[0].SLP.CompanyProductID;
                unit.Product.SLP = slps[0].SLP.SLP;
                if (upcs.Length == 0)
                    unit.Product.UPC = slps[0].SLP.UPC;
            }

            if (upcs.Length == 1)
            {
                unit.Product.UPC = upcs[0].UPC.PrimaryUPC;
                unit.SortingIndex = upcs[0].UPC.SortingIndex;
            }

            _productRepository.CheckIn(unit.Product);
        }
    }
}
