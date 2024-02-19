namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            var patientDtos = JsonConvert.DeserializeObject<ImportPatientDto[]>(jsonString);
            var patients = new List<Patient>();
            StringBuilder sb = new();

            foreach (var patientDto in patientDtos)
            {
                if (!IsValid(patientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if(patientDto.AgeGroup < 0 ||  patientDto.AgeGroup > 2 || patientDto.Gender < 0 || patientDto.Gender > 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient patientToAdd = new() 
                { 
                    FullName = patientDto.FullName,
                    AgeGroup = (AgeGroup)patientDto.AgeGroup,
                    Gender = (Gender)patientDto.Gender
                };

                foreach (var currentMedicine in patientDto.Medicines)
                {
                    if(patientToAdd.PatientsMedicines.Any(pm => pm.MedicineId == currentMedicine))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    patientToAdd.PatientsMedicines.Add(new PatientMedicine()
                    {
                       
                        MedicineId = currentMedicine
                    });
                }
                patients.Add(patientToAdd);
                sb.AppendLine(string.Format(SuccessfullyImportedPatient, patientToAdd.FullName, patientToAdd.PatientsMedicines.Count));
            }

            context.Patients.AddRange(patients);    
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {

            var serializer = new XmlSerializer(typeof(ImportPharmacyDto[]), new XmlRootAttribute("Pharmacies"));
            var pharmaciesDtos = (ImportPharmacyDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new();
            List<Pharmacy> pharmacies = new();

            foreach (var pharmaciesDto in pharmaciesDtos)
            {
                if (!IsValid(pharmaciesDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if(pharmaciesDto.IsNonStop != "true" && pharmaciesDto.IsNonStop != "false")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Pharmacy pharmacy = new Pharmacy()
                {
                    IsNonStop = Convert.ToBoolean(pharmaciesDto.IsNonStop),
                    Name = pharmaciesDto.Name,
                    PhoneNumber = pharmaciesDto.PhoneNumber
                };

                foreach (var medicine in pharmaciesDto.Medicines)
                {
                   
                    if (!IsValid(medicine))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(medicine.Price < 0.01m || medicine.Price > 1000.0m)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if(medicine.Producer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    DateTime productionDate;

                    if(!DateTime.TryParseExact(medicine.ProductionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out productionDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime expiryDate;
                    if(!DateTime.TryParseExact(medicine.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out expiryDate))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(productionDate >= expiryDate || medicine.Category > 4 || medicine.Category < 0)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Medicine medicineToAdd = new Medicine()
                    {
                        Name = medicine.Name,   
                        Price = decimal.Parse(medicine.Price.ToString("f2")),
                        ProductionDate = productionDate,
                        ExpiryDate = expiryDate,
                        Producer = medicine.Producer,
                        Category = (Category)medicine.Category
                    };

                    if(pharmacy.Medicines.Any(m => m.Name == medicineToAdd.Name && m.Producer == medicineToAdd.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    pharmacy.Medicines.Add(medicineToAdd);
                }
                pharmacies.Add(pharmacy);
                sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count()));
            }
            context.Pharmacies.AddRange(pharmacies);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
