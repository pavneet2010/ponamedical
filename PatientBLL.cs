using PonaMeds.BLL.BLLInterface;
using PonaMeds.DLL;
using PonaMeds.DLL.DALInterface;
using PonaMeds.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PonaMeds.BLL
{
    public class PatientBLL : IPatientBLL
    {
        private IPatientDal objPatientDal;
        private IAddEditPatient objAddEditPatientDal;

        public PatientBLL()
        {
            objPatientDal = new PatientDal();
            objAddEditPatientDal = new AddEditPatientDAL();
        }

        public List<PatientModel> GetPatientDetails(string UserName)
        {
            List<GetPatientDetails_Result> lstPatient = objPatientDal.PullPatientDetails(UserName);
            List<PatientModel> lstPatientModel = new List<PatientModel>();
            if (lstPatient.Count > 0)
            {
                foreach (var item in lstPatient)
                {
                    PatientModel objPatientModel = new PatientModel();
                    objPatientModel.ipkAccount = Convert.ToInt64(item.ipkAccount);
                    objPatientModel.AccountNo = item.sAccountNo;
                    objPatientModel.Id = item.sPatIdNo;
                    objPatientModel.Initials = item.sPatInitials;
                    objPatientModel.MedicalAidNumber = item.sPatMedAidNo;
                    objPatientModel.Surname = item.sPatSurname;
                    objPatientModel.Name = item.sPatName;
                    objPatientModel.DateOfBirth = item.dPatDOB.ToString("yyyy/MM/dd");
                    objPatientModel.Gender = item.sPatGender;
                    lstPatientModel.Add(objPatientModel);
                }
            }
            return lstPatientModel;
        }

        public bool DeletePatientAccount(string UserName, string ipkAccount)
        {
            return objPatientDal.DeletePatientDetails(UserName, ipkAccount);
        }

        public bool PutPatientDetails(AddEditPatientViewModel patientModel)
        {
            // bool saveAddress = objPatientDal.PushAddressDetail(patientModel.DependentIdNumber, patientModel.SurName, patientModel.Initials, patientModel.FirstName, patientModel.NearestFamilyTiltle, patientModel.Relationship, patientModel.Address1, patientModel.Address2, patientModel.Address3, patientModel.Address4, patientModel.PostalCode, patientModel.HomeNumber, patientModel.WorkNumber, patientModel.CellPhoneNumber, patientModel.FaxNumber, patientModel.Email, patientModel.Comments, Convert.ToInt32(patientModel.AccountNo.Trim()));
            //bool savePatient = objPatientDal.PushPatientDetails(Convert.ToInt32(patientModel.AccountNo),Convert.ToInt16(patientModel.Patient), patientModel.Dependent, patientModel.DependentSurName, patientModel.DependentInitials, patientModel.DependentFirstName,Convert.ToInt16(patientModel.DependentTiltle),Convert.ToDateTime(patientModel.DateofBirth), patientModel.Gender,0,Convert.ToString(patientModel.MedicalAidNo), patientModel.MedicalAids,Convert.ToString(patientModel.HospitalNumber), 0, patientModel.RelationshipsNumber);
            bool savePatient = objPatientDal.PushAccountDetails(patientModel.AccountNo, patientModel.IntroductionDate, patientModel.PatientStatus,
                patientModel.Doctors, patientModel.Hospital, patientModel.RefferingDoctor, patientModel.IdNumber, patientModel.MainMemberTiltle,
                patientModel.MainInitials, patientModel.MainFirstName, patientModel.MainSurName, patientModel.Employer, patientModel.Patient,
                patientModel.Dependent, Convert.ToInt32(patientModel.MainMemberTiltle), patientModel.DependentInitials,
                patientModel.DependentFirstName, patientModel.DependentSurName, patientModel.DependentIdNumber,
                Convert.ToDateTime(patientModel.MainDateofBirth), patientModel.MainGender, patientModel.Relationships, patientModel.Hospitals,
                patientModel.PatientType, patientModel.MedicalAids, patientModel.MedicalAidPlans, patientModel.MedicalAidNo, Convert.ToInt32(patientModel.ServiceScale),
                patientModel.UserID, patientModel.UseOnStatement, Convert.ToString(patientModel.SPatMembershipNo));

            if (savePatient)
                return objPatientDal.PushAddressDetail(patientModel.DependentIdNumber,
                    patientModel.SurName, patientModel.Initials, patientModel.FirstName,
                    patientModel.NearestFamilyTiltle, patientModel.Relationship, patientModel.Address1,
                    patientModel.Address2, patientModel.Address3, patientModel.Address4, patientModel.PostalCode,
                    patientModel.HomeNumber, patientModel.WorkNumber, patientModel.CellPhoneNumber, patientModel.FaxNumber,
                    patientModel.Email, patientModel.Comments, patientModel.AccountNo.ToString());
            return false;
        }

        public bool PutEditedPatientDetails(AddEditPatientViewModel patientModel, string UserID)
        {
            PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities();
            List<Account> lstAccount = new List<Account>();
            lstAccount = dbContextPonaMedicalEntities.Accounts.Where(s => s.sAccountNo == patientModel.AccountNo.ToString()).Distinct().ToList();
            long ifkAccountNo = Convert.ToInt64(lstAccount[0].ipkAccount.ToString());

            bool editAddress = objPatientDal.PushEditedAddressDetail(patientModel.DependentIdNumber, patientModel.SurName,
                patientModel.Initials, patientModel.FirstName, patientModel.NearestFamilyTiltle, patientModel.Relationship,
                patientModel.Address1, patientModel.Address2, patientModel.Address3, patientModel.Address4, patientModel.PostalCode,
                patientModel.HomeNumber, patientModel.WorkNumber, patientModel.CellPhoneNumber, patientModel.FaxNumber, patientModel.Email, patientModel.Comments, ifkAccountNo.ToString());

            if (editAddress)
            {
                if (patientModel.PatientType == null)
                {
                    patientModel.PatientType = "4";
                }
                if (patientModel.PatientStatus == null)
                {
                    patientModel.PatientStatus = "4";
                }
                if (patientModel.RefferingDoctor == null)
                {
                    patientModel.RefferingDoctor = "55";
                }
                return objPatientDal.PushEditedPatientDetails(patientModel.AccountNo, Convert.ToDateTime(patientModel.IntroductionDates), patientModel.PatientStatus,
                    patientModel.Doctors, patientModel.Hospital, patientModel.RefferingDoctor, patientModel.IdNumber, patientModel.MainMemberTiltle, patientModel.MainInitials,
                    patientModel.MainFirstName, patientModel.MainSurName, patientModel.Employer, Convert.ToDateTime(patientModel.DateofBirth),
                    patientModel.MainGender, patientModel.RelationshipsNumber, patientModel.PatientType, patientModel.MedicalAids, patientModel.MedicalAidPlans, patientModel.MedicalAidNos,
                    Convert.ToInt32(patientModel.ServiceScale), UserID, Convert.ToInt32(patientModel.MedicalAidScheme), Convert.ToDateTime(patientModel.MainDateofBirth),
                    Convert.ToBoolean(patientModel.UseOnStatement), patientModel.SPatMembershipNo.ToString());
            }
            return false;
        }

        public bool InsertRecord(HttpPostedFileBase file, string AccountNumber, string UserName)
        {
            return objPatientDal.PushUploadedFile(file, AccountNumber, UserName);
        }

        public List<PatientDocument> GetFileDetails(string accountNumber)
        {
            List<tblFile> lstFiles = objPatientDal.PullPatientFileDetails(accountNumber);
            List<PatientDocument> lstPatientDocs = new List<PatientDocument>();
            foreach (var item in lstFiles)
            {
                PatientDocument objPatientModel = new PatientDocument();
                objPatientModel.FileName = item.sFileName;
                objPatientModel.AccountNumber = item.ifkAccount;
                objPatientModel.FileId = item.iId;
                objPatientModel.UploadedBy = item.UploadedBy;
                objPatientModel.UploadedDateTime = Convert.ToDateTime(item.UploadedDateTime);
                lstPatientDocs.Add(objPatientModel);
            }
            return lstPatientDocs;
        }

        public bool DeleteAccountFile(string AccountNumber, string FileId)
        {
            return objPatientDal.DeleteAccountFileDetails(AccountNumber, FileId);
        }

        public tblFile DownloadFile(string AccountNumber, string FileId)
        {
            return objPatientDal.DownloadAccountFileDetails(AccountNumber, FileId);
        }

        public List<SearchPatientModel> GetsearchPatientDetails(int userId, string searchBy, string SearchText)
        {
            List<SearchPatient1_Result> lstsearchPatient = objPatientDal.GetsearchPatientDetails(userId, searchBy, SearchText);
            List<SearchPatientModel> lstPatientModel = new List<SearchPatientModel>();
            foreach (var item in lstsearchPatient)
            {
                SearchPatientModel objPatientModel = new SearchPatientModel();
                objPatientModel.AccountNo = item.sAccountNo;
                objPatientModel.Id = item.sPatIdNo;
                objPatientModel.Initials = item.sPatInitials;
                objPatientModel.MedicalAidNumber = item.sPatMedAidNo;
                objPatientModel.Surname = item.sPatSurname;
                objPatientModel.Name = item.sPatName;
                objPatientModel.dPatDOB = Convert.ToString(item.dPatDOB);
                objPatientModel.Gender = item.sPatGender;
                lstPatientModel.Add(objPatientModel);
            }
            return lstPatientModel;
        }

        public List<SearchMedAid> GetSearchMedAidDetails(string SearchText)
        {
            List<Pona_SearchMedAid_Result> lstsearchMedAid = objPatientDal.GetSearchMedAidDetails(SearchText);
            List<SearchMedAid> lstMedAidModel = new List<SearchMedAid>();
            foreach (var item in lstsearchMedAid)
            {
                SearchMedAid objMedAidModel = new SearchMedAid();
                objMedAidModel.MedicalAidNo = item.MedicalAidNo;
                objMedAidModel.Name = item.Name;
                objMedAidModel.Code = item.Code;
                objMedAidModel.Administrator = item.Administrator;
                lstMedAidModel.Add(objMedAidModel);
            }
            return lstMedAidModel;
        }


        List<SearchServiceCode> IPatientBLL.GetSearchServiceCode(string searchText, int? UserId)
        {
            List<Pona_SearchServiceCode_Result> lstsearchServiceCode = objPatientDal.GetSearchServiceCode(searchText, UserId);
            List<SearchServiceCode> lstServiceCodeModel = new List<SearchServiceCode>();
            foreach (var item in lstsearchServiceCode)
            {
                SearchServiceCode objServiceCodeModel = new SearchServiceCode();
                objServiceCodeModel.CodingSystem = item.CodingSystem;
                objServiceCodeModel.TransCode = item.TransCode;
                objServiceCodeModel.SCode = item.SCode;
                objServiceCodeModel.Code = item.Code;
                objServiceCodeModel.Name = item.Name;
                lstServiceCodeModel.Add(objServiceCodeModel);
            }
            return lstServiceCodeModel;
        }
        public List<SearchClaims> GetSearchClaim(int UserID, string searchText)
        {
            List<Pona_SearchClaims_Result> lstsearchClaims = objPatientDal.GetSearchClaims(UserID, searchText);
            List<SearchClaims> lstSearchClaimModel = new List<SearchClaims>();
            foreach (var item in lstsearchClaims)
            {
                SearchClaims objClaimModel = new SearchClaims();
                objClaimModel.Claim = item.Claim;
                objClaimModel.date = Convert.ToString(string.Format("{0:yyyy/MM/dd}", item.date));
                objClaimModel.time = Convert.ToString(string.Format("{0:h:m:s}", item.time));
                objClaimModel.SubmittedDate = Convert.ToString(string.Format("{0:yyyy/MM/dd}", item.SubmittedDate));
                objClaimModel.SubmittedTime = Convert.ToString(string.Format("{0:h:m:s}", item.SubmittedTime));
                objClaimModel.ClaimNo = item.ClaimNo;
                objClaimModel.ClaimStatus = item.ClaimStatus;
                objClaimModel.ClaimType = Convert.ToString(item.ClaimType);
                objClaimModel.MResult = item.MResult;
                objClaimModel.CResult = Convert.ToString(item.CResult);
                objClaimModel.CAmount = item.CAmount;
                objClaimModel.PAmount = item.PAmount;
                objClaimModel.LAmount = item.LAmount;
                objClaimModel.AccountNo = item.AccountNo;
                objClaimModel.Name = item.Name;
                objClaimModel.PNumber = item.PNumber;
                objClaimModel.Account = item.Account;
                objClaimModel.Surename = item.Surename;
                lstSearchClaimModel.Add(objClaimModel);
            }
            return lstSearchClaimModel;
        }

        public List<SearchMedAidScheme> GetSearchMedAidScheme(string searchText)
        {
            List<Pona_SearchMedAidScheme_Result> lstsearchMedAidScheme = objPatientDal.GetSearchMedAidScheme(searchText);
            List<SearchMedAidScheme> lstMedAidSchemeModel = new List<SearchMedAidScheme>();
            foreach (var item in lstsearchMedAidScheme)
            {
                SearchMedAidScheme objMedAidSchemeModel = new SearchMedAidScheme();
                objMedAidSchemeModel.MedicalAidScheme = item.MedicalAidScheme;
                objMedAidSchemeModel.MedAidNo = item.MedAidNo;
                objMedAidSchemeModel.Code = item.Code;
                objMedAidSchemeModel.descrip = item.descrip;
                objMedAidSchemeModel.serv = item.serv;
                objMedAidSchemeModel.Note = item.Note;
                objMedAidSchemeModel.Valid = item.Valid;
                lstMedAidSchemeModel.Add(objMedAidSchemeModel);
            }
            return lstMedAidSchemeModel;
        }

        public List<RecentPatients> GetRecentPatientList(int userId)
        {
            List<Pona_GetRecentPatient_Result> lstRecentPatient = objPatientDal.GetRecentPatientList(userId);
            List<RecentPatients> lstRecentPatientModel = new List<RecentPatients>();
            foreach (var item in lstRecentPatient)
            {
                RecentPatients objRecentPatientModel = new RecentPatients();
                objRecentPatientModel.ipkAccount = item.AccountNumber;
                objRecentPatientModel.Name = item.sPatName;
                objRecentPatientModel.Surname = item.sPatSurname;
                objRecentPatientModel.Title = item.Title;
                lstRecentPatientModel.Add(objRecentPatientModel);
            }
            return lstRecentPatientModel;
        }

        public bool InsertDependent(string ipkAccountNumber, string DependentNumber, string DependentTiltle, string DependentInitials, string DependentFirstName, string DependentSurName, string DependentIdNumber, string DateofBirth, string DependentGender, string DependentRelationships, string DependentHospitals)
        {
            return objPatientDal.PushDependentDetails(ipkAccountNumber, DependentNumber, DependentTiltle, DependentInitials, DependentFirstName, DependentSurName, DependentIdNumber, DateofBirth, DependentGender, DependentRelationships, DependentHospitals);
        }

        public DependentModel GetDependentDetails(string ipkAccountNumber, string DependentNumber)
        {
            DependentModel objDependent = new DependentModel();
            Patient objPatient = objPatientDal.PullDependentDetails(ipkAccountNumber, DependentNumber);
            if (objPatient != null)
            {
                objDependent.PatTitle = objAddEditPatientDal.PullSpecificTitle(objPatient.ifkPatTitle);
                objDependent.Relationship = objAddEditPatientDal.PullSpecificRelationship(objPatient.ifkRelationship);
                objDependent.iPatientNumber = objPatient.iPatientNumber;
                objDependent.sPatIdNo = objPatient.sPatIdNo;
                objDependent.sPatSurname = objPatient.sPatSurname;
                objDependent.sPatInitials = objPatient.sPatInitials;
                objDependent.sPatName = objPatient.sPatName;
                objDependent.dPatDOB = objPatient.dPatDOB.HasValue ? objPatient.dPatDOB.Value.ToString("yyyy-MM-dd") : "<not available>";
                objDependent.sPatGender = objPatient.sPatGender;
                objDependent.sHospitalNumber = objPatient.sHospitalNumber;
                // objDependent.ipkPatTitleId = Convert.ToInt32(objPatient.ifkPatTitle);
                return objDependent;
            }
            else
            {
                return null;
            }

        }

        public AddEditPatientViewModel CheckEligibility(string AccountNumber)
        {
            AddEditPatientViewModel objPatientDetails = new AddEditPatientViewModel();
            PonaMedicalSolutionEntities dbContext = new PonaMedicalSolutionEntities();
            List<Account> lstAccount = new List<Account>();
            lstAccount = dbContext.Accounts.Where(s => s.sAccountNo == AccountNumber).Distinct().ToList();
            if (lstAccount.Count > 0)
            {
                objPatientDetails.SPatMembershipNo = lstAccount[0].SPatMembershipNo.ToString();
            }
            //Generate the XML and place into the 



            return objPatientDetails;


        }

        public bool EditDependent(string AccountNumber, string DependentNumber, string DependentTiltle, string DependentInitials, string DependentFirstName, string DependentSurName, string DependentIdNumber, string DateofBirth, string DependentGender, string DependentRelationships, string DependentHospitals)
        {
            return objPatientDal.PushEditedDependentDetails(AccountNumber, DependentNumber, DependentTiltle, DependentInitials, DependentFirstName, DependentSurName, DependentIdNumber, DateofBirth, DependentGender, DependentRelationships, DependentHospitals);
        }
        public List<ConsultationDetails> GetTransactionDetails(string accountNumber)
        {
            List<Transaction> lstTransactions = objPatientDal.PullTransactionDetails(accountNumber);
            List<ConsultationDetails> lstConsultationDetails = new List<ConsultationDetails>();
            PonaMedicalSolutionEntities dbcontext = new PonaMedicalSolutionEntities();
            foreach (var item in lstTransactions)
            {
                ConsultationDetails objConsultationDetails = new ConsultationDetails();
                objConsultationDetails.ServiceDate = item.dTransDate.ToString();
                objConsultationDetails.Nappi = item.sNappi;
                objConsultationDetails.Units = item.dDrugsQuantity;
                objConsultationDetails.NetAmount = item.nGrossAmt;
                objConsultationDetails.PatientAmount = item.nPatAmt;
                objConsultationDetails.OutstandingAmount = item.nOutstandingAmt;
                objConsultationDetails.ClaimNo = dbcontext.Claims.Where(Z => Z.ipkClaims == item.ifkClaims).Select(Z => Z.sClaimsNumber).FirstOrDefault();
                objConsultationDetails.InvoiceNo = item.ifkInvoices.ToString();
                objConsultationDetails.OutstandingAmount = item.nOutstandingAmt;
                objConsultationDetails.claimStatus = dbcontext.ClaimsStatus.Where(Z => Z.ipkClaimsStatus == item.ifkClaimsStatus).Select(Z => Z.sDisplay).FirstOrDefault();
                objConsultationDetails.DrugDescription = item.sDrugsDescription;
                objConsultationDetails.AccoutNumber = item.ifkAccount.ToString();
                lstConsultationDetails.Add(objConsultationDetails);
            }
            return lstConsultationDetails;
        }
        public bool InsertConsultation(string AccountNumber, string ConsultDate, string SpecificCalc, string TariffCode, string Description, string Reference, string Nappy, string Authorisation, string PerformedinHospital, string Chronic, string NHMinutes, string AMinutes, string Units, string StandAloneTranscation, string UnitAmount, string Quantity, string GrossCharges, string PatientNetLiable)
        {
            return objPatientDal.pushConsultation(AccountNumber, ConsultDate, SpecificCalc, TariffCode, Description, Reference, Nappy, Authorisation, PerformedinHospital, Chronic, NHMinutes, AMinutes, Units, StandAloneTranscation, UnitAmount, Quantity, GrossCharges, PatientNetLiable);
        }
        public List<ServiceCode> GetServiceCode()
        {
            List<ServiceCode> lstServiceCode = objPatientDal.PullServiceCode();
            return lstServiceCode;
        }
        public string GetNHminutes(string TarrifCode)
        {
            return objPatientDal.PullNHMinutes(TarrifCode);

        }
        public List<string> GetPatientName(string accountNumber)
        {
            List<string> lstPatient = objPatientDal.PullPatient(accountNumber);

            return lstPatient;
        }
        public List<Anaesthetist> GetDoctors()
        {
            List<Anaesthetist> lstDoctors = objPatientDal.PullDoctors();

            return lstDoctors;
        }
        public List<DiagnosisDetails> GetDiagnosis()
        {
            List<Diagnosi> lstDiagnosis = objPatientDal.PullDiagnosis();
            List<DiagnosisDetails> lstDiagnosisDetails = new List<DiagnosisDetails>();
            if (lstDiagnosis.Count > 0)
            {
                foreach (var item in lstDiagnosis)
                {
                    DiagnosisDetails objDiagnosisDetails = new DiagnosisDetails();
                    objDiagnosisDetails.Diagnosis = item.spkDiagnosis;
                    objDiagnosisDetails.Description = item.sDescription;
                    objDiagnosisDetails.Valid = item.sValid;
                    objDiagnosisDetails.PMB = item.spkPmb;
                    objDiagnosisDetails.ValidPrimary = item.sValidPrimary;
                    objDiagnosisDetails.ValidAsterisk = item.sValidAsterisk;
                    objDiagnosisDetails.ValidDagger = item.sValidDagger;
                    objDiagnosisDetails.ValidSequelae = item.sValidSequelae;
                    objDiagnosisDetails.Gender = item.sGender;
                    objDiagnosisDetails.AgeRange = item.sAgeRange;
                    lstDiagnosisDetails.Add(objDiagnosisDetails);
                }
            }
            return lstDiagnosisDetails;

        }

    }
}