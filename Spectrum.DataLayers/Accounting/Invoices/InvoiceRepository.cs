using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Accounting.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.Invoices
{
    public class InvoiceRepository : IInvoiceRepository, IDisposable
    {
        private readonly IMongoCollection<InvoiceModel> _invoices;
        private const string CollectionName = "Invoices";

        public InvoiceRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _invoices = database.GetCollection<InvoiceModel>(CollectionName);
        }

        public async Task<List<InvoiceModel>> GetInvoicesAsync()
        {
            return await _invoices.Find(invoice => true).ToListAsync();
        }

        public async Task<List<InvoiceModel>> GetInvoicesByProjectIdAsync(string projectId)
        {
            if (string.IsNullOrWhiteSpace(projectId))
            {
                return new List<InvoiceModel>();
            }

            var filter = Builders<InvoiceModel>.Filter.Eq(x => x.ProjectId, projectId);
            return await _invoices.Find(filter).ToListAsync();
        }

        public async Task<InvoiceModel> GetInvoiceByIdAsync(string id)
        {
            var filter = Builders<InvoiceModel>.Filter.Eq(x => x._id, id);
            return await _invoices.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<string> GetNextInvoiceNoAsync(int year)
        {
            var prefix = year + "-";
            var invoices = await _invoices.Find(invoice => invoice.InvoiceNo.StartsWith(prefix))
                .Project(invoice => invoice.InvoiceNo)
                .ToListAsync();

            var nextNumber = invoices
                .Select(invoiceNo => ParseInvoiceSequence(invoiceNo, prefix))
                .DefaultIfEmpty(0)
                .Max() + 1;

            return string.Format("{0}-{1}", year, nextNumber.ToString().PadLeft(6, '0'));
        }

        public async Task<string> AddNewInvoiceAsync(InvoiceModel invoice)
        {
            try
            {
                invoice.CreatedAt = DateTime.UtcNow;
                await _invoices.InsertOneAsync(invoice);
                return invoice._id;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateInvoiceAsync(InvoiceModel invoice)
        {
            try
            {
                var result = await _invoices.ReplaceOneAsync(x => x._id == invoice._id, invoice);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteInvoiceAsync(string id)
        {
            try
            {
                var result = await _invoices.DeleteOneAsync(x => x._id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Check if there's records for invoice document.
        /// </summary>
        public async Task<long> GetCountAsync()
        {
            return await _invoices.CountDocumentsAsync(new BsonDocument());
        }

        private static int ParseInvoiceSequence(string invoiceNo, string prefix)
        {
            if (string.IsNullOrWhiteSpace(invoiceNo) || !invoiceNo.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return 0;

            var sequence = invoiceNo.Substring(prefix.Length);
            return int.TryParse(sequence, out var parsedSequence) ? parsedSequence : 0;
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
