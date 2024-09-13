using Microsoft.VisualStudio.TestTools.UnitTesting;
using Philips.Tonic.Business.Patients.Contracts.Logic;
using Philips.Tonic.Business.Patients.Contracts.Models;
using Philips.Tonic.Business.UserManagement.Contracts.Interfaces;
using Philips.Tonic.Business.UserManagement.Contracts.Objects;
using Philips.Tonic.Business.Common.Contracts.Enumerations;
using System;
using TechTalk.SpecFlow;
using System.Linq;
using Philips.Tonic.Business.Patients.Contracts.Enums;

namespace Philips.Tonic.APITests.Steps
{
    [Binding]
    public class APIAccessoryLogic
    { 
    private readonly ScenarioContext _scenarioContext;
    private readonly FeatureContext _featureContext;

    public APIAccessoryLogic(ScenarioContext scenarioContext, FeatureContext featureContext)
    {
        _scenarioContext = scenarioContext;
        _featureContext = featureContext;
    }


        [Given(@"Upsert accesory to patient")]
        public void GivenUpsertAccesoryToPatient()
        {
            TestHelpers.AddOrUpdate(_scenarioContext, TestHelpers.InsertDefaultPatientModel());
            var loginResponse = _scenarioContext.Get<LoginResponse>(typeof(LoginResponse).Name);
            var patientModel = _scenarioContext.Get<PatientModel>(typeof(PatientModel).Name);
            var accessoryLogic = _scenarioContext.Get<IAccessoryLogic>(typeof(IAccessoryLogic).Name);

            var accessoryModel = TestHelpers.GetAccessory(patientModel);
            var am = accessoryLogic.Upsert(accessoryModel);

            Assert.AreEqual(am.EquipmentSizeId, accessoryModel.EquipmentSizeId, "Saved EquipmentSizeId is diffrent");
            Assert.AreEqual(am.CreatorUserId, loginResponse.User.UserId, "userID is diffrent");
            Assert.AreEqual(am.EquipmentId, accessoryModel.EquipmentId, "EquipmentId is diffrent");
            Assert.AreEqual(am.PatientPersonId, patientModel.PersonId, "Personid is diffrent");
            Assert.AreEqual(am.EndDate, accessoryModel.EndDate, "Enddate is diffrent");
            Assert.AreEqual(am.StartDate, accessoryModel.StartDate, "Startdate is diffrent");


        }

        [Then(@"I verify GetActivityLogsByPatientId for accesory assignemnt")]
        public void ThenIVerifyGetActivityLogsByPatientIdForAccesoryAssignemnt()
        {
            var activityLogLogic = _scenarioContext.Get<IActivityLogLogic>(typeof(IActivityLogLogic).Name);

            var patientModel = _scenarioContext.Get<PatientUpdateResult>(typeof(PatientUpdateResult).Name);
            var pm = _scenarioContext.Get<PatientModel>(typeof(PatientModel).Name);

            var activityModel = activityLogLogic.GetActivityLogsByPatientId(patientModel.Patient.PersonId);
            Assert.IsNotNull(activityModel, "Activity log is empty!");

            var log = activityModel.First(x => x.InteractionTypeId == (byte)InteractionTypes.AccessoryAssignment);

            Assert.IsNotNull(log, "Activity log not contains stod interaction");
            Assert.AreEqual(log.PatientPersonId, patientModel.Patient.PersonId, "Person ID is different for the update stod");
            Assert.AreEqual(log.InteractionTypeModel.Description, "Accesory Assignment", "Description");
        }


        [Given(@"I unassign the accessory")]
        public void GivenIUnassignTheAccessory()
        {
            var accessoryLogic = _scenarioContext.Get<IAccessoryLogic>(typeof(IAccessoryLogic).Name);
            var patientModel = _scenarioContext.Get<PatientModel>(typeof(PatientModel).Name);
            var accessoryModel = TestHelpers.GetAccessory(patientModel);
            accessoryLogic.UnAssign(accessoryModel);

        }

        [Then(@"I verify GetActivityLogsByPatientId for unassigned assignemnt for accesory")]
        public void ThenIVerifyGetActivityLogsByPatientIdForUnassignedAssignemntForAccesory()
        {
            var activityLogLogic = _scenarioContext.Get<IActivityLogLogic>(typeof(IActivityLogLogic).Name);

            var patientModel = _scenarioContext.Get<PatientUpdateResult>(typeof(PatientUpdateResult).Name);

            var pm = _scenarioContext.Get<PatientModel>(typeof(PatientModel).Name);

            var activityModel = activityLogLogic.GetActivityLogsByPatientId(patientModel.Patient.PersonId);
            Assert.IsNotNull(activityModel, "Activity log is empty!");

            var log = activityModel.First(x => x.InteractionTypeId == (byte)InteractionTypes.AccessoryUnassignment);

            Assert.IsNotNull(log, "Activity log not contains stod interaction");
            Assert.AreEqual(log.PatientPersonId, patientModel.Patient.PersonId, "Person ID is different for the update stod");
            Assert.AreEqual(log.InteractionTypeModel.Description, "Accesory UnAssignemnt", "Description");
        }


        [Given(@": GetAccessory")]
        public void GivenGetAccessory()
        {
            TestHelpers.AddOrUpdate(_scenarioContext, TestHelpers.InsertDefaultPatientModel());
            var loginResponse = _scenarioContext.Get<LoginResponse>(typeof(LoginResponse).Name);
            var patientModel = _scenarioContext.Get<PatientModel>(typeof(PatientModel).Name);
            var accessoryLogic = _scenarioContext.Get<IAccessoryLogic>(typeof(IAccessoryLogic).Name);
            

            var accessoryModel = TestHelpers.GetAccessory(patientModel);
            var am = accessoryLogic.Upsert(accessoryModel);
            var getaccessoryModel = accessoryLogic.GetAccessory(am.InteractionId);

            Assert.AreEqual(am.EquipmentSizeId, getaccessoryModel.EquipmentSizeId, "Saved EquipmentSizeId is diffrent");
            Assert.AreEqual(am.CreatorUserId, loginResponse.User.UserId, "userID is diffrent");
            Assert.AreEqual(am.EquipmentId, getaccessoryModel.EquipmentId, "EquipmentId is diffrent");
            Assert.AreEqual(am.PatientPersonId, patientModel.PersonId, "Personid is diffrent");
            Assert.AreEqual(am.EndDate, getaccessoryModel.EndDate, "Enddate is diffrent");
            Assert.AreEqual(am.StartDate, getaccessoryModel.StartDate, "Startdate is diffrent");
        }



        [Given(@"GetAccessoriesByPatientId")]
        public void GivenGetAccessoriesByPatientId()
        {
            TestHelpers.AddOrUpdate(_scenarioContext, TestHelpers.InsertDefaultPatientModel());
            var loginResponse = _scenarioContext.Get<LoginResponse>(typeof(LoginResponse).Name);
            var patientModel = _scenarioContext.Get<PatientModel>(typeof(PatientModel).Name);
            var accessoryLogic = _scenarioContext.Get<IAccessoryLogic>(typeof(IAccessoryLogic).Name);

            var accessoryModel = TestHelpers.GetAccessory(patientModel);
            var am = accessoryLogic.Upsert(accessoryModel);
            var accessorylist = accessoryLogic.GetAccessoriesByPatientId(patientModel.PersonId);

            Assert.IsNotNull(accessorylist);
            
        }
    }

 }



