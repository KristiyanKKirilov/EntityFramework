namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ExportPatientDto[]), new XmlRootAttribute("Patients"));
            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
            xmlns.Add(string.Empty, string.Empty);
            StringBuilder sb = new();
            using StringWriter sw = new(sb);

            var patients = context.Patients
                .Where(p => p.PatientsMedicines.Count >= 1 && p.PatientsMedicines.Any(pm => pm.Medicine.ProductionDate > DateTime.Parse(date)))
                .Select(p => new ExportPatientDto() 
                { 
                    FullName = p.FullName,
                    AgeGroup = p.AgeGroup.ToString(),
                    Gender = p.Gender.ToString().ToLower(),
                    Medicines = p.PatientsMedicines
                    .Where(pm => pm.Medicine.ProductionDate > DateTime.Parse(date) && pm.Medicine.PatientsMedicines.Count >= 1)
                    .ToArray()
                    .OrderByDescending(x => x.Medicine.ExpiryDate).ThenBy(x => x.Medicine.Price)
                    .Select(pm => new ExportPatientMedicineDto()
                    {
                        Category = pm.Medicine.Category.ToString().ToLower(),
                        Name = pm.Medicine.Name,
                        Price = $"{pm.Medicine.Price:f2}",
                        Producer = pm.Medicine.Producer,
                        ExpiryDate = pm.Medicine.ExpiryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                    })                    
                    .ToArray()
                }).OrderByDescending(p => p.Medicines.Count())
                        .ThenBy(p => p.FullName)
                   .ToArray();

            serializer.Serialize(sw, patients, xmlns);
            return sb.ToString().TrimEnd();
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicines = context.Medicines
                 .Where(m => m.Category == (Category)medicineCategory && m.Pharmacy.IsNonStop == true)
                 .OrderBy(m => m.Price)
                    .ThenBy(m =>m.Name)
                 .Select(m => new ExportMedicineDto()
                 {
                     Name = m.Name,
                     Price = $"{m.Price:f2}",
                     Pharmacy = new ExportMedicinePharmacyDto()
                     {
                         Name = m.Pharmacy.Name,
                         PhoneNumber = m.Pharmacy.PhoneNumber
                     }
                 }).ToArray();

            string result = JsonConvert.SerializeObject(medicines, Formatting.Indented);

            return result.TrimEnd();

        }
    }
}
