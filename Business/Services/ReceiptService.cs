using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReceiptService : IReceiptService
    {
        private IUnitOfWork @object;
        private IMapper mapper;

        public ReceiptService(IUnitOfWork @object, IMapper mapper)
        {
            this.@object = @object;
            this.mapper = mapper;
        }

        public Task AddAsync(ReceiptModel model)
        {
            @object.ReceiptRepository.AddAsync(mapper.Map<ReceiptModel, Receipt>(model));
            @object.SaveAsync();
            return Task.CompletedTask;
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {

            decimal productPrice = 0;

            if (@object.ProductRepository != null)
            {
                var product = await @object.ProductRepository.GetByIdAsync(productId);
                if (product == null)
                    throw new MarketException("Product with this productId doesn`t exists");

                productPrice = product.Price;
            }

            if (@object.ReceiptRepository != null)
            {
                var receipt = await @object.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);

                if (receipt != null && receipt.ReceiptDetails != null)
                {
                    var receiptDetails = receipt.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId);

                    if (receiptDetails != null)
                    {
                        receiptDetails.Quantity += quantity;
                        await @object.SaveAsync();
                        return;
                    }
                }
            }

            decimal discount = (await @object.ReceiptRepository.GetByIdWithDetailsAsync(receiptId)).Customer.DiscountValue;

            ReceiptDetail receiptDetail = new ReceiptDetail()
            {
                ProductId = productId,
                Quantity = quantity,
                ReceiptId = receiptId,
                DiscountUnitPrice = productPrice * ((100 - discount) / 100),
                UnitPrice = productPrice
            };

            await @object.ReceiptDetailRepository.AddAsync(receiptDetail);

            await @object.SaveAsync();
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var receipt = await @object.ReceiptRepository.GetByIdAsync(@receiptId);
            receipt.IsCheckedOut = true;
            await @object.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            var receipt = await @object.ReceiptRepository.GetByIdWithDetailsAsync(modelId);

            foreach (var receiptDetail in receipt.ReceiptDetails)
            {
                @object.ReceiptDetailRepository.Delete(receiptDetail);
            }

            await @object.ReceiptRepository.DeleteByIdAsync(modelId);
            await @object.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var receipt = await @object.ReceiptRepository.GetAllWithDetailsAsync();
            return mapper.Map<IEnumerable<Receipt>, IEnumerable<ReceiptModel>>(receipt);
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            var receipt = await @object.ReceiptRepository.GetByIdWithDetailsAsync(id);
            return mapper.Map<Receipt, ReceiptModel>(receipt);
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var receiptDetail = (await @object.ReceiptRepository.GetByIdWithDetailsAsync(receiptId)).ReceiptDetails;
            return mapper.Map< IEnumerable <ReceiptDetail>, IEnumerable <ReceiptDetailModel> >(receiptDetail);
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receipt = (await @object.ReceiptRepository.GetAllWithDetailsAsync())
                .Where(st => st.OperationDate >= startDate && st.OperationDate <= endDate);
            return mapper.Map< IEnumerable <Receipt>, IEnumerable <ReceiptModel>>(receipt);
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receiptDetails = (await @object.ReceiptRepository.GetByIdWithDetailsAsync(receiptId))
                .ReceiptDetails.Where(r => r.ProductId == productId);
            foreach (var receiptDetail in receiptDetails)
            {
                if (receiptDetail.Quantity <= quantity)
                    @object.ReceiptDetailRepository.Delete(receiptDetail);
                else
                    receiptDetail.Quantity = quantity;
            }
            await @object.SaveAsync();

        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            var receiptDetails = (await @object.ReceiptRepository.GetByIdWithDetailsAsync(receiptId)).ReceiptDetails;

            decimal result = 0;
            foreach (var receiptDetail in receiptDetails)
            {
                result += receiptDetail.Quantity*receiptDetail.DiscountUnitPrice;
            }
            return result;
        }

        public Task UpdateAsync(ReceiptModel model)
        {
            @object.ReceiptRepository.Update(mapper.Map<ReceiptModel, Receipt>(model));
            @object.SaveAsync();
            return Task.CompletedTask;
        }
    }
}
