using PonaMeds.BLL;
using PonaMeds.BLL.BLLInterface;
using PonaMeds.DLL;
using PonaMeds.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PonaMeds.Web.UI.Controllers;
using System.Web.Hosting;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Xml;
using System.Web.Configuration;

namespace PonaMeds.Controllers
{
    [HandleError]
    public class PatientController : Controller
    {
        //Log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IPatientBLL objIPatientBLL;
        private IAddEditPatientBLL objAddEditPatientBLL;

        public PatientController()
        {
            objIPatientBLL = new PatientBLL();
            objAddEditPatientBLL = new AddEditPAtientDetailBLL();
        }

        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }

        private PonaMedicalSolutionEntities db = new PonaMedicalSolutionEntities();

        // GET: Accounts
        public ActionResult PatientLists(string UserName)
        {
            try
            {
                UserName = Session["UserName"].ToString(); //Need to be changed as per logic to be implemented
                PatientViewModel lstPatientModel = new PatientViewModel();
                lstPatientModel.PatientModel = objIPatientBLL.GetPatientDetails(UserName);
                return View(lstPatientModel);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Session["ErrorDetails"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult PatientConsultation(string accountNumber)

        {
            try
            {
                PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities();
                List<Account> lstAccount = new List<Account>();
                lstAccount = dbContextPonaMedicalEntities.Accounts.Where(s => s.sAccountNo == accountNumber).Distinct().ToList();
                string ifkAccountNo = lstAccount[0].ipkAccount.ToString();
                int practiceID = Convert.ToInt32(lstAccount[0].PracticeId.ToString());
                long IntIfkAccountNo = Convert.ToInt32(ifkAccountNo);
                string strpracticeID = practiceID.ToString();
                Session["AccountNumber"] = accountNumber.ToString();
                PonaMedicalSolutionEntities dbContext = new PonaMedicalSolutionEntities();
                dbContext.Pona_UpdateLastAccessedDate(accountNumber);

                List<AddEditPatientViewModel> lstAddEditPatientViewModel = new List<AddEditPatientViewModel>();
                List<SelectListItem> patientStatus = new List<SelectListItem>();
                List<SelectListItem> hospitals = new List<SelectListItem>();
                List<SelectListItem> doctors = new List<SelectListItem>();
                List<SelectListItem> referringDoctors = new List<SelectListItem>();
                List<SelectListItem> address = new List<SelectListItem>();
                List<SelectListItem> title = new List<SelectListItem>();
                List<SelectListItem> relationship = new List<SelectListItem>();
                List<SelectListItem> gender = new List<SelectListItem>();
                List<SelectListItem> patientType = new List<SelectListItem>();
                List<SelectListItem> medicalAid = new List<SelectListItem>();
                List<SelectListItem> medicalAidPlans = new List<SelectListItem>();
                List<SelectListItem> medicalAidNumber = new List<SelectListItem>();
                List<SelectListItem> serviceScale = new List<SelectListItem>();
                List<SelectListItem> lstPatient = new List<SelectListItem>();
                SelectListItem objSelectListMale = new SelectListItem() { Text = "Male", Value = "Male" };
                SelectListItem objSelectListFemale = new SelectListItem() { Text = "Female", Value = "Female" };
                gender.Add(objSelectListMale);
                gender.Add(objSelectListFemale);
                IQueryable<Account> objAccount;
                IQueryable<Address> objAddress;
                lstAddEditPatientViewModel = objAddEditPatientBLL.GetPatientDetails(ifkAccountNo, out objAccount, out objAddress);

                List<AccStatu> lstAccStatus = dbContextPonaMedicalEntities.AccStatus.Where(k => k.sValid == "Y").Distinct().ToList();
                List<Hospital> lstHospitals = dbContextPonaMedicalEntities.Hospitals.Distinct().ToList();
                List<Anaesthetist> lstAnaesthetist = dbContextPonaMedicalEntities.Anaesthetists.Where(k => k.PracticeId == practiceID).ToList();
                List<Surgeon> lstSurgeon = dbContextPonaMedicalEntities.Surgeons.Where(k => k.sPracticeNo == strpracticeID).ToList();
                List<Address> lstAddress = dbContextPonaMedicalEntities.Addresses.Where(k => k.ifkAccount == IntIfkAccountNo).ToList();
                List<Title> lstTitle = dbContextPonaMedicalEntities.Titles.Distinct().ToList();
                List<Relationship> lstRelationship = dbContextPonaMedicalEntities.Relationships.Distinct().ToList();
                List<AccType> lstPatientType = dbContextPonaMedicalEntities.AccTypes.Distinct().ToList();
                List<MedAid> lstMedAids = dbContextPonaMedicalEntities.MedAids.Distinct().ToList();
                List<MedAidScheme> lstMedAidplans = dbContextPonaMedicalEntities.MedAidSchemes.Distinct().ToList();
                List<ServiceScale> lstServiceScale = dbContextPonaMedicalEntities.ServiceScales.Distinct().ToList();

                AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();

                if (lstAccStatus != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkAccStatus;
                    foreach (var item in lstAccStatus)
                    {
                        SelectListItem objSelectListItem = new SelectListItem() { Text = item.sDescription.ToString(), Value = item.ipkAccStatus.ToString(), Selected = item.ipkAccStatus == SelectedItem };
                        patientStatus.Add(objSelectListItem);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }
                if (lstHospitals != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkHospital;
                    foreach (var item in lstHospitals)
                    {
                        SelectListItem objSelectListItemhospital = new SelectListItem() { Text = item.sName, Value = item.ipkHospital.ToString(), Selected = item.ipkHospital == SelectedItem };
                        hospitals.Add(objSelectListItemhospital);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }

                if (lstAnaesthetist != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkAnaesthetist;
                    foreach (var item in lstAnaesthetist)
                    {
                        SelectListItem objSelectListItemDoctor = new SelectListItem() { Text = item.sSurname + " , " + item.sSurname + "," + item.sName, Value = item.ipkAnaesthetist.ToString(), Selected = item.ipkAnaesthetist == SelectedItem };
                        doctors.Add(objSelectListItemDoctor);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }


                if (lstSurgeon != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkSurgeon;
                    foreach (var item in lstSurgeon)
                    {
                        SelectListItem objSelectListItemReferringDoctor = new SelectListItem() { Text = item.sSurname + " , " + item.sName, Value = item.ipkSurgeon.ToString(), Selected = item.ipkSurgeon == SelectedItem };
                        referringDoctors.Add(objSelectListItemReferringDoctor);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }

                if (lstTitle != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkPatTitle;
                    foreach (var item in lstTitle)
                    {
                        SelectListItem objSelectListItemTitle = new SelectListItem() { Text = item.sTitleLong, Value = item.ipkTitle.ToString(), Selected = item.ipkTitle == SelectedItem };
                        title.Add(objSelectListItemTitle);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }
                if (lstAddress != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkCurrentAddress;
                    foreach (var item in lstAddress)
                    {
                        SelectListItem objSelectListItemAddress = new SelectListItem() { Text = item.sAddr1.ToString(), Value = item.ipkAddress.ToString(), Selected = item.ipkAddress == SelectedItem };
                        address.Add(objSelectListItemAddress);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }
                if (lstRelationship != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkRelationship;
                    foreach (var item in lstRelationship)
                    {
                        SelectListItem objSelectListItemRelationship = new SelectListItem() { Text = item.sDescription.ToString(), Value = item.ipkRelationship.ToString(), Selected = item.ipkRelationship == SelectedItem };
                        relationship.Add(objSelectListItemRelationship);
                    }
                }
                if (lstPatientType != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkAccType;
                    foreach (var item in lstPatientType)
                    {
                        SelectListItem objSelectListItemPatientType = new SelectListItem() { Text = item.sDescription, Value = item.ipkAccType.ToString(), Selected = item.ipkAccType == SelectedItem };
                        patientType.Add(objSelectListItemPatientType);
                    }
                }
                if (lstMedAids != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkDebtMedAid;
                    foreach (var item in lstMedAids)
                    {
                        SelectListItem objSelectListItemMedicalAids = new SelectListItem() { Text = item.sName, Value = item.ipkMedAid.ToString(), Selected = item.ipkMedAid == SelectedItem };
                        medicalAid.Add(objSelectListItemMedicalAids);
                    }
                }
                if (lstMedAidplans != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkMedAidScheme;
                    foreach (var item in lstMedAidplans)
                    {
                        SelectListItem objSelectListItemMedicalAidPlan = new SelectListItem() { Text = item.sDescription, Value = item.ipkMedAidScheme.ToString(), Selected = item.ipkMedAidScheme.ToString() == SelectedItem.ToString() };
                        medicalAidPlans.Add(objSelectListItemMedicalAidPlan);
                    }
                }
                if (lstServiceScale != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkServiceScaleOp;
                    foreach (var item in lstServiceScale)
                    {
                        SelectListItem objSelectListItemServiceScale = new SelectListItem() { Text = item.sDescription, Value = item.ipkServiceScale.ToString(), Selected = item.ipkServiceScale == SelectedItem };
                        serviceScale.Add(objSelectListItemServiceScale);
                    }
                }
                if (lstMedAids != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().sPatMedAidNo;
                    foreach (var item in lstMedAids)
                    {
                        SelectListItem objSelectListItemMedicalAidNumber = new SelectListItem() { Text = item.ipkMedAid.ToString(), Value = item.ipkMedAid.ToString(), Selected = item.ipkMedAid.ToString() == SelectedItem };
                        medicalAidNumber.Add(objSelectListItemMedicalAidNumber);
                    }
                }
                List<SelectListItem> lstPatients = new List<SelectListItem>();
                IQueryable<Patient> objPatient = objAddEditPatientBLL.GetPatients(ifkAccountNo);
                foreach (var item in objPatient)
                {
                    SelectListItem objPatients = new SelectListItem() { Text = item.iPatientNumber.ToString(), Value = item.iPatientNumber.ToString(), Selected = item.iPatientNumber == objPatient.FirstOrDefault().iPatientNumber };
                    lstPatients.Add(objPatients);
                }

                if (accountNumber != null) objAddEditPatientViewModel.ddlPatient = lstPatients;
                else objAddEditPatientViewModel.ddlPatient = lstPatient;
                foreach (var item in gender)
                {

                    if (item.Value.ToLower() == objAccount.SingleOrDefault().sPatGender.ToLower())
                    {
                        item.Selected = true;
                    }
                }
                DependentModel objDependent = objIPatientBLL.GetDependentDetails(ifkAccountNo, lstPatients.Where(x => x.Selected == true).Select(a => a.Value).FirstOrDefault());
                if (objDependent != null)
                {
                    objDependent.Age = CalculateAge(Convert.ToDateTime(objDependent.dPatDOB));
                    objAddEditPatientViewModel.DependentTiltle = objDependent.PatTitle;
                    objAddEditPatientViewModel.EditRelationships = objDependent.Relationship;
                    objAddEditPatientViewModel.Dependent = objDependent.iPatientNumber.ToString();
                    objAddEditPatientViewModel.DependentInitials = objDependent.sPatInitials;
                    objAddEditPatientViewModel.DependentFirstName = objDependent.sPatName;
                    objAddEditPatientViewModel.DependentSurName = objDependent.sPatSurname;
                    objAddEditPatientViewModel.DependentIdNumber = objDependent.sPatIdNo;
                    objAddEditPatientViewModel.DependentDateofBirth = objDependent.dPatDOB;
                    objAddEditPatientViewModel.EditGender = objDependent.sPatGender;
                    objAddEditPatientViewModel.DependentAge = objDependent.Age;
                    objAddEditPatientViewModel.Hospitals = objDependent.sHospitalNumber;
                }
                objAddEditPatientViewModel.AccountNo = objAccount.SingleOrDefault().sAccountNo;
                objAddEditPatientViewModel.IntroductionDates = objAccount.SingleOrDefault().dAccDate.ToString("yyyy-MM-dd");
                objAddEditPatientViewModel.MainMemberTiltle = objAccount.SingleOrDefault().ifkPatTitle;
                objAddEditPatientViewModel.IdNumber = objAccount.SingleOrDefault().sPatIdNo;
                objAddEditPatientViewModel.MainInitials = objAccount.SingleOrDefault().sPatInitials;
                objAddEditPatientViewModel.MainFirstName = objAccount.SingleOrDefault().sPatName;
                objAddEditPatientViewModel.MainSurName = objAccount.SingleOrDefault().sPatSurname;
                objAddEditPatientViewModel.Employer = objAccount.SingleOrDefault().sDebtEmployer;
                objAddEditPatientViewModel.MainDateofBirth = objAccount.SingleOrDefault().dPatDOB.ToString("yyyy-MM-dd");
                objAddEditPatientViewModel.UseOnStatement = Convert.ToBoolean(objAccount.SingleOrDefault().tiStatementPrint);
                //  objAddEditPatientViewModel.RelationshipsNumber = objPatient.SingleOrDefault().ifkRelationship;
                if (objAddress.Any())
                {
                    objAddEditPatientViewModel.Email = objAddress.SingleOrDefault().sEMail;
                    objAddEditPatientViewModel.WorkNumber = objAddress.SingleOrDefault().sTelWork;
                    objAddEditPatientViewModel.CellPhoneNumber = objAddress.SingleOrDefault().sTelCell;
                    objAddEditPatientViewModel.FaxNumber = objAddress.SingleOrDefault().sTelFax;
                    objAddEditPatientViewModel.HomeNumber = objAddress.SingleOrDefault().sTelHome;
                    objAddEditPatientViewModel.PostalCode = objAddress.SingleOrDefault().sPostalCode;
                    objAddEditPatientViewModel.Address1 = objAddress.SingleOrDefault().sAddr1;
                    objAddEditPatientViewModel.Address2 = objAddress.SingleOrDefault().sAddr2;
                    objAddEditPatientViewModel.Address3 = objAddress.SingleOrDefault().sAddr3;
                    objAddEditPatientViewModel.Address4 = objAddress.SingleOrDefault().sAddr4;
                    objAddEditPatientViewModel.NearestFamilyTiltle = objAddress.SingleOrDefault().ifkTitle;
                    objAddEditPatientViewModel.Initials = objAddress.SingleOrDefault().sInitials;
                    objAddEditPatientViewModel.FirstName = objAddress.SingleOrDefault().sName;
                    objAddEditPatientViewModel.SurName = objAddress.SingleOrDefault().sSurname;
                    objAddEditPatientViewModel.Relationship = Convert.ToInt32(objAddress.SingleOrDefault().sRelationship);
                    objAddEditPatientViewModel.Comments = objAddress.SingleOrDefault().sRemarks;
                }
                objAddEditPatientViewModel.lastServiceDate = objAccount.SingleOrDefault().dLastServiceDate;
                int numberOfDays = CalculateDays(objAddEditPatientViewModel.lastServiceDate.GetValueOrDefault());
                objAddEditPatientViewModel.Current = objAccount.SingleOrDefault().nCurrent;
                objAddEditPatientViewModel.Total = objAccount.SingleOrDefault().nTotalBilled;
                objAddEditPatientViewModel.ddlPatientStatus = patientStatus;
                objAddEditPatientViewModel.ddlHospital = hospitals;
                objAddEditPatientViewModel.ddlDoctors = doctors;
                objAddEditPatientViewModel.ddlRefferingDoctor = referringDoctors;
                objAddEditPatientViewModel.ddlAddressDetail = address;
                objAddEditPatientViewModel.ddlTitle = title;
                objAddEditPatientViewModel.ddlMainMemberTitle = title;
                objAddEditPatientViewModel.ddlDependentTiltle = title;
                objAddEditPatientViewModel.ddlRelationship = relationship;
                objAddEditPatientViewModel.ddlNearestFriendRelationship = relationship;
                objAddEditPatientViewModel.ddlPatientType = patientType;
                objAddEditPatientViewModel.ddlMedicalAids = medicalAid;
                objAddEditPatientViewModel.ddlMedicalAidPlans = medicalAidPlans;
                objAddEditPatientViewModel.ddlMedicalAidNo = medicalAidNumber;
                objAddEditPatientViewModel.ddlServiceScale = serviceScale;
                objAddEditPatientViewModel.ddlGender = gender;
                objAddEditPatientViewModel.SPatMembershipNo = objAccount.SingleOrDefault().SPatMembershipNo;

                List<PatientDocument> listPatientDocuments = new List<PatientDocument>();
                PatientBLL objPatientBll = new PatientBLL();
                listPatientDocuments = objPatientBll.GetFileDetails(Session["AccountNumber"].ToString());
                if (listPatientDocuments.Count > 0)
                {
                    objAddEditPatientViewModel.PatientDocs = listPatientDocuments;
                }
                List<ConsultationDetails> lstConsultationDetails = new List<ConsultationDetails>();
                lstConsultationDetails = objPatientBll.GetTransactionDetails(ifkAccountNo);
                if (lstConsultationDetails.Count > 0)
                {
                    objAddEditPatientViewModel.ConsultationDetails = lstConsultationDetails;
                }
                List<ServiceCode> lstServiceCode = new List<ServiceCode>();
                lstServiceCode = objPatientBll.GetServiceCode();
                List<SelectListItem> lstServiceCodes = new List<SelectListItem>();
                if (lstServiceCode != null)
                {
                    foreach (var item in lstServiceCode)
                    {
                        SelectListItem objSelectListItems = new SelectListItem() { Text = item.sCode.ToString(), Value = item.sName.ToString() };
                        lstServiceCodes.Add(objSelectListItems);
                    }
                }
                AddConsultationViewModel objAddConsultationViewModel = new AddConsultationViewModel();
                objAddConsultationViewModel.ddlTariffCode = lstServiceCodes;

                List<string> listPatient = new List<string>();
                listPatient = objPatientBll.GetPatientName(accountNumber);
                List<SelectListItem> listPatients = new List<SelectListItem>();
                if (listPatient != null)
                {
                    foreach (var item in listPatient)
                    {
                        SelectListItem objSelectListItems = new SelectListItem() { Text = item, Value = item };
                        listPatients.Add(objSelectListItems);
                    }
                }
                objAddConsultationViewModel.ddlPatient = listPatients;
                List<Anaesthetist> lstDoctors = new List<Anaesthetist>();
                lstDoctors = objPatientBll.GetDoctors();
                List<SelectListItem> listDoctor = new List<SelectListItem>();
                if (lstDoctors != null)
                {
                    foreach (var item in lstDoctors)
                    {
                        SelectListItem objSelectListItems = new SelectListItem() { Text = item.sInitials+" "+item.sName+" "+item.sSurname, Value = item.ipkAnaesthetist.ToString() };
                        listDoctor.Add(objSelectListItems);
                    }
                }
                objAddConsultationViewModel.ddlDoctor = listDoctor;
                objAddEditPatientViewModel.AddConsultation = objAddConsultationViewModel;
                objAddEditPatientViewModel.DiagnosisDetails = objPatientBll.GetDiagnosis();
                return View(objAddEditPatientViewModel);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Session["ErrorDetails"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult AddPatient(string AccountNumber)
        {
            try
            {
                int practiceID = Convert.ToInt32(Session["PracticeID"]);
                string strpracticeID = Convert.ToString(Session["PracticeID"]);

                AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
                PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities();
                List<AddEditPatientViewModel> lstAddEditPatientViewModel = new List<AddEditPatientViewModel>();

                List<SelectListItem> patientStatus = new List<SelectListItem>();
                List<SelectListItem> hospitals = new List<SelectListItem>();
                List<SelectListItem> doctors = new List<SelectListItem>();
                List<SelectListItem> referringDoctors = new List<SelectListItem>();
                List<SelectListItem> address = new List<SelectListItem>();
                List<SelectListItem> title = new List<SelectListItem>();
                List<SelectListItem> relationship = new List<SelectListItem>();
                List<SelectListItem> gender = new List<SelectListItem>();
                List<SelectListItem> patientType = new List<SelectListItem>();
                List<SelectListItem> medicalAid = new List<SelectListItem>();
                List<SelectListItem> medicalAidPlans = new List<SelectListItem>();
                List<SelectListItem> medicalAidNumber = new List<SelectListItem>();
                List<SelectListItem> serviceScale = new List<SelectListItem>();
                List<SelectListItem> lstPatient = new List<SelectListItem>();
                lstAddEditPatientViewModel = objAddEditPatientBLL.GetPatientStatus(practiceID);
                SelectListItem objSelectListMale = new SelectListItem() { Text = "Male", Value = "Male" };
                SelectListItem objSelectListFemale = new SelectListItem() { Text = "Female", Value = "Female" };
                gender.Add(objSelectListMale);
                gender.Add(objSelectListFemale);

                List<AccStatu> lstAccStatus = dbContextPonaMedicalEntities.AccStatus.Where(k => k.sValid == "Y").Distinct().ToList();
                List<Hospital> lstHospitals = dbContextPonaMedicalEntities.Hospitals.Distinct().ToList();
                List<Anaesthetist> lstAnaesthetist = dbContextPonaMedicalEntities.Anaesthetists.Where(k => k.PracticeId == practiceID).ToList();
                List<Surgeon> lstSurgeon = dbContextPonaMedicalEntities.Surgeons.Where(k => k.sPracticeNo == strpracticeID).ToList();
                //List<Address> lstAddress = dbContextPonaMedicalEntities.Addresses.Where(k => k.ifkAccount == IntIfkAccountNo).ToList();
                List<Title> lstTitle = dbContextPonaMedicalEntities.Titles.Distinct().ToList();
                List<Relationship> lstRelationship = dbContextPonaMedicalEntities.Relationships.Distinct().ToList();
                List<AccType> lstPatientType = dbContextPonaMedicalEntities.AccTypes.Distinct().ToList();
                List<MedAid> lstMedAids = dbContextPonaMedicalEntities.MedAids.Distinct().ToList();
                List<MedAidScheme> lstMedAidplans = dbContextPonaMedicalEntities.MedAidSchemes.Distinct().ToList();
                List<ServiceScale> lstServiceScale = dbContextPonaMedicalEntities.ServiceScales.Distinct().ToList();



                if (lstAccStatus != null)
                {
                    foreach (var item in lstAccStatus)
                    {
                        SelectListItem objSelectListItem = new SelectListItem() { Text = item.sDescription.ToString(), Value = item.ipkAccStatus.ToString()};
                        patientStatus.Add(objSelectListItem);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }
                if (lstHospitals != null)
                {
                    foreach (var item in lstHospitals)
                    {
                        SelectListItem objSelectListItemhospital = new SelectListItem() { Text = item.sName, Value = item.ipkHospital.ToString() };
                        hospitals.Add(objSelectListItemhospital);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }

                if (lstAnaesthetist != null)
                {
                    foreach (var item in lstAnaesthetist)
                    {
                        SelectListItem objSelectListItemDoctor = new SelectListItem() { Text = item.sSurname + " , " + item.sName, Value = item.ipkAnaesthetist.ToString() };
                        doctors.Add(objSelectListItemDoctor);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }


                if (lstSurgeon != null)
                {
                    foreach (var item in lstSurgeon)
                    {
                        SelectListItem objSelectListItemReferringDoctor = new SelectListItem() { Text = item.sSurname + " , " + item.sName, Value = item.ipkSurgeon.ToString() };
                        referringDoctors.Add(objSelectListItemReferringDoctor);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }

                if (lstTitle != null)
                {
                    foreach (var item in lstTitle)
                    {
                        SelectListItem objSelectListItemTitle = new SelectListItem() { Text = item.sTitleLong, Value = item.ipkTitle.ToString() };
                        title.Add(objSelectListItemTitle);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }               
                if (lstRelationship != null)
                {
                    foreach (var item in lstRelationship)
                    {
                        SelectListItem objSelectListItemRelationship = new SelectListItem() { Text = item.sDescription.ToString(), Value = item.ipkRelationship.ToString() };
                        relationship.Add(objSelectListItemRelationship);
                    }
                }
                if (lstPatientType != null)
                {
                    foreach (var item in lstPatientType)
                    {
                        SelectListItem objSelectListItemPatientType = new SelectListItem() { Text = item.sDescription, Value = item.ipkAccType.ToString()};
                        patientType.Add(objSelectListItemPatientType);
                    }
                }
                if (lstMedAids != null)
                {
                    foreach (var item in lstMedAids)
                    {
                        SelectListItem objSelectListItemMedicalAids = new SelectListItem() { Text = item.sName, Value = item.ipkMedAid.ToString()};
                        medicalAid.Add(objSelectListItemMedicalAids);
                    }
                }
                if (lstMedAidplans != null)
                {
                    foreach (var item in lstMedAidplans)
                    {
                        SelectListItem objSelectListItemMedicalAidPlan = new SelectListItem() { Text = item.sDescription, Value = item.ipkMedAidScheme.ToString()};
                        medicalAidPlans.Add(objSelectListItemMedicalAidPlan);
                    }
                }
                if (lstServiceScale != null)
                {
                    foreach (var item in lstServiceScale)
                    {
                        SelectListItem objSelectListItemServiceScale = new SelectListItem() { Text = item.sDescription, Value = item.ipkServiceScale.ToString()};
                        serviceScale.Add(objSelectListItemServiceScale);
                    }
                }

                if (lstMedAids != null)
                {
                    foreach (var item in lstMedAidplans)
                    {
                        SelectListItem objSelectListItemMedicalAidNumber = new SelectListItem() { Text = item.ifkMedAid.ToString(), Value = item.ifkMedAid.ToString() };
                        medicalAidNumber.Add(objSelectListItemMedicalAidNumber);
                    }
                }
                List<SelectListItem> lstPatients = new List<SelectListItem>();
                IQueryable<Patient> objPatient = objAddEditPatientBLL.GetPatients(AccountNumber);
                foreach (var item in objPatient)
                {
                    SelectListItem objPatients = new SelectListItem() { Text = item.iPatientNumber.ToString(), Value = item.iPatientNumber.ToString(), Selected = item.sPatName == objPatient.FirstOrDefault().sPatName };
                    lstPatients.Add(objPatients);
                }
                if (AccountNumber != null) objAddEditPatientViewModel.ddlPatient = lstPatients;
                else objAddEditPatientViewModel.ddlPatient = lstPatient;
                objAddEditPatientViewModel.AccountNo = objAddEditPatientBLL.GetAccountNumber();
                objAddEditPatientViewModel.ddlPatientStatus = patientStatus;
                objAddEditPatientViewModel.ddlHospital = hospitals;
                objAddEditPatientViewModel.ddlDoctors = doctors;
                objAddEditPatientViewModel.ddlRefferingDoctor = doctors;
                objAddEditPatientViewModel.ddlAddressDetail = address;
                objAddEditPatientViewModel.ddlTitle = title;
                objAddEditPatientViewModel.ddlMainMemberTitle = title;
                objAddEditPatientViewModel.ddlDependentTiltle = title;
                objAddEditPatientViewModel.ddlRelationship = relationship;
                objAddEditPatientViewModel.ddlNearestFriendRelationship = relationship;
                //objAddEditPatientViewModel.ddlPatient = lstPatient;
                objAddEditPatientViewModel.ddlPatientType = patientType;
                objAddEditPatientViewModel.ddlMedicalAids = medicalAid;
                objAddEditPatientViewModel.ddlMedicalAidPlans = medicalAidPlans;
                objAddEditPatientViewModel.ddlMedicalAidNo = medicalAidNumber;
                objAddEditPatientViewModel.ddlServiceScale = serviceScale;
                objAddEditPatientViewModel.ddlGender = gender;
                return View(objAddEditPatientViewModel);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Session["ErrorDetails"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        public bool DeletePatient(string ipkAccount)
        {
            string UserName = Session["UserName"].ToString();
            return objIPatientBLL.DeletePatientAccount(UserName, ipkAccount);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SaveNewPatient(AddEditPatientViewModel model)
        {
            try
            {
                model.UserID = Session["UserName"].ToString();
                bool patientSaved = objIPatientBLL.PutPatientDetails(model);
                if (patientSaved)
                    this.AddToastMessage("Success", "Patient details Added Successfully!.", ToastType.Success);
                else
                    this.AddToastMessage("Error", "There was problem in adding Patient, Please try again.", ToastType.Error);
                PonaMedicalSolutionEntities dbContext = new PonaMedicalSolutionEntities();
                dbContext.Pona_UpdateLastAccessedDate(model.AccountNo);
                return RedirectToAction("PatientLists", "Patient");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Session["ErrorDetails"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult EditPatients(string accountNumber)
        {
            try
            {
                PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities();
                List<Account> lstAccount = new List<Account>();
                lstAccount = dbContextPonaMedicalEntities.Accounts.Where(s => s.sAccountNo == accountNumber).Distinct().ToList();
                string ifkAccountNo = lstAccount[0].ipkAccount.ToString();
                int practiceID = Convert.ToInt32(lstAccount[0].PracticeId.ToString());
                long IntIfkAccountNo = Convert.ToInt32(ifkAccountNo);
                string strpracticeID = practiceID.ToString();

                Session["AccountNumber"] = accountNumber.ToString();
                PonaMedicalSolutionEntities dbContext = new PonaMedicalSolutionEntities();
                dbContext.Pona_UpdateLastAccessedDate(accountNumber);
                AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
                List<AddEditPatientViewModel> lstAddEditPatientViewModel = new List<AddEditPatientViewModel>();
                List<SelectListItem> patientStatus = new List<SelectListItem>();
                List<SelectListItem> hospitals = new List<SelectListItem>();
                List<SelectListItem> doctors = new List<SelectListItem>();
                List<SelectListItem> referringDoctors = new List<SelectListItem>();
                List<SelectListItem> address = new List<SelectListItem>();
                List<SelectListItem> title = new List<SelectListItem>();
                List<SelectListItem> relationship = new List<SelectListItem>();
                List<SelectListItem> gender = new List<SelectListItem>();
                List<SelectListItem> patientType = new List<SelectListItem>();
                List<SelectListItem> medicalAid = new List<SelectListItem>();
                List<SelectListItem> medicalAidPlans = new List<SelectListItem>();
                List<SelectListItem> medicalAidNumber = new List<SelectListItem>();
                List<SelectListItem> serviceScale = new List<SelectListItem>();
                List<SelectListItem> lstPatient = new List<SelectListItem>();
                SelectListItem objSelectListMale = new SelectListItem() { Text = "Male", Value = "Male" };
                SelectListItem objSelectListFemale = new SelectListItem() { Text = "Female", Value = "Female" };
                gender.Add(objSelectListMale);
                gender.Add(objSelectListFemale);
                IQueryable<Account> objAccount;
                IQueryable<Address> objAddress;
                lstAddEditPatientViewModel = objAddEditPatientBLL.GetPatientDetails(ifkAccountNo, out objAccount, out objAddress);
                List<AccStatu> lstAccStatus = dbContextPonaMedicalEntities.AccStatus.Where(k => k.sValid == "Y").Distinct().ToList();
                List<Hospital> lstHospitals = dbContextPonaMedicalEntities.Hospitals.Distinct().ToList();
                List<Anaesthetist> lstAnaesthetist = dbContextPonaMedicalEntities.Anaesthetists.Where(k => k.PracticeId == practiceID).ToList();
                List<Surgeon> lstSurgeon = dbContextPonaMedicalEntities.Surgeons.Where(k => k.sPracticeNo == strpracticeID).ToList();
                List<Address> lstAddress = dbContextPonaMedicalEntities.Addresses.Where(k => k.ifkAccount == IntIfkAccountNo).ToList();
                List<Title> lstTitle = dbContextPonaMedicalEntities.Titles.Distinct().ToList();
                List<Relationship> lstRelationship = dbContextPonaMedicalEntities.Relationships.Distinct().ToList();
                List<AccType> lstPatientType = dbContextPonaMedicalEntities.AccTypes.Distinct().ToList();
                List<MedAid> lstMedAids = dbContextPonaMedicalEntities.MedAids.Distinct().ToList();
                List<MedAidScheme> lstMedAidplans = dbContextPonaMedicalEntities.MedAidSchemes.Distinct().ToList();
                List<ServiceScale> lstServiceScale = dbContextPonaMedicalEntities.ServiceScales.Distinct().ToList();

                if (lstAccStatus != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkAccStatus;
                    foreach (var item in lstAccStatus)
                    {
                        SelectListItem objSelectListItem = new SelectListItem() { Text = item.sDescription.ToString(), Value = item.ipkAccStatus.ToString(), Selected = item.ipkAccStatus == SelectedItem };
                        patientStatus.Add(objSelectListItem);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }
                if (lstHospitals != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkHospital;
                    foreach (var item in lstHospitals)
                    {
                        SelectListItem objSelectListItemhospital = new SelectListItem() { Text = item.sName, Value = item.ipkHospital.ToString(), Selected = item.ipkHospital == SelectedItem };
                        hospitals.Add(objSelectListItemhospital);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }

                if (lstAnaesthetist != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkAnaesthetist;
                    foreach (var item in lstAnaesthetist)
                    {
                        SelectListItem objSelectListItemDoctor = new SelectListItem() { Text = item.sSurname+" , "+ item.sName, Value = item.ipkAnaesthetist.ToString(), Selected = item.ipkAnaesthetist == SelectedItem };
                        doctors.Add(objSelectListItemDoctor);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }


                if (lstSurgeon != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkSurgeon;
                    foreach (var item in lstSurgeon)
                    {
                        SelectListItem objSelectListItemReferringDoctor = new SelectListItem() { Text = item.sSurname + " , " + item.sName, Value = item.ipkSurgeon.ToString(), Selected = item.ipkSurgeon == SelectedItem };
                        referringDoctors.Add(objSelectListItemReferringDoctor);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }

                if (lstTitle != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkPatTitle;
                    foreach (var item in lstTitle)
                    {
                        SelectListItem objSelectListItemTitle = new SelectListItem() { Text = item.sTitleLong, Value = item.ipkTitle.ToString(), Selected = item.ipkTitle == SelectedItem };
                        title.Add(objSelectListItemTitle);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }
                if (lstAddress != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkCurrentAddress;
                    foreach (var item in lstAddress)
                    {
                        SelectListItem objSelectListItemAddress = new SelectListItem() { Text = item.sAddr1.ToString(), Value = item.ipkAddress.ToString(), Selected = item.ipkAddress == SelectedItem };
                        address.Add(objSelectListItemAddress);
                        lstAddEditPatientViewModel.Add(objAddEditPatientViewModel);
                    }
                }
                if (lstRelationship != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkRelationship;
                    foreach (var item in lstRelationship)
                    {
                        SelectListItem objSelectListItemRelationship = new SelectListItem() { Text = item.sDescription.ToString(), Value = item.ipkRelationship.ToString(), Selected = item.ipkRelationship == SelectedItem };
                        relationship.Add(objSelectListItemRelationship);
                    }
                }
                if (lstPatientType != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkAccType;
                    foreach (var item in lstPatientType)
                    {
                        SelectListItem objSelectListItemPatientType = new SelectListItem() { Text = item.sDescription, Value = item.ipkAccType.ToString(), Selected = item.ipkAccType == SelectedItem };
                        patientType.Add(objSelectListItemPatientType);
                    }
                }
                if (lstMedAids != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkDebtMedAid;
                    foreach (var item in lstMedAids)
                    {
                        SelectListItem objSelectListItemMedicalAids = new SelectListItem() { Text = item.sName, Value = item.ipkMedAid.ToString(), Selected = item.ipkMedAid == SelectedItem };
                        medicalAid.Add(objSelectListItemMedicalAids);
                    }
                }
                if (lstMedAidplans != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkMedAidScheme;
                    foreach (var item in lstMedAidplans)
                    {
                        SelectListItem objSelectListItemMedicalAidPlan = new SelectListItem() { Text = item.sDescription, Value = item.ipkMedAidScheme.ToString(), Selected = item.ipkMedAidScheme.ToString() == SelectedItem.ToString() };
                        medicalAidPlans.Add(objSelectListItemMedicalAidPlan);
                    }
                }
                if (lstServiceScale != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().ifkServiceScaleOp;
                    foreach (var item in lstServiceScale)
                    {
                        SelectListItem objSelectListItemServiceScale = new SelectListItem() { Text = item.sDescription, Value = item.ipkServiceScale.ToString(), Selected = item.ipkServiceScale == SelectedItem };
                        serviceScale.Add(objSelectListItemServiceScale);
                    }
                }
                if (lstMedAids != null)
                {
                    var SelectedItem = objAccount.SingleOrDefault().sPatMedAidNo;
                    foreach (var item in lstMedAids)
                    {
                        SelectListItem objSelectListItemMedicalAidNumber = new SelectListItem() { Text = item.ipkMedAid.ToString(), Value = item.ipkMedAid.ToString(), Selected = item.ipkMedAid.ToString() == SelectedItem };
                        medicalAidNumber.Add(objSelectListItemMedicalAidNumber);
                    }
                }

                List<SelectListItem> lstPatients = new List<SelectListItem>();

                IQueryable<Patient> objPatient = objAddEditPatientBLL.GetPatients(ifkAccountNo);
                foreach (var item in objPatient)
                {
                    SelectListItem objPatients = new SelectListItem() { Text = item.iPatientNumber.ToString(), Value = item.iPatientNumber.ToString(), Selected = item.iPatientNumber == objPatient.FirstOrDefault().iPatientNumber };
                    lstPatients.Add(objPatients);
                }
                if (accountNumber != null) objAddEditPatientViewModel.ddlPatient = lstPatients;
                else objAddEditPatientViewModel.ddlPatient = lstPatient;
                foreach (var item in gender)
                {

                    if (item.Value.ToLower() == objAccount.SingleOrDefault().sPatGender.ToLower())
                    {
                        item.Selected = true;
                    }
                }

                DependentModel objDependent = objIPatientBLL.GetDependentDetails(ifkAccountNo, lstPatients.Where(x => x.Selected == true).Select(a => a.Value).FirstOrDefault());
                if (objDependent != null)
                {
                    objDependent.Age = CalculateAge(Convert.ToDateTime(objDependent.dPatDOB));
                    objAddEditPatientViewModel.DependentTiltle = objDependent.PatTitle;
                    objAddEditPatientViewModel.EditRelationships = objDependent.Relationship;
                    objAddEditPatientViewModel.Dependent = objDependent.iPatientNumber.ToString();
                    objAddEditPatientViewModel.DependentInitials = objDependent.sPatInitials;
                    objAddEditPatientViewModel.DependentFirstName = objDependent.sPatName;
                    objAddEditPatientViewModel.DependentSurName = objDependent.sPatSurname;
                    objAddEditPatientViewModel.DependentIdNumber = objDependent.sPatIdNo;
                    objAddEditPatientViewModel.DependentDateofBirth = objDependent.dPatDOB;
                    objAddEditPatientViewModel.EditGender = objDependent.sPatGender;
                    objAddEditPatientViewModel.DependentAge = objDependent.Age;
                    objAddEditPatientViewModel.Hospitals = objDependent.sHospitalNumber;
                }

                objAddEditPatientViewModel.AccountNo = objAccount.SingleOrDefault().sAccountNo;
                objAddEditPatientViewModel.IntroductionDates = objAccount.SingleOrDefault().dAccDate.ToString("yyyy-MM-dd");
                objAddEditPatientViewModel.MainMemberTiltle = objAccount.SingleOrDefault().ifkPatTitle;
                objAddEditPatientViewModel.IdNumber = objAccount.SingleOrDefault().sPatIdNo;
                objAddEditPatientViewModel.MainInitials = objAccount.SingleOrDefault().sPatInitials;
                objAddEditPatientViewModel.MainFirstName = objAccount.SingleOrDefault().sPatName;
                objAddEditPatientViewModel.MainSurName = objAccount.SingleOrDefault().sPatSurname;
                objAddEditPatientViewModel.Employer = objAccount.SingleOrDefault().sDebtEmployer;
                objAddEditPatientViewModel.MainDateofBirth = objAccount.SingleOrDefault().dPatDOB.ToString("yyyy-MM-dd");
                objAddEditPatientViewModel.UseOnStatement = Convert.ToBoolean(objAccount.SingleOrDefault().tiStatementPrint);
                //  objAddEditPatientViewModel.RelationshipsNumber = objPatient.SingleOrDefault().ifkRelationship;
                if (objAddress.Any())
                {
                    objAddEditPatientViewModel.Email = objAddress.FirstOrDefault().sEMail;
                    objAddEditPatientViewModel.WorkNumber = objAddress.FirstOrDefault().sTelWork;
                    objAddEditPatientViewModel.CellPhoneNumber = objAddress.FirstOrDefault().sTelCell;
                    objAddEditPatientViewModel.FaxNumber = objAddress.FirstOrDefault().sTelFax;
                    objAddEditPatientViewModel.HomeNumber = objAddress.FirstOrDefault().sTelHome;
                    objAddEditPatientViewModel.PostalCode = objAddress.FirstOrDefault().sPostalCode;
                    objAddEditPatientViewModel.Address1 = objAddress.FirstOrDefault().sAddr1;
                    objAddEditPatientViewModel.Address2 = objAddress.FirstOrDefault().sAddr2;
                    objAddEditPatientViewModel.Address3 = objAddress.FirstOrDefault().sAddr3;
                    objAddEditPatientViewModel.Address4 = objAddress.FirstOrDefault().sAddr4;
                    objAddEditPatientViewModel.NearestFamilyTiltle = objAddress.FirstOrDefault().ifkTitle;
                    objAddEditPatientViewModel.Initials = objAddress.FirstOrDefault().sInitials;
                    objAddEditPatientViewModel.FirstName = objAddress.FirstOrDefault().sName;
                    objAddEditPatientViewModel.SurName = objAddress.FirstOrDefault().sSurname;
                    objAddEditPatientViewModel.Relationship = Convert.ToInt32(objAddress.FirstOrDefault().sRelationship);
                    objAddEditPatientViewModel.Comments = objAddress.FirstOrDefault().sRemarks;
                }
                objAddEditPatientViewModel.lastServiceDate = objAccount.SingleOrDefault().dLastServiceDate;
                int numberOfDays = CalculateDays(objAddEditPatientViewModel.lastServiceDate.GetValueOrDefault());
                objAddEditPatientViewModel.Current = objAccount.SingleOrDefault().nCurrent;
                objAddEditPatientViewModel.Total = objAccount.SingleOrDefault().nTotalBilled;
                objAddEditPatientViewModel.ddlPatientStatus = patientStatus;
                objAddEditPatientViewModel.ddlHospital = hospitals;
                objAddEditPatientViewModel.ddlDoctors = doctors;
                objAddEditPatientViewModel.ddlRefferingDoctor = referringDoctors;
                objAddEditPatientViewModel.ddlAddressDetail = address;
                objAddEditPatientViewModel.ddlTitle = title;
                objAddEditPatientViewModel.ddlMainMemberTitle = title;
                objAddEditPatientViewModel.ddlDependentTiltle = title;
                objAddEditPatientViewModel.ddlRelationship = relationship;
                objAddEditPatientViewModel.ddlNearestFriendRelationship = relationship;
                objAddEditPatientViewModel.ddlPatientType = patientType;
                objAddEditPatientViewModel.ddlMedicalAids = medicalAid;
                objAddEditPatientViewModel.ddlMedicalAidPlans = medicalAidPlans;
                objAddEditPatientViewModel.ddlMedicalAidNo = medicalAidNumber;
                objAddEditPatientViewModel.ddlServiceScale = serviceScale;
                objAddEditPatientViewModel.ddlGender = gender;
                objAddEditPatientViewModel.SPatMembershipNo = objAccount.SingleOrDefault().SPatMembershipNo;
                return View(objAddEditPatientViewModel);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Session["ErrorDetails"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        public string CalculateAge(DateTime DateOfBirth)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - DateOfBirth.Year;
            if (now < DateOfBirth.AddYears(age)) age--;
            return age.ToString();
        }

        public int CalculateDays(DateTime lastServiceDate)
        {
            DateTime now = DateTime.Today;
            int days = (now - lastServiceDate).Days;
            return days;
        }

        public ActionResult SaveEditedPatient(AddEditPatientViewModel model)
        {
            try
            {
                string UserID = Session["UserName"].ToString();
                bool patientSaved = objIPatientBLL.PutEditedPatientDetails(model, UserID);
                if (patientSaved)
                {
                    this.AddToastMessage("Success", "Patient Updated Successfully!.", ToastType.Success);
                }
                else
                {
                    this.AddToastMessage("Error", "There was problem in updating Patient record, please try again.", ToastType.Error);
                }
                return RedirectToAction("EditPatients", "Patient", new { accountNumber = Session["AccountNumber"].ToString() });

            }
            catch (Exception ex)
            {
                log.Error(ex);
                Session["ErrorDetails"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    bool saved = objIPatientBLL.InsertRecord(file, Session["AccountNumber"].ToString(), Session["UserActualName"].ToString());
                    if (saved)
                    {
                        this.AddToastMessage("Success", "File uploaded successfully!.", ToastType.Success);
                    }
                    else
                    {
                        this.AddToastMessage("Error", "There was technical problem occured during file upload, Please try again.", ToastType.Error);
                    }


                }
                return RedirectToAction("PatientConsultation", "Patient", new { accountNumber = Session["AccountNumber"].ToString() });
            }
            catch (Exception ex)
            {
                Session["ErrorDetails"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        public bool DeleteFile(string AccountNumber, string FileId)
        {
            this.AddToastMessage("Success", "File Deleted Successfully !", ToastType.Success);
            return objIPatientBLL.DeleteAccountFile(AccountNumber, FileId);
        }

        public FileResult DownloadFile(string AccountNumber, string FileId)
        {
            try
            {
                var Record = objIPatientBLL.DownloadFile(AccountNumber, FileId);
                string fileName = Record.sFileName;
                return File(Record.File_Data, Record.sFileType, fileName);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Session["ErrorDetails"] = ex.Message;
                return null;
            }
        }

        public ActionResult DownloadAttachment(string AccountNumber, string FileId)
        {
            // Find user by passed id
            // Student student = db.Students.FirstOrDefault(s => s.Id == studentId);

            var Record = objIPatientBLL.DownloadFile(AccountNumber, FileId);
            string fileName = Record.sFileName;

            byte[] fileBytes = Record.File_Data;

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public bool AddDependent(string AccountNumber, string DependentNumber, string DependentTiltle, string DependentInitials, string DependentFirstName, string DependentSurName, string DependentIdNumber, string DateofBirth, string DependentGender, string DependentRelationships, string DependentHospitals)
        {
            bool result = objIPatientBLL.InsertDependent(AccountNumber, DependentNumber, DependentTiltle, DependentInitials, DependentFirstName, DependentSurName, DependentIdNumber, DateofBirth, DependentGender, DependentRelationships, DependentHospitals);
            return result;
        }

        public JsonResult ChangeDependent(string AccountNumber, string DependentNumber)
        {
            PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities();
            List<Account> lstAccount = new List<Account>();
            lstAccount = dbContextPonaMedicalEntities.Accounts.Where(s => s.sAccountNo == AccountNumber).Distinct().ToList();
            long ifkAccountNo = Convert.ToInt64(lstAccount[0].ipkAccount.ToString());
            DependentModel objPatient = objIPatientBLL.GetDependentDetails(ifkAccountNo.ToString(), DependentNumber);
            objPatient.Age = CalculateAge(Convert.ToDateTime(objPatient.dPatDOB));
            return Json(objPatient, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEligibility(string AccountNumber)
        {
            AddEditPatientViewModel objPatientCheckEligibility = new AddEditPatientViewModel();
            PonaMedicalSolutionEntities dbContext = new PonaMedicalSolutionEntities();
            List<Account> lstAccount = new List<Account>();
            lstAccount = dbContext.Accounts.Where(s => s.sAccountNo == AccountNumber).Distinct().ToList();
            try
            {
                if (lstAccount.Count > 0)
                {
                    if (Convert.ToString(lstAccount[0].SPatMembershipNo) != "")
                    {
                        string BHFNumber = WebConfigurationManager.AppSettings["BHFNumber"];
                        string VendorNumber = WebConfigurationManager.AppSettings["VendorNumber"];
                        string VendorVersion = WebConfigurationManager.AppSettings["VendorVersion"];
                        // string DropFileLocation = WebConfigurationManager.AppSettings["DropFileLocation"];
                        string DropFileLocation = @"c:/mail";

                        string EligibilityRequest = "<?xml version='1.0' standalone='yes'?><DOCUMENT version='3.3' reply_tp = '1'>";
                        EligibilityRequest += "<TX sp_bhf = '" + BHFNumber + "' sp_hpc = '0' tx_nbr = '11757' plan = '612346' dt_cr = '" + DateTime.Now.ToString("yyyyMMdd") + "' dt_os = '"
                            + DateTime.Now.ToString("yyyyMMdd") + "' tx_cd = '20' msg_fmt = '13' orig = '04' bin = '2' cntry_cd = 'ZA' sect_cd = 'PR' >";
                        EligibilityRequest += "<VEND vend_id = '" + VendorNumber + "' pc_nbr = '1' wks_nbr = '1' vend_ver = '" + VendorVersion + "'></VEND>";
                        EligibilityRequest += "<MEM ch_id = '" + Convert.ToString(lstAccount[0].SPatMembershipNo) + "' sname = '" + Convert.ToString(lstAccount[0].sPatSurname) + "' inits = '"
                            + Convert.ToString(lstAccount[0].sPatInitials) + "'></MEM>";
                        EligibilityRequest += "<PAT dep_cd = '01' dob = '" + lstAccount[0].dPatDOB.ToString("yyyyMMdd") + "' id_nbr = '" + Convert.ToString(lstAccount[0].sPatIdNo) + "' sname = '"
                            + Convert.ToString(lstAccount[0].sPatSurname) + "' fname = '" + Convert.ToString(lstAccount[0].sPatName) + "' inits = '" + Convert.ToString(lstAccount[0].sPatInitials) + "' gend = '"
                            + Convert.ToString(lstAccount[0].sPatGender) + "' rlnship = '" + Convert.ToString(lstAccount[0].ifkRelationship) + "' status = '" + Convert.ToString(lstAccount[0].ifkAccStatus)
                            + "  ' newborn = 'N' ></PAT>";
                        EligibilityRequest += "</TX></DOCUMENT>";

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(EligibilityRequest);
                        string FileName = "rtreq.p" + lstAccount[0].SPatMembershipNo + ".XML";
                        doc.Save(@"c:/mail/" + FileName);

                        string Response = @"c:/mail/" + "rtreply.d" + lstAccount[0].SPatMembershipNo + ".XML";
                        string ResponseRejection = @"c:/mail/" + "rtrej.d" + lstAccount[0].SPatMembershipNo + ".XML";

                        //Wait for response
                        var timeout = DateTime.Now.Add(TimeSpan.FromMinutes(1));
                        for (;;)
                        {
                            if (System.IO.File.Exists(Response) || System.IO.File.Exists(ResponseRejection))
                            {
                                break;
                            }
                            if (DateTime.Now > timeout)
                            {
                                objPatientCheckEligibility.EligibilityStatus = "Please check medikredit service.";
                                log.Error("Application timeout; app_boxed could not be created; try again");
                                Environment.Exit(0);
                            }
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                        }

                        //Read the response 
                        if (System.IO.File.Exists(Response))
                        {
                            DataSet dsResponse = new DataSet();
                            dsResponse.ReadXml(Response);
                            if (dsResponse.Tables.Count != 0)
                            {
                                for (int intLoop = 0; intLoop < dsResponse.Tables["TX"].Rows.Count; intLoop++)
                                {
                                    if (dsResponse.Tables["TX"].Rows[intLoop]["res"].ToString() == "A")
                                    {
                                        //assignt to object 
                                        objPatientCheckEligibility.EligibilityStatus = "Status OK";
                                    }
                                }
                            }
                            //Move file
                            System.IO.File.Delete(@"c:/mail/" + "Response/ " + "rtreply.d" + lstAccount[0].SPatMembershipNo + ".XML");
                            System.IO.File.Move(Response, @"c:/mail/" + "Response/ " + "rtreply.d" + lstAccount[0].SPatMembershipNo + ".XML");
                        }
                        else if (System.IO.File.Exists(ResponseRejection))
                        {
                            DataSet dsResponseRejection = new DataSet();
                            dsResponseRejection.ReadXml(ResponseRejection);
                            if (dsResponseRejection.Tables.Count != 0)
                            {
                                for (int intLoop = 0; intLoop < dsResponseRejection.Tables["RJ"].Rows.Count; intLoop++)
                                {
                                    int RejectionCode = Convert.ToInt32(dsResponseRejection.Tables["RJ"].Rows[intLoop]["cd"].ToString());
                                    string RejectionReason = "Rejection occured with code: " + dsResponseRejection.Tables["RJ"].Rows[intLoop]["cd"].ToString();
                                    if (dbContext.RejectionCodes.Where(k => k.Error_Type == RejectionCode).Select(k => k.Short_Desc) != null)
                                    {
                                        RejectionReason = Convert.ToString(dbContext.RejectionCodes.Where(k => k.Error_Type == RejectionCode).Select(k => k.Short_Desc).FirstOrDefault());
                                    }
                                    objPatientCheckEligibility.EligibilityStatus = RejectionReason;
                                }
                            }
                            //Move file
                            System.IO.File.Delete(@"c:/mail/" + "Rejection/ " + "rtrej.d" + lstAccount[0].SPatMembershipNo + ".XML");
                            System.IO.File.Move(ResponseRejection, @"c:/mail/" + "Rejection/" + "rtrej.d" + lstAccount[0].SPatMembershipNo + ".XML");
                        }
                    }
                    else
                    {
                        objPatientCheckEligibility.EligibilityStatus = "Patient don't have medical aid, please check membership number.";
                    }
                }
            }
            catch (Exception ex)
            {
                objPatientCheckEligibility.EligibilityStatus = "Error Occurred while processing this request: " + ex.Message;
                log.Error(ex.Message);
            }
            return Json(objPatientCheckEligibility, JsonRequestBehavior.AllowGet);
        }

        //SendClaim
        public JsonResult SendClaim(string AccountNumber, string ClaimNumber)
        {
            AddEditPatientViewModel objPatientCheckEligibility = new AddEditPatientViewModel();
            PonaMedicalSolutionEntities dbContext = new PonaMedicalSolutionEntities();
            List<Account> lstAccount = new List<Account>();
            int ipkAccountNumber = Convert.ToInt32(AccountNumber);
            lstAccount = dbContext.Accounts.Where(s => s.ipkAccount == ipkAccountNumber).Distinct().ToList();
            try
            {
                if (lstAccount.Count > 0)
                {
                    if (Convert.ToString(lstAccount[0].SPatMembershipNo) != "")
                    {
                        //Prepare the claim
                        string BHFNumber = WebConfigurationManager.AppSettings["BHFNumber"];
                        string VendorNumber = WebConfigurationManager.AppSettings["VendorNumber"];
                        string VendorVersion = WebConfigurationManager.AppSettings["VendorVersion"];
                        //string DropFileLocation = WebConfigurationManager.AppSettings["DropFileLocation"];
                        string DropFileLocation = @"c:/mail/";
                        Random random = new Random();
                        long ipkClaims = dbContext.Claims.Where(k => k.ifkAccount == ipkAccountNumber && k.sClaimsNumber == ClaimNumber).Select(k => k.ipkClaims).FirstOrDefault();
                        string TranscationNo = ipkClaims.ToString(); ; //random.Next(0, 1000000000);  // retrive old transcation number

                        string ClaimRequest = "<?xml version='1.0' standalone='yes'?><DOCUMENT version='3.3' reply_tp = '1'>";
                        ClaimRequest += "<TX sp_bhf='" + BHFNumber + "' sp_hpc='' grp_prac='' tx_nbr='" + TranscationNo + "' plan='612346' dt_cr = '"
                            + DateTime.Now.ToString("yyyyMMdd") + "' dt_os = '" + DateTime.Now.ToString("yyyyMMdd") +
                            "' tx_cd ='21' nbr_items='7' pay_adv='P' clm_orig='E' cl_tp='' amd_ind='' msg_fmt='13' bin='000002' cntry_cd='ZA' sect_cd='PR' orig='3'>";
                        ClaimRequest += "<VEND vend_id='" + VendorNumber + "' pc_nbr='' wks_nbr='' hb_id='' vend_ver='" + VendorVersion + "'/>";
                        ClaimRequest += "<ECHO echo_type='001' echo_data='" + TranscationNo + "'/>";
                        ClaimRequest += "<MEM mbr_ent='' ch_id='" + Convert.ToString(lstAccount[0].SPatMembershipNo) + "' sname='" + Convert.ToString(lstAccount[0].sPatSurname)
                            + "' inits='" + Convert.ToString(lstAccount[0].sPatInitials) + "'/>";
                        ClaimRequest += "<PAT pat_ent='' dep_cd='" + Convert.ToString(lstAccount[0].sPatMedAidDepNo) + "' dob='" + lstAccount[0].dPatDOB.ToString("yyyyMMdd")
                            + "' inits='" + Convert.ToString(lstAccount[0].sPatInitials) + "' fname='" + Convert.ToString(lstAccount[0].sPatName) + "' id_nbr='" + Convert.ToString(lstAccount[0].sPatIdNo)
                            + "' passport='' sname='" + Convert.ToString(lstAccount[0].sPatSurname) + "' gend='" + Convert.ToString(lstAccount[0].sPatGender) + "' rlnship='" + Convert.ToString(lstAccount[0].ifkRelationship)
                            + "' weight='' post_cd='' status='" + Convert.ToString(lstAccount[0].AccStatu.ipkAccStatus) + "' newborn='' race=''/>";
                        ClaimRequest += "<BHF prescr='" + BHFNumber + "' refer='' admit=''/>";

                        //get the record from claim table
                        List<Claim> listClaimDetails = dbContext.Claims.Where(k => k.ifkAccount == ipkAccountNumber && k.ipkClaims == ipkClaims).ToList();
                        List<Transaction> listTranscationDetails = dbContext.Transactions.Where(k => k.ifkAccount == ipkAccountNumber && k.ifkClaims == ipkClaims).ToList();
                        //ClaimRequest += "<FIN gross='88.76' copy_fee='' late_fee='' ex_tm='' disp_fee='' serv_fee='' elig_amt='' sub_disc=''/>";

                        foreach (var item in listClaimDetails)
                        {
                            ClaimRequest += "<FIN gross='" + item.dClaimedAmount + "' copy_fee='' late_fee='' ex_tm='' disp_fee='' serv_fee='' elig_amt='' sub_disc=''/>";
                        }

                        ClaimRequest += "<DOCTOR event_nbr='' auth_nbr='' mem_acc_nbr='' ref_trk_nbr='' plcsv_cd='' disp_cd=''/>";
                        //get the record from transcation table
                        for (int loop = 0; loop < listTranscationDetails.Count; loop++)
                        {
                            if (listTranscationDetails[loop].sDrugsDescription == "CONSULTATION")
                            {
                                ClaimRequest += "<ITEM line_num='" + loop + "' pay_adv='P' tp='2' cdg_set='02' auth_nbr='' orig_tp_ind='0' pdiem='' descr='NEW AND ESTABLISHED PATIENT: CONSULTATION'"
                                    + " ord_nbr='' lab_nbr='' los='' grp='' process_tp='' oti=''>";
                                ClaimRequest += "<ITM_ECHO itm_echo_type='" + loop + "' itm_echo_data='" + Convert.ToString(listTranscationDetails[loop].ipkTransaction) + "'/>";
                                ClaimRequest += " <PROC tar_cd = '0192' qty = '1.00' units = '1.00' unit_tp = '06' >" +
                                    "<TRMNT st_dt = '" + Convert.ToString(listTranscationDetails[loop].dTransDate) + "' end_dt = '" + Convert.ToString(listTranscationDetails[loop].dTransDate) + "'/></PROC>"; //need to add dates
                                ClaimRequest += "<DIAG stg_ind='1' cd='" + Convert.ToString(listTranscationDetails[loop].sICD10Code) + "'/>";
                                ClaimRequest += "<FIN tar_ind='03' price='0' bs_of_cst='3' gross='" + Convert.ToString(listTranscationDetails[loop].nGrossAmt) + "'/>";
                                ClaimRequest += "</ITEM>";
                            }
                            else
                            {
                                ClaimRequest += "<ITEM line_num='" + loop + "' pay_adv='P' tp='1' cdg_set='03' auth_nbr='' orig_tp_ind='0' pdiem='' descr='" + Convert.ToString(listTranscationDetails[loop].sDrugsDescription)
                                 + "' ord_nbr='' lab_nbr='' los='' grp='' process_tp='' oti=''>";
                                ClaimRequest += "<ITM_ECHO itm_echo_type='" + loop + "' itm_echo_data='" + Convert.ToString(listTranscationDetails[loop].ipkTransaction) + "'/>";
                                ClaimRequest += "<MED nappi_cd='" + Convert.ToString(listTranscationDetails[loop].sNappi) + "' dly_ds='000000' est_days_sup='000' basis_of_eds='' qty='" + Convert.ToString(listTranscationDetails[loop].dDrugsQuantity) +
                                    "' mix_cd='' subst_ind=''/>";
                                ClaimRequest += "<DIAG stg_ind='1' cd='" + Convert.ToString(listTranscationDetails[loop].sICD10Code) + "'/>";
                                ClaimRequest += "<FIN tar_ind='' price='" + Convert.ToString(listTranscationDetails[loop].dDrugsOriginalPackPrice) + "' bs_of_cst='' sub_disc='' copay='' cont_fee='' disp_fee='' serv_fee='' gross='" +
                                   Convert.ToString(listTranscationDetails[loop].nGrossAmt) + "' fee_tp=''/>";
                                ClaimRequest += "</ITEM>";
                            }
                        }
                        ClaimRequest += "</TX></DOCUMENT>";

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(ClaimRequest);
                        string FileName = "rtreq.pClaim" + TranscationNo + ".XML";
                        doc.Save(@"c:/mail/" + FileName);

                        string Response = @"c:/mail/" + "rtreply.dClaim" + TranscationNo + ".XML";
                        string ResponseRejection = @"c:/mail/" + "rtrej.dClaim" + TranscationNo + ".XML";

                        //Wait for response
                        var timeout = DateTime.Now.Add(TimeSpan.FromMinutes(1));
                        for (;;)
                        {
                            if (System.IO.File.Exists(Response) || System.IO.File.Exists(ResponseRejection))
                            {
                                break;
                            }
                            if (DateTime.Now > timeout)
                            {
                                objPatientCheckEligibility.ClaimStatus = "Medikredit service is not running, please contact support team";
                                log.Error("Application timeout; app_boxed could not be created; try again");
                                Environment.Exit(0);
                            }
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                        }
                        //Read the response 
                        string ClaimStatusResponse = "";
                        if (System.IO.File.Exists(Response))
                        {
                            DataSet dsResponse = new DataSet();
                            dsResponse.ReadXml(Response);
                            if (dsResponse.Tables.Count != 0)
                            {
                                for (int intLoop = 0; intLoop < dsResponse.Tables["TX"].Rows.Count; intLoop++)
                                {
                                    ClaimStatusResponse = dsResponse.Tables["TX"].Rows[intLoop]["res"].ToString();
                                    switch (ClaimStatusResponse)
                                    {
                                        case "D":
                                            objPatientCheckEligibility.ClaimStatus = "Duplicate Claim";
                                            if (dsResponse.Tables["RJ"].Rows.Count > 0)
                                            {
                                                for (int intRJLoop = 0; intLoop < dsResponse.Tables["RJ"].Rows.Count; intLoop++)
                                                {
                                                    objPatientCheckEligibility.ClaimStatus += "-" + dsResponse.Tables["RJ"].Rows[intRJLoop]["desc"].ToString();
                                                }
                                            }
                                            dbContext.Pona_UpdateClaimsStatus(ClaimNumber, ClaimStatusResponse, Convert.ToInt64(AccountNumber), 01, Convert.ToInt32(Session["UserName"].ToString()), objPatientCheckEligibility.ClaimStatus);
                                            break;
                                        case "R":
                                            objPatientCheckEligibility.ClaimStatus = "Claim Rejected";
                                            if (dsResponse.Tables["RJ"].Rows.Count > 0)
                                            {
                                                for (int intRJLoop = 0; intLoop < dsResponse.Tables["RJ"].Rows.Count; intRJLoop++)
                                                {
                                                    int RejectionCode = Convert.ToInt32(dsResponse.Tables["RJ"].Rows[intRJLoop]["cd"].ToString());
                                                    string RejectionReason = "Claim Rejected with Rejection code: " + dsResponse.Tables["RJ"].Rows[intRJLoop]["cd"].ToString();
                                                    if (dbContext.RejectionCodes.Where(k => k.Error_Type == RejectionCode).Select(k => k.Short_Desc) != null)
                                                    {
                                                        RejectionReason = Convert.ToString(dbContext.RejectionCodes.Where(k => k.Error_Type == RejectionCode).Select(k => k.Short_Desc).FirstOrDefault());
                                                    }
                                                    objPatientCheckEligibility.ClaimStatus = RejectionReason;
                                                }
                                            }
                                            dbContext.Pona_UpdateClaimsStatus(ClaimNumber, ClaimStatusResponse, Convert.ToInt64(AccountNumber), 01, Convert.ToInt32(Session["UserName"].ToString()), objPatientCheckEligibility.ClaimStatus);
                                            break;
                                        case "P":
                                            objPatientCheckEligibility.ClaimStatus = "Claim Processed";
                                            dbContext.Pona_UpdateClaimsStatus(ClaimNumber, ClaimStatusResponse, Convert.ToInt64(AccountNumber), 01, Convert.ToInt32(Session["UserName"].ToString()), objPatientCheckEligibility.ClaimStatus);
                                            break;
                                    }
                                    //update the claim level status
                                }

                                //transcation level looping
                                for (int intLoop = 0; intLoop < dsResponse.Tables["ITEM"].Rows.Count; intLoop++)
                                {
                                    string ipkTranscationno = "0";
                                    string Item_id = "0";
                                    if (dsResponse.Tables["ITM_ECHO"].Rows.Count > 0)
                                    {
                                        ipkTranscationno = dsResponse.Tables["ITM_ECHO"].Rows[intLoop]["itm_echo_data"].ToString();
                                        Item_id = dsResponse.Tables["ITM_ECHO"].Rows[intLoop]["ITEM_Id"].ToString();
                                    }
                                    switch (dsResponse.Tables["ITEM"].Rows[intLoop]["status"].ToString())
                                    {
                                        case "W":
                                            string warningmessage = "Transaction Processed successfully with warnings message :";

                                            if (dsResponse.Tables["WARN"].Rows.Count > 0)
                                            {
                                                for (int intWLoop = 0; intWLoop < dsResponse.Tables["WARN"].Rows.Count; intWLoop++)
                                                {
                                                    if (Item_id == dsResponse.Tables["WARN"].Rows[intWLoop]["ITEM_Id"].ToString())
                                                    {
                                                        warningmessage += dsResponse.Tables["WARN"].Rows[intWLoop]["desc"].ToString() + ", ";
                                                    }
                                                }
                                            }
                                            if (ClaimStatusResponse == "D")
                                            {
                                                dbContext.UpdateTransactionsStatus(Convert.ToInt32(ipkTranscationno), 15, "Transcation Rejected as Duplicate");
                                            }
                                            else
                                            {
                                                dbContext.UpdateTransactionsStatus(Convert.ToInt32(ipkTranscationno), 5, warningmessage); //Status 5 is processed
                                            }
                                            break;
                                        case "P":
                                            if (ClaimStatusResponse == "D")
                                            {
                                                dbContext.UpdateTransactionsStatus(Convert.ToInt32(ipkTranscationno), 15, "Transcation Rejected as Duplicate");
                                            }
                                            else
                                            {
                                                dbContext.UpdateTransactionsStatus(Convert.ToInt32(ipkTranscationno), 5, "Transcation Processed Successfully!"); //Status 5 is processed 
                                            }
                                            break;
                                        case "R":
                                            if (ClaimStatusResponse == "D")
                                            {
                                                dbContext.UpdateTransactionsStatus(Convert.ToInt32(ipkTranscationno), 15, "Transcation Rejected as Duplicate");
                                            }
                                            else
                                            {
                                                dbContext.UpdateTransactionsStatus(Convert.ToInt32(ipkTranscationno), 7, "Transcation Rejected");
                                            }
                                            break;
                                        case "D":
                                            if (ClaimStatusResponse == "D")
                                            {
                                                dbContext.UpdateTransactionsStatus(Convert.ToInt32(ipkTranscationno), 15, "Transcation Rejected as Duplicate");
                                            }
                                            else
                                            {
                                                dbContext.UpdateTransactionsStatus(Convert.ToInt32(ipkTranscationno), 15, "Transcation Rejected as Duplicate");
                                            }
                                            break;
                                    }
                                }
                                //Move file

                                System.IO.File.Delete(@"c:/mail/" + "Response/" + "rtreply.dClaim" + lstAccount[0].SPatMembershipNo + ".XML");
                                System.IO.File.Move(Response, @"c:/mail/" + "Response/" + "rtreply.dClaim" + lstAccount[0].SPatMembershipNo + ".XML");
                            }
                        }
                        else if (System.IO.File.Exists(ResponseRejection))
                        {
                            DataSet dsResponseRejection = new DataSet();
                            dsResponseRejection.ReadXml(ResponseRejection);
                            if (dsResponseRejection.Tables.Count != 0)
                            {
                                for (int intLoop = 0; intLoop < dsResponseRejection.Tables["RJ"].Rows.Count; intLoop++)
                                {
                                    objPatientCheckEligibility.ClaimStatus += dsResponseRejection.Tables["RJ"].Rows[intLoop]["desc"].ToString() + ",";
                                    dbContext.Pona_UpdateRejectedClaims(ClaimNumber, Convert.ToInt64(AccountNumber), 01, objPatientCheckEligibility.ClaimStatus);
                                }
                            }                         

                            //Move file
                            System.IO.File.Delete(@"c:/mail/" + "Rejection/" + "rtrej.dClaim" + lstAccount[0].SPatMembershipNo + ".XML");
                            System.IO.File.Move(ResponseRejection, @"c:/mail/" + "Rejection/" + "rtrej.dClaim" + lstAccount[0].SPatMembershipNo + ".XML");
                        }
                    }
                    else
                    {
                        objPatientCheckEligibility.ClaimStatus = "Patient don't have medical aid, please check membership number.";
                    }
                }
            }
            catch (Exception ex)
            {
                objPatientCheckEligibility.ClaimStatus = "Error occurred while processing this request";
                log.Error(ex.Message);
            }
            return Json(objPatientCheckEligibility, JsonRequestBehavior.AllowGet);
        }


        //ReverseClaim
        public JsonResult ReverseClaim(string AccountNumber, string ClaimNumber)
        {
            AddEditPatientViewModel objPatientCheckEligibility = new AddEditPatientViewModel();
            PonaMedicalSolutionEntities dbContext = new PonaMedicalSolutionEntities();
            List<Account> lstAccount = new List<Account>();
            int ipkAccountNumber = Convert.ToInt32(AccountNumber);
            lstAccount = dbContext.Accounts.Where(s => s.ipkAccount == ipkAccountNumber).Distinct().ToList();
            try
            {
                if (lstAccount.Count > 0)
                {
                    if (Convert.ToString(lstAccount[0].SPatMembershipNo) != "")
                    {
                        //Prepare the claim
                        string BHFNumber = WebConfigurationManager.AppSettings["BHFNumber"];
                        string VendorNumber = WebConfigurationManager.AppSettings["VendorNumber"];
                        string VendorVersion = WebConfigurationManager.AppSettings["VendorVersion"];
                        //string DropFileLocation = WebConfigurationManager.AppSettings["DropFileLocation"];
                        string DropFileLocation = @"c:/mail/";  // to be changed in future
                        Random random = new Random();
                        long ipkClaims = dbContext.Claims.Where(k => k.ifkAccount == ipkAccountNumber && k.sClaimsNumber == ClaimNumber).Select(k => k.ipkClaims).FirstOrDefault();
                        string TranscationNo = ipkClaims.ToString(); ; //random.Next(0, 1000000000);  // retrive old transcation number

                        string ClaimRequest = "<?xml version='1.0' standalone='yes'?><DOCUMENT version='3.3' reply_tp = '1'>";
                        ClaimRequest += "<TX sp_bhf='" + BHFNumber + "' sp_hpc='' grp_prac='' tx_nbr='" + TranscationNo + "' plan='612346' dt_cr = '"
                                         + DateTime.Now.ToString("yyyyMMdd") + "' dt_os = '" + DateTime.Now.ToString("yyyyMMdd")
                                         + "' tx_cd ='11' nbr_items='7' pay_adv='P' clm_orig='E' cl_tp='' amd_ind='' msg_fmt='16' bin='000002' cntry_cd='ZA' sect_cd='PR' orig='3'>";
                        ClaimRequest += "<VEND vend_id='" + VendorNumber + "' pc_nbr='' wks_nbr='' hb_id='' vend_ver='" + VendorVersion + "'/>";
                        ClaimRequest += "<ECHO echo_type='001' echo_data='" + TranscationNo + "'/>";
                        ClaimRequest += "<MEM mbr_ent='' ch_id='" + Convert.ToString(lstAccount[0].SPatMembershipNo) + "' sname='" + Convert.ToString(lstAccount[0].sPatSurname)
                            + "' inits='" + Convert.ToString(lstAccount[0].sPatInitials) + "'/>";
                        ClaimRequest += "<PAT pat_ent='' dep_cd='" + Convert.ToString(lstAccount[0].sPatMedAidDepNo) + "' dob='" + lstAccount[0].dPatDOB.ToString("yyyyMMdd")
                            + "' inits='" + Convert.ToString(lstAccount[0].sPatInitials) + "' fname='" + Convert.ToString(lstAccount[0].sPatName) + "' id_nbr='" + Convert.ToString(lstAccount[0].sPatIdNo)
                            + "' passport='' sname='" + Convert.ToString(lstAccount[0].sPatSurname) + "' gend='" + Convert.ToString(lstAccount[0].sPatGender) + "' rlnship='" + Convert.ToString(lstAccount[0].ifkRelationship)
                            + "' weight='' post_cd='' status='" + Convert.ToString(lstAccount[0].AccStatu.ipkAccStatus) + "' newborn='' race=''/>";
                        ClaimRequest += "<BHF prescr='" + BHFNumber + "' refer='' admit=''/>";
                        //get the record from claim table

                        List<Claim> listClaimDetails = dbContext.Claims.Where(k => k.ifkAccount == ipkAccountNumber && k.ipkClaims == ipkClaims).ToList();
                        List<Transaction> listTranscationDetails = dbContext.Transactions.Where(k => k.ifkAccount == ipkAccountNumber && k.ifkClaims == ipkClaims).ToList();
                        //ClaimRequest += "<FIN gross='88.76' copy_fee='' late_fee='' ex_tm='' disp_fee='' serv_fee='' elig_amt='' sub_disc=''/>";
                        foreach (var item in listClaimDetails)
                        {
                            ClaimRequest += "<FIN gross='" + item.dClaimedAmount + "' copy_fee='' late_fee='' ex_tm='' disp_fee='' serv_fee='' elig_amt='' sub_disc=''/>";
                        }

                        ClaimRequest += "<DOCTOR event_nbr='' auth_nbr='' mem_acc_nbr='' ref_trk_nbr='' plcsv_cd='' disp_cd=''/>";
                        //get the record from transcation table
                        for (int loop = 0; loop < listTranscationDetails.Count; loop++)
                        {
                            if (listTranscationDetails[loop].sDrugsDescription == "CONSULTATION")
                            {
                                ClaimRequest += "<ITEM line_num='" + loop + "' pay_adv='P' tp='2' cdg_set='02' auth_nbr='' orig_tp_ind='0' pdiem='' descr='NEW AND ESTABLISHED PATIENT: CONSULTATION'"
                                                + " ord_nbr='' lab_nbr='' los='' grp='' process_tp='' oti=''>";
                                ClaimRequest += "<ITM_ECHO itm_echo_type='" + loop + "' itm_echo_data='" + Convert.ToString(listTranscationDetails[loop].ipkTransaction) + "'/>";
                                ClaimRequest += " <PROC tar_cd = '0192' qty = '1.00' units = '1.00' unit_tp = '06' >" +
                                                "<TRMNT st_dt = '" + Convert.ToString(listTranscationDetails[loop].dTransDate) + "' end_dt = '"
                                                 + Convert.ToString(listTranscationDetails[loop].dTransDate) + "'/></PROC>"; //need to add dates
                                ClaimRequest += "<DIAG stg_ind='1' cd='" + Convert.ToString(listTranscationDetails[loop].sICD10Code) + "'/>";
                                ClaimRequest += "<FIN tar_ind='03' price='0' bs_of_cst='3' gross='" + Convert.ToString(listTranscationDetails[loop].nGrossAmt) + "'/>";
                                ClaimRequest += "</ITEM>";
                            }
                            else
                            {
                                ClaimRequest += "<ITEM line_num='" + loop + "' pay_adv='P' tp='1' cdg_set='03' auth_nbr='' orig_tp_ind='0' pdiem='' descr='" + Convert.ToString(listTranscationDetails[loop].sDrugsDescription)
                                 + "' ord_nbr='' lab_nbr='' los='' grp='' process_tp='' oti=''>";
                                ClaimRequest += "<ITM_ECHO itm_echo_type='" + loop + "' itm_echo_data='" + Convert.ToString(listTranscationDetails[loop].ipkTransaction) + "'/>";
                                ClaimRequest += "<MED nappi_cd='" + Convert.ToString(listTranscationDetails[loop].sNappi) + "' dly_ds='000000' est_days_sup='000' basis_of_eds='' qty='" + Convert.ToString(listTranscationDetails[loop].dDrugsQuantity) +
                                    "' mix_cd='' subst_ind=''/>";
                                ClaimRequest += "<DIAG stg_ind='1' cd='" + Convert.ToString(listTranscationDetails[loop].sICD10Code) + "'/>";
                                ClaimRequest += "<FIN tar_ind='' price='" + Convert.ToString(listTranscationDetails[loop].dDrugsOriginalPackPrice) + "' bs_of_cst='' sub_disc='' copay='' cont_fee='' disp_fee='' serv_fee='' gross='" +
                                   Convert.ToString(listTranscationDetails[loop].nGrossAmt) + "' fee_tp=''/>";
                                ClaimRequest += "</ITEM>";
                            }
                        }
                        ClaimRequest += "</TX></DOCUMENT>";

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(ClaimRequest);
                        string FileName = "rtreq.pClaimReverse" + TranscationNo + ".XML";
                        doc.Save(@"c:/mail/" + FileName);

                        string Response = @"c:/mail/" + "rtreply.dClaimReverse" + TranscationNo + ".XML";
                        string ResponseRejection = @"c:/mail/" + "rtrej.dClaimReverse" + TranscationNo + ".XML";

                        //Wait for response
                        var timeout = DateTime.Now.Add(TimeSpan.FromMinutes(1));
                        for (;;)
                        {
                            if (System.IO.File.Exists(Response) || System.IO.File.Exists(ResponseRejection))
                            {
                                break;
                            }
                            if (DateTime.Now > timeout)
                            {
                                objPatientCheckEligibility.ClaimStatus = "Medikredit service is not running, please contact support team";
                                log.Error("Application timeout; app_boxed could not be created; try again");
                                Environment.Exit(0);
                            }
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                        }
                        //Read the response 
                        if (System.IO.File.Exists(Response))
                        {
                            DataSet dsResponse = new DataSet();
                            dsResponse.ReadXml(Response);
                            //Get Patient Number 
                            int PatientNumber = 01; //need to get it from database
                            if (dsResponse.Tables.Count != 0)
                            {
                                for (int intLoop = 0; intLoop < dsResponse.Tables["TX"].Rows.Count; intLoop++)
                                {
                                    switch (dsResponse.Tables["TX"].Rows[intLoop]["res"].ToString())
                                    {
                                        case "R":
                                            objPatientCheckEligibility.ClaimStatus = "Claim Reversal Rejected";
                                            if (dsResponse.Tables["RJ"].Rows.Count > 0)
                                            {
                                                for (int intRJLoop = 0; intLoop < dsResponse.Tables["RJ"].Rows.Count; intRJLoop++)
                                                {
                                                    int RejectionCode = Convert.ToInt32(dsResponse.Tables["RJ"].Rows[intRJLoop]["cd"].ToString());
                                                    string RejectionReason = "Claim Rejected with Rejection code: " + dsResponse.Tables["RJ"].Rows[intRJLoop]["cd"].ToString();
                                                    if (dbContext.RejectionCodes.Where(k => k.Error_Type == RejectionCode).Select(k => k.Short_Desc) != null)
                                                    {
                                                        RejectionReason = Convert.ToString(dbContext.RejectionCodes.Where(k => k.Error_Type == RejectionCode).Select(k => k.Short_Desc).FirstOrDefault());
                                                    }
                                                    objPatientCheckEligibility.ClaimStatus = RejectionReason;
                                                }
                                            }
                                            dbContext.Pona_UpdateRejectedClaims(ClaimNumber, Convert.ToInt64(AccountNumber), PatientNumber, objPatientCheckEligibility.ClaimStatus);
                                            break;

                                        case "A":
                                            objPatientCheckEligibility.ClaimStatus = "Claim Reversed";
                                            dbContext.Pona_UpdateReversedClaims(ClaimNumber, Convert.ToInt64(AccountNumber), PatientNumber, objPatientCheckEligibility.ClaimStatus);
                                            break;
                                    }
                                    //update the claim level status
                                }

                                //transcation level looping
                                for (int intLoop = 0; intLoop < dsResponse.Tables["ITEM"].Rows.Count; intLoop++)
                                {
                                    string ipkTranscationno = "0";
                                    string Item_id = "0";
                                    if (dsResponse.Tables["ITM_ECHO"].Rows.Count > 0)
                                    {
                                        ipkTranscationno = dsResponse.Tables["ITM_ECHO"].Rows[intLoop]["itm_echo_data"].ToString();
                                        Item_id = dsResponse.Tables["ITM_ECHO"].Rows[intLoop]["ITEM_Id"].ToString();

                                        switch (dsResponse.Tables["ITEM"].Rows[intLoop]["status"].ToString())
                                        {
                                            case "W":
                                                string warningmessage = "Transaction Processed successfully with warnings message :";
                                                if (dsResponse.Tables["WARN"].Rows.Count > 0)
                                                {
                                                    for (int intWLoop = 0; intWLoop < dsResponse.Tables["WARN"].Rows.Count; intWLoop++)
                                                    {
                                                        if (Item_id == dsResponse.Tables["WARN"].Rows[intWLoop]["ITEM_Id"].ToString())
                                                        {
                                                            warningmessage += dsResponse.Tables["WARN"].Rows[intWLoop]["desc"].ToString() + ", ";
                                                        }
                                                    }
                                                }
                                                dbContext.UpdateTransactionsStatus(Convert.ToInt32(ipkTranscationno), 5, warningmessage); //Status 5 is processed 
                                                break;
                                            case "R":
                                                dbContext.UpdateTransactionsStatus(Convert.ToInt32(ipkTranscationno), 20, "Transcation Reversal Rejected");
                                                break;
                                            case "D":
                                                dbContext.UpdateTransactionsStatus(Convert.ToInt32(ipkTranscationno), 15, "Transcation Reversal Rejected as Duplicate");
                                                break;
                                            case "A":
                                                dbContext.UpdateTransactionsStatus(Convert.ToInt32(ipkTranscationno), 18, "Transcation Reversal was successful");
                                                break;
                                        }
                                    }

                                }
                                //Move file

                                System.IO.File.Delete(@"c:/mail/" + "Response/" + "rtreply.dClaim" + lstAccount[0].SPatMembershipNo + ".XML");
                                System.IO.File.Move(Response, @"c:/mail/" + "Response/" + "rtreply.dClaim" + lstAccount[0].SPatMembershipNo + ".XML");
                            }

                        }
                        else if (System.IO.File.Exists(ResponseRejection))
                        {
                            DataSet dsResponseRejection = new DataSet();
                            dsResponseRejection.ReadXml(ResponseRejection);
                            if (dsResponseRejection.Tables.Count != 0)
                            {
                                for (int intLoop = 0; intLoop < dsResponseRejection.Tables["RJ"].Rows.Count; intLoop++)
                                {
                                    objPatientCheckEligibility.ClaimStatus = "Claim Reversal rejeted";
                                    dbContext.Pona_UpdateRejectedClaims(ClaimNumber, Convert.ToInt64(AccountNumber), 01, objPatientCheckEligibility.ClaimStatus);
                                }
                            }

                            //Update Transcation level
                            //transcation level looping
                            for (int intLoop = 0; intLoop < dsResponseRejection.Tables["ITEM"].Rows.Count; intLoop++)
                            {
                                string ipkTranscationno = "0";
                                if (dsResponseRejection.Tables["ITM_ECHO"].Rows.Count > 0)
                                {
                                    ipkTranscationno = dsResponseRejection.Tables["ITM_ECHO"].Rows[intLoop]["itm_echo_data"].ToString();
                                    switch (dsResponseRejection.Tables["ITEM"].Rows[intLoop]["status"].ToString())
                                    {
                                        case "A":
                                            dbContext.UpdateTransactionsStatus(Convert.ToInt32(ipkTranscationno), 20, "Transcation Reversal Rejected");
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                //Move file
                                System.IO.File.Delete(@"c:/mail/" + "Rejection/" + "rtrej.dClaim" + lstAccount[0].SPatMembershipNo + ".XML");
                                System.IO.File.Move(ResponseRejection, @"c:/mail/" + "Rejection/" + "rtrej.dClaim" + lstAccount[0].SPatMembershipNo + ".XML");
                            }
                        }
                        else
                        {
                            objPatientCheckEligibility.ClaimStatus = "Patient don't have medical aid, please check membership number.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objPatientCheckEligibility.ClaimStatus = "Error occurred while processing this request";
                log.Error(ex.Message);
            }
            return Json(objPatientCheckEligibility, JsonRequestBehavior.AllowGet);
        }


        //CheckDependents
        public JsonResult CheckDependents(string AccountNumber)
        {
            AddEditPatientViewModel objPatientCheckEligibility = new AddEditPatientViewModel();
            PonaMedicalSolutionEntities dbContext = new PonaMedicalSolutionEntities();
            List<Account> lstAccount = new List<Account>();
            lstAccount = dbContext.Accounts.Where(s => s.sAccountNo == AccountNumber).Distinct().ToList();
            try
            {
                if (lstAccount.Count > 0)
                {
                    if (Convert.ToString(lstAccount[0].SPatMembershipNo) != "")
                    {
                        string BHFNumber = WebConfigurationManager.AppSettings["BHFNumber"];
                        string VendorNumber = WebConfigurationManager.AppSettings["VendorNumber"];
                        string VendorVersion = WebConfigurationManager.AppSettings["VendorVersion"];
                        // string DropFileLocation = WebConfigurationManager.AppSettings["DropFileLocation"];
                        string DropFileLocation = @"c:/mail/";
                        string ifkAccountNo = lstAccount[0].ipkAccount.ToString();

                        string EligibilityRequest = "<?xml version='1.0' standalone='yes'?><DOCUMENT version='3.3' reply_tp = '1'>";
                        EligibilityRequest += "<TX sp_bhf = '" + BHFNumber + "' sp_hpc = '0' tx_nbr = '11757' plan = '612346' dt_cr = '" + DateTime.Now.ToString("yyyyMMdd") + "' dt_os = '" + DateTime.Now.ToString("yyyyMMdd") + "' tx_cd = '30' msg_fmt = '13' orig = '04' bin = '2' cntry_cd = 'ZA' sect_cd = 'PR' >";
                        EligibilityRequest += "<VEND vend_id = '" + VendorNumber + "' pc_nbr = '1' wks_nbr = '1' vend_ver = '" + VendorVersion + "'></VEND>";
                        EligibilityRequest += "<MEM ch_id = '" + Convert.ToString(lstAccount[0].SPatMembershipNo) + "' sname = '" + Convert.ToString(lstAccount[0].sPatSurname) + "' inits = '" + Convert.ToString(lstAccount[0].sPatInitials) + "'></MEM>";
                        EligibilityRequest += "</TX></DOCUMENT>";

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(EligibilityRequest);
                        string FileName = "rtreq.pDependentCheck" + ifkAccountNo + ".XML";
                        doc.Save(@"c:/mail/" + FileName);

                        string Response = @"c:/mail/" + "rtreply.dDependentCheck" + ifkAccountNo + ".XML";
                        string ResponseRejection = @"c:/mail/" + "rtrej.dDependentCheck" + ifkAccountNo + ".XML";

                        //Wait for response
                        var timeout = DateTime.Now.Add(TimeSpan.FromMinutes(1));
                        for (;;)
                        {
                            if (System.IO.File.Exists(Response) || System.IO.File.Exists(ResponseRejection))
                            {
                                break;
                            }
                            if (DateTime.Now > timeout)
                            {
                                objPatientCheckEligibility.EligibilityStatus = "Please check Medikredit service.";
                                log.Error("Application timeout; app_boxed could not be created; try again");
                                Environment.Exit(0);
                            }
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                        }

                        //Read the response 
                        if (System.IO.File.Exists(Response))
                        {
                            DataSet dsResponse = new DataSet();
                            dsResponse.ReadXml(Response);
                            if (dsResponse.Tables.Count != 0)
                            {
                                for (int intLoop = 0; intLoop < dsResponse.Tables["TX"].Rows.Count; intLoop++)
                                {
                                    if (dsResponse.Tables["TX"].Rows[intLoop]["res"].ToString() == "A")
                                    {
                                        //assignt to object 
                                        for (int intInnerLoop = 0; intInnerLoop < dsResponse.Tables["MEM"].Rows.Count; intInnerLoop++)
                                        {
                                            objPatientCheckEligibility.EligibilityStatus = "Client has dependents, No : " + dsResponse.Tables["MEM"].Rows[intLoop]["nbr_depn"].ToString();
                                        }

                                        //Add dependents 
                                        for (int intInnerLoop = 1; intInnerLoop < dsResponse.Tables["PAT"].Rows.Count; intInnerLoop++)
                                        {
                                            string DependentNumber = dsResponse.Tables["PAT"].Rows[intInnerLoop]["dep_cd"].ToString();
                                            string DependentTiltle = "1";
                                            string DependentInitials = dsResponse.Tables["PAT"].Rows[intInnerLoop]["inits"].ToString();
                                            string DependentFirstName = dsResponse.Tables["PAT"].Rows[intInnerLoop]["fname"].ToString();
                                            string DependentSurName = dsResponse.Tables["PAT"].Rows[intInnerLoop]["sname"].ToString();
                                            string DependentIdNumber = dsResponse.Tables["PAT"].Rows[intInnerLoop]["id_nbr"].ToString();
                                            string DateofBirth = DateTime.ParseExact(dsResponse.Tables["PAT"].Rows[intInnerLoop]["dob"].ToString(), "yyyyMMdd", null).ToString("yyyy-MM-dd");
                                            string DependentGender = dsResponse.Tables["PAT"].Rows[intInnerLoop]["gender"].ToString();
                                            string DependentRelationships = "1";//dsResponse.Tables["PAT"].Rows[intInnerLoop]["nbr_depn"].ToString();
                                            string DependentHospitals = "1";
                                            bool result = objIPatientBLL.InsertDependent(ifkAccountNo, DependentNumber, DependentTiltle, DependentInitials, DependentFirstName, DependentSurName, DependentIdNumber, DateofBirth, DependentGender, DependentRelationships, DependentHospitals);
                                            if (!result)
                                            {
                                                objPatientCheckEligibility.EligibilityStatus = "Problem Occured while adding dependents, please try again.";
                                            }
                                        }
                                    }
                                }
                            }
                            //Move file
                            System.IO.File.Delete(@"c:/mail/" + "Response/ " + "rtreply.dDependentCheck" + lstAccount[0].SPatMembershipNo + ".XML");
                            System.IO.File.Move(Response, @"c:/mail/" + "Response/ " + "rtreply.dDependentCheck" + lstAccount[0].SPatMembershipNo + ".XML");
                        }
                        else if (System.IO.File.Exists(ResponseRejection))
                        {
                            DataSet dsResponseRejection = new DataSet();
                            dsResponseRejection.ReadXml(ResponseRejection);
                            if (dsResponseRejection.Tables.Count != 0)
                            {
                                for (int intLoop = 0; intLoop < dsResponseRejection.Tables["RJ"].Rows.Count; intLoop++)
                                {
                                    int RejectionCode = Convert.ToInt32(dsResponseRejection.Tables["RJ"].Rows[intLoop]["cd"].ToString());
                                    string RejectionReason = "Rejection occured with code: " + dsResponseRejection.Tables["RJ"].Rows[intLoop]["cd"].ToString();
                                    if (dbContext.RejectionCodes.Where(k => k.Error_Type == RejectionCode).Select(k => k.Short_Desc) != null)
                                    {
                                        RejectionReason = Convert.ToString(dbContext.RejectionCodes.Where(k => k.Error_Type == RejectionCode).Select(k => k.Short_Desc).FirstOrDefault());
                                    }
                                    objPatientCheckEligibility.EligibilityStatus = RejectionReason;

                                }

                            }
                            //Move file
                            System.IO.File.Delete(@"c:/mail/" + "Rejection/ " + "rtrej.dDependentCheck" + lstAccount[0].SPatMembershipNo + ".XML");
                            System.IO.File.Move(ResponseRejection, @"c:/mail/" + "Rejection/" + "rtrej.dDependentCheck" + lstAccount[0].SPatMembershipNo + ".XML");
                        }
                    }
                    else
                    {
                        objPatientCheckEligibility.EligibilityStatus = "Patient don't have any medical aid, please check membership number.";
                    }
                }
            }
            catch (Exception ex)
            {
                objPatientCheckEligibility.EligibilityStatus = "Error Occurred while processing this request";
                log.Error(ex.Message);
            }
            return Json(objPatientCheckEligibility, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetDependent(string AccountNumber)
        {
            try
            {
                AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
                AddEditPatientViewModel lstAddEditPatientViewModel = new AddEditPatientViewModel();
                List<SelectListItem> lstPatient = new List<SelectListItem>();

                IQueryable<Patient> objPatient = objAddEditPatientBLL.GetPatients(AccountNumber);
                foreach (var item in objPatient)
                {
                    SelectListItem objPatients = new SelectListItem() { Text = item.sPatName, Value = item.sPatName, Selected = item.sPatName == objPatient.FirstOrDefault().sPatName };
                    lstPatient.Add(objPatients);
                }
                objAddEditPatientViewModel.ddlPatient = lstPatient;
                return PartialView("GetDependent", objAddEditPatientViewModel);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        public bool EditDependent(string AccountNumber, string DependentNumber, string DependentTiltle, string DependentInitials, string DependentFirstName, string DependentSurName, string DependentIdNumber, string DateofBirth, string DependentGender, string DependentRelationships, string DependentHospitals)
        {
            bool result = objIPatientBLL.EditDependent(AccountNumber, DependentNumber, DependentTiltle, DependentInitials, DependentFirstName, DependentSurName, DependentIdNumber, DateofBirth, DependentGender, DependentRelationships, DependentHospitals);
            return result;
        }
        [HttpPost]
        public bool AddConsultation(string AccountNumber, string ConsultDate, string SpecificCalc, string TariffCode, string Description, string Reference, string Nappy, string Authorisation, string PerformedinHospital, string Chronic, string NHMinutes, string AMinutes, string Units, string StandAloneTranscation, string UnitAmount, string Quantity, string GrossCharges, string PatientNetLiable)
        {
            return objIPatientBLL.InsertConsultation(AccountNumber, ConsultDate, SpecificCalc, TariffCode, Description, Reference, Nappy, Authorisation, PerformedinHospital, Chronic, NHMinutes, AMinutes, Units, StandAloneTranscation, UnitAmount, Quantity, GrossCharges, PatientNetLiable);
        }
        [HttpPost]
        public string GetNHMinutes(string TarrifCode)
        {
            return objIPatientBLL.GetNHminutes(TarrifCode);
        }
        public ActionResult Dummy()
        {
            return View();
        }
    }
}