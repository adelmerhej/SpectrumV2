using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpectrumV1.Models.HumanResources.Candidates;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace SpectrumV1.DataLayers.HumanResources.Employees.Services
{
    public class CvParsingService : ICvParsingService, IDisposable
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiKey;
        private readonly string _model;
        private bool _disposed;

        public CvParsingService(string apiKey, string model = "gpt-4.1")
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException(nameof(apiKey));

            _apiKey = apiKey;
            _model = model;
        }

        #region Public API

        public async Task<CandidateModel> ParseCvAsync(string filePath, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath))
                throw new FileNotFoundException("CV file not found.", filePath);

            // 1. Extract text from file
            var cvText = ExtractText(filePath);

            // 2. Send to AI for parsing
            var jsonResult = await CallAiParser(cvText, cancellationToken).ConfigureAwait(false);

            // 3. Normalize AI JSON before deserialization
            jsonResult = NormalizeCandidateJson(jsonResult);

            // 4. Deserialize into model
            var candidate = JsonConvert.DeserializeObject<CandidateModel>(jsonResult);

            return candidate;
        }

        #endregion

        #region AI Call

        private async Task<string> CallAiParser(string cvText, CancellationToken cancellationToken)
        {
            var prompt = BuildPrompt(cvText);

            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = "You are an expert CV parser. Extract structured data accurately." },
                    new { role = "user", content = prompt }
                },
                response_format = new { type = "json_object" }
            };

            using (var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions"))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                using (var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var parsed = JObject.Parse(responseString);
                    var messageContent = parsed["choices"]?[0]?["message"]?["content"]?.ToString();

                    if (string.IsNullOrEmpty(messageContent))
                        throw new InvalidOperationException("AI response did not contain valid content.");

                    return messageContent;
                }
            }
        }

        #endregion

        #region JSON Normalization

        private string NormalizeCandidateJson(string jsonResult)
        {
            var obj = JObject.Parse(jsonResult);
            NormalizeDateProperty(obj, "DateOfBirth");
            return obj.ToString(Formatting.None);
        }

        private void NormalizeDateProperty(JObject obj, string propertyName)
        {
            var token = obj[propertyName];
            if (token == null || token.Type == JTokenType.Null)
                return;

            var value = token.Type == JTokenType.String ? token.Value<string>() : null;

            if (string.IsNullOrWhiteSpace(value))
            {
                obj[propertyName] = JValue.CreateNull();
                return;
            }

            DateTime parsedDate;
            var formats = new[]
            {
                "yyyy-MM-dd",
                "dd/MM/yyyy",
                "d/M/yyyy",
                "dd-MM-yyyy",
                "d-M-yyyy",
                "MM/dd/yyyy",
                "M/d/yyyy",
                "yyyy/MM/dd"
            };

            if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate) ||
                DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate) ||
                DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedDate))
            {
                obj[propertyName] = parsedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            else
            {
                obj[propertyName] = JValue.CreateNull();
            }
        }

        #endregion

        #region Prompt Builder

        private string BuildPrompt(string cvText)
        {
            return $@"
            Extract structured candidate information from the following CV.

            Return ONLY valid JSON matching this schema:

            {{
              ""FirstName"": string,
              ""LastName"": string,
              ""Email"": string,
              ""Phone"": string,
              ""DateOfBirth"": string,
              ""Gender"": string,
              ""Nationality"": string,
              ""City"": string,
              ""Address"": string,
              ""Position"": string,
              ""YearsOfExperience"": string,
              ""Skills"": string,
              ""Summary"": string,
              ""Education"": [
                {{
                  ""Degree"": string,
                  ""Institution"": string,
                  ""Year"": string,
                  ""Specialization"" : string
                }}
              ],
              ""History"": [
                {{
                  ""Company"": string,
                  ""Position"": string,
                  ""StartDate"": string,
                  ""EndDate"": string
                }}
              ],
              ""Confidence"": number,
              ""RawInsights"": string
            }}

            Rules:
            - If value is missing → return null
            - DateOfBirth must be in yyyy-MM-dd format or null
            - Do NOT hallucinate data
            - Keep Skills as comma-separated string
            - Confidence between 0 and 1

            CV Content:
            -----------------------
            {cvText}
            ";
        }

        #endregion

        #region Text Extraction

        private string ExtractText(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();

            if (extension == ".txt")
                return File.ReadAllText(filePath);

            if (extension == ".pdf")
                return ExtractFromPdf(filePath);

            if (extension == ".docx")
                return ExtractFromDocx(filePath);

            throw new NotSupportedException($"Unsupported file format: {extension}");
        }

        private string ExtractFromPdf(string filePath)
        {
            var sb = new StringBuilder();

            using (var document = PdfDocument.Open(filePath))
            {
                foreach (Page page in document.GetPages())
                {
                    sb.AppendLine(page.Text);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private string ExtractFromDocx(string filePath)
        {
            var sb = new StringBuilder();

            using (var document = WordprocessingDocument.Open(filePath, false))
            {
                var body = document.MainDocumentPart?.Document?.Body;
                if (body == null)
                {
                    return string.Empty;
                }

                foreach (var paragraph in body.Descendants<Paragraph>())
                {
                    var paragraphText = string.Concat(paragraph.Descendants<Text>().Select(t => t.Text)).Trim();

                    if (!string.IsNullOrWhiteSpace(paragraphText))
                    {
                        sb.AppendLine(paragraphText);
                    }
                }
            }

            return sb.ToString();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }

        #endregion
    }
}
