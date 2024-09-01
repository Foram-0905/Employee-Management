import { environment } from '../../../environments/environment';

export const BaseURI = environment.BaseURI;
export const Login = BaseURI + 'User/UserLogin';
export const SaveDesignation = BaseURI + 'Designation/SaveDesignation';
export const GetAllDesignation = BaseURI + 'Designation/GetDesignation';
export const getDesignationById = BaseURI + 'Designation/getDesignationById';
export const deleteDesignation = BaseURI + 'Designation/DeleteDesignation';
export const GetFilterDesignation = BaseURI + 'Designation/GetFilterDesignation';

export const resetPassword = BaseURI + 'User/ResetPassword';
export const changeToken = BaseURI + 'User/RefreshToken';
export const sendMail = BaseURI + 'User/ForgotPassword';

export const SaveSlgGroup = BaseURI + 'Slggroup/SaveSlggroup';
export const  GetAllSlgGroups = BaseURI + 'Slggroup/Getslggroup';
export const  getSLGGroupById = BaseURI + 'Slggroup/GetSLGgroupById';
export const  deleteSLGGroup = BaseURI + 'Slggroup/DeleteSlggroup';
export const getFilterslggroup= BaseURI + 'Slggroup/GetFilterSLGgroup';

export const SaveState = BaseURI + 'State/Savestate';
export const  GetAllState = BaseURI + 'State/Getstate';
export const  GetStateByCountryId = BaseURI + 'State/GetStatesByCountryId';
export const  getStateById = BaseURI + 'State/GetstateById';
export const  deleteState = BaseURI + 'State/Deletestate';
export const  getFilterState = BaseURI + 'State/GetFilterstate';

export const GetCurrency = BaseURI +'Currency/GetCurrency';
export const getCurrencyById = BaseURI +'Currency/getCurrencyById';
export const SaveCurrency = BaseURI +'Currency/SaveCurrency';
export const getFilterCurrency =BaseURI + 'Currency/GetFilterCurrency';

export const getCountry = BaseURI +'Country/Getcountry';
export const saveCountry = BaseURI +'Country/Savecountry';
export const getCountryById = BaseURI + 'Country/GetCountryById';
export const deleteCountry = BaseURI + 'Country/DeleteCountry';

export const deleteCurrencyById=  BaseURI+'Currency/DeleteCurrency';

export const GetCountry = BaseURI +'Country/Getcountry';
export const SaveCountry = BaseURI +'Country/Savecountry';

export const GetAssets = BaseURI +'Asset/GetAssets';
export const GetAssetsbyId = BaseURI +'Asset/GetAssetbyId';
export const DeleteAssets = BaseURI +'Asset/DeleteAsset';
export const SaveAssets = BaseURI +'Asset/SaveAsset';
export const GetAssetsStatus = BaseURI+'Asset_Status/GetAssetsStatus';
export const GetAssetstype = BaseURI+ 'Asset_type/GetAssetstype';
export const GetFilterAssets =BaseURI +'Asset/GetFilterAsset';

// export const GetEmployee =BaseURI+ 'Employee/GetEmployee' ;
export const getFilterCountry = BaseURI + 'Country/GetFilterCountry';

export const SavelanguageLevel = BaseURI + 'Languagelevel/Savelanguagelevel';
export const GetAllLanguageLevel = BaseURI + 'Languagelevel/Getlanguagelevel';
export const DeleteLanguageLevel = BaseURI + 'Languagelevel/DeleteLanguagelevel';
export const GetLanguageLevelById = BaseURI + 'Languagelevel/GetLanguagelevelById';
export const getFilterLanguageLevel= BaseURI + 'Languagelevel/GetFilterLanguagelevel';

export const SaveEducationlevel = BaseURI +'EducationLevel/SaveEducationLevel';
export const  GetAllEducationLevel= BaseURI+'EducationLevel/GetEducationLevel';
export const  GetEducationLevelById= BaseURI+'EducationLevel/GetEducationLevelById';
export const  DeleteEducationLevel= BaseURI+'EducationLevel/DeleteEducationLevel';
export const getFilterEducationLevel= BaseURI + 'EducationLevel/GetFilterEducationLevel';

export const getAllPublicHoliiday = BaseURI +'PublicHoliday/GetPublicHoliday';
export const getPublicHolidayById = BaseURI + 'PublicHoliday/GetPublicHolidayById';
export const savePublicHoliday = BaseURI + 'PublicHoliday/SavePublicHoliday';
export const deletePublicHoliday = BaseURI + 'PublicHoliday/DeletePublicHoliday';
//role
export const GetAllrole = BaseURI + 'Role/GetRole';


//permission
export const getPermissionById = BaseURI + 'Permission/GetPermissionByRole';
export const setPermission = BaseURI + 'Permission/SetPermission';

//ManageLeave
export const applyLeave = BaseURI + 'ManageLeave/ApplyLeave';
export const GetFilterLeaves = BaseURI + 'ManageLeave/GetFilterLeave';
export const getLeavesById = BaseURI + 'ManageLeave/GetLeaveById';
export const deleteLeave = BaseURI + 'ManageLeave/DeleteLeave';

//leave
export const ApplyLeaveFromCalendar = BaseURI + 'Leave/ApplyLeave';
export const GetLeaves = BaseURI + 'Leave/GetFilterLeave';
export const GetFilterPendingLeave = BaseURI + 'Leave/GetFilterPendingLeave';
export const GetFilterLeaveHistory = BaseURI + 'Leave/GetFilterLeaveHistory';
export const GetAllLeaves = BaseURI + 'Leave/GetAllLeaveByEmployee';
export const ApprovOrRejectLeaves = BaseURI + 'Leave/ApprovOrRejectLeave';
export const GetLeaveByDate = BaseURI + 'Leave/GetLeaveByDate';


export const getFilterPublicHoliday = BaseURI + "PublicHoliday/GetFilterPublicHoliday";
export const GetCity = BaseURI + 'City/GetCity';
export const GetCityById = BaseURI + 'City/GetCityById';
export const GetCityByState = BaseURI + 'City/GetCityByState';
export const DeleteCityById = BaseURI + 'City/DeleteCity';
export const  GetFilterCity=  BaseURI+ "City/GetFilterCity";
export const SaveCities = BaseURI + 'City/SaveCity';

export const getLeaveType = BaseURI + 'LeaveType/GetLeaveType';
export const saveLeaveType = BaseURI + 'LeaveType/SaveLeaveType';
export const getLeaveTypeByID = BaseURI + 'LeaveType/GetLeaveTypeByID';
export  const deleteLeaveType = BaseURI + 'LeaveType/DeleteLeaveType';
export const getFilterLeaveType = BaseURI+'LeaveType/GetFilterLeaveType';

export const GetLeaveCategory = BaseURI  + "LeaveCategory/GetLeaveCategory";
export const SaveLeaveCategory = BaseURI +  "LeaveCategory/SaveLeaveCategory" ;
export const  GetLeaveCategoryById = BaseURI+ "LeaveCategory/GetLeaveCategoryById" ;
export const DeleteLeaveCategory = BaseURI+"LeaveCategory/DeleteLeaveCategory";

export const GetAllRole = BaseURI  + "Role/GetRole";
export const SaveRole= BaseURI +  "Role/SaveRole";
export const  GetRoleByid = BaseURI+ "Role/GetRoleByid";
export const DeleteRole= BaseURI+"Role/DeleteRole";
export const  GetFilterRole=  BaseURI+ "Role/GetFilterRole";

export const getFilterLeaveCategory = BaseURI + "LeaveCategory/GetFilterLeaveCategory";

export const GetEducation= BaseURI + 'Education/GetEducation';
export const GetEducationById= BaseURI + 'Education/GetEducationById';
export const GetFilterEducation= BaseURI + 'Education/GetFilterEducation';
export const SaveEducation= BaseURI +'Education/SaveEducation';
export  const DeleteEducation=BaseURI+'Education/DeleteEducation';
export const GetEducationByEmployee=BaseURI+'Education/GetEducationByEmployee';

export const SaveLanguageCompetence= BaseURI +  "LanguageCompetence/SaveLanguageCompetence";
export const  GetLanguageCompetence= BaseURI+ "LanguageCompetence/GetLanguageCompetence";
export const  DeleteLanguageCompetence= BaseURI+ "LanguageCompetence/DeleteLanguageCompetence";
export const  GetLanguageCompetenceByEmployeeId= BaseURI+ "LanguageCompetence/GetLanguageCompetenceByEmployeeId";

export const GetAllContactAddress = BaseURI  + "ContactAddress/GetContactAddress";
export const SaveContactAddress= BaseURI +  "ContactAddress/SaveContactAddress";
export const  GetContactAddressByid = BaseURI+ "ContactAddress/GetContactAddressByid";
export const DeleteContactAddress= BaseURI+"ContactAddress/DeleteContactAddress";
export const  GetFilterContactAddress=  BaseURI+ "ContactAddress/GetFilterContactAddress";

export const GetAllWorklocation = BaseURI  + "GetWorklocation/GetWorklocation";
export const SaveWorkLocation= BaseURI +  "GetWorklocation/SaveWorklocation";

export const GetAllBankDetails  = BaseURI  + "GetAllBankDetails/GetAllBankDetails";
export const SaveBankDetails= BaseURI +  "SaveBankDetails/BankDetails";

export const GetJobHistory = BaseURI  + "JobHistory/GetJobHistory";
export const SaveJobHistory= BaseURI +  "JobHistory/SaveJoBHistory";
export const  GetJobHistoryByEmployeeId = BaseURI+ "JobHistory/GetJobHistoryByEmployeeId";
export const  GetJobHistoryByid = BaseURI+ "JobHistory/GetJobHistoryById";
export const DeleteJobHistory= BaseURI+"JobHistory/DeleteJobHistory";
export const  GetFilterJobHistory=  BaseURI+ "JobHistory/GetFilterGetJobHistory";

export const  GetEmployeeType=  BaseURI+ "EmployeeType/GetEmployeeType";

export const GetBonus= BaseURI + "Bonus/GetBonus";
export const GetFilterBonus= BaseURI + "Bonus/GetFilterBonus";
export const GetBonusById = BaseURI +  "Bonus/GetBonusById";
export const SaveBonus = BaseURI + "Bonus/SaveBonus";
export const DeleteBonus = BaseURI + "Bonus/DeleteBonus";

export const GetSalaryType= BaseURI + "SalaryType/GetSalaryTypes";

export const GetEmployee = BaseURI  + "Employee/GetEmployee";
export const SaveEmployee = BaseURI  + "Employee/SaveEmployee";
export const DeleteEmployee = BaseURI  + "Employee/DeleteEmployee";
export const GetEmployeeById = BaseURI  + "Employee/GetEmployeeById";
export const getEmployeeByLeader = BaseURI  + "Employee/GetEmployeeByLeader";
export const getEmployeeByHr = BaseURI  + "Employee/getEmployeeByHr";
export const GetAvailableLeave = BaseURI +"Employee/GetAvailableLeaveByEmployeeId";
/////employe componenet-database
export const GetEmploymentType = BaseURI  + "EmploymentType/GetEmploymentType";
export const GetTypeofEmployment = BaseURI  + "TypeofEmployment/GetTypeofEmployment";
export const Gettaxclass = BaseURI  + "taxclass/Gettaxclass";
export const GetMaritalStatus = BaseURI  + "MaritalStatus/GetMaritalStatus";
export const GetEmployeenStatus = BaseURI  + "EmployeenStatus/GetEmployeenStatus";
export const GetLeaveTypeEmployee = BaseURI  + "LeaveTypeEmployee/GetLeaveTypeEmployee";
export const GetDeliverymethod = BaseURI  + "Deliverymethod/GetDeliverymethod";

//org-chart
export const GetEmployeeforChart = BaseURI  + "Organisationalchart/GetEmployeeForChart";


export const GetFilterEmployee=BaseURI +"Employee/GetFilterEmployee";
export const getWorkPermit = BaseURI + "WorkPermit/GetWorkPermit";
export const saveWorkPermit = BaseURI + "WorkPermit/SaveWorkPermit";
export const getWorkPermitById = BaseURI + "WorkPermit/GetWorkPermitById";
export const deleteWorkPermit = BaseURI + "WorkPermit/DeleteWorkPermit";

export const getIdentityCard = BaseURI + "IdentityCard/GetIdentityCard";
export const  saveIdentityCard = BaseURI + "IdentityCard/SaveIdentityCard";
export const getIdentityCardById = BaseURI+"IdentityCard/GetIdentityCardById";
export const GetIdentityCardByEmployeeId=BaseURI+"IdentityCard/GetIdentityCardByEmployeeId";
export const deleteIdentityCard = BaseURI+"IdentityCard/DeleteIdentityCard";

export const getDocument = BaseURI+"Document/GetDocument";
export const saveDocument = BaseURI+"Document/SaveDocument";
export const getDocumemtById = BaseURI+"Document/GetDocumentById";
export const deleteDocument = BaseURI + "Document/DeleteDocument";

export const getSalaryType = BaseURI+"SalaryType/GetSalaryTypes";
export const getSalary = BaseURI+"Salary/GetSalary";
export const saveSalary = BaseURI+"Salary/SaveSalary";
export const getSalaryById = BaseURI + "Salary/GetSalaryById";
export const getSalaryByEmployee = BaseURI + "Salary/GetSalaryByEmployee";
export const deleteSalary = BaseURI + "Salary/DeleteSalary";
export const getFilterSalary = BaseURI + "Salary/GetFilterSalary";
export const getPreviousMonth = BaseURI + "Salary/GetPreviousMonthSalary";
export const getTwoMonthsAgo = BaseURI + "Salary/GetTwoMonthsAgoSalary";
export const getLastMonth = BaseURI + "Salary/GetPreviousMonthSalary";
export const getLastTwoMonth = BaseURI + "Salary/GetTwoMonthsAgoSalary";

export const getDocumentlist = BaseURI+"DocumentList/GetDocumentList";
export const getFilterDocumentlist = BaseURI + "DocumentList/GetFilterDocumentList";
export const saveDocumentlist = BaseURI+"DocumentList/SaveDocumentList";
export const getDocumemtlistById = BaseURI+"DocumentList/GetDocumentListById";
export const deleteDocumentlist = BaseURI + "DocumentList/DeleteDocumentList";
export const GetDocumentListByEmployeeId = BaseURI + "DocumentList/GetDocumentListByEmployeeId";
export const GetDocumentListByEntityId = BaseURI + "DocumentList/GetDocumentListByEntityId";

export const getContact = BaseURI+"Contact/GetContact";
export const saveContact = BaseURI+"Contact/SaveContact";
export const GetContactById = BaseURI+"Contact/GetContactById";
export const GetContactByEmployeeId=BaseURI+"Contact/GetContactByEmployeeId";


export const getTransaction = BaseURI + "TransactionType/GetTransactionTypes";
export const getNotification = BaseURI + "Notification/GetNotification";
export const getNotificationByEmployeeId = BaseURI + "Notification/GetNotificationByEmployeeId";
export const saveNotification = BaseURI + "Notification/SaveNotification";
export const updateNotification = BaseURI + "Notification/UpdateNotification";
export const getNotificationId = BaseURI + "Notification/GetNotificationId";

export const saveConsultantRate = BaseURI+"ConsultantRate/SaveConsultantRate";