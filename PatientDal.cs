using PonaMeds.DLL.DALInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;

namespace PonaMeds.DLL
{
    public class PatientDal : IDisposable, IPatientDal
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<GetPatientDetails_Result> PullPatientDetails(string UserName)
        {
            try
            {
                long usernmaelong = Convert.ToInt64(UserName);
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    return dbContextPonaMedicalEntities.GetPatientDetails(UserName).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public bool DeletePatientDetails(string UserName, string ipkAccount)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    string ipkAccounts = ipkAccount;
                    //var deletedRecord = dbContextPonaMedicalEntities.Accounts.Where(m => m.ipkAccount == ipkAccounts).FirstOrDefault();
                    //dbContextPonaMedicalEntities.Accounts.Remove(deletedRecord);
                    //dbContextPonaMedicalEntities.SaveChanges();
                    dbContextPonaMedicalEntities.Pona_DeletePatient(Convert.ToInt32(UserName),ipkAccount);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool PushAccountDetails(string AccountNo, DateTime IntroductionDate, string PatientStatus, string Doctors, string Hospital, string RefferingDoctor,
            string patientIdNumber, long MainMemberTiltle, string MainInitials, string MainFirstName, string MainSurName, string Employer, string Patient, string Dependent,
            int DependentTiltle, string DependentInitials, string DependentFirstName, string DependentSurName, string DependentIdNumber, DateTime DateofBirth, string Gender,
            string Relationships, string Hospitals, string PatientType, string MedicalAids, string MedicalAidPlans, long MedicalAidNo, int ServiceScale, string UserID, bool UseOnStatement, string PatientMembershiopnumber)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    int addressNumber = Convert.ToInt32(dbContextPonaMedicalEntities.Addresses.OrderByDescending(k => k.ipkAddress).Select(k => k.ipkAddress).FirstOrDefault());
                    int? medicalAidPlans = NullableInt(MedicalAidPlans);
                    if(PatientType == null)
                    {
                        PatientType = "4";
                    }
                    if(PatientStatus == null)
                    {
                        PatientStatus = "4";
                    }
                    if (RefferingDoctor == null)
                    {
                        RefferingDoctor = "55";
                    }
                    
                    ObjectResult<Nullable<int>> result = dbContextPonaMedicalEntities.InsertAccount(
                        AccountNo, "", IntroductionDate, 0, Convert.ToInt32(PatientType.Trim()), ServiceScale, Convert.ToInt32(PatientStatus.Trim()),
                        "", "", Convert.ToInt32(Doctors.Trim()), Convert.ToInt32(Hospital.Trim()), Convert.ToInt32(RefferingDoctor.Trim()),
                        patientIdNumber, DependentSurName, DependentInitials, DependentFirstName, DependentTiltle, Employer, addressNumber, Convert.ToInt32(MedicalAidNo),
                        MedicalAidNo.ToString(), IntroductionDate, patientIdNumber, MainSurName, MainInitials, MainFirstName, Convert.ToInt32(MainMemberTiltle), DateofBirth, Gender,
                        0, MedicalAidNo.ToString(), "", "", "", "", DateTime.Now, "", "", ServiceScale, ServiceScale, ServiceScale, 0, 0, 0, 0, Convert.ToByte(UseOnStatement), 0, 0, 0, 0,
                        DateTime.Now.ToString(), "", "", medicalAidPlans, Convert.ToInt32(UserID.Trim()), 1, UserID, PatientMembershiopnumber);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public int? NullableInt(string str)
        {
            int i;
            if (int.TryParse(str, out i))
                return i;
            return null;
        }

        public bool PushAddressDetail(string DependentIdNumber, string Surname, string Initials, string Name, long Title, long Relationship, string Address1, string Address2, string Address3, string Address4, string PostalCode, string TelHome, string TelWork, string TelCell, string TelFax, string Email, string Reamrks, string AccountNumber)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    List<Account> lstAccount = new List<Account>();
                    lstAccount = dbContextPonaMedicalEntities.Accounts.Where(s => s.sAccountNo == AccountNumber).Distinct().ToList();
                    long ifkAccountNo = Convert.ToInt64(lstAccount[0].ipkAccount.ToString());

                    Address objAddress = new Address();
                    objAddress.sDebtorIdNo = "123";
                    objAddress.sSurname = Surname;
                    objAddress.sInitials = Initials;
                    objAddress.sName = Name;
                    objAddress.ifkTitle = Title;
                    objAddress.sRelationship = Convert.ToString(Relationship);
                    objAddress.sAddr1 = Address1;
                    objAddress.sAddr2 = Address2;
                    objAddress.sAddr3 = Address3;
                    objAddress.sAddr4 = Address4;
                    objAddress.sPostalCode = PostalCode;
                    objAddress.sRemarks = Reamrks;
                    objAddress.sEMail = Email;
                    objAddress.sTelCell = TelCell;
                    objAddress.sTelFax = TelFax;
                    objAddress.sTelHome = TelHome;
                    objAddress.sTelWork = TelWork;
                    objAddress.ifkAccount = ifkAccountNo;
                    dbContextPonaMedicalEntities.Addresses.Add(objAddress);
                    dbContextPonaMedicalEntities.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool PushEditedAddressDetail(string DependentIdNumber, string Surname, string Initials, string Name, long Title, long Relationship, string Address1, string Address2, string Address3, string Address4, string PostalCode, string TelHome, string TelWork, string TelCell, string TelFax, string Email, string Reamrks, string AccountNumber)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    long ipkAccountNumber = Convert.ToInt64(AccountNumber);
                    //int Account = Convert.ToInt32(dbContextPonaMedicalEntities.Addresses.Where(k => k.ifkAccount == ipkAccountNumber).Select(k => k.ifkAccount).FirstOrDefault());
                    var objAddress = dbContextPonaMedicalEntities.Addresses.Where(k => k.ifkAccount == ipkAccountNumber).FirstOrDefault() == null ? new Address() : dbContextPonaMedicalEntities.Addresses.Where(k => k.ifkAccount == ipkAccountNumber).FirstOrDefault();
                    objAddress.sDebtorIdNo = "123";
                    objAddress.sSurname = Surname;
                    objAddress.sInitials = Initials;
                    objAddress.sName = Name;
                    objAddress.ifkTitle = Title;
                    objAddress.sRelationship = Convert.ToString(Relationship);
                    objAddress.sAddr1 = Address1;
                    objAddress.sAddr2 = Address2;
                    objAddress.sAddr3 = Address3;
                    objAddress.sAddr4 = Address4;
                    objAddress.sPostalCode = PostalCode;
                    objAddress.sRemarks = Reamrks;
                    objAddress.sEMail = Email;
                    objAddress.sTelCell = TelCell;
                    objAddress.sTelFax = TelFax;
                    objAddress.sTelHome = TelHome;
                    objAddress.sTelWork = TelWork;
                    objAddress.ifkAccount =Convert.ToInt32(AccountNumber);
                    if (objAddress == null)
                        dbContextPonaMedicalEntities.Addresses.Add(objAddress);
                    dbContextPonaMedicalEntities.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool PushEditedPatientDetails(string AccountNo, DateTime IntroductionDate, string PatientStatus,
            string Doctors, string Hospital, string RefferingDoctor, string IdNumber, long MainMemberTiltle, string MainInitials, 
            string MainFirstName, string MainSurName, string Employer, DateTime DateofBirth, string Gender, long? Relationships,
            string PatientType, string MedicalAids, string MedicalAidPlans, string MedicalAidNo, int ServiceScale,string UserID, int MedAidScheme ,DateTime patDOB, bool UseOnStatement, string PatientMembershipNumber)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    Int32 AccountNumbers = Convert.ToInt32(AccountNo);
                    int ipkAccountNo = Convert.ToInt32(dbContextPonaMedicalEntities.Accounts.Where(k => k.sAccountNo == AccountNo).Select(k => k.ipkAccount).FirstOrDefault());

                    int addressNumber = Convert.ToInt32(dbContextPonaMedicalEntities.Addresses.Where(k => k.ifkAccount == AccountNumbers).Select(k => k.ipkAddress).FirstOrDefault());
                    if (MedicalAids == null)
                        MedicalAids = "0";               

                    int result = dbContextPonaMedicalEntities.UpdateAccountDetails(
                        ipkAccountNo,
                        AccountNo,                                   //saccoutno
                        "",                                          //upload file location (we are not using this)
                        IntroductionDate,
                        0,                                           //@ifkAccPeriod
                        34,                                          //@@ifkAccType   
                        2,                                          //@ifkServiceCategory
                        Convert.ToInt32(PatientStatus.Trim()), //@ifkAccStatus
                        "", //@sGroupPracticeNo
                        "", //@sAnaesPracticeNo
                        Convert.ToInt32(Doctors.Trim()), //@ifkAnaesthetist
                        Convert.ToInt32(Hospital.Trim()), //@ifkHospital
                        Convert.ToInt32(RefferingDoctor), //@ifkSurgeon
                        IdNumber, //@sDebtIdNo
                        "", //@sDebtSurname
                        "", //@sDebtSurname
                        "", //@sDebtName
                        Convert.ToInt32(MainMemberTiltle), //@ifkDebtTitle
                        Employer,//@sDebtEmployer
                        addressNumber,//@ifkCurrentAddress
                        Convert.ToInt32(MedicalAids),//@ifkDebtMedAid
                        "",//@sDebtMedAidNo
                        IntroductionDate,// @dFirstBenefitDate
                        IdNumber,//@sPatIdNo
                        MainSurName,//@sPatSurname
                        MainInitials,//@sPatInitials
                        MainFirstName,//@sPatName
                        Convert.ToInt32(MainMemberTiltle),//@ifkPatTitle
                        patDOB, //@dPatDOB
                        Gender, //@sPatGender
                        0, //@tiPatPregnant
                        MedicalAids.ToString(), //@sPatMedAidNo
                        "01",//@sPatMedAidDepNo
                        "",//@sWCACompanyName
                        "", // @sWCACompanyReg
                        "", //@sWCAEmployeeNo
                        DateTime.Now,//@dWCAInjuryDate
                        "", //@sWCAClaimNo
                        "",//@sWCARemarks
                        ServiceScale, //@ifkServiceScalePreOp
                        ServiceScale, //@ifkServiceScaleOp
                        ServiceScale, //@ifkServiceScalePostOp
                        0, //@tiAHCase
                        0, //@tiChargeInterest
                        0, //@iInterestDays
                        0, //@nInterestRate
                        Convert.ToByte(UseOnStatement), //@tiStatementPrint  -- use on stetment
                        0, //@tiEDISubmit
                        0, //@tiNotify
                        0, //@tiHandOver
                        0, //@tiLocked
                        "",//@sText1
                        "",//@sText2
                        "", //@sText3
                        MedAidScheme, //@ifkMedAidScheme
                        Convert.ToInt32(UserID),//@ifkDoneBy
                        PatientMembershipNumber);

                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool PushUploadedFile(HttpPostedFileBase file, string AccountNumber, string UserName)
        {
            string filename = Path.GetFileName(file.FileName);
            string contentType = file.ContentType;
            using (Stream fs = file.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                    {
                        tblFile objFile = new tblFile();
                        objFile.sFileName = filename;
                        objFile.sFileType = contentType;
                        objFile.File_Data = bytes;
                        objFile.ifkAccount = AccountNumber.ToString();
                        objFile.UploadedBy = UserName;
                        objFile.UploadedDateTime = DateTime.Now;
                        dbContextPonaMedicalEntities.tblFiles.Add(objFile);
                        dbContextPonaMedicalEntities.SaveChanges();
                        return true;
                    }
                }
            }
        }

        public List<tblFile> PullPatientFileDetails(string accountNumber)
        {
            using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
            {
                long AccountNo = Convert.ToInt32(accountNumber);
                return dbContextPonaMedicalEntities.tblFiles.Where(k => k.ifkAccount == accountNumber).ToList();
            }
        }

        public bool DeleteAccountFileDetails(string AccountNumber, string FileId)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    int fileId = Convert.ToInt32(FileId);
                    var deletedRecord = dbContextPonaMedicalEntities.tblFiles.Where(m => m.iId == fileId).FirstOrDefault();
                    dbContextPonaMedicalEntities.tblFiles.Remove(deletedRecord);
                    dbContextPonaMedicalEntities.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public tblFile DownloadAccountFileDetails(string AccountNumber, string FileId)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    int fileId = Convert.ToInt32(FileId);
                    var Record = dbContextPonaMedicalEntities.tblFiles.Where(m => m.ifkAccount == AccountNumber && m.iId == fileId).FirstOrDefault();
                    return Record;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public List<SearchPatient1_Result> GetsearchPatientDetails(int userId, string searchBy, string SearchText)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    return dbContextPonaMedicalEntities.SearchPatient1(userId, searchBy, SearchText).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public List<Pona_SearchServiceCode_Result> GetSearchServiceCode(string searchText, int? UserId)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    return dbContextPonaMedicalEntities.Pona_SearchServiceCode(searchText, UserId).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public List<Pona_SearchMedAid_Result> GetSearchMedAidDetails(string searchText)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    return dbContextPonaMedicalEntities.Pona_SearchMedAid(searchText).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public List<Pona_SearchClaims_Result> GetSearchClaims(int UserID, string searchText)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    return dbContextPonaMedicalEntities.Pona_SearchClaims(searchText, UserID).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public List<Pona_SearchMedAidScheme_Result> GetSearchMedAidScheme(string searchText)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    return dbContextPonaMedicalEntities.Pona_SearchMedAidScheme(searchText).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public List<Pona_GetRecentPatient_Result> GetRecentPatientList(int userId)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    return dbContextPonaMedicalEntities.Pona_GetRecentPatient(userId).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public bool PushPatientDetails(string AccountNumber, int? PatientNumber, string PatientIdNo, string PatientSurname, string PatientInitials, string PatientName, int? PatientTitle, string PatientDOB, string PatientGender, byte? PatientPregnant, string PatientMedAidNo, string PatientMedAidDepNo, string HospitalNumber, int? ifkDoneBy, long? Relationship)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    //int addressNumber = Convert.ToInt32(dbContextPonaMedicalEntities.Addresses.OrderByDescending(k => k.ipkAddress).Select(k => k.ipkAddress).FirstOrDefault());
                    int result = dbContextPonaMedicalEntities.InsertPatients(AccountNumber, PatientNumber, PatientIdNo, PatientSurname, PatientInitials, PatientName, PatientTitle, Convert.ToDateTime(PatientDOB), PatientGender, PatientPregnant, PatientMedAidNo, PatientMedAidDepNo, HospitalNumber, ifkDoneBy, Relationship);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool PushEditedPatientDetails(string AccountNumber, int? PatientNumber, 
            string PatientIdNo, string PatientSurname, string PatientInitials, string PatientName, int? PatientTitle, DateTime? PatientDOB,
            string PatientGender, byte? PatientPregnant, string PatientMedAidNo, string PatientMedAidDepNo, string HospitalNumber, int? ifkDoneBy, long? Relationship)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    int result = dbContextPonaMedicalEntities.UpdatePatients(AccountNumber, PatientNumber, PatientIdNo, PatientSurname, PatientInitials, PatientName, PatientTitle, PatientDOB, PatientGender, PatientPregnant, PatientMedAidNo, PatientMedAidDepNo, HospitalNumber, ifkDoneBy, Relationship);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public bool PushDependentDetails(string ipkAccountNumber, string DependentNumber, string DependentTiltle, string DependentInitials,
            string DependentFirstName, string DependentSurName, string DependentIdNumber, string DateofBirth, string DependentGender,
            string DependentRelationships, string DependentHospitals)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    Patient objPatient = new Patient();
                    objPatient.ifkAccount = Convert.ToInt64(ipkAccountNumber);
                    objPatient.iPatientNumber = Convert.ToInt32(DependentNumber);
                    objPatient.ifkPatTitle = Convert.ToInt32(DependentTiltle);
                    objPatient.sPatInitials = DependentInitials;
                    objPatient.sPatName = DependentFirstName;
                    objPatient.sPatSurname = DependentSurName;
                    objPatient.sPatIdNo = DependentIdNumber;
                    objPatient.sPatGender = DependentGender;
                    objPatient.sHospitalNumber = DependentHospitals;
                    objPatient.ifkRelationship = Convert.ToInt32(DependentRelationships);
                    objPatient.dPatDOB = Convert.ToDateTime(DateofBirth);
                    dbContextPonaMedicalEntities.Patients.Add(objPatient);
                    dbContextPonaMedicalEntities.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public Patient PullDependentDetails(string AccountNumber, string DependentNumber)
        {
            using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
            {
                long AccountNo = Convert.ToInt32(AccountNumber);
                int DependentNo = Convert.ToInt32(DependentNumber);
                return dbContextPonaMedicalEntities.Patients.Where(k => k.ifkAccount == AccountNo && k.iPatientNumber == DependentNo).FirstOrDefault();
            }
        }
        public bool PushEditedDependentDetails(string AccountNumber, string DependentNumber, string DependentTiltle, 
            string DependentInitials, string DependentFirstName, string DependentSurName, string DependentIdNumber, 
            string DateofBirth, string DependentGender, string DependentRelationships, string DependentHospitals)
        {

            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    long AccountNo = Convert.ToInt32(AccountNumber);
                    int DependentNo = Convert.ToInt32(DependentNumber);
                    var objPatient = dbContextPonaMedicalEntities.Patients.Where(k => k.ifkAccount == AccountNo && k.iPatientNumber == DependentNo).FirstOrDefault();
                    objPatient.ifkAccount = Convert.ToInt64(AccountNumber);
                    objPatient.iPatientNumber = Convert.ToInt32(DependentNumber);
                    objPatient.ifkPatTitle = Convert.ToInt32(DependentTiltle);
                    objPatient.sPatInitials = DependentInitials;
                    objPatient.sPatName = DependentFirstName;
                    objPatient.sPatSurname = DependentSurName;
                    objPatient.sPatIdNo = DependentIdNumber;
                    objPatient.sPatGender = DependentGender;
                    objPatient.sHospitalNumber = DependentHospitals;
                    objPatient.ifkRelationship = Convert.ToInt32(DependentRelationships);
                    objPatient.dPatDOB = Convert.ToDateTime(DateofBirth);
                    //dbContextPonaMedicalEntities.Patients.Add(objPatient);
                    dbContextPonaMedicalEntities.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }
        public List<Transaction> PullTransactionDetails(string accountNumber)
        {
            using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
            {
                long AccountNo = Convert.ToInt32(accountNumber);
                return dbContextPonaMedicalEntities.Transactions.Where(k => k.ifkAccount == AccountNo).ToList();
            }
        }
        public List<Transaction> PullTransactionDetailsForSelectedClaim(string accountNumber, string ClaimNumber)
        {
            using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
            {
                long AccountNo = Convert.ToInt32(accountNumber);
                int ClaimNumberInt = Convert.ToInt32(ClaimNumber);
                return dbContextPonaMedicalEntities.Transactions.Where(k => k.ifkAccount == AccountNo && k.ifkClaims == ClaimNumberInt).ToList();
            }
        }
        public bool pushConsultation(string AccountNumber,string ConsultDate, string SpecificCalc, string TariffCode, string Description, string Reference, string Nappy, string Authorisation, string PerformedinHospital, string Chronic, string NHMinutes, string AMinutes, string Units, string StandAloneTranscation, string UnitAmount, string Quantity, string GrossCharges, string PatientNetLiable)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    dbContextPonaMedicalEntities.InsertTransaction(Convert.ToInt16(AccountNumber),0,"",Convert.ToDateTime(ConsultDate),0,Convert.ToInt32(3022),133,1,0, Convert.ToByte(PerformedinHospital.ToLower()=="true"?1:0),0,DateTime.Now,DateTime.Now,Convert.ToDouble(Units), Convert.ToDouble(NHMinutes),Convert.ToDouble(AMinutes),Convert.ToDouble(UnitAmount),0,0,0,0,0, Authorisation, Reference,Description,"",0,0,"","","",0,0,"",Nappy,"","");
                    return true;
                }
                   
            }
            catch(Exception ex)
            {
                log.Error(ex);
                return false;
            }
           
        }
        public List<ServiceCode> PullServiceCode()
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {

                    return dbContextPonaMedicalEntities.ServiceCodes.ToList();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public string PullNHMinutes(string TarrifCode)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    double sCode = Convert.ToDouble(TarrifCode);
                    return dbContextPonaMedicalEntities.TransCodes.Where(k => k.sCode == TarrifCode).FirstOrDefault().nRateUnits.ToString();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
                return null;
            }
           
        }
        public List<string> PullPatient(string accountNumber)
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    List<string> lstPatientName = new List<string>();
                    long accountNumbers = Convert.ToInt32(accountNumber);
                    List<Account> listAccount = new List<Account>();
                    string accountPatientName = "";
                    string patientName = "";
                    string title = "";
                    List<Patient> listDependants = new List<Patient>();

                    listAccount = dbContextPonaMedicalEntities.Accounts.Where(k => k.ipkAccount == accountNumbers).ToList();

                    foreach (var item in listAccount)
                    {
                        title = dbContextPonaMedicalEntities.Titles.Where(s => s.ipkTitle == item.ifkPatTitle).Select(s => s.sTitleLong).FirstOrDefault();
                        accountPatientName = title + ". " + item.sPatSurname + "," + item.sPatName;
                        lstPatientName.Add(accountPatientName);
                    }

                    listDependants = dbContextPonaMedicalEntities.Patients.Where(k => k.ifkAccount == accountNumbers).ToList();
                    if (listDependants.Count > 0)
                    {
                        foreach (var item in listDependants)
                        {
                            title = dbContextPonaMedicalEntities.Titles.Where(s => s.ipkTitle == item.ifkPatTitle).Select(s => s.sTitleLong).FirstOrDefault();
                            patientName = title + ". " + item.sPatSurname + "," + item.sPatName;
                            lstPatientName.Add(patientName);
                        }
                    }                    
                    return lstPatientName;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }

        }
        public List<Anaesthetist> PullDoctors()
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    var doctors = dbContextPonaMedicalEntities.Anaesthetists.ToList();
                    return doctors;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public List<Diagnosi> PullDiagnosis()
        {
            try
            {
                using (PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities())
                {
                    var doctors = dbContextPonaMedicalEntities.Diagnosis.ToList();
                    return doctors;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}