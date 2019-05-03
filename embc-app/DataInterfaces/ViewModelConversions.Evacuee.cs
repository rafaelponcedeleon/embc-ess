using Gov.Jag.Embc.Public.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static Gov.Jag.Embc.Public.Models.Db.Enumerations;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Person ToViewModel(this Models.Db.Evacuee source, Models.Db.IncidentRegistration incidentRegistration)
        {
            ViewModels.Person result = null;
            var isHeadOfHousehold = false;
            if (source != null)
            {
                if (source.EvacueeTypeCode == EvacueeType.HeadOfHousehold.GetDisplayName())
                {
                    result = new ViewModels.HeadOfHousehold();
                    isHeadOfHousehold = true;
                }
                else
                {
                    result = new ViewModels.FamilyMember();
                }

                result.Id = source.IncidentRegSeqId;

                result.FirstName = source.FirstName;
                result.LastName = source.LastName;

                if (isHeadOfHousehold)
                {
                    var resultHoh = result as ViewModels.HeadOfHousehold;
                    resultHoh.PhoneNumber = incidentRegistration.PhoneNumber;
                    resultHoh.PhoneNumberAlt = incidentRegistration.PhoneNumberAlt;
                    resultHoh.Email = incidentRegistration.Email;

                    resultHoh.PrimaryResidence = incidentRegistration.IncidentRegistrationAddresses.Single(a => a.AddressType == AddressType.Primary).ToViewModel();
                    if (incidentRegistration.IncidentRegistrationAddresses.Any(a => a.AddressType == AddressType.Mailing))
                    {
                        resultHoh.MailingAddress = incidentRegistration.IncidentRegistrationAddresses.Single(a => a.AddressType == AddressType.Mailing).ToViewModel();
                    }

                    var familyMembers = incidentRegistration.Evacuees.Where(e => e.EvacueeTypeCode != EvacueeType.HeadOfHousehold.GetDisplayName());
                    if (familyMembers.Any())
                    {
                        resultHoh.FamilyMembers = new List<ViewModels.FamilyMember>();
                        foreach (var familyMember in familyMembers)
                        {
                            resultHoh.FamilyMembers.Add(familyMember.ToViewModel(incidentRegistration) as ViewModels.FamilyMember);
                        }
                    }
                }

                var resultEvacuee = result as ViewModels.Evacuee;
                resultEvacuee.Nickname = source.Nickname;
                resultEvacuee.Initials = source.Initials;
                resultEvacuee.Gender = source.Gender;
                resultEvacuee.Dob = source.Dob
                    ;
                if (!isHeadOfHousehold)
                {
                    var resultFm = result as ViewModels.FamilyMember;
                    resultFm.RelationshipToEvacuee = source.EvacueeTypeCode == EvacueeType.ImmediateFamily.GetDisplayName()
                        ? EvacueeType.ImmediateFamily.ToViewModel() : EvacueeType.HeadOfHousehold.ToViewModel();
                    resultFm.SameLastNameAsEvacuee = source.SameLastNameAsEvacuee;
                }
            }
            return result;
        }
        public static Models.Db.Evacuee ToModel(this ViewModels.Person source)
        {
            Models.Db.Evacuee result = null;

            if (source != null)
            {
                result = new Models.Db.Evacuee();

                if (source.Id != null)
                {
                    result.IncidentRegistrationId = Models.Db.Evacuee.GetIncidentRegistrationIdFromIncidentRegSeqId(source.Id);
                    result.EvacueeSequenceNumber = Models.Db.Evacuee.GetEvacueeSequenceNumberFromIncidentRegSeqId(source.Id);
                }

                result.FirstName = source.FirstName;
                result.LastName = source.LastName;

                if (source is ViewModels.Evacuee sourceEvacuee)
                {
                    result.Nickname = sourceEvacuee.Nickname;
                    result.Initials = sourceEvacuee.Initials;
                    result.Gender = source.Gender;
                    result.Dob = sourceEvacuee.Dob;
                }

                if (source is ViewModels.HeadOfHousehold sourceHoh)
                {
                    result.EvacueeSequenceNumber = 1;
                    result.EvacueeTypeCode = EvacueeType.HeadOfHousehold.GetDisplayName();
                }

                if (source is ViewModels.FamilyMember sourceFm)
                {
                    result.EvacueeTypeCode = sourceFm.RelationshipToEvacuee.Code;
                    result.SameLastNameAsEvacuee = sourceFm.SameLastNameAsEvacuee;
                }
            }

            return result;
        }
    }
}
