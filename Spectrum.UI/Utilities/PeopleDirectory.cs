using Spectrum.DataLayers.DataAccess;
using Spectrum.DataLayers.Members.Engineers;
using Spectrum.Models.HumanResources.Employees;
using Spectrum.Models.Members.Engineers;
using SpectrumV1.DataLayers.HumanResources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spectrum.UI.Utilities
{
	internal static class PeopleDirectory
	{
		internal sealed class PersonLookup
		{
			public string Key { get; set; }
			public string FullName { get; set; }
			public string Source { get; set; }
			public string EmployeeId { get; set; }
			public string EngineerId { get; set; }
			public string Country { get; set; }
			public string City { get; set; }
			public string Address { get; set; }
			public string Email { get; set; }
			public string Phone1 { get; set; }
			public string Phone2 { get; set; }
			public string Phone3 { get; set; }
			public string Specialization { get; set; }
			public string Status { get; set; }
		}

		internal static async Task<IList<PersonLookup>> GetPeopleAsync()
		{
			var employeeRepo = new EmployeeRepository(DatabaseFactory.ProfilePrimary);
			var engineerRepo = new EngineerRepository(DatabaseFactory.ProfilePrimary);

			var employeesTask = employeeRepo.GetEmployeesAsync();
			var engineersTask = engineerRepo.GetEngineersAsync();

			await Task.WhenAll(employeesTask, engineersTask);

			var employees = employeesTask.Result ?? new List<EmployeeModel>();
			var engineers = engineersTask.Result ?? new List<EngineerModel>();

			var people = new Dictionary<string, PersonLookup>(StringComparer.OrdinalIgnoreCase);

			foreach (var e in employees)
			{
				var name = (e?.FullName ?? string.Empty).Trim();
				if (string.IsNullOrWhiteSpace(name)) continue;
				var key = NormalizeKey(name);
				if (string.IsNullOrWhiteSpace(key)) continue;

				if (!people.TryGetValue(key, out var p))
				{
					p = new PersonLookup { Key = key };
					people[key] = p;
				}

				p.FullName = name;
				p.Source = string.IsNullOrEmpty(p.Source) ? "Employee" : p.Source;
				p.EmployeeId = e._id;
			}

			foreach (var eng in engineers)
			{
				var name = (eng?.EngineerName ?? string.Empty).Trim();
				if (string.IsNullOrWhiteSpace(name)) continue;
				var key = NormalizeKey(name);
				if (string.IsNullOrWhiteSpace(key)) continue;

				if (!people.TryGetValue(key, out var p))
				{
					p = new PersonLookup { Key = key, FullName = name };
					people[key] = p;
				}

				p.FullName = string.IsNullOrWhiteSpace(p.FullName) ? name : p.FullName;
				p.EngineerId = eng._id;
				p.Country = eng.Country;
				p.City = eng.City;
				p.Address = eng.Address;
				p.Email = eng.Email;
				p.Phone1 = eng.PhoneNumber1;
				p.Phone2 = eng.PhoneNumber2;
				p.Phone3 = eng.PhoneNumber3;
				p.Specialization = eng.Specialization;
				p.Status = eng.Status;

				if (string.IsNullOrWhiteSpace(p.Source)) p.Source = "Engineer";
				else if (!p.Source.Contains("Engineer")) p.Source = p.Source + "+Engineer";
			}

			return people.Values
				.Where(x => !string.IsNullOrWhiteSpace(x.FullName))
				.OrderBy(x => x.FullName)
				.ToList();
		}

		private static string NormalizeKey(string name)
		{
			if (string.IsNullOrWhiteSpace(name)) return null;
			var cleaned = new string(name
				.Trim()
				.ToUpperInvariant()
				.Where(ch => char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch))
				.ToArray());
			cleaned = string.Join(" ", cleaned.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
			return cleaned;
		}
	}
}
