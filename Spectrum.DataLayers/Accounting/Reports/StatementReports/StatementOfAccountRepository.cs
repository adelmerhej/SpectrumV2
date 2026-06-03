using MongoDB.Bson;
using MongoDB.Driver;
using Spectrum.Models.Accounting.Charts;
using Spectrum.Models.Accounting.Journals;
using Spectrum.DataLayers.DataAccess;
using Spectrum.Models.Common.Currencies;
using Spectrum.Models.Accounting.Reports.StatementReports;
using Spectrum.Models.Members.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spectrum.DataLayers.Accounting.Reports.StatementReports
{
    public class StatementOfAccountRepository
    {
        private readonly IMongoCollection<JournalModel> _journals;
        private readonly IMongoCollection<JournalDetailModel> _journalDetails;
        private readonly IMongoCollection<ChartModel> _charts;
        private readonly IMongoCollection<CurrencyModel> _currencies;
        private readonly IMongoCollection<ClientModel> _clients;
        private const string ClientsCollectionName = "Clients";
        private const string JournalsCollectionName = "Journals";
        private const string JournalDetailsCollectionName = "JournalDetails";
        private const string ChartsCollectionName = "Charts";
        private const string CurrenciesCollectionName = "Currencies";

        // Constructor for dependency injection
        public StatementOfAccountRepository(string profileName)
        {
            var database = DatabaseFactory.GetMongoDatabase(profileName);
            _journals = database.GetCollection<JournalModel>(JournalsCollectionName);
            _journalDetails = database.GetCollection<JournalDetailModel>(JournalDetailsCollectionName);
            _charts = database.GetCollection<ChartModel>(ChartsCollectionName);
            _currencies = database.GetCollection<CurrencyModel>(CurrenciesCollectionName);
            _clients = database.GetCollection<ClientModel>(ClientsCollectionName);
        }

        public async Task<IList<StatementOfAccountReportModel>> StatementOfAccountReportAsync(
            DateTime? dateFrom, DateTime? dateTo, string chartId, string currencyId, string journalType,
            string costCenter, string flowType, int workingYear, bool consolidatePastPeriods, bool isProtected)
        {
            var journalFilter = Builders<JournalModel>.Filter.Eq(x => x.Deleted, false);

            if (consolidatePastPeriods)
            {
                if (workingYear > 0)
                {
                    journalFilter &= Builders<JournalModel>.Filter.Lte(x => x.WorkingYear, workingYear);
                }
            }
            else
            {
                journalFilter &= Builders<JournalModel>.Filter.Eq(x => x.WorkingYear, workingYear);
            }

            if (!isProtected)
            {
                journalFilter &= Builders<JournalModel>.Filter.Eq(x => x.IsProtected, false);
            }

            if (dateFrom.HasValue)
            {
                journalFilter &= Builders<JournalModel>.Filter.Gte(x => x.JournalDate, dateFrom.Value.Date);
            }

            if (dateTo.HasValue)
            {
                journalFilter &= Builders<JournalModel>.Filter.Lt(x => x.JournalDate, dateTo.Value.Date.AddDays(1));
            }

            if (!string.IsNullOrWhiteSpace(journalType) && journalType != "0")
            {
                journalFilter &= Builders<JournalModel>.Filter.Eq(x => x.JournalType, journalType);
            }

            var journals = await _journals.Find(journalFilter).ToListAsync();
            if (journals.Count == 0)
            {
                return new List<StatementOfAccountReportModel>();
            }

            string accountNumber = null;

            if (!string.IsNullOrWhiteSpace(chartId) && chartId != "0")
            {
                var chart = await FindChartAsync(chartId);
                if (chart == null || string.IsNullOrWhiteSpace(chart.AccountNumber))
                {
                    return new List<StatementOfAccountReportModel>();
                }

                accountNumber = chart.AccountNumber;
            }

            string currencyCode = null;

            if (!string.IsNullOrWhiteSpace(currencyId) && currencyId != "0")
            {
                var currency = await FindCurrencyAsync(currencyId);
                if (currency == null || string.IsNullOrWhiteSpace(currency.CurrencyCode))
                {
                    return new List<StatementOfAccountReportModel>();
                }

                currencyCode = currency.CurrencyCode;
            }

            var journalsByJvNo = journals
                .Where(x => !string.IsNullOrWhiteSpace(x.JvNo))
                .GroupBy(x => x.JvNo, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);

            var detailEntries = journalsByJvNo.Values
                .SelectMany(journal => (journal.JournalDetails ?? new List<JournalDetailModel>())
                    .Where(detail => detail != null)
                    .Select(detail => (Journal: journal, Detail: detail)))
                .ToList();

            var journalNumbersWithoutDetails = journalsByJvNo.Values
                .Where(journal => journal.JournalDetails == null || journal.JournalDetails.Count == 0)
                .Select(journal => journal.JvNo)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (journalNumbersWithoutDetails.Count > 0)
            {
                var legacyDetailFilter = Builders<JournalDetailModel>.Filter.In(x => x.JvNo, journalNumbersWithoutDetails)
                    & Builders<JournalDetailModel>.Filter.Eq(x => x.Deleted, false);

                if (!string.IsNullOrWhiteSpace(accountNumber))
                {
                    legacyDetailFilter &= Builders<JournalDetailModel>.Filter.Eq(x => x.AccountNumber, accountNumber);
                }

                if (!string.IsNullOrWhiteSpace(currencyCode))
                {
                    legacyDetailFilter &= Builders<JournalDetailModel>.Filter.Eq(x => x.Currency, currencyCode);
                }

                if (!string.IsNullOrWhiteSpace(costCenter) && costCenter != "0")
                {
                    legacyDetailFilter &= Builders<JournalDetailModel>.Filter.Eq(x => x.CostCenter, costCenter);
                }

                if (!string.IsNullOrWhiteSpace(flowType) && flowType != "0")
                {
                    legacyDetailFilter &= Builders<JournalDetailModel>.Filter.Eq(x => x.FlowType, flowType);
                }

                var legacyDetails = await _journalDetails.Find(legacyDetailFilter)
                    .SortBy(x => x.Line)
                    .ToListAsync();

                detailEntries.AddRange(legacyDetails
                    .Where(detail => !string.IsNullOrWhiteSpace(detail.JvNo) && journalsByJvNo.ContainsKey(detail.JvNo))
                    .Select(detail => (Journal: journalsByJvNo[detail.JvNo], Detail: detail)));
            }

            var filteredDetails = detailEntries
                .Where(x => x.Detail != null && !x.Detail.Deleted)
                .Where(x => string.IsNullOrWhiteSpace(x.Detail.JvNo)
                    || string.Equals(x.Detail.JvNo, x.Journal.JvNo, StringComparison.OrdinalIgnoreCase))
                .Where(x => string.IsNullOrWhiteSpace(accountNumber)
                    || string.Equals(x.Detail.AccountNumber, accountNumber, StringComparison.OrdinalIgnoreCase))
                .Where(x => string.IsNullOrWhiteSpace(currencyCode)
                    || string.Equals(x.Detail.Currency, currencyCode, StringComparison.OrdinalIgnoreCase))
                .Where(x => string.IsNullOrWhiteSpace(costCenter)
                    || costCenter == "0"
                    || string.Equals(x.Detail.CostCenter, costCenter, StringComparison.OrdinalIgnoreCase))
                .Where(x => string.IsNullOrWhiteSpace(flowType)
                    || flowType == "0"
                    || string.Equals(x.Detail.FlowType, flowType, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (filteredDetails.Count == 0)
            {
                return new List<StatementOfAccountReportModel>();
            }

            var reportData = filteredDetails
                .Select(entry =>
                {
                    var journal = entry.Journal;
                    var detail = entry.Detail;
                    var number = string.Empty;
                    var serial = string.Empty;

                    if (!string.IsNullOrWhiteSpace(detail.AccountNumber))
                    {
                        var accountNumberParts = detail.AccountNumber
                            .Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                        if (accountNumberParts.Length > 0)
                        {
                            number = accountNumberParts[0];
                        }

                        if (accountNumberParts.Length > 1)
                        {
                            serial = accountNumberParts[1];
                        }
                    }

                    return new StatementOfAccountReportModel
                    {
                        JvNo = ParseInt(journal.JvNo),
                        JournalType = journal.JournalType,
                        JournalDate = journal.JournalDate,
                        Reference = journal.Reference,
                        Currency = journal.Currency,
                        Rate = journal.Rate,
                        IsPosted = journal.IsPosted,
                        Notes = journal.Notes,
                        WorkingYear = journal.WorkingYear,
                        IsProtected = journal.IsProtected,
                        Line = detail.Line,
                        Number = number,
                        Serial = serial,
                        AccountName = detail.AccountName,
                        CostCenter = detail.CostCenter,
                        ValueDate = detail.ValueDate,
                        CurrencyDetail = detail.Currency,
                        RateDetail = detail.Rate,
                        Description = detail.Description,
                        DbCr = detail.DbCr,
                        Amount = detail.Amount,
                        LAmount = detail.LAmount,
                        FAmount = detail.FAmount,
                        DocumentRef = detail.DocumentRef,
                        JobNo = string.Empty,
                        Department = string.Empty
                    };
                })
                .OrderBy(x => x.JournalDate)
                .ThenBy(x => x.JvNo)
                .ThenBy(x => x.Line)
                .ToList();

            return reportData;
        }

        private async Task<ChartModel> FindChartAsync(string chartId)
        {
            if (IsObjectId(chartId))
            {
                return await _charts.Find(x => x._id == chartId).FirstOrDefaultAsync();
            }

            var accountNumberParts = chartId
                .Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

            if (accountNumberParts.Length == 2)
            {
                return await _charts.Find(x => x.Number == accountNumberParts[0] && x.Serial == accountNumberParts[1])
                    .FirstOrDefaultAsync();
            }

            return await _charts.Find(x => x.Number == chartId).FirstOrDefaultAsync();
        }

        private async Task<CurrencyModel> FindCurrencyAsync(string currencyId)
        {
            if (IsObjectId(currencyId))
            {
                return await _currencies.Find(x => x._id == currencyId).FirstOrDefaultAsync();
            }

            return await _currencies.Find(x => x.CurrencyCode == currencyId).FirstOrDefaultAsync();
        }

        private static bool IsObjectId(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && ObjectId.TryParse(value, out _);
        }

        private static int ParseInt(string value)
        {
            int parsedValue;
            return int.TryParse(value, out parsedValue) ? parsedValue : 0;
        }
    }
}
