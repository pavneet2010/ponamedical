using PonaMeds.BLL.BLLInterface;
using PonaMeds.DLL;
using PonaMeds.DLL.DALInterface;
using PonaMeds.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PonaMeds.BLL
{
    public class AddEditPAtientDetailBLL : IAddEditPatientBLL
    {
        private IAddEditPatient objAddEditPatientDAL;

        public AddEditPAtientDetailBLL()
        {
            objAddEditPatientDAL = new AddEditPatientDAL();
        }

        public List<AddEditPatientViewModel> GetPatientStatus(int practiceID)
        {
            //List<AccStatu> lstAccStatus = objAddEditPatientDAL.PullPatientStatus();
            //List<Hospital> lstHospitals = objAddEditPatientDAL.PullHospital();
            //List<Anaesthetist> lstAnaesthetist = objAddEditPatientDAL.PullDoctors(practiceID);
            //List<Surgeon> lstSurgeon = objAddEditPatientDAL.PullSurgeon(practiceID);
            //List<Address> lstAddress = objAddEditPatientDAL.PullAddress();
            //List<Title> lstTitle = objAddEditPatientDAL.PullTitle();
            //List<Relationship> lstRelationship = objAddEditPatientDAL.PullRelationship();
            //List<AccType> lstPatientType = objAddEditPatientDAL.PullPatientType();
            //List<MedAid> lstMedAids = objAddEditPatientDAL.PullMedicalAid();
            //List<MedAidScheme> lstMedAidplans = objAddEditPatientDAL.PullMedicalAidPlan();
            //List<ServiceScale> lstServiceScale = objAddEditPatientDAL.PullServiceScale();
            List<AddEditPatientViewModel> lstAddEditPatientModel = new List<AddEditPatientViewModel>();

            //foreach (var item in lstAccStatus)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.PatientStatus = item.sName;
            //    objAddEditPatientViewModel.PatientStatusCode = item.ipkAccStatus;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstHospitals)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.Hospital = item.sName;
            //    objAddEditPatientViewModel.HospitalNumber = item.ipkHospital;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstAnaesthetist)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.Doctors = item.sSurname +","+ item.sName;
            //    objAddEditPatientViewModel.AnasthetistNumber = item.ipkAnaesthetist;
            //    objAddEditPatientViewModel.RefferingDoctor = item.sSurname + "," + item.sName;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstSurgeon)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.RefferingDoctor = item.sPracticeName;
            //    objAddEditPatientViewModel.RefferingDoctorId = item.ipkSurgeon;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstAddress)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.AddressDetail = item.ipkAddress;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstTitle)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.Tiltle = item.sTitleShort;
            //    objAddEditPatientViewModel.TiltleId = item.ipkTitle;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstRelationship)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.Relationships = item.sDescription;
            //    objAddEditPatientViewModel.RelationshipsNumber = item.ipkRelationship;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstPatientType)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.PatientType = item.sDescription;
            //    objAddEditPatientViewModel.AccountType = item.ipkAccType;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstMedAids)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.MedicalAids = item.sName;
            //    objAddEditPatientViewModel.MedicalAidNo = item.ipkMedAid;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstMedAidplans)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.MedicalAidPlans = item.sDescription;
            //    objAddEditPatientViewModel.MedicalAidScheme = item.ipkMedAidScheme;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstServiceScale)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.ServiceScale = item.sDescription;
            //    objAddEditPatientViewModel.ServiceScaleId = item.ipkServiceScale;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            return lstAddEditPatientModel;
        }

        public string GetAccountNumber()
        {
            return objAddEditPatientDAL.PullAccountNumber();
        }

        public List<AddEditPatientViewModel> GetPatientDetails(string accountNumber, out IQueryable<Account> objAccount, out IQueryable<Address> lstAddress)
        {
            PonaMedicalSolutionEntities dbContextPonaMedicalEntities = new PonaMedicalSolutionEntities();
            List<Account> lstAccount = new List<Account>();
            int IntaccountNumber = Convert.ToInt32(accountNumber);
            lstAccount = dbContextPonaMedicalEntities.Accounts.Where(s => s.ipkAccount == IntaccountNumber).Distinct().ToList();
            int practiceID = Convert.ToInt32(lstAccount[0].PracticeId.ToString());

            //List<AccStatu> lstAccStatus = objAddEditPatientDAL.PullPatientStatus();
            //List<Hospital> lstHospitals = objAddEditPatientDAL.PullHospital();
            //List<Anaesthetist> lstAnaesthetist = objAddEditPatientDAL.PullDoctors(practiceID);
            //List<Surgeon> lstSurgeon = objAddEditPatientDAL.PullSurgeon(practiceID);
            //List<Title> lstTitle = objAddEditPatientDAL.PullTitle();
            //List<Relationship> lstRelationship = objAddEditPatientDAL.PullRelationship();
            //List<AccType> lstPatientType = objAddEditPatientDAL.PullPatientType();
            //List<MedAid> lstMedAids = objAddEditPatientDAL.PullMedicalAid();
            //List<MedAidScheme> lstMedAidplans = objAddEditPatientDAL.PullMedicalAidPlan();
            //List<ServiceScale> lstServiceScale = objAddEditPatientDAL.PullServiceScale();
            IQueryable<Account> objectAccount = objAddEditPatientDAL.PullAccountDetails(accountNumber);
            //IQueryable<Patient> objectPatient = objAddEditPatientDAL.PullPatientDetails(accountNumber);
            objAccount = objectAccount;
            //objPatient = objectPatient;

            lstAddress = objAddEditPatientDAL.PullAddress(Convert.ToInt64(accountNumber));
            List<AddEditPatientViewModel> lstAddEditPatientModel = new List<AddEditPatientViewModel>();

            //foreach (var item in lstAccStatus)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.PatientStatus = item.sName;
            //    objAddEditPatientViewModel.PatientStatusCode = item.ipkAccStatus;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstHospitals)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.Hospital = item.sName;
            //    objAddEditPatientViewModel.HospitalNumber = item.ipkHospital;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstAnaesthetist)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.Doctors = item.sSurname+","+item.sName;
            //    objAddEditPatientViewModel.AnasthetistNumber = item.ipkAnaesthetist;
            //    objAddEditPatientViewModel.RefferingDoctor = item.sSurname + "," + item.sName;
            //    objAddEditPatientViewModel.RefferingDoctorId = item.ipkAnaesthetist;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstSurgeon)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.RefferingDoctor = item.sPracticeName;
            //    objAddEditPatientViewModel.RefferingDoctorId = item.ipkSurgeon;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstTitle)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.Tiltle = item.sTitleShort;
            //    objAddEditPatientViewModel.TiltleId = item.ipkTitle;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstRelationship)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.Relationships = item.sDescription;
            //    objAddEditPatientViewModel.RelationshipsNumber = item.ipkRelationship;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstPatientType)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.PatientType = item.sDescription;
            //    objAddEditPatientViewModel.AccountType = item.ipkAccType;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstMedAids)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.MedicalAids = item.sName;
            //    objAddEditPatientViewModel.MedicalAidNo = item.ipkMedAid;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstMedAidplans)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.MedicalAidPlans = item.sDescription;
            //    objAddEditPatientViewModel.MedicalAidScheme = item.ipkMedAidScheme;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            //foreach (var item in lstServiceScale)
            //{
            //    AddEditPatientViewModel objAddEditPatientViewModel = new AddEditPatientViewModel();
            //    objAddEditPatientViewModel.ServiceScale = item.sDescription;
            //    objAddEditPatientViewModel.ServiceScaleId = item.ipkServiceScale;
            //    lstAddEditPatientModel.Add(objAddEditPatientViewModel);
            //}
            return lstAddEditPatientModel;
        }

        public IQueryable<Patient> GetPatients(string ipkAccountNumber)
        {
            IQueryable<Patient> objPatient = objAddEditPatientDAL.PullPatientDetails(ipkAccountNumber);
            return objPatient;
        }
    }
}